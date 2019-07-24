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
    public class PrintWIPStatusController : Controller
    {
        private NittanDBcontext _context;
        public PrintWIPStatusController(NittanDBcontext context)
        {
            _context = context;
        }
       public WIPStatus wipStatus;
        public async Task<ActionResult>Index()
        {
            IPHostEntry heserver = Dns.GetHostEntry(Dns.GetHostName());
            var ip = heserver.AddressList[1].ToString();

            var p = _context.s_GlobalPams.SingleOrDefault(x => x.parm_key == "DateFormat");

            var Log_Select_Print =  _context.Log_Select_Print.Where(w => w.opt == "PrintWIPStatus" && w.ipaddress == ip);
                      
                var PrintWIPStatus = Log_Select_Print.ToList();//เอาชุดข้อมูลของ RowNumber ที่เลือกไปมาwhere contain
                var RowNumbers = PrintWIPStatus[0].name.ToString();
                int [] array = RowNumbers.Split(',').Select(n => Convert.ToInt32(n)).ToArray();

            //ViewBag.GlobalDtFormat = p.param_value.ToString();

            var viewbagfromdate = Convert.ToString(DateTime.Now);
           
            ViewBag.fromdate = Convert.ToDateTime(viewbagfromdate).ToString(p.param_value);

            //ViewBag.fromdate = Convert.ToDateTime(DateTime.Now).ToString(p.param_value);


            var list_WIPStatus = new List<WIPStatus>();
            DataTable dt = new DataTable();
            var msgj = "";
            int rownumber=0;           
            string model = "";
            string fcode = "";
            int CUTOFFBAR = 0;
            int FRICTION = 0;
            int FORGING = 0;
            int STELLITE_TIP = 0;
            int STELLITE_SEAT = 0;
            int STRAIGHTENING = 0;
            int STEM_ROUGH = 0;
            int STEM_FINISH = 0;
            int ISONITE = 0;
            int SEAT_FINISH = 0;
            int QC = 0;
            string QtyMove = "";
            string MachineCode = "";
            DateTime asof = DateTime.Now ;                       
         
            try
            {
                using (var cmd = _context.Database.GetDbConnection().CreateCommand())
                {
                    cmd.Parameters.Clear();
                    cmd.CommandText = "m_sp_rpt007_WIPStatus";
                    cmd.CommandType = CommandType.StoredProcedure;

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

                                if (dr["MachineCode"] != System.DBNull.Value)
                                {
                                    MachineCode = (string)dr["MachineCode"];
                                }

                                if (dr["Model"] != System.DBNull.Value)
                                {
                                    model = (string)dr["Model"];
                                }

                                if (dr["FCode"] != System.DBNull.Value)
                                {
                                    fcode = (string)dr["FCode"];
                                }

                                if (dr["CUTOFFBAR"] != System.DBNull.Value)
                                {
                                    CUTOFFBAR = (int)dr["CUTOFFBAR"];
                                }

                                if (dr["FRICTION"] != System.DBNull.Value)
                                {
                                    FRICTION = (int)dr["FRICTION"];
                                }

                                if (dr["FORGING"] != System.DBNull.Value)
                                {
                                    FORGING = (int)dr["FORGING"];
                                }

                                if (dr["STELLITE_TIP"] != System.DBNull.Value)
                                {
                                    STELLITE_TIP = (int)dr["STELLITE_TIP"];
                                }                              

                                if (dr["STELLITE_SEAT"] != System.DBNull.Value)
                                {
                                    STELLITE_SEAT = (int)dr["STELLITE_SEAT"];
                                }

                                if (dr["STRAIGHTENING"] != System.DBNull.Value)
                                {
                                    STRAIGHTENING = (int)dr["STRAIGHTENING"];
                                }

                                if (dr["STEM_ROUGH"] != System.DBNull.Value)
                                {
                                    STEM_ROUGH = (int)dr["STEM_ROUGH"];
                                }

                                if (dr["STEM_FINISH"] != System.DBNull.Value)
                                {
                                    STEM_FINISH = (int)dr["STEM_FINISH"];
                                }

                                if (dr["ISONITE"] != System.DBNull.Value)
                                {
                                    ISONITE = (int)dr["ISONITE"];
                                }

                                if (dr["SEAT_FINISH"] != System.DBNull.Value)
                                {
                                    SEAT_FINISH = (int)dr["SEAT_FINISH"];
                                }

                                if (dr["QC"] != System.DBNull.Value)
                                {
                                    QC = (int)dr["QC"];
                                }

                                if (dr["QtyMove"] != System.DBNull.Value)
                                {
                                    QtyMove = Convert.ToString(dr["QtyMove"]);
                                }

                                wipStatus = new WIPStatus
                                {
                                     RowNumber = rownumber,
                                     Asof =asof ,
                                    MachineCode = MachineCode,                                     
                                     Fcode = fcode,
                                     Model = model,
                                     CUTOFFBAR = CUTOFFBAR,
                                    FRICTION = FRICTION,
                                    FORGING = FORGING,
                                    STELLITE_TIP = STELLITE_TIP,
                                    STELLITE_SEAT = STELLITE_SEAT,
                                    STRAIGHTENING = STRAIGHTENING,
                                    STEM_ROUGH= STEM_ROUGH,
                                    STEM_FINISH = STEM_FINISH,
                                    ISONITE = ISONITE,
                                    SEAT_FINISH = SEAT_FINISH,
                                    QC = QC,
                                    QtyMove = QtyMove
                                };

                                list_WIPStatus.Add(wipStatus);
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
            
           var list = list_WIPStatus.Where(w => array.Contains(w.RowNumber)).ToList();          
            //Thread.Sleep(5000);
            return View(list);            
        }
    }
}