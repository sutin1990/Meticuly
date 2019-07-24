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
    public class PrintFGMovementController : Controller
    {
        private NittanDBcontext _context;
        public PrintFGMovementController(NittanDBcontext context)
        {
            _context = context;
        }
       public FGMovement fgMovements;
        public async Task<ActionResult>Index(string fromdate,string todate)
        {
            if (fromdate == null) { fromdate = "2019-03-1"; }
            IPHostEntry heserver = Dns.GetHostEntry(Dns.GetHostName());
            var ip = heserver.AddressList[1].ToString();

            var p = _context.s_GlobalPams.SingleOrDefault(x => x.parm_key == "DateFormat");

            var Log_Select_Print = _context.Log_Select_Print.Where(w => w.opt == "PrintFGMovement" && w.ipaddress == ip);
            //var oldipaddress = Log_Select_Print.FirstOrDefault().ipaddress;
            //if (oldipaddress!=null)
            //{
            //    Log_Select_Print = _context.Log_Select_Print.Where(w => w.opt == "PrintFGMovement" && w.ipaddress == ipaddress);
            //}

            var PrintFGMovement = Log_Select_Print.ToList();//เอาชุดข้อมูลของ RowNumber ที่เลือกไปมาwhere contain
                var RowNumbers = PrintFGMovement[0].name.ToString();
                int [] array = RowNumbers.Split(',').Select(n => Convert.ToInt32(n)).ToArray();

            ViewBag.GlobalDtFormat = p.param_value.ToString();

            ViewBag.fromdate = Convert.ToDateTime(fromdate).ToString(p.param_value);
            var viewbagtodate = Convert.ToString(DateTime.Now);
            if (todate != null)
            {
                viewbagtodate = todate;
            }
            ViewBag.todate = Convert.ToDateTime(viewbagtodate).ToString(p.param_value);

            if (todate == null) { todate = ""; }

            var list_FGmovement = new List<FGMovement>();
            DataTable dt = new DataTable();
            var msgj = "";
            int rownumber=0;           
            string model = "";
            string fcode = "";           
            string uom = "";
            string barcode = "";
            DateTime transdate = DateTime.Now ;
            int Qty = 0;           
         
            try
            {
                using (var cmd = _context.Database.GetDbConnection().CreateCommand())
                {
                    cmd.Parameters.Clear();
                    cmd.CommandText = "m_sp_rpt002_FGMovement";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@FromDate", SqlDbType.NVarChar) { Value = fromdate });
                    cmd.Parameters.Add(new SqlParameter("@ToDate", SqlDbType.NVarChar) { Value = todate });

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

                                if (dr["Model"] != System.DBNull.Value)
                                {
                                    model = (string)dr["Model"];
                                }

                                if (dr["Fcode"] != System.DBNull.Value)
                                {
                                    fcode = (string)dr["Fcode"];
                                }                               

                                if (dr["Uom"] != System.DBNull.Value)
                                {
                                    uom = (string)dr["Uom"];
                                }                                

                                if (dr["BarCode"] != System.DBNull.Value)
                                {
                                    barcode = (string)dr["BarCode"];
                                }
                                
                                if (dr["TransDate"] != System.DBNull.Value)
                                {
                                    transdate = (DateTime)dr["TransDate"];
                                }

                                if (dr["qty"] != System.DBNull.Value)
                                {
                                    Qty = (int)dr["qty"];
                                }
                                fgMovements = new FGMovement {
                                     RowNumber = rownumber,
                                     FCode = fcode,
                                     Model = model,
                                     Uom = uom,
                                     BarCode = barcode,
                                     TransDate = transdate,
                                     qty=Qty
                                };

                                list_FGmovement.Add(fgMovements);
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
            
           var list = list_FGmovement.Where(w => array.Contains(w.RowNumber)).ToList();          
            //Thread.Sleep(5000);
            return View(list);            
        }
    }
}