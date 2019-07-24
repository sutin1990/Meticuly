
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using DevExtreme.AspNet.Data;
using DevExtreme.AspNet.Mvc;
using MCP_WEB.Data;
using MCP_WEB.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting.Internal;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OfficeOpenXml;
using MCP_WEB.Views.WeeklyPlan;
using Newtonsoft.Json;
using System.Collections;
using Newtonsoft.Json.Linq;

namespace MCP_WEB.Controllers.FrontEnd
{
    [Authorize]
    public class FileUploadController : Controller
    {
        private NittanDBcontext _context;
        public FileUploadController(NittanDBcontext context)
        {
            _context = context;

        }

        [HttpPost("FileUpload")]
        public async Task<IActionResult> Post(List<IFormFile> files)
        {
            try
            {
                var filePath = Path.GetTempFileName();
                FileInfo file = new FileInfo(filePath);
                string process_id = Guid.NewGuid().ToString();

                if (files.Count > 0)
                {
                    // full path to file in temp location
                    foreach (var formFile in files)
                    {
                        if (formFile.Length > 0)
                        {
                            using (var stream = new FileStream(filePath, FileMode.Create))
                            {
                                await formFile.CopyToAsync(stream);
                            }
                        }
                    }

                    // Call Store
                    var importlist = WeeklyPlanInsert(filePath, process_id);

                    // Store Return Logfiles
                    var processLog = (from i in _context.s_ProcessLog
                                      where i.ProcessID == process_id
                                      select new
                                      {
                                          i.id,
                                          i.ProcessID,
                                          i.ProcessDate,
                                          i.ErrorKey,
                                          i.RecordKey1,//ItemCode
                                          i.RecordKey2,//SeriesLot
                                          i.Msg
                                      });

                    // Create data to confirm popup
                    foreach (var i in importlist)
                    {
                        var q = processLog
                                .OrderByDescending(x => x.id)
                                .FirstOrDefault(x => x.ProcessID == process_id);

                        if (q != null)
                        {
                            i.Msg = q.Msg;
                            i.ErrorKey = q.ErrorKey;
                        }
                        //else
                        //{
                        //    i.Msg = "Invalid Part Number[]";
                        //    i.ErrorKey = q.ErrorKey;
                        //}
                    }

                    return Json(new { status = "success", ProcessLog = processLog, ImportList = importlist });
                }

                return Json(new { status = "success", ProcessId = process_id });
            }
            catch (Exception ex)
            {

                return Json(new { status = "error", message = ex.Message });
            }

        }

        //public async Task<IQueryable> WeeklyPlanInsert(string filePath, string ProcessId)
        public List<ImportList> WeeklyPlanInsert(string filePath, string ProcessId)
        {
            //Read Excel
            FileInfo file = new FileInfo(filePath);
            using (ExcelPackage package = new ExcelPackage(file))
            {
                string PartNo, FCode, SeriesLot;
                decimal QtyAllLot;

                var ImpList = new List<ImportList>();

                ExcelWorksheet worksheet = package.Workbook.Worksheets[1];
                int rowCount = worksheet.Dimension.Rows;
                int ColCount = worksheet.Dimension.Columns;
                for (int row = 2; row <= rowCount; row++)
                {
                    try
                    {
                        if ( worksheet.Cells[row, 1].Value != null )
                        {
                            PartNo = worksheet.Cells[row, 1].Value.ToString();
                        }
                        else { PartNo = ""; }
                    }
                    catch { PartNo = ""; }
                    try
                    {
                        if( worksheet.Cells[row, 2].Value != null ) { 
                        FCode = worksheet.Cells[row, 2].Value.ToString();
                        } else { FCode = ""; }
                    }
                    catch { FCode = ""; }
                    try
                    {
                        if (worksheet.Cells[row, 3].Value != null)
                        {
                            QtyAllLot = decimal.Parse(worksheet.Cells[row, 3].Value.ToString());
                        }
                        else { QtyAllLot = 0; }
                    }
                    catch { QtyAllLot = 0; }
                    try
                    {
                        if (worksheet.Cells[row, 4].Value != null)
                        {
                            SeriesLot = worksheet.Cells[row, 4].Value.ToString();
                        }else { SeriesLot = ""; }
                    }
                    catch { SeriesLot = ""; }

                    //Add ImportList
                    ImportList t = new ImportList();
                    t.id = row - 1;
                    t.ProcessID = ProcessId;
                    t.ProcessDate = DateTime.Now;
                    t.FCode = FCode;
                    t.ItemCode = PartNo;
                    t.QtyOrderAll = (int)QtyAllLot;
                    t.SeriesLot = SeriesLot;
                    t.Msg = "Success";
                    t.Confirm = "No";
                    ImpList.Add(t);

                    //Add user id
                    var identity = (ClaimsIdentity)User.Identity;
                    IEnumerable<Claim> claims = identity.Claims;
                    var c = claims.FirstOrDefault();

                    using (var cmd = _context.Database.GetDbConnection().CreateCommand())
                    {
                        cmd.Parameters.Clear();
                        cmd.CommandText = "m_sp_WeeklyPlan_Insert";
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add(new SqlParameter("@FCode", SqlDbType.NVarChar) { Value = FCode });
                        cmd.Parameters.Add(new SqlParameter("@ItemCode", SqlDbType.NVarChar) { Value = PartNo });
                        cmd.Parameters.Add(new SqlParameter("@QtyOrderAll", SqlDbType.Int) { Value = QtyAllLot });
                        cmd.Parameters.Add(new SqlParameter("@SeriesLot", SqlDbType.NVarChar) { Value = SeriesLot });
                        cmd.Parameters.Add(new SqlParameter("@CreateBy", SqlDbType.NVarChar) { Value = c.Value });
                        cmd.Parameters.Add(new SqlParameter("@PlanUserDef1", SqlDbType.NVarChar) { Value = "" });
                        cmd.Parameters.Add(new SqlParameter("@PlanUserDef2", SqlDbType.NVarChar) { Value = "" });
                        cmd.Parameters.Add(new SqlParameter("@ConfirmReplace", SqlDbType.NVarChar) { Value = "N" });
                        cmd.Parameters.Add(new SqlParameter("@ProcessID", SqlDbType.NVarChar) { Value = ProcessId });

                        try
                        {
                            if (cmd.Connection.State != ConnectionState.Open)
                            {
                                cmd.Connection.Open();
                            }
                            //await cmd.ExecuteNonQueryAsync();
                            cmd.ExecuteNonQuery();
                        }
                        catch (SqlException ex)
                        {
                            //return ex.Message;
                            throw ex;
                        }
                    }

                }

                return ImpList;
            }
        }

        [HttpGet]
        public IActionResult WeeklyPlanConfirm(string item)
        {
             try
            {
                //JArray s = JArray.Parse(item);
                var importList = JsonConvert.DeserializeObject<List<ImportList>>(item);

                //Add user id
                var identity = (ClaimsIdentity)User.Identity;
                IEnumerable<Claim> claims = identity.Claims;
                var c = claims.FirstOrDefault();

                //Call Store Procedure
                string PartNo, FCode, SeriesLot;//, Confirm;
                decimal QtyAllLot;
                Int32? ErrorKey;
                string ProcessId = Guid.NewGuid().ToString();
                for (var i = 0; i < importList.Count; i++)
                {
                    try
                    {
                        ErrorKey = importList[i].ErrorKey;
                        if (ErrorKey == null)
                            ErrorKey = 0;
                    }
                    catch { ErrorKey = 0; }
                    //try
                    //{
                    //    Confirm = importList[i].Confirm;
                    //}
                    //catch { Confirm = "No"; }
                    try
                    {
                        PartNo = importList[i].ItemCode;
                    }
                    catch { PartNo = ""; }
                    try
                    {
                        FCode = importList[i].FCode;
                    }
                    catch { FCode = ""; }
                    try
                    {
                        QtyAllLot = importList[i].QtyOrderAll;
                    }
                    catch { QtyAllLot = 0; }
                    try
                    {
                        SeriesLot = importList[i].SeriesLot;
                    }
                    catch { SeriesLot = ""; }

                    //if (Confirm == "Yes" && ErrorKey>=100)
                    if (ErrorKey >= 100)
                    {
                        using (var cmd = _context.Database.GetDbConnection().CreateCommand())
                        {
                            cmd.Parameters.Clear();
                            cmd.CommandText = "m_sp_WeeklyPlan_Insert";
                            cmd.CommandType = CommandType.StoredProcedure;
                            cmd.Parameters.Add(new SqlParameter("@FCode", SqlDbType.NVarChar) { Value = FCode });
                            cmd.Parameters.Add(new SqlParameter("@ItemCode", SqlDbType.NVarChar) { Value = PartNo });
                            cmd.Parameters.Add(new SqlParameter("@QtyOrderAll", SqlDbType.Int) { Value = QtyAllLot });
                            cmd.Parameters.Add(new SqlParameter("@SeriesLot", SqlDbType.NVarChar) { Value = SeriesLot });
                            cmd.Parameters.Add(new SqlParameter("@CreateBy", SqlDbType.NVarChar) { Value = c.Value });
                            cmd.Parameters.Add(new SqlParameter("@PlanUserDef1", SqlDbType.NVarChar) { Value = "" });
                            cmd.Parameters.Add(new SqlParameter("@PlanUserDef2", SqlDbType.NVarChar) { Value = "" });
                            cmd.Parameters.Add(new SqlParameter("@ConfirmReplace", SqlDbType.NVarChar) { Value = "Y" });
                            cmd.Parameters.Add(new SqlParameter("@ProcessID", SqlDbType.NVarChar) { Value = ProcessId });

                            try
                            {
                                if (cmd.Connection.State != ConnectionState.Open)
                                {
                                    cmd.Connection.Open();
                                }
                                //await cmd.ExecuteNonQueryAsync();
                                cmd.ExecuteNonQuery();
                            }
                            catch (SqlException ex)
                            {
                                //return ex.Message;
                                throw ex;
                            }
                        }
                    }

                }


                return Json(new { status = "success" });
            }
            catch (Exception ex)
            {
                return Json(new { status = "error", message = ex.Message });
            }

        }

        class CustomException : Exception
        {
            public CustomException(string message)
            {

            }

        }
    }
}