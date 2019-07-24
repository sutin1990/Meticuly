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
    public class PrintWIPProcessBalanceController : Controller
    {
        private NittanDBcontext _context;
        public PrintWIPProcessBalanceController(NittanDBcontext context)
        {
            _context = context;
        }
       public WIPProcessBalance wipProcessBalance;
        public async Task<ActionResult>Index(string fromdate, string Process, string machinemaste)
        {
            IPHostEntry heserver = Dns.GetHostEntry(Dns.GetHostName());
            var ip = heserver.AddressList[1].ToString();

            var p = _context.s_GlobalPams.SingleOrDefault(x => x.parm_key == "DateFormat");

            var Log_Select_Print =  _context.Log_Select_Print.Where(w => w.opt == "PrintWIPProcessBalance" && w.ipaddress == ip);
                      
                var PrintWIPProcessBalance = Log_Select_Print.ToList();//เอาชุดข้อมูลของ RowNumber ที่เลือกไปมาwhere contain
                var RowNumbers = PrintWIPProcessBalance[0].name.ToString();
                int [] array = RowNumbers.Split(',').Select(n => Convert.ToInt32(n)).ToArray();

            ViewBag.GlobalDtFormat = p.param_value.ToString();
            //ViewBag.fromdate = DateTime.Now;
            var viewbagfromdate =Convert.ToString(DateTime.Now);
            if (fromdate != null)
            {
                viewbagfromdate = fromdate;
            }
            ViewBag.fromdate = viewbagfromdate;
            //ViewBag.todate = Convert.ToDateTime(PrintFGMovement[0].todate).ToString(p.param_value);

            if (fromdate == null) { fromdate = "201903"; }
            if (Process == null) { Process = ""; }
            if (machinemaste == null) { machinemaste = ""; }
            //fromdate = fromdate.Replace("-", "");
            //fromdate = fromdate.Substring(0, 6);
            ViewBag.machincode = machinemaste;
            var list_WIPProcessBalance = new List<WIPProcessBalance>();
            DataTable dt = new DataTable();
            var msgj = "";
            int rownumber=0;           
            string model = "";
            string fcode = "";           
            string uom = "";
            string material1 = "";
            string itemname = "";
            string serieslot = "";
            string machinecode = "";
            string processcode = "";
            DateTime asof = DateTime.Now ;
            int qtymove = 0;           
         
            try
            {
                using (var cmd = _context.Database.GetDbConnection().CreateCommand())
                {
                    cmd.Parameters.Clear();
                    cmd.CommandText = "m_sp_rpt003_WIPProcessBalance";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@period", SqlDbType.Int) { Value = Convert.ToInt32(fromdate) });
                    cmd.Parameters.Add(new SqlParameter("@ProcessCode", SqlDbType.NVarChar) { Value = Process });
                    cmd.Parameters.Add(new SqlParameter("@MCCode", SqlDbType.NVarChar) { Value = machinemaste });

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

                                if (dr["Asof"] != System.DBNull.Value)
                                {
                                    asof = (DateTime)dr["Asof"];
                                }

                                if (dr["Model"] != System.DBNull.Value)
                                {
                                    model = (string)dr["Model"];
                                }

                                if (dr["FCode"] != System.DBNull.Value)
                                {
                                    fcode = (string)dr["FCode"];
                                }

                                if (dr["Material1"] != System.DBNull.Value)
                                {
                                    material1 = (string)dr["Material1"];
                                }

                                if (dr["ItemName"] != System.DBNull.Value)
                                {
                                    itemname = (string)dr["ItemName"];
                                }

                                if (dr["BarCode"] != System.DBNull.Value)
                                {
                                    serieslot = (string)dr["BarCode"];
                                }

                                if (dr["MachineCode"] != System.DBNull.Value)
                                {
                                    machinecode = (string)dr["MachineCode"];
                                }                              

                                if (dr["QtyMove"] != System.DBNull.Value)
                                {
                                    qtymove = (int)dr["QtyMove"];
                                }

                                if (dr["ProcessCode"] != System.DBNull.Value)
                                {
                                    processcode = (string)dr["ProcessCode"];
                                }

                                wipProcessBalance = new WIPProcessBalance {
                                     RowNumber = rownumber,
                                     Asof =asof ,                                     
                                     Fcode = fcode,
                                     Model = model,
                                     Material1 = material1,
                                     ItemName = itemname,
                                     BarCode = serieslot,
                                     MachineCode = machinecode,
                                     QtyMove = qtymove,
                                     ProcessCode = processcode

                                };

                                list_WIPProcessBalance.Add(wipProcessBalance);
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
            
           var list = list_WIPProcessBalance.Where(w => array.Contains(w.RowNumber)).ToList();          
            //Thread.Sleep(5000);
            return View(list);            
        }
    }
}