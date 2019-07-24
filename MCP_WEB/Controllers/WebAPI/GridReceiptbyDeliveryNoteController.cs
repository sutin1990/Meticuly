using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MCP_WEB.Data;
using DevExtreme.AspNet.Mvc;
using DevExtreme.AspNet.Data;
using System.Data;
using Newtonsoft.Json;
using Microsoft.EntityFrameworkCore;
using System.Data.SqlClient;
using System.Data.Common;

namespace MCP_WEB.Controllers.WebAPI
{
    //[Route("api/[controller]")]
    //[ApiController]
    [Route("api/[controller]/[action]")]
    public class GridReceiptbyDeliveryNoteController : Controller
    {
        private NittanDBcontext _context;
        public GridReceiptbyDeliveryNoteController(NittanDBcontext context)
        {
            _context = context;
        }
        [HttpGet]
        public object Get(DataSourceLoadOptions loadOptions)
        {
            var Receipt = from rc in _context.VW_MFC_ReceiptbyDeliveryNote.GroupBy(g=>new { g.DeliveryNote,g.StatusDelivery }).Select(g=>g.First())                       
                          select new { rc.DeliveryNote,rc.StatusDelivery };            
            return DataSourceLoader.Load(Receipt, loadOptions);
        }
        [HttpGet]
        public object GetDetails(DataSourceLoadOptions loadOptions,string deliverynote)
        {
            var Receipt = from rc in _context.VW_MFC_ReceiptbyDeliveryNote.GroupBy(g => new { g.DeliveryNote, g.StatusDelivery,g.Model,g.MoveTicket }).Select(g => g.First())
                          where rc.DeliveryNote == deliverynote
                          select new { rc.DeliveryNote,rc.Model, rc.StatusDelivery,rc.MoveTicket,rc.StatusMT,rc.MTQty,rc.BoxQty,rc.ExcessQty,rc.FGQty,rc.FGBoxQty,rc.FGExcessQty };
            return DataSourceLoader.Load(Receipt, loadOptions);
        }

        [HttpPost]
        public object CheckStatusMT(DataSourceLoadOptions loadOptions, string[] deliverynote)
        {            
            var Receipt = from rc in _context.VW_MFC_ReceiptbyDeliveryNote.GroupBy(g => new { g.DeliveryNote, g.StatusDelivery, g.Model, g.MoveTicket }).Select(g => g.First())
                          where deliverynote.Contains(rc.DeliveryNote)  
                          select new { rc.DeliveryNote, rc.Model, rc.StatusDelivery, rc.MoveTicket, rc.StatusMT};
            return DataSourceLoader.Load(Receipt, loadOptions);
        }

        [HttpPost]
        public object Click_ReceivedbyDeliveryNote(DataSourceLoadOptions loadOptions, string[] deliverynote)
        {
            string result = string.Join(",", deliverynote);
            var msgj = "";
            DataTable dt = new DataTable();

            try
            {
                using (var cmd = _context.Database.GetDbConnection().CreateCommand())
                {
                    cmd.Parameters.Clear();
                    cmd.CommandText = "m_sp_ReceiptbyDeliveryNote";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@ListDN", SqlDbType.NVarChar) { Value = result });

                    if (cmd.Connection.State != ConnectionState.Open)
                    {
                        cmd.Connection.Open();
                    }

                    DbDataReader DbReader = cmd.ExecuteReader();

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
        public object Click_ReturnedbyDeliveryNote(DataSourceLoadOptions loadOptions, string[] deliverynote)
        {
            string result = string.Join(",", deliverynote);
            var msgj = "";
            DataTable dt = new DataTable();

            try
            {
                using (var cmd = _context.Database.GetDbConnection().CreateCommand())
                {
                    cmd.Parameters.Clear();
                    cmd.CommandText = "m_sp_ReturnedbyDeliveryNote";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@ListDN", SqlDbType.NVarChar) { Value = result });

                    if (cmd.Connection.State != ConnectionState.Open)
                    {
                        cmd.Connection.Open();
                    }

                    DbDataReader DbReader = cmd.ExecuteReader();

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