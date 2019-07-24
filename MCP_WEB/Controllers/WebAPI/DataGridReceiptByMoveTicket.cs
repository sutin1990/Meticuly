using DevExtreme.AspNet.Data;
using DevExtreme.AspNet.Mvc;
using MCP_WEB.Data;
using MCP_WEB.Helper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Security.Claims;

namespace MCP_WEB.Models.Controllers
{
    [Produces("application/json")]
    [Route("api/[controller]/[action]")]
    public class DataGridReceiptByMoveTicketController : Controller
    {
        private NittanDBcontext _context;

        public DataGridReceiptByMoveTicketController(NittanDBcontext context)
        {
            this._context = context;
        }

        [HttpGet]
        public object Get(DataSourceLoadOptions loadOptions)
        {
            var RecMT = from rc in _context.VW_MFC_ReceiptbyMoveTicket
                            //.GroupBy(g => new { g.DeliveryNote, g.StatusDelivery }).Select(g => g.First())
                        select new { rc.DeliveryNote, rc.Model, rc.StatusDelivery, rc.MoveTicket,
                            rc.StatusMT,
                            MTQty = rc.MTQty == null ?0: rc.MTQty,
                            BoxQty = rc.BoxQty == null ?0: rc.BoxQty,
                            ExcessQty = rc.ExcessQty == null?0:rc.ExcessQty,
                            FGQty = rc.FGQty == null ? 0:rc.FGQty,
                            FGBoxQty = rc.FGBoxQty == null ? 0:rc.FGBoxQty,
                            FGExcessQty = rc.FGExcessQty == null ? 0 : rc.FGExcessQty
                        };
            return DataSourceLoader.Load(RecMT, loadOptions);
        }

        [HttpGet]
        public object Gridpopup_RecbyMT(DataSourceLoadOptions loadOptions, string moveticket)
        {
            var RecMT = from ml in _context.MoveTicket_LOT
                            where ml.MoveTicket==moveticket
                        select new { ml.RECID, ml.BoxNo, ml.Barcode, ml.QtyLot, FGQty = 0, Reason = "", Remark = "" };

            return DataSourceLoader.Load(RecMT, loadOptions);
        }

        [HttpGet]
        public object Getmasterdetail(DataSourceLoadOptions loadOptions,  string moveticket)
        {
            var mt_lot = from ml in _context.VW_MFC_ReceiptbyMoveTicket_Detail                         
                         where ml.MoveTicket == moveticket
                         select new  {ml.BoxNo, ml.Barcode,ml.QtyLot,ml.FGQty, ml.MoveTicketLOTUserDef1,ml.Remarks,ml.StatusLot };
            return DataSourceLoader.Load(mt_lot,loadOptions);
        }

        [HttpGet]
        public object GetReason(DataSourceLoadOptions loadOptions, string moveticket)
        {
            var Reason = from mr in _context.m_Reason
                         select new { mr.ReasonID, mr.DepDesc };
            return DataSourceLoader.Load(Reason, loadOptions);
        }

        [HttpPost]
        public IActionResult SaveData(string deliverynote,string moveticket, string dttable)
        {
            var jsonresult = dttable;
            //var result = JsonConvert.DeserializeObject(jsonresult);
            DataTable dt = new DataTable();
            //JArray CleanJsonObject = JArray.Parse(jsonresult);
            //dynamic data = JObject.Parse(CleanJsonObject[0].ToString());
            var msgj = "";

            try
            {
                using (var cmd = _context.Database.GetDbConnection().CreateCommand())
                {
                    cmd.Parameters.Clear();
                    cmd.CommandText = "m_sp_EditReceiptbyMoveTicket";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@jsonresult", SqlDbType.NVarChar) { Value = jsonresult });
                    cmd.Parameters.Add(new SqlParameter("@moveticket", SqlDbType.Int) { Value = moveticket });
                    cmd.Parameters.Add(new SqlParameter("@DeliveryNote", SqlDbType.NVarChar) { Value = deliverynote });

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
                            dt.Columns.Add("SqlStatus", typeof(System.String));
                            dt.Columns.Add("SqlErrtext", typeof(System.String));
                            foreach (DataRow dr in dt.Select())
                            {
                                dr["SqlStatus"] = "Success";
                                dr["SqlErrtext"] = "";
                            }
                        }
                        else
                        {
                            dt.Columns.Add("SqlStatus", typeof(System.String));
                            dt.Columns.Add("SqlErrtext", typeof(System.String));
                            foreach (DataRow dr in dt.Select())
                            {
                                dr["SqlStatus"] = "ErrorSelectLot";
                                dr["SqlErrtext"] = "";
                            }
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
            //return JsonConvert.SerializeObject(dt, Formatting.Indented);

            return Json(dt);
        }

        [HttpPost]
        public object CheckStatusMT(DataSourceLoadOptions loadOptions, string[] moveticket)
        {
            var Receipt = from rc in _context.VW_MFC_ReceiptbyMoveTicket.GroupBy(g => new { g.DeliveryNote, g.StatusDelivery, g.Model, g.MoveTicket }).Select(g => g.First())
                          where moveticket.Contains(rc.MoveTicket)
                          select new { rc.DeliveryNote, rc.Model, rc.StatusDelivery, rc.MoveTicket, rc.StatusMT };
            return DataSourceLoader.Load(Receipt, loadOptions);
        }

        [HttpPost]
        public object Click_ReceiptbyMoveTicket(DataSourceLoadOptions loadOptions, string[] moveticket)
        {
            string result = string.Join(",", moveticket);
            var msgj = "";
            DataTable dt = new DataTable();

            try
            {
                using (var cmd = _context.Database.GetDbConnection().CreateCommand())
                {
                    cmd.Parameters.Clear();
                    cmd.CommandText = "m_sp_ReceiptbyMoveTicket";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@ListMT", SqlDbType.NVarChar) { Value = result });

                    if (cmd.Connection.State != ConnectionState.Open)
                    {
                        cmd.Connection.Open();
                    }

                    var DbReader = cmd.ExecuteReader();

                    if (DbReader.HasRows)
                    {
                        dt.Load(DbReader);
                        dt.Columns.Add("SqlStatus", typeof(System.String));
                        dt.Columns.Add("SqlErrtext", typeof(System.String));
                        foreach (DataRow dr in dt.Select())
                        {
                            dr["SqlStatus"] = "Success";
                            dr["SqlErrtext"] = "";
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
            return Json(dt);
        }

        [HttpPost]
        public object Click_ReturnedbyMoveTicket(DataSourceLoadOptions loadOptions, string[] moveticket)
        {
            string result = string.Join(",", moveticket);
            var msgj = "";
            DataTable dt = new DataTable();

            try
            {
                using (var cmd = _context.Database.GetDbConnection().CreateCommand())
                {
                    cmd.Parameters.Clear();
                    cmd.CommandText = "m_sp_ReturnedbyMoveTicket";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@ListMT", SqlDbType.NVarChar) { Value = result });

                    if (cmd.Connection.State != ConnectionState.Open)
                    {
                        cmd.Connection.Open();
                    }

                    var DbReader = cmd.ExecuteReader();

                    if (DbReader.HasRows)
                    {
                        dt.Load(DbReader);
                        dt.Columns.Add("SqlStatus", typeof(System.String));
                        dt.Columns.Add("SqlErrtext", typeof(System.String));
                        foreach (DataRow dr in dt.Select())
                        {
                            dr["SqlStatus"] = "Success";
                            dr["SqlErrtext"] = "";
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
            return Json(dt);
        }

    }
}