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
    public class PrintDailyReportController : Controller
    {
        private NittanDBcontext _context;
        public PrintDailyReportController(NittanDBcontext context)
        {
            _context = context;
        }
       public DailyReport dailyReport;
        public async Task<ActionResult>Index(string fromdate, string Process)
        {
            IPHostEntry heserver = Dns.GetHostEntry(Dns.GetHostName());
            var ip = heserver.AddressList[1].ToString();

            if (fromdate == null) { fromdate = "2019-03-1"; }
            var period = fromdate;
            //var period = fromdate.Replace("-", "");
            //period = period.Substring(0, 6);

            var p = _context.s_GlobalPams.SingleOrDefault(x => x.parm_key == "DateFormat");

            var Log_Select_Print =  _context.Log_Select_Print.Where(w => w.opt == "PrintDailyReport" && w.ipaddress == ip);
                      
                var PrintFGMovement = Log_Select_Print.ToList();//เอาชุดข้อมูลของ RowNumber ที่เลือกไปมาwhere contain
                var RowNumbers = PrintFGMovement[0].name.ToString();
                int [] array = RowNumbers.Split(',').Select(n => Convert.ToInt32(n)).ToArray();

            ViewBag.GlobalDtFormat = p.param_value.ToString();

            ViewBag.fromdate = Convert.ToString(period);
            //ViewBag.todate = Convert.ToDateTime(PrintFGMovement[0].todate).ToString(p.param_value);

            var list_DailyReport = new List<DailyReport>();
            DataTable dt = new DataTable();
            var msgj = "";
            int rownumber=0;           
            string model = "";
            string fcode = "";
            string _period = "";
            string MachineCode = "";
            //int D01 = 0;
            //int D02 = 0;
            //int D03 = 0;
            //int D04 = 0;
            //int D05 = 0;
            //int D06 = 0;
            //int D07 = 0;
            //int D08 = 0;
            //int D09 = 0;
            //int D10 = 0;
            //int D11 = 0;
            //int D12 = 0;
            //int D13 = 0;
            //int D14 = 0;
            //int D15 = 0;
            //int D16 = 0;
            //int D17 = 0;
            //int D18 = 0;
            //int D19 = 0;
            //int D20 = 0;
            //int D21 = 0;
            //int D22 = 0;
            //int D23 = 0;
            //int D24 = 0;
            //int D25 = 0;
            //int D26 = 0;
            //int D27 = 0;
            //int D28 = 0;
            //int D29 = 0;
            //int D30 = 0;
            //int D31 = 0;
            //int N01 = 0;
            //int N02 = 0;
            //int N03 = 0;
            //int N04 = 0;
            //int N05 = 0;
            //int N06 = 0;
            //int N07 = 0;
            //int N08 = 0;
            //int N09 = 0;
            //int N10 = 0;
            //int N11 = 0;
            //int N12 = 0;
            //int N13 = 0;
            //int N14 = 0;
            //int N15 = 0;
            //int N16 = 0;
            //int N17 = 0;
            //int N18 = 0;
            //int N19 = 0;
            //int N20 = 0;
            //int N21 = 0;
            //int N22 = 0;
            //int N23 = 0;
            //int N24 = 0;
            //int N25 = 0;
            //int N26 = 0;
            //int N27 = 0;
            //int N28 = 0;
            //int N29 = 0;
            //int N30 = 0;
            //int N31 = 0;
            int total = 0;
            List<int> D = new List<int>();
            DateTime asof = DateTime.Now ;                       
         
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
                            
                            foreach (DataRow dr in dt.Select())
                            {

                                if (dr["RowNumber"] != System.DBNull.Value)
                                {
                                    rownumber =  Convert.ToInt32(dr["RowNumber"]);
                                }

                                if (dr["period"] != System.DBNull.Value)
                                {
                                    _period = (string)dr["period"];
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

                                if (dr["total"] != System.DBNull.Value)
                                {
                                    total = (int)dr["total"];
                                }

                                //if (dr["D01"] != System.DBNull.Value)
                                //{
                                //    D01 = (int)dr["D01"];
                                //}

                                //if (dr["D02"] != System.DBNull.Value)
                                //{
                                //    D02 = (int)dr["D02"];
                                //}

                                //if (dr["D03"] != System.DBNull.Value)
                                //{
                                //    D03 = (int)dr["D03"];
                                //}

                                //if (dr["D04"] != System.DBNull.Value)
                                //{
                                //    D04 = (int)dr["D04"];
                                //}

                                //if (dr["D05"] != System.DBNull.Value)
                                //{
                                //    D05 = (int)dr["D05"];
                                //}

                                //if (dr["D06"] != System.DBNull.Value)
                                //{
                                //    D06 = (int)dr["D06"];
                                //}

                                //if (dr["D07"] != System.DBNull.Value)
                                //{
                                //    D07 = (int)dr["D07"];
                                //}
                                //if (dr["D08"] != System.DBNull.Value)
                                //{
                                //    D08 = (int)dr["D08"];
                                //}

                                //if (dr["D09"] != System.DBNull.Value)
                                //{
                                //    D09 = (int)dr["D09"];
                                //}

                                //if (dr["D10"] != System.DBNull.Value)
                                //{
                                //    D10 = (int)dr["D10"];
                                //}

                                //if (dr["D11"] != System.DBNull.Value)
                                //{
                                //    D11 = (int)dr["D11"];
                                //}

                                //if (dr["D12"] != System.DBNull.Value)
                                //{
                                //    D12 = (int)dr["D12"];
                                //}

                                //if (dr["D13"] != System.DBNull.Value)
                                //{
                                //    D13 = (int)dr["D13"];
                                //}

                                //if (dr["D14"] != System.DBNull.Value)
                                //{
                                //    D14 = (int)dr["D14"];
                                //}

                                //if (dr["D15"] != System.DBNull.Value)
                                //{
                                //    D15 = (int)dr["D15"];
                                //}

                                //if (dr["D16"] != System.DBNull.Value)
                                //{
                                //    D16 = (int)dr["D16"];
                                //}

                                //if (dr["D17"] != System.DBNull.Value)
                                //{
                                //    D17 = (int)dr["D17"];
                                //}

                                //if (dr["D18"] != System.DBNull.Value)
                                //{
                                //    D18 = (int)dr["D18"];
                                //}

                                //if (dr["D19"] != System.DBNull.Value)
                                //{
                                //    D19 = (int)dr["D19"];
                                //}

                                //if (dr["D20"] != System.DBNull.Value)
                                //{
                                //    D20 = (int)dr["D20"];
                                //}

                                //if (dr["D21"] != System.DBNull.Value)
                                //{
                                //    D21 = (int)dr["D21"];
                                //}

                                //if (dr["D22"] != System.DBNull.Value)
                                //{
                                //    D22 = (int)dr["D22"];
                                //}

                                //if (dr["D23"] != System.DBNull.Value)
                                //{
                                //    D23 = (int)dr["D23"];
                                //}

                                //if (dr["D24"] != System.DBNull.Value)
                                //{
                                //    D24 = (int)dr["D24"];
                                //}

                                //if (dr["D25"] != System.DBNull.Value)
                                //{
                                //    D25 = (int)dr["D25"];
                                //}

                                //if (dr["D26"] != System.DBNull.Value)
                                //{
                                //    D26 = (int)dr["D26"];
                                //}

                                //if (dr["D27"] != System.DBNull.Value)
                                //{
                                //    D27 = (int)dr["D27"];
                                //}

                                //if (dr["D28"] != System.DBNull.Value)
                                //{
                                //    D28 = (int)dr["D28"];
                                //}

                                //if (dr["D29"] != System.DBNull.Value)
                                //{
                                //    D29 = (int)dr["D29"];
                                //}

                                //if (dr["D30"] != System.DBNull.Value)
                                //{
                                //    D30 = (int)dr["D30"];
                                //}

                                //if (dr["D31"] != System.DBNull.Value)
                                //{
                                //    D31 = (int)dr["D31"];
                                //}

                                //if (dr["N01"] != System.DBNull.Value)
                                //{
                                //    N01 = (int)dr["N01"];
                                //}

                                //if (dr["N02"] != System.DBNull.Value)
                                //{
                                //    N02 = (int)dr["N02"];
                                //}

                                //if (dr["N03"] != System.DBNull.Value)
                                //{
                                //    N03 = (int)dr["N03"];
                                //}

                                //if (dr["N04"] != System.DBNull.Value)
                                //{
                                //    N04 = (int)dr["N04"];
                                //}

                                //if (dr["N05"] != System.DBNull.Value)
                                //{
                                //    N05 = (int)dr["N05"];
                                //}

                                //if (dr["N06"] != System.DBNull.Value)
                                //{
                                //    N06 = (int)dr["N06"];
                                //}

                                //if (dr["N07"] != System.DBNull.Value)
                                //{
                                //    N07 = (int)dr["N07"];
                                //}
                                //if (dr["N08"] != System.DBNull.Value)
                                //{
                                //    N08 = (int)dr["N08"];
                                //}

                                //if (dr["N09"] != System.DBNull.Value)
                                //{
                                //    N09 = (int)dr["N09"];
                                //}

                                //if (dr["N10"] != System.DBNull.Value)
                                //{
                                //    N10 = (int)dr["N10"];
                                //}

                                //if (dr["N11"] != System.DBNull.Value)
                                //{
                                //    N11 = (int)dr["N11"];
                                //}

                                //if (dr["N12"] != System.DBNull.Value)
                                //{
                                //    N12 = (int)dr["N12"];
                                //}

                                //if (dr["N13"] != System.DBNull.Value)
                                //{
                                //    N13 = (int)dr["N13"];
                                //}

                                //if (dr["N14"] != System.DBNull.Value)
                                //{
                                //    N14 = (int)dr["N14"];
                                //}

                                //if (dr["N15"] != System.DBNull.Value)
                                //{
                                //    N15 = (int)dr["N15"];
                                //}

                                //if (dr["N16"] != System.DBNull.Value)
                                //{
                                //    N16 = (int)dr["N16"];
                                //}

                                //if (dr["N17"] != System.DBNull.Value)
                                //{
                                //    N17 = (int)dr["N17"];
                                //}

                                //if (dr["N18"] != System.DBNull.Value)
                                //{
                                //    N18 = (int)dr["N18"];
                                //}

                                //if (dr["N19"] != System.DBNull.Value)
                                //{
                                //    N19 = (int)dr["N19"];
                                //}

                                //if (dr["N20"] != System.DBNull.Value)
                                //{
                                //    N20 = (int)dr["N20"];
                                //}

                                //if (dr["N21"] != System.DBNull.Value)
                                //{
                                //    N21 = (int)dr["N21"];
                                //}

                                //if (dr["N22"] != System.DBNull.Value)
                                //{
                                //    N22 = (int)dr["N22"];
                                //}

                                //if (dr["N23"] != System.DBNull.Value)
                                //{
                                //    N23 = (int)dr["N23"];
                                //}

                                //if (dr["N24"] != System.DBNull.Value)
                                //{
                                //    N24 = (int)dr["N24"];
                                //}

                                //if (dr["N25"] != System.DBNull.Value)
                                //{
                                //    N25 = (int)dr["N25"];
                                //}

                                //if (dr["N26"] != System.DBNull.Value)
                                //{
                                //    N26 = (int)dr["N26"];
                                //}

                                //if (dr["N27"] != System.DBNull.Value)
                                //{
                                //    N27 = (int)dr["N27"];
                                //}

                                //if (dr["N28"] != System.DBNull.Value)
                                //{
                                //    N28 = (int)dr["N28"];
                                //}

                                //if (dr["N29"] != System.DBNull.Value)
                                //{
                                //    N29 = (int)dr["N29"];
                                //}

                                //if (dr["N30"] != System.DBNull.Value)
                                //{
                                //    N30 = (int)dr["N30"];
                                //}

                                //if (dr["N31"] != System.DBNull.Value)
                                //{
                                //    N31 = (int)dr["N31"];
                                //}

                                //for(var i = 0; i < 31; i++)
                                //  {
                                //      var day_no = "0" + (i + 1);
                                //      day_no = day_no.Substring(day_no.Length - 2);
                                //      D.Add((int)dr["D" + day_no]);

                                //  }


                                dailyReport = new DailyReport
                                {
                                    RowNumber = rownumber,
                                    MachineCode = MachineCode,
                                    period = _period,                                     
                                    FCode = fcode,
                                    Model = model,
                                    total = total,
                                    //D01 = D01,
                                    //D02 = D02,
                                    //D03 = D03,
                                    //D04 = D04,
                                    //D05 = D05,
                                    //D06 = D06,
                                    //D07 = D07,
                                    //D08 = D08,
                                    //D09 = D09,
                                    //D10 = D10,
                                    //D11 = D11,
                                    //D12 = D12,
                                    //D13 = D13,
                                    //D14 = D14,
                                    //D15 = D15,
                                    //D16 = D16,
                                    //D17 = D17,
                                    //D18 = D18,
                                    //D19 = D19,
                                    //D20 = D20,
                                    //D21 = D21,
                                    //D22 = D22,
                                    //D23 = D23,
                                    //D24 = D24,
                                    //D25 = D25,
                                    //D26 = D26,
                                    //D27 = D27,
                                    //D28 = D28,
                                    //D29 = D29,
                                    //D30 = D30,
                                    //D31 = D31,
                                    //N01 = N01,
                                    //N02 = N02,
                                    //N03 = N03,
                                    //N04 = N04,
                                    //N05 = N05,
                                    //N06 = N06,
                                    //N07 = N07,
                                    //N08 = N08,
                                    //N09 = N09,
                                    //N10 = N10,
                                    //N11 = N11,
                                    //N12 = N12,
                                    //N13 = N13,
                                    //N14 = N14,
                                    //N15 = N15,
                                    //N16 = N16,
                                    //N17 = N17,
                                    //N18 = N18,
                                    //N19 = N19,
                                    //N20 = N20,
                                    //N21 = N21,
                                    //N22 = N22,
                                    //N23 = N23,
                                    //N24 = N24,
                                    //N25 = N25,
                                    //N26 = N26,
                                    //N27 = N27,
                                    //N28 = N28,
                                    //N29 = N29,
                                    //N30 = N30,
                                    //N31 = N31,

                                };

                                list_DailyReport.Add(dailyReport);
                            }
                            //ViewBag.table = dt;
                            //ViewData.Model = dt.AsEnumerable();
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
            
           var list = list_DailyReport.Where(w => array.Contains(w.RowNumber)).ToList();
            ViewBag.listdata = list;
            //Thread.Sleep(5000);
            return View(dt);            
        }
    }
}