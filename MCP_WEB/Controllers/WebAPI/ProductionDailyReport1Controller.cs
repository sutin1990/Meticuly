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
    public class ProductionDailyReport1Controller : Controller
    {
        private NittanDBcontext _context;
        public ProductionDailyReport1Controller(NittanDBcontext context)
        {
            this._context = context;
        }

        [HttpGet]
        public IActionResult Get(DataSourceLoadOptions loadOptions)
        {
            DataTable dt = new DataTable();
            var msgj = "";
            try
            {
                using (var cmd = _context.Database.GetDbConnection().CreateCommand())
                {
                    cmd.Parameters.Clear();
                    cmd.CommandText = "m_sp_ProductionDailyReport1";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@S_TransDate", SqlDbType.NVarChar) { Value = "" });
                    cmd.Parameters.Add(new SqlParameter("@E_TransDate", SqlDbType.NVarChar) { Value = "" });
                    cmd.Parameters.Add(new SqlParameter("@MachineCode", SqlDbType.Int) { Value = 0 });

                    if (cmd.Connection.State != ConnectionState.Open)
                    {
                        cmd.Connection.Open();
                    }

                    var DbReader = cmd.ExecuteReader();

                    if (DbReader.HasRows)
                    {
                        dt.Load(DbReader);

                        if (dt.Columns.Count > 1)
                        {
                            dt.Columns.Add("SqlStatus", typeof(System.String));
                            dt.Columns.Add("SqlErrtext", typeof(System.String));
                            foreach (DataRow dr in dt.Select())
                            {
                                dr["SqlStatus"] = "Success";
                                dr["SqlErrtext"] = "";
                            }
                        }
                        else
                        {
                            dt.Columns.Add("SqlStatus", typeof(System.String));
                            dt.Columns.Add("SqlErrtext", typeof(System.String));
                            foreach (DataRow dr in dt.Select())
                            {
                                dr["SqlStatus"] = "NoData";
                                dr["SqlErrtext"] = "";
                            }
                        }

                    }
                    cmd.Connection.Close();
                }
            }
            catch (SqlException ex)
            {
                msgj = ex.Message;
                dt.Columns.Add("SqlStatus", typeof(System.String));
                dt.Columns.Add("SqlErrtext", typeof(System.String));
                foreach (DataRow dr in dt.Select())
                {
                    dr["SqlStatus"] = "Error";
                    dr["SqlErrtext"] = msgj;
                }

            }
            return Json(dt);
        }

        [HttpGet]
        public IActionResult Get_m_MachineMaster(DataSourceLoadOptions loadOptions)
        {
            var mm = _context.m_MachineMaster;
            return Json(DataSourceLoader.Load(mm, loadOptions));
        }

        [HttpPost]
        public IActionResult Filter(DataSourceLoadOptions loadOptions,string fromdate, string todate,int machinemaste)
        {            
            DataTable dt = new DataTable();
            var msgj = "";

            try
            {
                using (var cmd = _context.Database.GetDbConnection().CreateCommand())
                {
                    cmd.Parameters.Clear();
                    cmd.CommandText = "m_sp_ProductionDailyReport1";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@S_TransDate", SqlDbType.NVarChar) { Value = fromdate });
                    cmd.Parameters.Add(new SqlParameter("@E_TransDate", SqlDbType.NVarChar) { Value = todate });
                    cmd.Parameters.Add(new SqlParameter("@MachineCode", SqlDbType.Int) { Value = machinemaste });

                    if (cmd.Connection.State != ConnectionState.Open)
                    {
                        cmd.Connection.Open();
                    }

                    var DbReader = cmd.ExecuteReader();

                    if (DbReader.HasRows)
                    {
                        dt.Load(DbReader);

                        if (dt.Columns.Count > 1)
                        {
                            dt.Columns.Add("SqlStatus", typeof(System.String));
                            dt.Columns.Add("SqlErrtext", typeof(System.String));
                            foreach (DataRow dr in dt.Select())
                            {
                                dr["SqlStatus"] = "Success";
                                dr["SqlErrtext"] = "";
                            }
                        }
                        else
                        {
                            dt.Columns.Add("SqlStatus", typeof(System.String));
                            dt.Columns.Add("SqlErrtext", typeof(System.String));
                            foreach (DataRow dr in dt.Select())
                            {
                                dr["SqlStatus"] = "NoData";
                                dr["SqlErrtext"] = "";
                            }
                        }

                    }
                    cmd.Connection.Close();
                }
            }
            catch (SqlException ex)
            {
                msgj = ex.Message;
                dt.Columns.Add("SqlStatus", typeof(System.String));
                dt.Columns.Add("SqlErrtext", typeof(System.String));
                foreach (DataRow dr in dt.Select())
                {
                    dr["SqlStatus"] = "Error";
                    dr["SqlErrtext"] = msgj;
                }

            }          

            return Json(dt);
            //return Json(DataSourceLoader.Load(mm, loadOptions));
        }

        [HttpPost]
        public IActionResult PrintProductionDailyReport1(string FCode)
        {
            string[] fcode;
            List<VW_MFC_ProductionDailyReport1> pdr = _context.VW_MFC_ProductionDailyReport1.ToList();

            if (FCode != null)
            {
                //fcode = FCode.Split(",");
                pdr = pdr.Where(w => FCode.Contains(w.FCode)).ToList();
            }
            return View(pdr);
            //return Json(DataSourceLoader.Load(pdr, loadOptions));
        }
    }
}