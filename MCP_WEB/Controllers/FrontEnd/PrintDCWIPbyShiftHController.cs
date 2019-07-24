using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MCP_WEB.Data;
using MCP_WEB.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using System.Threading;
using System.Data;
using System.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System.Net;

namespace MCP_WEB.Controllers.FrontEnd
{
    public class PrintDCWIPbyShiftHController : Controller
    {
        private NittanDBcontext _context;
        public PrintDCWIPbyShiftHController(NittanDBcontext context)
        {
            _context = context;
        }
       public DCWIPbyShiftH DCWIPbyShiftH;
        public async Task<ActionResult>Index(string fromdate, string todate, string shift)
        {
            IPHostEntry heserver = Dns.GetHostEntry(Dns.GetHostName());
            var ip = heserver.AddressList[1].ToString();

            if (fromdate == null) { fromdate = "2019-03-1"; }
            if (todate == null) { todate = ""; }
            if (shift == null) { shift = ""; }
            ViewBag.shift = shift;
            var p = _context.s_GlobalPams.SingleOrDefault(x => x.parm_key == "DateFormat");

            var Log_Select_Print =  _context.Log_Select_Print.Where(w => w.opt == "PrintDCWIPbyShiftHistory" && w.ipaddress == ip);
                      
                var PrintFGMovement = Log_Select_Print.ToList();//เอาชุดข้อมูลของ RowNumber ที่เลือกไปมาwhere contain
                var RowNumbers = PrintFGMovement[0].name.ToString();
                int [] array = RowNumbers.Split(',').Select(n => Convert.ToInt32(n)).ToArray();

            ViewBag.GlobalDtFormat = p.param_value.ToString();

            var viewbagfromdate = Convert.ToString(DateTime.Now);

            ViewBag.fromdate = Convert.ToDateTime(viewbagfromdate).ToString(p.param_value);

            //ViewBag.fromdate = Convert.ToDateTime(DateTime.Now).ToString(p.param_value);


            var list_DCWIPbyShiftH = new List<DCWIPbyShiftH>();
            DataTable dt = new DataTable();
            var msgj = "";
            int rownumber=0;  
            int qtyorder = 0;
            decimal wip = 0;
            decimal ng = 0;
            decimal nc = 0;
            decimal wiprate = 0;
            decimal ngrate = 0;
            decimal ncrate = 0;            
            string barcode = "";
            string processcode = "";
         
            try
            {
                using (var cmd = _context.Database.GetDbConnection().CreateCommand())
                {
                    cmd.Parameters.Clear();
                    cmd.CommandText = "m_sp_rpt011_DCWIPbyShiftH";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@FromDate", SqlDbType.NVarChar) { Value = fromdate });
                    cmd.Parameters.Add(new SqlParameter("@ToDate", SqlDbType.NVarChar) { Value = todate });
                    cmd.Parameters.Add(new SqlParameter("@shift", SqlDbType.NVarChar) { Value = shift });

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
                          
                            foreach (DataRow dr in dt.Select())
                            {

                                if (dr["RowNumber"] != System.DBNull.Value)
                                {
                                    rownumber =  Convert.ToInt32(dr["RowNumber"]);
                                } 

                                if (dr["ProcessCode"] != System.DBNull.Value)
                                {
                                    processcode = (string)dr["ProcessCode"];
                                }

                                if (dr["Barcode"] != System.DBNull.Value)
                                {
                                    barcode = (string)dr["Barcode"];
                                }

                                if (dr["QtyOrder"] != System.DBNull.Value)
                                {
                                    qtyorder = (int)dr["QtyOrder"];
                                }

                                if (dr["WIP"] != System.DBNull.Value)
                                {
                                    wip = (decimal)dr["WIP"];
                                }

                                if (dr["NG"] != System.DBNull.Value)
                                {
                                    ng = (decimal)dr["NG"];
                                }

                                if (dr["NC"] != System.DBNull.Value)
                                {
                                    nc = (decimal)dr["NC"];
                                }

                                if (dr["WIPRate"] != System.DBNull.Value)
                                {
                                    wiprate = (decimal)dr["WIPRate"];
                                }

                                if (dr["NGRate"] != System.DBNull.Value)
                                {
                                    ngrate = (decimal)dr["NGRate"];
                                }

                                if (dr["NCRate"] != System.DBNull.Value)
                                {
                                    ncrate = (decimal)dr["NCRate"];
                                }

                                DCWIPbyShiftH = new DCWIPbyShiftH
                                {                                    
                                    RowNumber = rownumber,   
                                    ProcessCode = processcode,
                                    Barcode = barcode,
                                    QtyOrder = qtyorder,
                                    WIP = wip,
                                    NG = ng,
                                    NC = nc,
                                    WIPRate = wiprate,
                                    NGRate = ngrate,
                                    NCRate = ncrate,

                                };

                                list_DCWIPbyShiftH.Add(DCWIPbyShiftH);
                            }
                        }
                        else
                        {
                           
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
            
           var list = list_DCWIPbyShiftH.Where(w => array.Contains(w.RowNumber)).ToList();          
            //Thread.Sleep(5000);
            return View(list);            
        }
    }
}