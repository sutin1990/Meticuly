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
using System.Net;
using System.Security.Claims;

namespace MCP_WEB.Controllers.WebAPI
{
    //[Route("api/[controller]")]
    [Produces("application/json")]
    [Route("api/[controller]/[action]")]
    //[ApiController]
    public class GridWIPStatusController : Controller
    {
        private NittanDBcontext _context;
        public GridWIPStatusController(NittanDBcontext context)
        {
            this._context = context;
        }        

        [HttpPost]
        public IActionResult Filter(DataSourceLoadOptions loadOptions)
        {
            //if (fromdate == null) { fromdate = "201903"; }
            //if (Process == null) { Process = ""; }
            //if (machinemaste == null) { machinemaste = ""; }
            //fromdate = fromdate.Replace("-","");
            //fromdate = fromdate.Substring(0, 6);
            DataTable dt = new DataTable();
            var msgj = "";

            try
            {
                using (var cmd = _context.Database.GetDbConnection().CreateCommand())
                {
                    cmd.Parameters.Clear();
                    cmd.CommandText = "m_sp_rpt007_WIPStatus";
                    cmd.CommandType = CommandType.StoredProcedure;
                    //cmd.Parameters.Add(new SqlParameter("@period", SqlDbType.NVarChar) { Value = fromdate });
                    //cmd.Parameters.Add(new SqlParameter("@ProcessCode", SqlDbType.NVarChar) { Value = Process });
                    //cmd.Parameters.Add(new SqlParameter("@MCCode", SqlDbType.NVarChar) { Value = machinemaste });

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
        public IActionResult Print(string RowNumber)
        {
            IPHostEntry heserver = Dns.GetHostEntry(Dns.GetHostName());
            var ip = heserver.AddressList[1].ToString();

            var Log_Select_Print = _context.Log_Select_Print.Where(w => w.opt == "PrintWIPStatus");
            //List<VW_MFC_ProductionDailyReport1> pdr = _context.VW_MFC_ProductionDailyReport1.ToList();
            //List<VW_MFC_ProductionDailyReport1> pdr = _context.VW_MFC_ProductionDailyReport1.ToList();
            var flag = false;
            if (RowNumber != null)
            {
                //HttpContext.Session.SetString("Test", FCode);
                // HttpContext.Session.SetString("fcode", FCode);

                flag = true;
                if (Log_Select_Print.Count() > 0)
                {
                    var oldipaddress = Log_Select_Print.Where(w => w.opt == "PrintWIPStatus" && w.ipaddress == ip);
                    if (oldipaddress.Count() > 0)
                    {
                        var pp_update = _context.Log_Select_Print.FirstOrDefault(f => f.opt == "PrintWIPStatus" && f.ipaddress == ip);
                        pp_update.name = RowNumber;
                        pp_update.ipaddress = ip;
                        _context.Log_Select_Print.Update(pp_update);
                    }
                    else
                    {
                        var pp_insert = new Log_Select_Print
                        {
                            opt = "PrintWIPStatus",
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
                        opt = "PrintWIPStatus",
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