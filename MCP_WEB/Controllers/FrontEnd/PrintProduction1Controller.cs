using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MCP_WEB.Data;
using MCP_WEB.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using System.Threading;
using System.Net;
using System.Data;
using Microsoft.EntityFrameworkCore;
using System.Data.SqlClient;

namespace MCP_WEB.Controllers.FrontEnd
{
    public class PrintProduction1Controller : Controller
    {
        private NittanDBcontext _context;
        public PrintProduction1Controller(NittanDBcontext context)
        {
            _context = context;
        }
        public VW_MFC_ProductionDailyReport1 vW_MFC_ProductionDailyReport1;
        public async Task<ActionResult> Index(string FCode, string fromdate, string todate)
        {
            var p = _context.s_GlobalPams.SingleOrDefault(x => x.parm_key == "DateFormat");
            var viewfromdate = Convert.ToString(DateTime.Now);
            var viewtodate = Convert.ToString(DateTime.Now);
            if (fromdate != null)
            {
                viewfromdate = fromdate;
            }

            if (todate != null)
            {
                viewtodate = todate;
            }
            
            if(fromdate == null && todate == null)
            {                
                var MinDate = (from d in _context.WoRouting select d.TransDate).Min();                
                var MaxDate = (from d in _context.WoRouting select d.TransDate).Max();

                viewfromdate = MinDate.ToString();
                viewtodate = MaxDate.ToString();
            }
            ViewBag.fromdate = Convert.ToDateTime(viewfromdate).ToString(p.param_value);
            ViewBag.todate = Convert.ToDateTime(viewtodate).ToString(p.param_value);

            if (fromdate == null) { fromdate = ""; }
            if (todate == null) { todate = ""; }

            IPHostEntry heserver = Dns.GetHostEntry(Dns.GetHostName());
            var ip = heserver.AddressList[1].ToString();            

            var Log_Select_Print = _context.Log_Select_Print.Where(w => w.opt == "PrintProduction1" && w.ipaddress == ip);
            var PrintProductionDailyReport1 = Log_Select_Print.ToList();//เอาชุดข้อมูลของ RowNumber ที่เลือกไปมาwhere contain
            var RowNumbers = PrintProductionDailyReport1[0].name.ToString();
            int[] array = RowNumbers.Split(',').Select(n => Convert.ToInt32(n)).ToArray();


            var list_ProductionDailyReport1 = new List<VW_MFC_ProductionDailyReport1>();
            DataTable dt = new DataTable();
            var msgj = "";

            int rownumber = 0;
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
            int Total = 0;
            try
            {
                using (var cmd = _context.Database.GetDbConnection().CreateCommand())
                {
                    cmd.Parameters.Clear();
                    cmd.CommandText = "m_sp_ProductionDailyReport1";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@S_TransDate", SqlDbType.NVarChar) { Value = fromdate });
                    cmd.Parameters.Add(new SqlParameter("@E_TransDate", SqlDbType.NVarChar) { Value = todate });
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

                            foreach (DataRow dr in dt.Select())
                            {

                                if (dr["RowNumber"] != System.DBNull.Value)
                                {
                                    rownumber = Convert.ToInt32(dr["RowNumber"]);
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

                                if (dr["STELLITETIP"] != System.DBNull.Value)
                                {
                                    STELLITE_TIP = (int)dr["STELLITETIP"];
                                }

                                if (dr["STELLITESEAT"] != System.DBNull.Value)
                                {
                                    STELLITE_SEAT = (int)dr["STELLITESEAT"];
                                }

                                if (dr["STRAIGHTENING"] != System.DBNull.Value)
                                {
                                    STRAIGHTENING = (int)dr["STRAIGHTENING"];
                                }

                                if (dr["STEMROUGH"] != System.DBNull.Value)
                                {
                                    STEM_ROUGH = (int)dr["STEMROUGH"];
                                }

                                if (dr["STEMFINISH"] != System.DBNull.Value)
                                {
                                    STEM_FINISH = (int)dr["STEMFINISH"];
                                }

                                if (dr["ISONITE"] != System.DBNull.Value)
                                {
                                    ISONITE = (int)dr["ISONITE"];
                                }

                                if (dr["SEATFINISH"] != System.DBNull.Value)
                                {
                                    SEAT_FINISH = (int)dr["SEATFINISH"];
                                }

                                if (dr["QCVISUAL"] != System.DBNull.Value)
                                {
                                    QC = (int)dr["QCVISUAL"];
                                }

                                if (dr["Total"] != System.DBNull.Value)
                                {
                                    Total = (int)dr["Total"];
                                }

                                vW_MFC_ProductionDailyReport1 = new VW_MFC_ProductionDailyReport1
                                {
                                    RowNumber = rownumber,
                                    FCode = fcode,
                                    Model = model,
                                    CUTOFFBAR = CUTOFFBAR,
                                    FRICTION = FRICTION,
                                    FORGING = FORGING,
                                    STELLITETIP = STELLITE_TIP,
                                    STELLITESEAT = STELLITE_SEAT,
                                    STRAIGHTENING = STRAIGHTENING,
                                    STEMROUGH = STEM_ROUGH,
                                    STEMFINISH = STEM_FINISH,
                                    ISONITE = ISONITE,
                                    SEATFINISH = SEAT_FINISH,
                                    QCVISUAL = QC,
                                    Total = Total
                                };

                                list_ProductionDailyReport1.Add(vW_MFC_ProductionDailyReport1);
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

            var list =  list_ProductionDailyReport1.Where(w => array.Contains(w.RowNumber)).ToList();
            return View(list);            
        }
    }
}