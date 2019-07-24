using DevExtreme.AspNet.Data;
using DevExtreme.AspNet.Mvc;
using MCP_WEB.Data;
using MCP_WEB.Helper;
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
using System.Net;
using System.Security.Claims;

namespace MCP_WEB.Models.Controllers
{
    //[Produces("application/json")]
    [Route("api/[controller]/[action]")]
    public class GridDaily_ReportController : Controller
    {
        private NittanDBcontext _context;

        public GridDaily_ReportController(NittanDBcontext context)
        {
            this._context = context;
        }  
        
        [HttpGet]
        public IActionResult GetProcess()
        {
            var m_p = from p in _context.m_ProcessMaster
                      where p.SysemProcessFLag == "Y"
                      select new
                      {
                          p.ProcessCode,
                          p.ProcessName
                      };
            return Json(m_p);
        }

        [HttpPost]
        public IActionResult Get(string fromdate, string Process)
        {
            //string result = string.Join(",", itemprocess);
            if (fromdate == null) { fromdate = "2019-03-1"; }
            var period = fromdate;
            //var period = fromdate.Replace("-", "");
            //period = period.Substring(0, 6);

            var S_TransDate = Convert.ToDateTime(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
            var E_TransDate = Convert.ToDateTime(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));

            //var jsonresult = dttable;
            //var result = JsonConvert.DeserializeObject(jsonresult);
            DataTable dt = new DataTable();
            //JArray CleanJsonObject = JArray.Parse(jsonresult);
            //dynamic data = JObject.Parse(CleanJsonObject[0].ToString());
            var msgj = "";

            try
            {
                using (var cmd = _context.Database.GetDbConnection().CreateCommand())
                {
                    cmd.Parameters.Clear();
                    cmd.CommandText = "m_sp_rpt008_DailyReport";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@period", SqlDbType.Int) { Value = Convert.ToInt32(period) });                                      
                    cmd.Parameters.Add(new SqlParameter("@ProcessCode", SqlDbType.NVarChar) { Value = Process });

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
                            //dt.Columns.Add("SqlStatus", typeof(System.String));
                            //dt.Columns.Add("SqlErrtext", typeof(System.String));
                            //foreach (DataRow dr in dt.Select())
                            //{
                            //    dr["SqlStatus"] = "Success";
                            //    dr["SqlErrtext"] = "";
                            //}
                        }
                        else
                        {
                            //dt.Columns.Add("SqlStatus", typeof(System.String));
                            //dt.Columns.Add("SqlErrtext", typeof(System.String));
                            //foreach (DataRow dr in dt.Select())
                            //{
                            //    dr["SqlStatus"] = "ErrorSelectLot";
                            //    dr["SqlErrtext"] = "";
                            //}
                        }

                    }
                    cmd.Connection.Close();
                }
            }
            catch (SqlException ex)
            {
                //msgj = ex.Message;
                //dt.Columns.Add("SqlStatus", typeof(System.String));
                //dt.Columns.Add("SqlErrtext", typeof(System.String));
                //foreach (DataRow dr in dt.Select())
                //{
                //    dr["SqlStatus"] = "Error";
                //    dr["SqlErrtext"] = msgj;
                //}

            }
            //return JsonConvert.SerializeObject(dt, Formatting.Indented);

            return Json(dt);
        }

        [HttpPost]
        public IActionResult GetonlyFCode(string fromdate, string todate, string[] itemprocess,string fcode,string processcode)
        {
            string result = string.Join(",", itemprocess);
            var MachineCode = 13;
            var S_TransDate = Convert.ToDateTime(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
            var E_TransDate = Convert.ToDateTime(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));

            //var jsonresult = dttable;
            //var result = JsonConvert.DeserializeObject(jsonresult);
            DataTable dt = new DataTable();
            //JArray CleanJsonObject = JArray.Parse(jsonresult);
            //dynamic data = JObject.Parse(CleanJsonObject[0].ToString());
            var msgj = "";

            try
            {
                using (var cmd = _context.Database.GetDbConnection().CreateCommand())
                {
                    cmd.Parameters.Clear();
                    cmd.CommandText = "m_sp_ProductionDailyReport2_filter";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@S_TransDate", SqlDbType.NVarChar) { Value = fromdate });
                    cmd.Parameters.Add(new SqlParameter("@E_TransDate ", SqlDbType.NVarChar) { Value = todate });
                    cmd.Parameters.Add(new SqlParameter("@ProcessCodeS", SqlDbType.NVarChar) { Value = result });
                    cmd.Parameters.Add(new SqlParameter("@FC", SqlDbType.NVarChar) { Value = fcode });
                    cmd.Parameters.Add(new SqlParameter("@processcode", SqlDbType.NVarChar) { Value = processcode });

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
                            //dt.Columns.Add("SqlStatus", typeof(System.String));
                            //dt.Columns.Add("SqlErrtext", typeof(System.String));
                            //foreach (DataRow dr in dt.Select())
                            //{
                            //    dr["SqlStatus"] = "Success";
                            //    dr["SqlErrtext"] = "";
                            //}
                        }
                        else
                        {
                            //dt.Columns.Add("SqlStatus", typeof(System.String));
                            //dt.Columns.Add("SqlErrtext", typeof(System.String));
                            //foreach (DataRow dr in dt.Select())
                            //{
                            //    dr["SqlStatus"] = "ErrorSelectLot";
                            //    dr["SqlErrtext"] = "";
                            //}
                        }

                    }
                    cmd.Connection.Close();
                }
            }
            catch (SqlException ex)
            {
                //msgj = ex.Message;
                //dt.Columns.Add("SqlStatus", typeof(System.String));
                //dt.Columns.Add("SqlErrtext", typeof(System.String));
                //foreach (DataRow dr in dt.Select())
                //{
                //    dr["SqlStatus"] = "Error";
                //    dr["SqlErrtext"] = msgj;
                //}

            }
            //return JsonConvert.SerializeObject(dt, Formatting.Indented);

            return Json(dt);
        }

        [HttpPost]
        public IActionResult Print(string RowNumber, string fromdate)
        {
            IPHostEntry heserver = Dns.GetHostEntry(Dns.GetHostName());
            var ip = heserver.AddressList[1].ToString();

            var Log_Select_Print = _context.Log_Select_Print.Where(w => w.opt == "PrintDailyReport");

            var flag = false;
            if (RowNumber != null)
            {
                //HttpContext.Session.SetString("Test", FCode);
                // HttpContext.Session.SetString("fcode", FCode);

                flag = true;
                if (Log_Select_Print.Count() > 0)
                {
                    var oldipaddress = Log_Select_Print.Where(w => w.opt == "PrintDailyReport" && w.ipaddress == ip);
                    if (oldipaddress.Count() > 0)
                    {
                        var pp_update = _context.Log_Select_Print.FirstOrDefault(f => f.opt == "PrintDailyReport" && f.ipaddress == ip);
                        pp_update.name = RowNumber;
                        pp_update.ipaddress = ip;
                        _context.Log_Select_Print.Update(pp_update);
                    }
                    else
                    {
                        var pp_insert = new Log_Select_Print
                        {
                            opt = "PrintDailyReport",
                            name = RowNumber,
                            ipaddress = ip
                        };
                        _context.Log_Select_Print.Add(pp_insert);

                    }

                    _context.SaveChanges();
                }
                else
                {
                    var pp_insert = new Log_Select_Print
                    {
                        opt = "PrintDailyReport",
                        name = RowNumber,
                        ipaddress = ip
                    };
                    _context.Log_Select_Print.Add(pp_insert);
                    _context.SaveChanges();
                }
            }
            //return View(pdr);
            return Json(flag);
        }


    }
}

