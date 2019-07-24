using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MCP_WEB.Data;
using MCP_WEB.Models;
using DevExtreme.AspNet.Data;
using DevExtreme.AspNet.Mvc;
using System.Data;
using Microsoft.EntityFrameworkCore;
using System.Data.SqlClient;
using System.Data.Common;
using Newtonsoft.Json;

namespace MCP_WEB.Controllers.WebAPI
{
    [Route("api/[controller]/[action]")]
    public class DeliverynoteSelectController : ControllerBase
    {
        private NittanDBcontext _context;

        public DeliverynoteSelectController(NittanDBcontext context)
        {
            _context = context;
        }

        [HttpGet]
        public object Get(DataSourceLoadOptions loadOptions)
        {
            //ที่ status Deliverynote ที่เป็น cancel ไม่ออกมาเพราะว่า ใช้ moveticket left join 
            //ไปที่ deliverynote เพราะใน Moveticket จะเก็บเฉพาะ deliverynote ที่ status Deliverynote ไม่เป็น cancel อยู่แล้ว
            var deliverynote = from dn in _context.VW_MFC_Deliverynote_Select
                          select new {dn.DeliveryNote,dn.StatusDelivery,dn.MoveTicket,
                                    dn.StatusMT,dn.FCode,dn.Model,dn.MTQty,dn.BoxQty,dn.ExcessQty };
            
            return DataSourceLoader.Load(deliverynote, loadOptions);
        }

        [HttpGet]
        public object GetMoveticket(DataSourceLoadOptions loadOptions,string deliverynote)
        {
            var MoveTicket = from m in _context.MoveTicket.Where(w=>w.DeliveryNote == deliverynote && w.StatusMT!="Void")
                               select new
                               {
                                   m.moveTicket,
                                   m.StatusMT,
                                   BoxQty = m.NumOfBox ,
                                   MTQty = m.QtyMT ,
                                   ExcessQty = m.QtyExcess 
                               };

            return DataSourceLoader.Load(MoveTicket, loadOptions);
        }

        [HttpGet]
        public object cancelleddeliverynote(string c_deliverynote)
        {            
            var msgj = "";            
            DataTable dt = new DataTable();
            try
            {
                using (var cmd = _context.Database.GetDbConnection().CreateCommand())
                {
                    cmd.Parameters.Clear();
                    cmd.CommandText = "m_sp_CancelledDeliveryNote";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@deliverynote", SqlDbType.NVarChar) { Value = c_deliverynote });                  
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
            return JsonConvert.SerializeObject(dt, Formatting.Indented);           
        }

        [HttpGet]
        public object deletedeliverynote(string d_deliverynote,string d_moveticket)
        {
            var msgj = "";
            DataTable dt = new DataTable();
            try
            {
                using (var cmd = _context.Database.GetDbConnection().CreateCommand())
                {
                    cmd.Parameters.Clear();
                    cmd.CommandText = "m_sp_DeleteDeliveryNote";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@deliverynote", SqlDbType.NVarChar) { Value = d_deliverynote });
                    cmd.Parameters.Add(new SqlParameter("@moveticket", SqlDbType.NVarChar) { Value = d_moveticket });
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
            return JsonConvert.SerializeObject(dt);
        }


        [HttpPost]
        public object createdeliverynote(string[] c_moveticket)
        {
            string result = string.Join(",", c_moveticket);
            var msgj = "";
            DataTable dt = new DataTable();

            //DataTable tvp = new DataTable();
            //tvp.Columns.Add(new DataColumn("MT", typeof(int)));

            //// populate DataTable from your List here
            //foreach (var mt in c_moveticket)
            //{
            //    tvp.Rows.Add(mt);
            //}
            try
            {
                using (var cmd = _context.Database.GetDbConnection().CreateCommand())
                {
                    cmd.Parameters.Clear();
                    cmd.CommandText = "m_sp_CreateDeliveryNote";
                    cmd.CommandType = CommandType.StoredProcedure;                   
                    cmd.Parameters.Add(new SqlParameter("@List", SqlDbType.NVarChar) { Value = result });
                    
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
            return JsonConvert.SerializeObject(dt, Formatting.Indented);
        }
    }
}