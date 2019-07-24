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
    public class GridProductionDailyReport2Controller : Controller
    {
        private NittanDBcontext _context;

        public GridProductionDailyReport2Controller(NittanDBcontext context)
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
        public IActionResult Get(string fromdate, string todate, string[]itemprocess)
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
                    cmd.CommandText = "m_sp_ProductionDailyReport2_newpivot";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@S_TransDate", SqlDbType.NVarChar) { Value = fromdate });
                    cmd.Parameters.Add(new SqlParameter("@E_TransDate ", SqlDbType.NVarChar) { Value = todate });                    
                    cmd.Parameters.Add(new SqlParameter("@IdProcess", SqlDbType.NVarChar) { Value = result });

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

        //[HttpPost]
        //public IActionResult GetonlyFCode(string fromdate, string todate, string[] itemprocess,string fcode,string processcode)
        //{
        //    string result = string.Join(",", itemprocess);
        //    var MachineCode = 13;
        //    var S_TransDate = Convert.ToDateTime(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
        //    var E_TransDate = Convert.ToDateTime(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));

        //    //var jsonresult = dttable;
        //    //var result = JsonConvert.DeserializeObject(jsonresult);
        //    DataTable dt = new DataTable();
        //    //JArray CleanJsonObject = JArray.Parse(jsonresult);
        //    //dynamic data = JObject.Parse(CleanJsonObject[0].ToString());
        //    var msgj = "";

        //    try
        //    {
        //        using (var cmd = _context.Database.GetDbConnection().CreateCommand())
        //        {
        //            cmd.Parameters.Clear();
        //            cmd.CommandText = "m_sp_ProductionDailyReport2_filter";
        //            cmd.CommandType = CommandType.StoredProcedure;
        //            cmd.Parameters.Add(new SqlParameter("@S_TransDate", SqlDbType.NVarChar) { Value = fromdate });
        //            cmd.Parameters.Add(new SqlParameter("@E_TransDate ", SqlDbType.NVarChar) { Value = todate });
        //            cmd.Parameters.Add(new SqlParameter("@ProcessCodeS", SqlDbType.NVarChar) { Value = result });
        //            cmd.Parameters.Add(new SqlParameter("@FC", SqlDbType.NVarChar) { Value = fcode });
        //            cmd.Parameters.Add(new SqlParameter("@processcode", SqlDbType.NVarChar) { Value = processcode });

        //            if (cmd.Connection.State != ConnectionState.Open)
        //            {
        //                cmd.Connection.Open();
        //            }

        //            var DbReader = cmd.ExecuteReader();

        //            if (DbReader.HasRows)
        //            {
        //                dt.Load(DbReader);

        //                if (dt.Columns.Count > 1)
        //                {
        //                    //dt.Columns.Add("SqlStatus", typeof(System.String));
        //                    //dt.Columns.Add("SqlErrtext", typeof(System.String));
        //                    //foreach (DataRow dr in dt.Select())
        //                    //{
        //                    //    dr["SqlStatus"] = "Success";
        //                    //    dr["SqlErrtext"] = "";
        //                    //}
        //                }
        //                else
        //                {
        //                    //dt.Columns.Add("SqlStatus", typeof(System.String));
        //                    //dt.Columns.Add("SqlErrtext", typeof(System.String));
        //                    //foreach (DataRow dr in dt.Select())
        //                    //{
        //                    //    dr["SqlStatus"] = "ErrorSelectLot";
        //                    //    dr["SqlErrtext"] = "";
        //                    //}
        //                }

        //            }
        //            cmd.Connection.Close();
        //        }
        //    }
        //    catch (SqlException ex)
        //    {
        //        //msgj = ex.Message;
        //        //dt.Columns.Add("SqlStatus", typeof(System.String));
        //        //dt.Columns.Add("SqlErrtext", typeof(System.String));
        //        //foreach (DataRow dr in dt.Select())
        //        //{
        //        //    dr["SqlStatus"] = "Error";
        //        //    dr["SqlErrtext"] = msgj;
        //        //}

        //    }
        //    //return JsonConvert.SerializeObject(dt, Formatting.Indented);

        //    return Json(dt);
        //}

        public VW_MFC_ProductionDailyReport2 pdr2;
        [HttpPost]
        public IActionResult PrintProductionDalilyReport2(string fromdate, string todate, string[] itemprocess)
        {
            string result = string.Join(",", itemprocess);
            var MachineCode = 13;
            var S_TransDate = Convert.ToDateTime(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
            var E_TransDate = Convert.ToDateTime(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
            var list = new List<VW_MFC_ProductionDailyReport2>();
            string fcode = "";
            string model = "";
            string ProcessCode = "";
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
                    cmd.CommandText = "m_sp_ProductionDailyReport2";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@S_TransDate", SqlDbType.NVarChar) { Value = fromdate });
                    cmd.Parameters.Add(new SqlParameter("@E_TransDate ", SqlDbType.NVarChar) { Value = todate });
                    cmd.Parameters.Add(new SqlParameter("@IdProcess", SqlDbType.NVarChar) { Value = result });

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
                            //list = (from DataRow dr in dt.Rows
                            // select new VW_MFC_ProductionDailyReport2()
                            // {                                 
                            //      FCode = (string)dr["FCode"],
                            //        Model = (string)dr["Model"],
                            //        ProcessCode = (string)dr["ProcessCode"],
                            //        MC1 = (int)dr["MC1"],
                            //        MC2 = (int)dr["MC2"],
                            //        MC3 = (int)dr["MC3"],
                            //        MC4 = (int)dr["MC4"],
                            //        MC5 = (int)dr["MC5"],
                            //        MC6 = (int)dr["MC6"],
                            //        MC7 = (int)dr["MC7"],
                            //        MC8 = (int)dr["MC8"],
                            //        MC9 = (int)dr["MC9"],
                            //        MC10 = (int)dr["MC10"],
                            //        MC11 = (int)dr["MC11"],
                            //        MC12 = (int)dr["MC12"],
                            //        MC13 = (int)dr["MC13"],
                            //        MC14 = (int)dr["MC14"],
                            //        MC15 = (int)dr["MC15"],
                            //        MC16 = (int)dr["MC16"],
                            //        MC17 = (int)dr["MC17"],
                            //        MC18 = (int)dr["MC18"],
                            //        MC19 = (int)dr["MC19"],
                            //        MC20 = (int)dr["MC20"],
                            // }).ToList();

                            //for (int i = 0; i < dt.Rows.Count; i++)
                            foreach (DataRow dr in dt.Select())
                            {
                                //var dr = dt.Rows[i];
                                if (dr["FCode"] != System.DBNull.Value) {
                                    fcode = (string)dr["FCode"];
                                }
                                if (dr["Model"] != System.DBNull.Value)
                                {
                                    model = (string)dr["Model"];
                                }
                                if (dr["ProcessCode"] != System.DBNull.Value)
                                {
                                    ProcessCode = (string)dr["ProcessCode"];
                                }
                                 pdr2 = new VW_MFC_ProductionDailyReport2
                                {
                                    FCode = fcode,
                                    Model = model,
                                    ProcessCode = ProcessCode
                                };
                            }

                               //list.Add(pdr2);

                                //}
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

            //return Json(list);
            // Response.Redirect("http://localhost:64789/PDR2/Index");
            //return Redirect("http://localhost:64789/PDR2/Index");
            
            return RedirectToAction("Index", "PDR2",new { pdr2 });
        }

        [HttpPost]
        public IActionResult Print(string FCode)
        {
            IPHostEntry heserver = Dns.GetHostEntry(Dns.GetHostName());
            var ip = heserver.AddressList[1].ToString();

            var Log_Select_Print = _context.Log_Select_Print.Where(w => w.opt == "PrintProductionDailyReport2");
            //List<VW_MFC_ProductionDailyReport1> pdr = _context.VW_MFC_ProductionDailyReport1.ToList();
            //List<VW_MFC_ProductionDailyReport1> pdr = _context.VW_MFC_ProductionDailyReport1.ToList();
            var flag = false;
            if (FCode != null)
            { 
                flag = true;
                if (Log_Select_Print.Count() > 0)
                {
                    var oldipaddress = Log_Select_Print.Where(w => w.opt == "PrintProductionDailyReport2" && w.ipaddress == ip);
                    if (oldipaddress.Count() > 0)
                    {
                        var pp_update = _context.Log_Select_Print.FirstOrDefault(f => f.opt == "PrintProductionDailyReport2" && f.ipaddress == ip);
                        pp_update.name = FCode;
                        pp_update.ipaddress = ip;
                        _context.Log_Select_Print.Update(pp_update);
                    }
                    else
                    {
                        var pp_insert = new Log_Select_Print
                        {
                            opt = "PrintProductionDailyReport2",
                            name = FCode,
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
                        opt = "PrintProductionDailyReport2",
                        name = FCode,
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

