using DevExtreme.AspNet.Data;
using DevExtreme.AspNet.Mvc;
using MCP_WEB.Data;
using MCP_WEB.Helper;
using MCP_WEB.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Security.Claims;

namespace MCP_WEB.Controllers.WebAPI
{
    //[Route("api/[controller]")]
    [Produces("application/json")]
    [Route("api/[controller]/[action]")]
    //[ApiController]
    public class GridVW_MFC_WIPAdjustController : Controller
    {
        private NittanDBcontext _context;
        public GridVW_MFC_WIPAdjustController(NittanDBcontext context)
        {
            this._context = context;
        }

        [HttpGet]
        public IActionResult Get_Data(DataSourceLoadOptions loadOptions)
        {
            var WIPAdjust = from wa in _context.VW_MFC_WIPAdjust
                            select new
                            {
                                wa.Operation,
                                wa.BarCode,
                                wa.Model,
                                wa.ProcessCode,
                                wa.WIPQty,
                            };
            return Json(DataSourceLoader.Load(WIPAdjust, loadOptions));
        }

        [HttpPost]
        public IActionResult Edit_data(DataSourceLoadOptions loadOptions, string barcode, string processcode,int qtyaudit,string modifyby)
        {
            DataTable dt = new DataTable();
            var msgj = "";            
            
            try
            {
                using (var cmd = _context.Database.GetDbConnection().CreateCommand())
                {
                    cmd.Parameters.Clear();
                    cmd.CommandText = "m_sp_WIPAdjust";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@Barcode", SqlDbType.NVarChar) { Value = barcode });
                    cmd.Parameters.Add(new SqlParameter("@ProcessCode", SqlDbType.NVarChar) { Value = processcode });                    
                    cmd.Parameters.Add(new SqlParameter("@QtyAudit", SqlDbType.Int) { Value = qtyaudit });                    
                    cmd.Parameters.Add(new SqlParameter("@ModifyBy", SqlDbType.NVarChar) { Value = modifyby });                    

                    if (cmd.Connection.State != ConnectionState.Open)
                    {
                        cmd.Connection.Open();
                    }

                    var DbReader = cmd.ExecuteReader();
                    
                        msgj  = "completed successfully";
                   

                    //while (DbReader.Read())
                    //{
                    //    msgj = DbReader.GetString(0);
                    //}
                    //if (DbReader.HasRows)
                    //{
                    //    dt.Load(DbReader);

                    //    if (dt.Columns.Count > 1)
                    //    {
                    //        dt.Columns.Add("SqlStatus", typeof(System.String));
                    //        dt.Columns.Add("SqlErrtext", typeof(System.String));
                    //        foreach (DataRow dr in dt.Select())
                    //        {
                    //            dr["SqlStatus"] = "Success";
                    //            dr["SqlErrtext"] = "";
                    //        }
                    //    }
                    //    else
                    //    {
                    //        dt.Columns.Add("SqlStatus", typeof(System.String));
                    //        dt.Columns.Add("SqlErrtext", typeof(System.String));
                    //        foreach (DataRow dr in dt.Select())
                    //        {
                    //            dr["SqlStatus"] = "NoData";
                    //            dr["SqlErrtext"] = "";
                    //        }
                    //    }

                    //}
                    cmd.Connection.Close();
                }
            }
            catch (SqlException ex)
            {
                msgj = ex.Message;
                //dt.Columns.Add("SqlStatus", typeof(System.String));
                //dt.Columns.Add("SqlErrtext", typeof(System.String));
                //foreach (DataRow dr in dt.Select())
                //{
                //    dr["SqlStatus"] = "Error";
                //    dr["SqlErrtext"] = msgj;
                //}

            }

            return Json(msgj);
        }

       

    }
}