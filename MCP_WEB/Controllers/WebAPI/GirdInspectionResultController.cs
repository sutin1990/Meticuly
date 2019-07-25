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
    public class GirdInspectionResultController : Controller
    {
        private NittanDBcontext _context;

        public GirdInspectionResultController(NittanDBcontext context)
        {
            this._context = context;
        }

        [HttpGet]
        public object Get(DataSourceLoadOptions loadOptions,int docnum)
        {
            try
            {
                //var isnumeric = int.TryParse(docnum, out int n);
                // if (isnumeric)
                // {
                //select* from SAP_MET_testcost.dbo.OWOR where DocNum = 100106--'L'=>Closed
                //select* from SAP_MET_testcost.dbo.OWOR where DocNum = 100105-- 'C'=> Canceled
                var HRS = from hrs in _context.VW_MFC_Header_Route_stage
                          where hrs.DocNum == docnum
                          select new
                          {
                              hrs.DocEntry,
                              hrs.DocNum,
                              hrs.OriginNum,
                              hrs.ItemCode,
                              hrs.ProdName,
                              hrs.PlannedQty,
                              hrs.Uom,
                              Status = hrs.Status == "L" ? "Closed" : hrs.Status == "C" ? "Canceled" : hrs.Status == "R" ? "Released" : hrs.Status == "P" ? "Planned" : "",
                              StartDate = Convert.ToDateTime(hrs.StartDate).ToString("dd/MM/yyyy"),
                              DueDate = Convert.ToDateTime(hrs.DueDate).ToString("dd/MM/yyyy")
                              };
                    
                    return DataSourceLoader.Load(HRS, loadOptions);
                
            }
            catch(Exception ex)
            {
                return false;
            }
            
        }

        [HttpGet]
        public IActionResult Get_Routestage(DataSourceLoadOptions loadOptions, string DocEntry)
        {
            //DocEntry = "117";
            try
            {
                var RS = from rs in _context.VW_MFC_Route_stage
                         where rs.DocEntry == Convert.ToInt32(DocEntry)
                         select new
                         {
                             rs.Code,
                             rs.DocEntry,
                             rs.Name,
                             Status =rs.Status == "C"?"Complate":rs.Status == "I"?"In-Progress":rs.Status == "P"?"Planned":"",
                             AbsEntry = rs.StgEntry,
                             rs.StageId,
                             rs.SeqNum                             
                             //rs.StageId,                         
                             //StartDate = Convert.ToDateTime(rs.StartDate).ToString("dd/MM/yyyy"),
                             //DueDate = Convert.ToDateTime(rs.EndDate).ToString("dd/MM/yyyy")
                         };
                return Json(DataSourceLoader.Load(RS, loadOptions));
            }
            catch(Exception ex)
            {
                return Json(false);
            }
            
        }

        [HttpGet]
        public IActionResult Get_RoutestageSUB(DataSourceLoadOptions loadOptions, int U_StgEntry, int Docentry)
        {
            //DocEntry = "117";            
            try
            {
                var VRS = _context.VW_MFC_M_INSPECT_RESULT.Where(w => w.U_StgEntry == U_StgEntry && w.U_DocEntry == Docentry).ToList();
                if (VRS.Count > 0)
                {
                    var RS = from a in _context.VW_MFC_M_INSPECT_RESULT
                             join b in _context.VW_MFC_ROUTEINSPECT on a.U_InspectEntry equals b.DocEntry
                             //join c in _context.VW_MFC_WOR4 on new { StgEntry = b.AbsEntry } equals new { c.StgEntry }
                             where a.U_StgEntry == U_StgEntry && b.U_ActiveFlag == "Y" && a.U_DocEntry == Docentry
                             select new
                             {
                                 a.Code,
                                 a.U_StgEntry,
                                 a.U_InspectMethod,
                                 a.U_InspectEntry,
                                 a.U_RptOrdering,
                                 a.U_PassQty,
                                 a.U_RejQty,
                                 a.U_Reason,
                                 a.U_Comment,
                                 a.U_UpdateBy,
                                 a.U_UpdateDate

                             };

                    var result = new
                    {
                        items = new[] {
                    new {type = "transection",message = "Success" , status =true, data = RS},
                    }
                    };
                    return Json(result);
                }
                else
                {
                    var RS = from a in _context.VW_MFC_ROUTEINSPECT 
                             join b in _context.VW_MFC_M_INSPECT_RESULT on a.DocEntry equals b.U_InspectEntry into bs from b in bs.DefaultIfEmpty()
                             where a.U_AbsEntry == U_StgEntry && a.U_ActiveFlag == "Y"
                             select new
                             {
                                 Code = a.DocEntry,
                                 U_StgEntry = a.U_AbsEntry,
                                 U_InspectMethod = a.U_MethodDesc,
                                 U_InspectEntry = a.DocEntry,
                                 U_RptOrdering = a.U_RptOrder,
                                 U_PassQty = b.U_PassQty == null ? 0 : b.U_PassQty,
                                 U_RejQty =  b.U_RejQty == null ? 0: b.U_RejQty,
                                 U_Reason = b.U_Reason == null ?"":b.U_Reason,
                                 U_Comment = b.U_Comment == null ? "" : b.U_Comment,                             
                                 b.U_UpdateBy,
                                 b.U_UpdateDate
                             };

                    var result = new
                    {
                        items = new[] {
                    new {type = "master",message = "Success" , status =true, data = RS},
                    }
                    };
                    return Json(result);
                }
                
                //return Json(DataSourceLoader.Load(RS, loadOptions));                
               
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
        public IActionResult Putdata(string values,string type,int U_DocEntry, int U_DocNum)
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
            dt_result.Columns.Add("CodeInsert", typeof(System.Int32));
            dt_result.Columns.Add("U_StgEntry", typeof(System.String));
            var msgj = "";
            var Code = 0;
            try
            {                
                DataTable dt = (DataTable)JsonConvert.DeserializeObject(values, (typeof(DataTable)));
                var count = dt.Rows.Count;                

                foreach (DataRow row in dt.Rows)
                {
                     Code = Convert.ToInt32(row["Code"]);
                    var U_AbsEntry = row["U_StgEntry"];
                    var U_InspectEntry = row["U_InspectEntry"] ;
                    var U_InspectMethod = row["U_InspectMethod"] ;
                    var U_RptOrdering = row["U_RptOrdering"] ;
                    var U_PassQty = Convert.ToDecimal(row["U_PassQty"]);
                    var U_RejQty = Convert.ToDecimal(row["U_RejQty"]);
                    var U_Reason = row["U_Reason"];
                    var U_Comment = row["U_Comment"];
                    var U_Lastupdateby = userLogin.Value;
                    var U_Lastupdatedate = DateTime.Now;

                    //var ROUTEINSPECT = _context.VW_MFC_ROUTEINSPECT_SUB.FirstOrDefault(s => s.DocEntry == DocEntry);
                    ////var ROUTEINSPECT = from r in _context.VW_MFC_ROUTEINSPECT.Where(s=>s.Code == Convert.ToString(DocEntry) )
                    //var u_absentry = ROUTEINSPECT.U_AbsEntry;
                    //var u_passqty = Convert.ToInt32(ROUTEINSPECT.U_PassQty);
                    //var u_rejectqty = Convert.ToInt32(ROUTEINSPECT.U_RejectQty);
                    //var u_rejectreason = ROUTEINSPECT.U_RejectReason;
                    //var u_comment = ROUTEINSPECT.U_Comment;

                    using (var cmd = _context.Database.GetDbConnection().CreateCommand())
                    {
                        cmd.Parameters.Clear();
                        cmd.CommandText = "m_sp_Inspection_Manage";
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add(new SqlParameter("@Code", SqlDbType.Int) { Value = Code });
                        cmd.Parameters.Add(new SqlParameter("@U_AbsEntry", SqlDbType.NVarChar) { Value = U_AbsEntry });
                        cmd.Parameters.Add(new SqlParameter("@U_InspectEntry", SqlDbType.Int) { Value = U_InspectEntry == null ? 0 : U_InspectEntry });
                        cmd.Parameters.Add(new SqlParameter("@U_InspectMethod", SqlDbType.NVarChar) { Value = U_InspectMethod == null ? "" : U_InspectMethod });
                        cmd.Parameters.Add(new SqlParameter("@U_RptOrdering", SqlDbType.Int) { Value = U_RptOrdering == null ? 0 : U_RptOrdering });
                        cmd.Parameters.Add(new SqlParameter("@U_PassQty", SqlDbType.Int) { Value = U_PassQty });
                        cmd.Parameters.Add(new SqlParameter("@U_RejQty", SqlDbType.Int) { Value = U_RejQty });
                        cmd.Parameters.Add(new SqlParameter("@U_Reason", SqlDbType.NVarChar) { Value = U_Reason });
                        cmd.Parameters.Add(new SqlParameter("@U_Comment", SqlDbType.NVarChar) { Value = U_Comment });
                        cmd.Parameters.Add(new SqlParameter("@U_Lastupdateby", SqlDbType.NVarChar) { Value = U_Lastupdateby });
                        cmd.Parameters.Add(new SqlParameter("@U_Lastupdatedate", SqlDbType.DateTime) { Value = U_Lastupdatedate });
                        cmd.Parameters.Add(new SqlParameter("@type", SqlDbType.NVarChar) { Value = type });
                        cmd.Parameters.Add(new SqlParameter("@U_DocEntry", SqlDbType.Int) { Value = U_DocEntry });
                        cmd.Parameters.Add(new SqlParameter("@U_DocNum", SqlDbType.Int) { Value = U_DocNum });
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
                                DataRow dr_s = dt_result.NewRow();
                                dr_s["SqlStatus"] = "S";
                                dr_s["SqlErrtext"] = "";
                                dr_s["statuscode"] = dt_sp.Rows[0]["statuscode"];
                                dr_s["statusmessage"] = dt_sp.Rows[0]["statusmessage"];
                                dr_s["Code"] = dt_sp.Rows[0]["Code"];
                                dr_s["CodeInsert"] = dt_sp.Rows[0]["CodeInsert"];
                                dr_s["U_StgEntry"] = dt_sp.Rows[0]["U_StgEntry"];
                                dt_result.Rows.Add(dr_s);
                            }
                        }
                        catch(Exception ex)
                        {
                            msgj = ex.Message;
                            DataRow dr_e = dt_result.NewRow();
                            dr_e["SqlStatus"] = "E";
                            dr_e["SqlErrtext"] = msgj;
                            dr_e["Code"] = Code;
                            dt_result.Rows.Add(dr_e);
                        }                        
                       
                        cmd.Connection.Close();
                    }
                    //if (u_absentry != Convert.ToInt32(U_AbsEntry) || u_passqty != U_PassQty || u_rejectqty != U_RejectQty || u_rejectreason != U_RejectReason.ToString() || u_comment != U_Comment.ToString())
                    //{

                    //}
                }
                
            }
            catch (SqlException ex)
            {
                msgj = ex.Message;
                msgj = ex.Message;
                DataRow dr_e = dt_result.NewRow();
                dr_e["SqlStatus"] = "E";
                dr_e["SqlErrtext"] = msgj;
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