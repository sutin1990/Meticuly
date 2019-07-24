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
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace MCP_WEB.Controllers.WebAPI
{
    [Produces("application/json")]
    [Route("api/[controller]/[action]")]
    //[ApiController]
    public class GirdInspectionMaintenanceController : Controller
    {
        private NittanDBcontext _context;

        public GirdInspectionMaintenanceController(NittanDBcontext context)
        {
            this._context = context;
        }

        [HttpGet]
        public object Get(DataSourceLoadOptions loadOptions)
        {
            try
            {
                var RS = (from a in _context.VW_MFC_ROUTEINSPECT
                         join b in _context.VW_MFC_OSRN on a.U_AbsEntry equals b.AbsEntry
                         //orderby a.U_RouteCode,a.U_MethodDesc,a.U_RptOrder descending
                         //join c in _context.VW_MFC_WOR4 on new { StgEntry = b.AbsEntry, DocEntry = a.U_AbsEntry } equals new { c.StgEntry, c.DocEntry }
                        // where a.U_AbsEntry == Convert.ToInt32(DocEntry) && a.U_RouteCode == Code
                         select new
                         {
                             a.DocEntry,
                             a.U_RouteCode,
                             b.Desc,
                             a.U_AbsEntry,                            
                             a.U_MethodDesc,
                             a.U_PassQty,
                             a.U_RejectQty,
                             a.U_RejectReason,
                             a.U_Comment,
                             a.U_Lastupdateby,
                             a.U_Lastupdatedate,
                             a.U_RptOrder,
                             U_ActiveFlag =  a.U_ActiveFlag == "Y" ? true:false

                         });                
                var result = new
                {
                    items = new[] {
                    new {message = "Success" , status =true, data = RS},
                    }
                };
                return Json(DataSourceLoader.Load(RS, loadOptions));
            }
            catch (Exception ex)
            {
                var result = new
                {
                    items = new[] {
                    new {message = ex.Message , status =false, data = "0"},
                    }
                };
                return Json(result);
            }
        }


        [HttpGet]
        public object Get_OSRN(DataSourceLoadOptions loadOptions)
        {
            try
            {
                var OR = from a in _context.VW_MFC_OSRN
                         select new
                         {
                             a.AbsEntry,
                             a.Code,
                             a.Desc
                         };
                var result = new
                {
                    items = new[] {
                    new {message = "Success" , status =true, data = OR},
                    }
                };
                return Json(DataSourceLoader.Load(OR, loadOptions));
            }
            catch (Exception ex)
            {
                var result = new
                {
                    items = new[] {
                    new {message = ex.Message , status =false, data = "0"},
                    }
                };
                return Json(result);
            }
        }

        [HttpPost]
        public IActionResult UpdateData(string values,string status)
        {
            var identity = (ClaimsIdentity)User.Identity;
            IEnumerable<Claim> claims = identity.Claims;
            var userLogin = claims.FirstOrDefault();
            DataTable dt_result = new DataTable() ;
            dt_result.Columns.Add("SqlStatus", typeof(System.String));
            dt_result.Columns.Add("SqlErrtext", typeof(System.String));
            dt_result.Columns.Add("statuscode", typeof(System.String));
            dt_result.Columns.Add("statusmessage", typeof(System.String));
            dt_result.Columns.Add("DocEntry", typeof(System.String));
            var msg = "";
            var DocEntry = 0;
            var U_AbsEntry ="";
            //var checkcolumn = 
            bool flag = true;
            try
            {                
                DataTable dt = (DataTable)JsonConvert.DeserializeObject(values, (typeof(DataTable)));
                var count = dt.Rows.Count;
                DataColumnCollection columns = dt.Columns;
                
                foreach (DataRow row in dt.Rows)
                {
                    

                    //dt_result = new DataTable();
                    if (columns.Contains("DocEntry") && columns.Contains("U_AbsEntry"))//edit
                    {
                        DocEntry = Convert.ToInt32(row["DocEntry"]);
                        U_AbsEntry = row["U_AbsEntry"].ToString();
                    }
                    else //add
                    {
                        var Code = row["U_RouteCode"];
                        var OSRN = _context.VW_MFC_OSRN.SingleOrDefault(s => s.Code == Code.ToString());
                        U_AbsEntry =  OSRN.AbsEntry.ToString();
                       
                    }                    
                    
                    var U_RouteCode = row["U_RouteCode"];
                    var U_MethodDesc = row["U_MethodDesc"];
                    var U_RptOrder = row["U_RptOrder"];
                    var U_ActiveFlag = "";

                    if (columns.Contains("U_ActiveFlag"))
                    {
                        if (row["U_ActiveFlag"].ToString().ToUpper() == "TRUE")
                        {
                            U_ActiveFlag = "Y";
                        }
                        else { U_ActiveFlag = "N"; }
                    }
                    else
                    {
                        U_ActiveFlag = "N";
                    }
                    
                    //var U_ActiveFlag = row["U_ActiveFlag"].ToString() == "true" ? "Y" : "N" ;
                    var U_Lastupdateby = userLogin.Value;

                    List<VW_MFC_ROUTEINSPECT> newList = _context.VW_MFC_ROUTEINSPECT.Where(m => 
                    m.U_RouteCode == U_RouteCode.ToString() &&
                    m.U_MethodDesc.ToString() == U_MethodDesc.ToString() &&
                    m.U_RptOrder == Convert.ToInt16(U_RptOrder)
                    ).ToList();

                   if(status == "A")
                    {
                        if (newList.Count > 0)
                        {
                            DataRow dr_e = dt_result.NewRow();
                            dr_e["SqlStatus"] = "E";
                            dr_e["SqlErrtext"] = "This data set already exists" + "[" + U_RouteCode.ToString() + "]";
                            dr_e["DocEntry"] = U_RouteCode;
                            dt_result.Rows.Add(dr_e);
                        }
                        else //ถ้าไม่มีข้อมูลก็ให้ insert/edit ได้เลย
                        {
                            using (var cmd = _context.Database.GetDbConnection().CreateCommand())
                            {
                                cmd.Parameters.Clear();
                                cmd.CommandText = "m_sp_MasterData_Manage";
                                cmd.CommandType = CommandType.StoredProcedure;
                                cmd.Parameters.Add(new SqlParameter("@DocEntry", SqlDbType.Int) { Value = DocEntry });
                                cmd.Parameters.Add(new SqlParameter("@U_AbsEntry", SqlDbType.NVarChar) { Value = U_AbsEntry });
                                cmd.Parameters.Add(new SqlParameter("@U_RouteCode", SqlDbType.NVarChar) { Value = U_RouteCode });
                                cmd.Parameters.Add(new SqlParameter("@U_MethodDesc", SqlDbType.NText) { Value = U_MethodDesc });
                                cmd.Parameters.Add(new SqlParameter("@U_RptOrder", SqlDbType.Int) { Value = U_RptOrder });
                                cmd.Parameters.Add(new SqlParameter("@U_ActiveFlag", SqlDbType.NVarChar) { Value = U_ActiveFlag });
                                cmd.Parameters.Add(new SqlParameter("@U_Lastupdateby", SqlDbType.NVarChar) { Value = U_Lastupdateby });
                                cmd.Parameters.Add(new SqlParameter("@TypeProcess", SqlDbType.NVarChar) { Value = status });
                                if (cmd.Connection.State != ConnectionState.Open)
                                {
                                    cmd.Connection.Open();
                                }
                                try
                                {
                                    DbDataReader DbReader = cmd.ExecuteReader();

                                    if (DbReader.HasRows)
                                    {
                                        DataTable dt_sp = new DataTable();
                                        dt_sp.Load(DbReader);
                                        //dt_result = new DataTable();
                                        DataRow dr_s = dt_result.NewRow();
                                        dr_s["SqlStatus"] = "S";
                                        dr_s["SqlErrtext"] = "";
                                        dr_s["statuscode"] = dt_sp.Rows[0]["statuscode"];
                                        dr_s["statusmessage"] = dt_sp.Rows[0]["statusmessage"];
                                        dr_s["DocEntry"] = dt_sp.Rows[0]["DocEntry"];
                                        dt_result.Rows.Add(dr_s);

                                        //foreach (DataRow dr in dt_result.Select())
                                        //{
                                        //    dr["SqlStatus"] = "S";
                                        //    dr["SqlErrtext"] = "";
                                        //    dr["statuscode"] = dt_sp.Rows[0]["statuscode"];
                                        //    dr["statusmessage"] = dt_sp.Rows[0]["statusmessage"];
                                        //    dr["DocEntry"] = dt_sp.Rows[0]["DocEntry"];
                                        //    dt_result.Rows.Add(dr);
                                        //}
                                    }
                                }
                                catch (Exception ex)
                                {
                                    msg = ex.Message;
                                    DataRow dr_e = dt_result.NewRow();
                                    dr_e["SqlStatus"] = "E";
                                    dr_e["SqlErrtext"] = msg;
                                    dr_e["DocEntry"] = U_RouteCode;
                                    dt_result.Rows.Add(dr_e);
                                }
                                cmd.Connection.Close();
                            }
                        }
                    }

                    if (status == "U")
                    {
                        if (newList.Count > 1)
                        {
                            DataRow dr_e = dt_result.NewRow();
                            dr_e["SqlStatus"] = "E";
                            dr_e["SqlErrtext"] = "This data set already exists" + "[" + U_RouteCode.ToString() + "]";
                            dr_e["DocEntry"] = U_RouteCode;
                            dt_result.Rows.Add(dr_e);
                        }
                        else //ถ้าไม่มีข้อมูลก็ให้ insert/edit ได้เลย
                        {
                            using (var cmd = _context.Database.GetDbConnection().CreateCommand())
                            {
                                cmd.Parameters.Clear();
                                cmd.CommandText = "m_sp_MasterData_Manage";
                                cmd.CommandType = CommandType.StoredProcedure;
                                cmd.Parameters.Add(new SqlParameter("@DocEntry", SqlDbType.Int) { Value = DocEntry });
                                cmd.Parameters.Add(new SqlParameter("@U_AbsEntry", SqlDbType.NVarChar) { Value = U_AbsEntry });
                                cmd.Parameters.Add(new SqlParameter("@U_RouteCode", SqlDbType.NVarChar) { Value = U_RouteCode });
                                cmd.Parameters.Add(new SqlParameter("@U_MethodDesc", SqlDbType.NText) { Value = U_MethodDesc });
                                cmd.Parameters.Add(new SqlParameter("@U_RptOrder", SqlDbType.Int) { Value = U_RptOrder });
                                cmd.Parameters.Add(new SqlParameter("@U_ActiveFlag", SqlDbType.NVarChar) { Value = U_ActiveFlag });
                                cmd.Parameters.Add(new SqlParameter("@U_Lastupdateby", SqlDbType.NVarChar) { Value = U_Lastupdateby });
                                cmd.Parameters.Add(new SqlParameter("@TypeProcess", SqlDbType.NVarChar) { Value = status });
                                if (cmd.Connection.State != ConnectionState.Open)
                                {
                                    cmd.Connection.Open();
                                }
                                try
                                {
                                    DbDataReader DbReader = cmd.ExecuteReader();

                                    if (DbReader.HasRows)
                                    {
                                        DataTable dt_sp = new DataTable();
                                        dt_sp.Load(DbReader);
                                        //dt_result = new DataTable();
                                        DataRow dr_s = dt_result.NewRow();
                                        dr_s["SqlStatus"] = "S";
                                        dr_s["SqlErrtext"] = "";
                                        dr_s["statuscode"] = dt_sp.Rows[0]["statuscode"];
                                        dr_s["statusmessage"] = dt_sp.Rows[0]["statusmessage"];
                                        dr_s["DocEntry"] = dt_sp.Rows[0]["DocEntry"];
                                        dt_result.Rows.Add(dr_s);

                                        //foreach (DataRow dr in dt_result.Select())
                                        //{
                                        //    dr["SqlStatus"] = "S";
                                        //    dr["SqlErrtext"] = "";
                                        //    dr["statuscode"] = dt_sp.Rows[0]["statuscode"];
                                        //    dr["statusmessage"] = dt_sp.Rows[0]["statusmessage"];
                                        //    dr["DocEntry"] = dt_sp.Rows[0]["DocEntry"];
                                        //    dt_result.Rows.Add(dr);
                                        //}
                                    }
                                }
                                catch (Exception ex)
                                {
                                    msg = ex.Message;
                                    DataRow dr_e = dt_result.NewRow();
                                    dr_e["SqlStatus"] = "E";
                                    dr_e["SqlErrtext"] = msg;
                                    dr_e["DocEntry"] = U_RouteCode;
                                    dt_result.Rows.Add(dr_e);
                                }
                                cmd.Connection.Close();
                            }
                        }
                    }



                }

               
            }
            catch (SqlException ex)
            {
                msg = ex.Message;
                //dt_result = new DataTable();
                //dt_result.Columns.Add("SqlStatus", typeof(System.String));
                //dt_result.Columns.Add("SqlErrtext", typeof(System.String));
                DataRow dr = dt_result.NewRow();
                dr["SqlStatus"] = "Error";
                dr["SqlErrtext"] = msg;
                dt_result.Rows.Add(dr);
                return Json(dt_result);
                //foreach (DataRow row in dt.Rows)
                //{
                //    row["SqlStatus"] = "Error";
                //    row["SqlErrtext"] = msgj;
                //    dt.Rows.Add(row);
                //}
            }

            return Json(dt_result);           
        }

        [HttpPost]
        public IActionResult DeleteData(string values)
        {
            var identity = (ClaimsIdentity)User.Identity;
            IEnumerable<Claim> claims = identity.Claims;
            var userLogin = claims.FirstOrDefault();
            DataTable dt_result = new DataTable();
            var msg = "";
            var DocEntry = 0;           
            try
            {
                DataTable dt = (DataTable)JsonConvert.DeserializeObject(values, (typeof(DataTable)));                
               
                foreach (DataRow row in dt.Rows)
                {                   
                    DocEntry = Convert.ToInt32(row["DocEntry"]);                  

                    using (var cmd = _context.Database.GetDbConnection().CreateCommand())
                    {
                        cmd.Parameters.Clear();
                        cmd.CommandText = "m_sp_MasterData_DeleteSelect";
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add(new SqlParameter("@DocEntry", SqlDbType.Int) { Value = DocEntry });                        
                        if (cmd.Connection.State != ConnectionState.Open)
                        {
                            cmd.Connection.Open();
                        }

                        DbDataReader DbReader = cmd.ExecuteReader();
                        if (DbReader.HasRows)
                        {                            
                            dt_result.Load(DbReader);                            
                        }
                        cmd.Connection.Close();
                    }
                }

            }
            catch (SqlException ex)
            {
                msg = ex.Message;       
            }

            return Json(dt_result);
        }

        private string GetFullErrorMessage(ModelStateDictionary modelState)
        {
            var messages = new List<string>();

            foreach (var entry in modelState)
            {
                foreach (var error in entry.Value.Errors)
                    messages.Add(error.ErrorMessage);
            }

            return String.Join(" ", messages);
        }

    }
}