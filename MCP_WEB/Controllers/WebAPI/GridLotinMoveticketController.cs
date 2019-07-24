using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using DevExtreme.AspNet.Data;
using DevExtreme.AspNet.Mvc;
using MCP_WEB.Data;
using MCP_WEB.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace MCP_WEB.Controllers.WebAPI
{
    [Route("api/[controller]/[action]")]
    //[ApiController]
    public class GridLotinMoveticketController : ControllerBase
    {
        private NittanDBcontext _context;

        public GridLotinMoveticketController(NittanDBcontext context)
        {
            _context = context;
        }
        [HttpGet]
        public object Get(DataSourceLoadOptions loadOptions)
        {
            var moveticket = (from mv in _context.VW_MFC_MoveTicketVoid.OrderBy(o=>o.moveTicket)
                              
                              select new
                             {
                                  mv.Model,
                                  mv.moveTicket,
                                  mv.BoxType,
                                  mv.TagType,
                                  mv.StatusMT,
                                  mv.NumOfBox,
                                  mv.QtyMT,
                                  mv.QtyExcess,
                                  mv.ItemCode
                             });

            return DataSourceLoader.Load(moveticket, loadOptions);
        }

        [HttpGet]
        public object GetIDmoveticket(DataSourceLoadOptions loadOptions, string IDmoveticket)
        {
            var moveticket = (from mt in _context.MoveTicket
                              join ml in _context.MoveTicket_LOT on mt.moveTicket equals ml.MoveTicket
                              join wp in _context.WeeklyPlan on ml.Barcode equals wp.BarCode
                              where mt.moveTicket == IDmoveticket
                              select new
                              {
                                  wp.Model,
                                  mt.moveTicket,
                                  mt.BoxType,
                                  mt.TagType,
                                  mt.StatusMT,
                                  mt.NumOfBox,
                                  mt.QtyMT,
                                  mt.QtyExcess,
                                  wp.ItemCode
                              }).Distinct();

            return DataSourceLoader.Load(moveticket, loadOptions);
        }

        [HttpGet]
        public object GetDetails(string id, DataSourceLoadOptions loadOptions)
        {
            var m_lot = from ml in _context.MoveTicket_LOT.Where(w => w.MoveTicket == id)
                          select new { ml.BoxNo,ml.Barcode,ml.QtyLot };            
            return DataSourceLoader.Load(m_lot, loadOptions);
        }

        [HttpGet]
        public object GetByModel(DataSourceLoadOptions loadOptions, string model)
        {
            var cal = _context.VW_MFC_PartDescTagSelect_Calculator.Where(w => w.Model == model);
            return DataSourceLoader.Load(cal, loadOptions);
        }

        [HttpGet]
        public object GetPackType(DataSourceLoadOptions loadOptions)
        {            
            var PackingType = _context.m_Package.Where(w => w.PackType == "Tag").ToList();//all           
            return DataSourceLoader.Load(PackingType, loadOptions);
        }

        [HttpGet]
        public object GetPackingID(DataSourceLoadOptions loadOptions)
        {
            var PackingID = _context.m_Package.Where(w => w.PackType == "Box").ToList(); //all
            return DataSourceLoader.Load(PackingID, loadOptions);
        }
        [HttpPost]
        public object UpadateMoveticket(DataSourceLoadOptions loadOptions,string moveticket, string PartDesType)
        {
            var identity = (ClaimsIdentity)User.Identity;
            IEnumerable<Claim> claims = identity.Claims;
            var userLogin = claims.FirstOrDefault();
            var status = "";
            if (moveticket != "" && PartDesType != "")
            {
                var savemoveticket = _context.MoveTicket.FirstOrDefault(m => m.moveTicket == moveticket);
                if (savemoveticket.moveTicket.Count() > 0)
                {
                    savemoveticket.TagType = PartDesType;
                    savemoveticket.TransDate = Convert.ToDateTime(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
                    savemoveticket.ModifyBy = userLogin.Value;
                    _context.MoveTicket.Update(savemoveticket);
                    _context.SaveChanges();
                    status = "1";
                }
                
            }
            else
            {
                status = "0";
            }          
            
            return DataSourceLoader.Load(status, loadOptions);
        }

        public MoveTicketViewModel move;
        [HttpGet]
        public IActionResult VoidMoveticket(DataSourceLoadOptions loadOptions, string moveticket, string Model)
        {
            var identity = (ClaimsIdentity)User.Identity;
            IEnumerable<Claim> claims = identity.Claims;
            var userLogin = claims.FirstOrDefault();
            //var status = "";
            string txtMoveTicket = "";
            string txtDeliveryNote = "";
            string txtBoxType = "";
            string txtTagType = "";
            int txtQtyMT = 0;
            string txtStatusMT = "";
            int txtNumOfBox = 0;
            int txtQtyExcess = 0;
            string txtRemarks = "";
            DateTime txtCreateDate;
            //DateTime txtTransDate;
            //string txtModifyBy = "";
            string txtMoveTicketUserDef1 = "";
            string txtMoveTicketUserDef2 = "";
            string txtMoveTicketUserDef3 = "";
            if (moveticket != "" && Model != "")
            {
                try
                {
                    using (var cmd = _context.Database.GetDbConnection().CreateCommand())
                    {
                        cmd.Parameters.Clear();
                        cmd.CommandText = "m_sp_Void_Moveticket";
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add(new SqlParameter("@model", SqlDbType.NVarChar) { Value = Model });
                        cmd.Parameters.Add(new SqlParameter("@moveticket", SqlDbType.NVarChar) { Value = moveticket });                        
                        cmd.Parameters.Add(new SqlParameter("@ModifyBy", SqlDbType.NVarChar) { Value = userLogin.Value });                        

                        if (cmd.Connection.State != ConnectionState.Open)
                        {
                            cmd.Connection.Open();
                        }
                        DataTable dt = new DataTable();
                        DbDataReader DbReader = cmd.ExecuteReader();
                        if (DbReader.HasRows)
                        {
                            dt.Load(DbReader);
                            foreach (DataRow dr in dt.Select())
                            {
                                if (dr["MoveTicket"] != System.DBNull.Value)
                                {
                                    txtMoveTicket = (string)dr["MoveTicket"];
                                }

                                if (dr["DeliveryNote"] != System.DBNull.Value)
                                {
                                    txtDeliveryNote = (string)dr["DeliveryNote"];
                                }

                                if (dr["BoxType"] != System.DBNull.Value)
                                {
                                    txtBoxType = (string)dr["BoxType"];
                                }

                                if (dr["TagType"] != System.DBNull.Value)
                                {
                                    txtTagType = (string)dr["TagType"];
                                }

                                if (dr["QtyMT"] != System.DBNull.Value)
                                {
                                    txtQtyMT = (int)dr["QtyMT"];
                                }

                                if (dr["StatusMT"] != System.DBNull.Value)
                                {
                                    txtStatusMT = (string)dr["StatusMT"];
                                }

                                if (dr["NumOfBox"] != System.DBNull.Value)
                                {
                                    txtNumOfBox = (int)dr["NumOfBox"];
                                }

                                if (dr["QtyExcess"] != System.DBNull.Value)
                                {
                                    txtQtyExcess = (int)dr["QtyExcess"];
                                }

                                if (dr["Remarks"] != System.DBNull.Value)
                                {
                                    txtRemarks = (string)dr["Remarks"];
                                }

                                if (dr["MoveTicketUserDef1"] != System.DBNull.Value)
                                {
                                    txtMoveTicketUserDef1 = (string)dr["MoveTicketUserDef1"];
                                }

                                if (dr["MoveTicketUserDef2"] != System.DBNull.Value)
                                {
                                    txtMoveTicketUserDef2 = (string)dr["MoveTicketUserDef2"];
                                }

                                if (dr["MoveTicketUserDef3"] != System.DBNull.Value)
                                {
                                    txtMoveTicketUserDef3 = (string)dr["MoveTicketUserDef3"];
                                }

                                //txtTransDate = (DateTime)dr["TransDate"];
                                txtCreateDate = (DateTime)dr["CreateDate"];
                                //txtModifyBy = (string)dr["ModifyBy"];

                                move = new MoveTicketViewModel
                                {
                                    moveTicket = txtMoveTicket,
                                    DeliveryNote = txtDeliveryNote,
                                    BoxType = txtBoxType,
                                    TagType = txtTagType,
                                    QtyMT = txtQtyMT,
                                    StatusMT = txtStatusMT,
                                    NumOfBox = txtNumOfBox,
                                    QtyExcess = txtQtyExcess,
                                    Remarks = txtRemarks,
                                    //TransDate = txtTransDate,
                                    CreateDate = txtCreateDate,
                                    //ModifyBy = txtModifyBy,
                                    MoveTicketUserDef1 = txtMoveTicketUserDef1,
                                    MoveTicketUserDef2 = txtMoveTicketUserDef2,
                                    MoveTicketUserDef3 = txtMoveTicketUserDef3,
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

            }
           
           
            return new JsonResult(move);
        }
    }
}