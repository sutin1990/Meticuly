using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using MCP_WEB.Data;
using MCP_WEB.Models;
using DevExtreme.AspNet.Mvc;
using DevExtreme.AspNet.Data;
using Microsoft.EntityFrameworkCore;
using System.Data;
using System.Data.SqlClient;
using System.Data.Common;
using Newtonsoft.Json;
using System.Security.Claims;

namespace MCP_WEB.Controllers.WebAPI{
    public class PartDescTagSelectController : Controller{
        private NittanDBcontext _context;
        public PartDescTagSelectController(NittanDBcontext context) {
            this._context = context;
        }
        
        [HttpGet]
        public IActionResult Get(DataSourceLoadOptions loadOptions) {            
            var pdt = (from vwp in _context.PartDescTagSelect
                       select new
                       {
                           vwp.Model,
                           vwp.SerailNoofBarcode,
                           vwp.PackingID,
                           vwp.PasrtDesType,
                           vwp.OrderQty,
                           vwp.WIPQTY,
                           vwp.QtyPacked,
                           vwp.QTYLeft
                       });
            return Json(DataSourceLoader.Load(pdt, loadOptions));
        }

        [HttpGet]
        public IActionResult GetByModel(DataSourceLoadOptions loadOptions,string model)
        {           
            var cal = _context.VW_MFC_PartDescTagSelect_Calculator.Where(w => w.Model == model);  
            return Json(DataSourceLoader.Load(cal, loadOptions));
        }

        [HttpGet]
        public IActionResult GetPackingID(DataSourceLoadOptions loadOptions,string PackID)
        {
             List<m_Package> PackingID = new List<m_Package>();
            if (PackID!=null)
            {
                 PackingID = _context.m_Package.Where(w => w.PackType == "Box" && w.PackID == PackID).ToList();//one row
            }
            else
            {
                 PackingID = _context.m_Package.Where(w => w.PackType == "Box").ToList(); //all
            }
           
            return Json(DataSourceLoader.Load(PackingID, loadOptions));
        }

        [HttpGet]
        public IActionResult GetPackType(DataSourceLoadOptions loadOptions)
        {            
            //_context.Database.ExecuteSqlCommand("m_sp_Model_Moveticket @model,@BoxType,@TagType,@QtyMT,@packQty,@NumOfBox,@QtyExcess", parameters: new[] { "KPPA INT", "B2", "T1", "850", "200", "5", "50" });
            //var userType = _context.Set().FromSql("dbo.SomeSproc @Id = {0}, @Name = {1}", 45, "Ada");
            var PackingType = _context.m_Package.Where(w => w.PackType == "Tag").ToList();//all           
            return Json(DataSourceLoader.Load(PackingType, loadOptions));
        }

        [HttpGet]
        public object CalculateNumBox(string model, int packQty,int sumwip)
        {           
            var identity = (ClaimsIdentity)User.Identity;
            IEnumerable<Claim> claims = identity.Claims;
            var userID = claims.FirstOrDefault();
            var msgj = "";
            List<Dictionary<string, object>> rows = new List<Dictionary<string, object>>();
            DataTable dt = new DataTable();
            
            try
            {
                using (var cmd = _context.Database.GetDbConnection().CreateCommand())
                {
                    cmd.Parameters.Clear();
                    cmd.CommandText = "m_sp_CalculateNumBox";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@Model", SqlDbType.NVarChar) { Value = model });
                    cmd.Parameters.Add(new SqlParameter("@PackQty", SqlDbType.Int) { Value = packQty });
                    cmd.Parameters.Add(new SqlParameter("@summarywip", SqlDbType.Int) { Value = sumwip });
                    cmd.Parameters.Add(new SqlParameter("@Operlation", SqlDbType.NVarChar) { Value = "getbox" });
                    cmd.Parameters.Add(new SqlParameter("@userID", SqlDbType.NVarChar) { Value = userID.Value });
                    if (cmd.Connection.State != ConnectionState.Open)
                    {
                        cmd.Connection.Open();
                    }
                    
                    DbDataReader DbReader = cmd.ExecuteReader();
                    if (DbReader.HasRows)
                    {
                        dt.Columns.Add("SqlStatus", typeof(System.String));
                        dt.Columns.Add("SqlErrtext", typeof(System.String));
                        DataRow dr = dt.NewRow();
                        dr["SqlStatus"] = "Seccess";
                        dr["SqlErrtext"] = "";
                        dt.Rows.Add(dr);
                        dt.Load(DbReader);
                       
                        //foreach (DataRow row in dt.Rows)
                        //{
                        //    row["SqlStatus"] = "Seccess";
                        //    row["SqlErrtext"] = "";
                        //    dt.Rows.Add(row);
                        //}
                    }
                    cmd.Connection.Close();
                }
            }
            catch (SqlException ex)
            {
                msgj = ex.Message;
                dt.Columns.Add("SqlStatus", typeof(System.String));
                dt.Columns.Add("SqlErrtext", typeof(System.String));
                DataRow dr = dt.NewRow();
                dr["SqlStatus"] = "Error";
                dr["SqlErrtext"] = msgj;
                dt.Rows.Add(dr);
                //foreach (DataRow row in dt.Rows)
                //{
                //    row["SqlStatus"] = "Error";
                //    row["SqlErrtext"] = msgj;
                //    dt.Rows.Add(row);
                //}
            }

            return JsonConvert.SerializeObject(dt, Formatting.Indented);
        }

        public MoveTicketViewModel move;
        [HttpPost]
        public IActionResult CallStore(string model,string BoxType,string TagType,int QtyMT,int packQty,int NumOfBox,int QtyExcess)
        {
            var identity = (ClaimsIdentity)User.Identity;
            IEnumerable<Claim> claims = identity.Claims;
            var userID = claims.FirstOrDefault();
            string txtMoveTicket = "";
            string txtBoxNo = "";
            string txtBarcode =  "";
            int txtQtyLot = 0;
            string txtStatusLot = "";
            DateTime txtCreateDate;
            if (TagType== null) { TagType = "NULL"; }
            try
            {
                using (var cmd = _context.Database.GetDbConnection().CreateCommand())
                {
                    cmd.Parameters.Clear();
                    cmd.CommandText = "m_sp_InsertMoveticket";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@model", SqlDbType.NVarChar) { Value = model });
                    cmd.Parameters.Add(new SqlParameter("@BoxType", SqlDbType.NVarChar) { Value = BoxType });
                    cmd.Parameters.Add(new SqlParameter("@TagType", SqlDbType.NVarChar) { Value = TagType });
                    cmd.Parameters.Add(new SqlParameter("@packQty", SqlDbType.Int) { Value = packQty });
                    cmd.Parameters.Add(new SqlParameter("@QtyMT", SqlDbType.Int) { Value = QtyMT });
                    cmd.Parameters.Add(new SqlParameter("@NumOfBox", SqlDbType.Int) { Value = NumOfBox });
                    cmd.Parameters.Add(new SqlParameter("@QtyExcess", SqlDbType.Int) { Value = QtyExcess });
                    cmd.Parameters.Add(new SqlParameter("@userID", SqlDbType.NVarChar) { Value = userID.Value });

                    if (cmd.Connection.State != ConnectionState.Open)
                    {
                        cmd.Connection.Open();
                    }
                    DataTable dt = new DataTable();
                    //dt.Columns.Add("SqlStatus", typeof(System.String));
                    //dt.Columns.Add("SqlErrtext", typeof(System.String));
                    DbDataReader DbReader = cmd.ExecuteReader();
                    //foreach (DataRow row in dt.Rows)
                    //{                        
                    //    row["SqlStatus"] = ""; 
                    //    row["SqlErrtext"] = ""; 
                    //}
                    if (DbReader.HasRows)
                    {
                        dt.Load(DbReader);
                        foreach (DataRow dr in dt.Select())
                        {                           
                           var wp = _context.WeeklyPlan.FirstOrDefault(w => w.BarCode == (string)dr["Barcode"]);
                            wp.QtyPacked = wp.QtyPacked+(int)dr["QtyLot"];
                            wp.UpdateDate = (DateTime)dr["CreateDate"];
                            _context.WeeklyPlan.Update(wp);
                            _context.SaveChanges();
                            if (dr["MoveTicket"] != System.DBNull.Value)
                            {
                                txtMoveTicket = (string)dr["MoveTicket"];
                            }

                            if (dr["BoxNo"] != System.DBNull.Value)
                            {
                               txtBoxNo = (string)dr["BoxNo"];
                            }

                            if (dr["Barcode"] != System.DBNull.Value)
                            {
                               txtBarcode = (string)dr["Barcode"];
                            }

                            if (dr["QtyLot"] != System.DBNull.Value)
                            {
                                txtQtyLot = (int)dr["QtyLot"];
                            }

                            if (dr["StatusLot"] != System.DBNull.Value)
                            {
                                txtStatusLot = (string)dr["StatusLot"];
                            }

                            //if (dr["CreateDate"] != System.DBNull.Value)
                            //{
                            //    txtStatusMT = (DateTime)dr["CreateDate"];
                            //}


                            ////txtTransDate = (DateTime)dr["TransDate"];
                            txtCreateDate = (DateTime)dr["CreateDate"];
                            //move.SqlStatus = "Success";
                            //move.SqlErrtext = "";
                            //txtModifyBy = (string)dr["ModifyBy"];

                            move = new MoveTicketViewModel
                            {
                                moveTicket = txtMoveTicket,
                                BoxNo= txtBoxNo,
                                Barcode=txtBarcode,
                                QtyLot = txtQtyLot,
                                StatusLot = txtStatusLot,
                                CreateDate = txtCreateDate,
                                //ModifyBy = txtModifyBy,                               
                                SqlStatus = "Success",
                                SqlErrtext = ""
                            };

                        }
                    }

                    cmd.Connection.Close();
                }
               
            }
            catch (SqlException ex)
            {
                move = new MoveTicketViewModel
                {
                    SqlStatus = "Error",
                    SqlErrtext = ex.Message
                };
            }
                return new JsonResult(move);

                // _context.Database.ExecuteSqlCommand("dbo.m_sp_Model_Moveticket @model,@BoxType,@TagType,@QtyMT,@packQty,@NumOfBox,@QtyExcess", parameters: new[] { "KPPA INT", "B2", "T1", "850", "200", "5", "50" });

        }


    }
}