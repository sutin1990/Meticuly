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
    //[Produces("application/json")]
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
        public IActionResult Get(DataSourceLoadOptions loadOptions)
        {
            List<VW_MFC_ROUTEINSPECT> VW_MFC_ROUTEINSPECT = new List<VW_MFC_ROUTEINSPECT>();
            try
            {
                var IR = _context.VW_MFC_M_INSPECT_RESULT.Select(s=>s.U_InspectEntry).ToList();

                var RS = from a in _context.VW_MFC_ROUTEINSPECT                          
                          join b in _context.VW_MFC_OSRN on a.U_AbsEntry equals b.AbsEntry
                         //orderby a.U_RouteCode, a.U_RptOrder descending
                         //join c in _context.VW_MFC_WOR4 on new { StgEntry = b.AbsEntry, DocEntry = a.U_AbsEntry } equals new { c.StgEntry, c.DocEntry }
                         // where a.U_AbsEntry == Convert.ToInt32(DocEntry) && a.U_RouteCode == Code
                         select new
                         {
                             a.DocEntry,
                             a.Code,
                             a.U_RouteCode,
                             b.Desc,
                             a.U_AbsEntry,                            
                             a.U_MethodDesc,
                             //a.U_PassQty,
                             //a.U_RejectQty,
                             //a.U_RejectReason,
                             //a.U_Comment,
                             a.U_Lastupdateby,
                             a.U_Lastupdatedate,
                             a.U_RptOrder,
                             U_ActiveFlag =  a.U_ActiveFlag == "Y" ? true:false,
                             USE = IR.Contains(Convert.ToInt32(a.Code))

                         };

                var result = new
                {
                    items = new[] {
                    new {message = "success" , statuserror =false, data = RS},
                    }
                };
                //return Json(result);
                return Json(DataSourceLoader.Load(RS,loadOptions));
            }
            catch (Exception ex)
            {
                var result = new
                {
                    items = new[] {
                    new {message = ex.Message , statuserror =true, data ="0" },
                    }
                };
                //return Json(result);
                return Json(DataSourceLoader.Load(VW_MFC_ROUTEINSPECT, loadOptions));
            }
        }


        [HttpGet]
        public IActionResult Get_OSRN(DataSourceLoadOptions loadOptions)
        {
            List<VW_MFC_OSRN> VW_MFC_OSRN = new List<VW_MFC_OSRN>();
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
                return Json(OR);
            }
            catch (Exception ex)
            {
                var result = new
                {
                    items = new[] {
                    new {message = ex.Message , status =false, data = "0"},
                    }
                };
                return Json(VW_MFC_OSRN);
            }
        }

        [HttpPost]
        public IActionResult UpdateData(string values,string status)
        {
            var identity = (ClaimsIdentity)User.Identity;
            
            string lastChanged;
            lastChanged = (from c in identity.Claims
                           where c.Type == "ContactName"
                           select c.Value).FirstOrDefault();

            if (string.IsNullOrEmpty(lastChanged))
            {
                return Json("UserExprie");
            }
            else
            {
                IEnumerable<Claim> claims = identity.Claims;
                var userLogin = claims.FirstOrDefault();
                DataTable dt_result = new DataTable();
                dt_result.Columns.Add("SqlStatus", typeof(System.String));
                dt_result.Columns.Add("SqlErrtext", typeof(System.String));
                dt_result.Columns.Add("statuscode", typeof(System.String));
                dt_result.Columns.Add("statusmessage", typeof(System.String));
                dt_result.Columns.Add("DocEntry", typeof(System.String));
                var msg = "";
                var DocEntry = 0;
                var U_AbsEntry = "";
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
                            U_AbsEntry = OSRN.AbsEntry.ToString();

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
                        m.U_RouteCode.ToString().ToUpper() == U_RouteCode.ToString().ToUpper() &&
                        m.U_MethodDesc.ToString().ToUpper() == U_MethodDesc.ToString().ToUpper() &&
                        m.U_RptOrder == Convert.ToInt16(U_RptOrder)
                        ).ToList();

                        if (status == "A")
                        {
                            if (newList.Count > 0)
                            {
                                DataRow dr_e = dt_result.NewRow();
                                dr_e["SqlStatus"] = "E";
                                dr_e["SqlErrtext"] = "This entry already exists.";
                                dr_e["DocEntry"] = U_RouteCode;
                                dt_result.Rows.Add(dr_e);
                                return Json(dt_result);
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
                            var checkhave = _context.VW_MFC_ROUTEINSPECT.FirstOrDefault(f => f.DocEntry == DocEntry);

                            if (checkhave.U_RouteCode.ToString().ToUpper() == U_RouteCode.ToString().ToUpper() &&
                                checkhave.U_MethodDesc.ToString().ToUpper() == U_MethodDesc.ToString().ToUpper() &&
                                checkhave.U_RptOrder == Convert.ToInt16(U_RptOrder))// ถ้ามันเป็นค่าเดิมก็ให้อัพเดดเวลาและuserไปปกติ
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
                            else // แต่ถ้ามันไม่เท่ากับค่าเดิม ก็ให้ไปเช็คว่า มีข้อมูล record U_RouteCode,U_MethodDesc,U_RptOrder เหมือนกันกับที่ใส่มาไหมถ้า newList มีค่ามากว่า 0 แสดงว่า ซํ้า
                            {
                                if (newList.Count > 0)// ข้อมูลซํ้า
                                {
                                    DataRow dr_e = dt_result.NewRow();
                                    dr_e["SqlStatus"] = "E";
                                    dr_e["SqlErrtext"] = "This entry already exists.";
                                    dr_e["DocEntry"] = U_RouteCode;
                                    dt_result.Rows.Add(dr_e);
                                    return Json(dt_result);
                                }
                                else //ถ้าไม่ซํ้าก็ให้ edit ได้เลย
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

                    
        }

        [HttpPost]
        public IActionResult DeleteData(string values,int Code)
        {
            var identity = (ClaimsIdentity)User.Identity;
            IEnumerable<Claim> claims = identity.Claims;
            var userLogin = claims.FirstOrDefault();
            DataTable dt_result = new DataTable();            
            dt_result.Columns.Add("SqlStatus", typeof(System.String));
            dt_result.Columns.Add("SqlErrtext", typeof(System.String));
            dt_result.Columns.Add("statuscode", typeof(System.String));
            dt_result.Columns.Add("statusmessage", typeof(System.String));
            dt_result.Columns.Add("Code", typeof(System.Int32));
            var msg = "";
            //var Code = 0;           
            try
            {
                //DataTable dt = (DataTable)JsonConvert.DeserializeObject(values, (typeof(DataTable)));                

                //foreach (DataRow row in dt.Rows)
                //{                   
                //Code = Convert.ToInt32(row["Code"]);       
                var check_use = _context.VW_MFC_M_INSPECT_RESULT.Where(w => w.U_InspectEntry == Code).ToList();
                if (check_use.Count > 0)
                {
                    DataRow dr_e = dt_result.NewRow();
                    dr_e["SqlStatus"] = "E";
                    dr_e["SqlErrtext"] = "There is an use of this Inspection Method";
                    dr_e["Code"] = Code;
                    dt_result.Rows.Add(dr_e);
                }
                else
                {
                    using (var cmd = _context.Database.GetDbConnection().CreateCommand())
                    {
                        cmd.Parameters.Clear();
                        cmd.CommandText = "m_sp_MasterData_DeleteSelect";
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add(new SqlParameter("@Code", SqlDbType.Int) { Value = Code });
                        if (cmd.Connection.State != ConnectionState.Open)
                        {
                            cmd.Connection.Open();
                        }

                        DbDataReader DbReader = cmd.ExecuteReader();
                        if (DbReader.HasRows)
                        {
                            DataTable dt_sp = new DataTable();
                            dt_sp.Load(DbReader);
                            DataRow dr_s = dt_result.NewRow();
                            dr_s["SqlStatus"] = "S";
                            dr_s["SqlErrtext"] = "";
                            dr_s["statuscode"] = dt_sp.Rows[0]["statuscode"];
                            dr_s["statusmessage"] = dt_sp.Rows[0]["statusmessage"];
                            dr_s["Code"] = dt_sp.Rows[0]["Code"];
                            dt_result.Rows.Add(dr_s);
                        }
                        cmd.Connection.Close();
                    }
                }
                    
                //}

            }
            catch (SqlException ex)
            {
                msg = ex.Message;
                DataRow dr_e = dt_result.NewRow();
                dr_e["SqlStatus"] = "E";
                dr_e["SqlErrtext"] = msg;
                dr_e["Code"] = Code;
                dt_result.Rows.Add(dr_e);
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