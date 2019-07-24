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
    public class PrintWipMovementController : Controller
    {
        private NittanDBcontext _context;
        public PrintWipMovementController(NittanDBcontext context)
        {
            _context = context;
        }
       public WipMovement wipMovements;
        public async Task<ActionResult>Index(string fromdate,string todate,string FCode, string Process, string ItemCode, string Model,string selector,bool desc)
        {
            IPHostEntry heserver = Dns.GetHostEntry(Dns.GetHostName());
            var ip = heserver.AddressList[1].ToString();

            ViewBag.selector = null;
            ViewBag.desc = null;
            string desc_or_asc = "";
            if (fromdate == null) { fromdate = "2019-03-1"; }
            if (todate == null) { todate = ""; }
            if (Process == null) { Process = ""; }
            if(FCode == null) { FCode = ""; }
            if (ItemCode == null) { ItemCode = ""; }
            if (Model == null) { Model = ""; }
            if (selector == null && desc_or_asc == null)
            {
                desc_or_asc = "";
                selector = "";
            }
            else
            {
                ViewBag.selector = selector;
                ViewBag.desc = desc;
                if (desc == true) { desc_or_asc = "desc"; } else { desc_or_asc = "asc"; }
            }
            
            
            var p = _context.s_GlobalPams.SingleOrDefault(x => x.parm_key == "DateFormat");

            var PrintWipMovement_data =  _context.Log_Select_Print.Where(w => w.opt == "PrintWipMovement" && w.ipaddress == ip);
                      
                var PrintWipMovement = PrintWipMovement_data.ToList();
                var RowNumbers = PrintWipMovement[0].name.ToString();
                int [] array = RowNumbers.Split(',').Select(n => Convert.ToInt32(n)).ToArray();
            //string [] array_rownumber = new string[] { string.Join(",",array) };
            //pdr =  pdr.Where(w => fcode.Contains(w.FCode)).ToList();
            //var Operatorname = _context.m_BPMaster.FirstOrDefault(f => f.BPCode == "100001");
            var Operatorname = _context.VW_MFC_BPMaster.FirstOrDefault();
            ViewBag.BPName = "";
            ViewBag.Address6 = "";
            if (Operatorname != null)
            {
                ViewBag.BPName = Operatorname.BPName.ToString();
                ViewBag.Address6 = Operatorname.BPAddress6.ToString();
            }           

            ViewBag.GlobalDtFormat = p.param_value.ToString();

            //ViewBag.fromdate = Convert.ToDateTime(fromdate).ToString(p.param_value);
            //ViewBag.todate = Convert.ToDateTime(todate).ToString(p.param_value);
            var list_wipmovement = new List<WipMovement>();
            DataTable dt = new DataTable();
            var msgj = "";
            
            int rownumber=0;
            string itemcode = "";
            string model = "";
            string fcode = "";
            decimal dimension1 = 0;
            string uom = "";
            string processcode = "";
            string refernce = "";
            string barcode = "";
            DateTime trdate = DateTime.Now ;
            int bf = 0;
            int trin = 0;
            int trout = 0;
            string qtymove = "";

            ViewBag.Model = Model;
            ViewBag.FCode = FCode;
            ViewBag.Process = Process;
            try
            {
                using (var cmd = _context.Database.GetDbConnection().CreateCommand())
                {
                    cmd.Parameters.Clear();
                    cmd.CommandText = "m_sp_rpt001_StockcardWIP";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@FromDate", SqlDbType.NVarChar) { Value = fromdate });
                    cmd.Parameters.Add(new SqlParameter("@ToDate", SqlDbType.NVarChar) { Value = todate });
                    cmd.Parameters.Add(new SqlParameter("@ProcessCode", SqlDbType.NVarChar) { Value = Process });
                    cmd.Parameters.Add(new SqlParameter("@FCode", SqlDbType.NVarChar) { Value = FCode });
                    cmd.Parameters.Add(new SqlParameter("@material", SqlDbType.NVarChar) { Value = ItemCode });
                    cmd.Parameters.Add(new SqlParameter("@model", SqlDbType.NVarChar) { Value = Model });
                    cmd.Parameters.Add(new SqlParameter("@orderby", SqlDbType.NVarChar) { Value = "" });
                    cmd.Parameters.Add(new SqlParameter("@desc_or_asc", SqlDbType.NVarChar) { Value = "" });
                    cmd.Parameters.Add(new SqlParameter("@ListRow", SqlDbType.NVarChar) { Value = "" });
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
                            //wipMovements =  dt.Select("RowNumber IN(" + RowNumber + ")").ToList();
                          
                            foreach (DataRow dr in dt.Select())
                            {

                                if (dr["RowNumber"] != System.DBNull.Value)
                                {
                                    rownumber =  Convert.ToInt32(dr["RowNumber"]);
                                }

                                if (dr["ItemCode"] != System.DBNull.Value)
                                {
                                    itemcode = (string)dr["ItemCode"];
                                }

                                if (dr["Model"] != System.DBNull.Value)
                                {
                                    model = (string)dr["Model"];
                                }

                                if (dr["Fcode"] != System.DBNull.Value)
                                {
                                    fcode = (string)dr["Fcode"];
                                }

                                if (dr["Dimension1"] != System.DBNull.Value)
                                {
                                    dimension1 = (decimal)dr["Dimension1"];
                                }

                                if (dr["Uom"] != System.DBNull.Value)
                                {
                                    uom = (string)dr["Uom"];
                                }

                                if (dr["ProcessCode"] != System.DBNull.Value)
                                {
                                    processcode = (string)dr["ProcessCode"];
                                }

                                if (dr["Refernce"] != System.DBNull.Value)
                                {
                                    refernce = (string)dr["Refernce"];
                                }

                                if (dr["Barcode"] != System.DBNull.Value)
                                {
                                    barcode = (string)dr["Barcode"];
                                }
                               

                                if (dr["BF"] != System.DBNull.Value)
                                {
                                    bf = (int)dr["BF"];
                                }

                                if (dr["TRIN"] != System.DBNull.Value)
                                {
                                    trin = (int)dr["TRIN"];
                                }

                                if (dr["TROUT"] != System.DBNull.Value)
                                {
                                    trout = (int)dr["TROUT"];
                                }

                                if (dr["QtyMove"] != System.DBNull.Value)
                                {
                                    qtymove = Convert.ToString(dr["QtyMove"]);
                                }

                                if (dr["Trdate"] != System.DBNull.Value)
                                {
                                    trdate = (DateTime)dr["Trdate"];
                                }
                                wipMovements = new WipMovement {
                                    RowNumber = rownumber,
                                    ItemCode = itemcode,
                                    Model = model,
                                    Fcode = fcode,
                                    Dimension1 = dimension1,
                                    Uom = uom,
                                    ProcessCode = processcode,
                                    Reference = refernce,
                                    Barcode = barcode,
                                    Trdate = trdate,
                                    BF = bf,
                                    TRIN = trin,
                                    TROUT = trout,
                                    QtyMove = Convert.ToInt32(qtymove),
                                };

                                list_wipmovement.Add(wipMovements);
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
            
        var list = list_wipmovement.Where(w => array.Contains(w.RowNumber)).ToList();
            //Thread.Sleep(5000);
            return View(list);            
        }
    }
}