using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using MCP_WEB.Data;
using MCP_WEB.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace MCP_WEB.Controllers.FrontEnd
{
    [Authorize]
    public class PDR2Controller : Controller
    {
        private NittanDBcontext _context;
        public PDR2Controller(NittanDBcontext context)
        {
            this._context = context;
        }
        public VW_MFC_ProductionDailyReport2 pdr2 = new VW_MFC_ProductionDailyReport2();
        public IActionResult Index(string fromdate, string todate, string[] itemprocess,string[] FCode)
        {
            var p = _context.s_GlobalPams.SingleOrDefault(x => x.parm_key == "DateFormat");
            ViewBag.GlobalDtFormat = p.param_value; // "dd-MM-YYYY";
            string split_fcode = string.Join(",", FCode);
            string split_process = string.Join(",", itemprocess);
            var MachineCode = 13;
            var S_TransDate = Convert.ToDateTime(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
            var E_TransDate = Convert.ToDateTime(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
            ViewBag.fromdate = fromdate;
            ViewBag.todate = todate;
            var list = new List<VW_MFC_ProductionDailyReport2>();
            string fcode = "";
            string model = "";
            string ProcessCode = "";
            int MC1 = 0;
            int MC2 = 0;
            int MC3 = 0;
            int MC4 = 0;
            int MC5 = 0;
            int MC6 = 0;
            int MC7 = 0;
            int MC8 = 0;
            int MC9 = 0;
            int MC10 = 0;
            int MC11 = 0;
            int MC12 = 0;
            int MC13 = 0;
            int MC14 = 0;
            int MC15 = 0;
            int MC16 = 0;
            int MC17 = 0;
            int MC18 = 0;
            int MC19 = 0;
            int MC20 = 0;
            //var jsonresult = dttable;
            //var result = JsonConvert.DeserializeObject(jsonresult);
            DataTable dt = new DataTable();
            //JArray CleanJsonObject = JArray.Parse(jsonresult);
            //dynamic data = JObject.Parse(CleanJsonObject[0].ToString());
            var msgj = "";
            DataTable schema = null;
            try
            {
                using (var cmd = _context.Database.GetDbConnection().CreateCommand())
                {
                    cmd.Parameters.Clear();
                    cmd.CommandText = "m_sp_ProductionDailyReport2_Print";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@S_TransDate", SqlDbType.NVarChar) { Value = fromdate });
                    cmd.Parameters.Add(new SqlParameter("@E_TransDate ", SqlDbType.NVarChar) { Value = todate });
                    cmd.Parameters.Add(new SqlParameter("@IdProcess", SqlDbType.NVarChar) { Value = split_process });
                    cmd.Parameters.Add(new SqlParameter("@FCode", SqlDbType.NVarChar) { Value = split_fcode });

                    if (cmd.Connection.State != ConnectionState.Open)
                    {
                        cmd.Connection.Open();
                    }

                    var DbReader = cmd.ExecuteReader();
                    schema = DbReader.GetSchemaTable();

                    if (DbReader.HasRows)
                    {
                        dt.Load(DbReader);

                        var columnMC = (from dc in dt.Columns.Cast<DataColumn>()
                                            where dc.ColumnName.Contains("MC") == true
                                            orderby Convert.ToInt32(dc.ColumnName.ToString().Substring(2))
                                            select dc.ColumnName).ToArray();
                        //if (dt.Columns.Count > 1)
                        //{
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
                            if (dr["FCode"] != System.DBNull.Value)
                            {
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

                            //for(var x =0;x< columnMC.Length; x++)
                            //{
                            //    if (columnMC[x].ToString() == "MC1")
                            //    {
                            //        MC1 = Convert.ToInt32(dr["MC1"]);
                            //    }

                            //    if (columnMC[x].ToString() == "MC2")
                            //    {
                            //        MC2 = Convert.ToInt32(dr["MC2"]);
                            //    }

                            //    if (columnMC[x].ToString() == "MC3")
                            //    {
                            //        MC3 = Convert.ToInt32(dr["MC3"]);
                            //    }

                            //    if (columnMC[x].ToString() == "MC4")
                            //    {
                            //        MC4 = Convert.ToInt32(dr["MC4"]);
                            //    }

                            //    if (columnMC[x].ToString() == "MC5")
                            //    {
                            //        MC5 = Convert.ToInt32(dr["MC5"]);
                            //    }

                            //    if (columnMC[x].ToString() == "MC6")
                            //    {
                            //        MC6 = Convert.ToInt32(dr["MC6"]);
                            //    }
                            //}
                            
                            if (dr.Table.Columns.Contains("MC1"))
                            {
                                if (dr["MC1"] != System.DBNull.Value)
                                {
                                    MC1 = Convert.ToInt32(dr["MC1"]);
                                }
                            }

                            if (dr.Table.Columns.Contains("MC2"))
                            {
                                if (dr["MC2"] != System.DBNull.Value)
                                {
                                    MC2 = Convert.ToInt32(dr["MC2"]);
                                }
                            }

                            if (dr.Table.Columns.Contains("MC3"))
                            {
                                if (dr["MC3"] != System.DBNull.Value)
                                {
                                    MC3 = Convert.ToInt32(dr["MC3"]);
                                }
                            }

                            if (dr.Table.Columns.Contains("MC4"))
                            {
                                if (dr["MC4"] != System.DBNull.Value)
                                {
                                    MC4 = Convert.ToInt32(dr["MC4"]);
                                }
                            }

                            if (dr.Table.Columns.Contains("MC5"))
                            {
                                if (dr["MC5"] != System.DBNull.Value)
                                {
                                    MC5 = Convert.ToInt32(dr["MC5"]);
                                }
                            }

                            if (dr.Table.Columns.Contains("MC6"))
                            {
                                if (dr["MC6"] != System.DBNull.Value)
                                {
                                    MC6 = Convert.ToInt32(dr["MC6"]);
                                }
                            }

                            if (dr.Table.Columns.Contains("MC7"))
                            {
                                if (dr["MC7"] != System.DBNull.Value)
                                {
                                    MC7 = Convert.ToInt32(dr["MC7"]);
                                }
                            }

                            if (dr.Table.Columns.Contains("MC8"))
                            {
                                if (dr["MC8"] != System.DBNull.Value)
                                {
                                    MC8 = Convert.ToInt32(dr["MC8"]);
                                }
                            }

                            if (dr.Table.Columns.Contains("MC9"))
                            {
                                if (dr["MC9"] != System.DBNull.Value)
                                {
                                    MC9 = Convert.ToInt32(dr["MC9"]);
                                }
                            }

                            if (dr.Table.Columns.Contains("MC10"))
                            {
                                if (dr["MC10"] != System.DBNull.Value)
                                {
                                    MC10 = Convert.ToInt32(dr["MC10"]);
                                }
                            }

                            if (dr.Table.Columns.Contains("MC11"))
                            {
                                if (dr["MC11"] != System.DBNull.Value)
                                {
                                    MC11 = Convert.ToInt32(dr["MC11"]);
                                }
                            }

                            if (dr.Table.Columns.Contains("MC12"))
                            {
                                if (dr["MC12"] != System.DBNull.Value)
                                {
                                    MC12 = Convert.ToInt32(dr["MC12"]);
                                }
                            }

                            if (dr.Table.Columns.Contains("MC13"))
                            {
                                if (dr["MC13"] != System.DBNull.Value)
                                {
                                    MC13 = Convert.ToInt32(dr["MC13"]);
                                }
                            }
                            if (dr.Table.Columns.Contains("MC14"))
                            {
                               if (dr["MC14"] != System.DBNull.Value)
                                {
                                    MC14 = Convert.ToInt32(dr["MC14"]);
                                }
                            }

                            if (dr.Table.Columns.Contains("MC15"))
                            {
                                if (dr["MC15"] != System.DBNull.Value)
                                {
                                    MC15 = Convert.ToInt32(dr["MC15"]);
                                }
                            }

                            if (dr.Table.Columns.Contains("MC16"))
                            {
                                if (dr["MC16"] != System.DBNull.Value)
                                {
                                    MC16 = Convert.ToInt32(dr["MC16"]);
                                }
                            }

                            if (dr.Table.Columns.Contains("MC17"))
                            {
                                if (dr["MC17"] != System.DBNull.Value)
                                {
                                    MC17 = Convert.ToInt32(dr["MC17"]);
                                }
                            }

                            if (dr.Table.Columns.Contains("MC18"))
                            {
                                if (dr["MC18"] != System.DBNull.Value)
                                {
                                    MC18 = Convert.ToInt32(dr["MC18"]);
                                }
                            }

                            if (dr.Table.Columns.Contains("MC19"))
                            {
                                if (dr["MC19"] != System.DBNull.Value)
                                {
                                    MC19 = Convert.ToInt32(dr["MC19"]);
                                }
                            }

                            if (dr.Table.Columns.Contains("MC20"))
                            {
                                if (dr["MC20"] != System.DBNull.Value)
                                {
                                    MC20 = Convert.ToInt32(dr["MC20"]);
                                }
                            }

                            pdr2 = new VW_MFC_ProductionDailyReport2
                                {
                                    FCode = fcode,
                                    Model = model,
                                    ProcessCode = ProcessCode,
                                    MC1=MC1,
                                    MC2=MC2,
                                    MC3=MC3,
                                    MC4=MC4,
                                    MC5=MC5,
                                    MC6=MC6,
                                    MC7=MC7,
                                    MC8=MC8,
                                    MC9=MC9,
                                    MC10=MC10,
                                    MC11=MC11,
                                    MC12=MC12,
                                    MC13=MC13,
                                    MC14=MC14,
                                    MC15=MC15,
                                    MC16=MC16,
                                    MC17=MC17,
                                    MC18=MC18,
                                    MC19=MC19,
                                    MC20=MC20
                                    

                                };
                            //for (var i = 0; i < columnMC.Length; i++)
                            //    {
                            //        if (dr[columnMC[i]] != System.DBNull.Value)
                            //        {
                            //            foreach(var property in pdr2.GetType().GetProperties())
                            //            {
                            //                if(property.Name == columnMC[i])
                            //                {
                            //                pdr2 = new VW_MFC_ProductionDailyReport2
                            //                {};
                            //                }
                            //            }
                            //        }
                            //    }

                            list.Add(pdr2);
                            
                        }

                        //foreach (DataRow col in dt.Rows)
                        //{

                        //ViewBag.column = col;
                        //foreach (DataColumn column in dt.Columns)
                        //{
                        //    ViewBag.column = column.ColumnName;
                        //}
                        //}


                        //}
                        //dt.Columns.Add("SqlStatus", typeof(System.String));
                        //dt.Columns.Add("SqlErrtext", typeof(System.String));
                        //foreach (DataRow dr in dt.Select())
                        //{
                        //    dr["SqlStatus"] = "Success";
                        //    dr["SqlErrtext"] = "";
                        //}
                        //}
                        //else
                        //{
                        //dt.Columns.Add("SqlStatus", typeof(System.String));
                        //dt.Columns.Add("SqlErrtext", typeof(System.String));
                        //foreach (DataRow dr in dt.Select())
                        //{
                        //    dr["SqlStatus"] = "ErrorSelectLot";
                        //    dr["SqlErrtext"] = "";
                        //}
                        //}
                        ViewBag.columnMC = columnMC;

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
            ViewBag.columnProcess = itemprocess;

            
            //var data = new            
            //{
            //    items = list.Select(item => new
            //    {
            //        FCode = item.FCode,
            //        Model = item.Model,
            //        ProcessCode = item.ProcessCode
            //    })
            //};
            return View(list);
            //return View(Json(dt));
        }
    }
}