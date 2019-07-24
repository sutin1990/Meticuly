using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using MCP_WEB.Data;
using MCP_WEB.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Routing;
using Microsoft.EntityFrameworkCore;

namespace MCP_WEB.Controllers.FrontEnd
{
    [Authorize]
    public class ReportingProcessController : Controller
    {
        private NittanDBcontext _context;
        public ReportingProcessController(NittanDBcontext context)
        {
            _context = context;
        }

        public IActionResult Index(string id, ReportingProcess Rep)
        {
            if (id == null)
            {
                ViewBag.ProcessText = Rep.mProcessText;
            }
            else
            {
                ViewBag.ProcessText = id;
            }

            //Get Shift list

            var q1 = from x in _context.m_Shift
                     orderby x.StartTime
                     select x;
            ViewBag.ShiftList = new SelectList(q1, nameof(m_Shift.ShiftID), nameof(m_Shift.ShiftDesc));

            //Get shift by time
            TimeSpan StartTm, EndTm, CurTm;
            CurTm = new TimeSpan(DateTime.Now.Hour, DateTime.Now.Minute, DateTime.Now.Second);
            foreach (var x in q1)
            {
                StartTm = new TimeSpan(x.StartTime.Hour, x.StartTime.Minute, x.StartTime.Second);
                EndTm = new TimeSpan(x.EndTime.Hour, x.EndTime.Minute, x.EndTime.Second);

                if ((CurTm >= StartTm) & (CurTm <= EndTm))
                {
                    ViewBag.ShiftDefault = x.ShiftID;
                    break;
                }
            }

            //get Machine list
            var q2 = from q in _context.m_MachineMaster
                     select q;
            ViewBag.MachineList = new SelectList(q2, nameof(m_MachineMaster.MachineCode), nameof(m_MachineMaster.MachineName));

            //Get datetime format
            var p = _context.s_GlobalPams.SingleOrDefault(x => x.parm_key == "DateTimeFormat");
            ViewBag.GlobalDtFormat = p.param_value;

            return View(Rep);
        }

        public ReportingProcess Rep;
        [HttpPost]
        //public IActionResult Scan(string ProcessText, string Barcode, ReportingProcessRequest request)
        public IActionResult Scan(string ProcessText, string Barcode, [FromBody]ReportingProcessRequest request)
        {

            ProcessText = request.ProcessText;
            Barcode = request.Barcode;

            //Storeprocedure Output
            string P_Barcode = string.Empty;
            Int32 P_OperationNo = 0;
            string P_ProcessCode = null;
            string P_Model = null;
            string P_Material1 = string.Empty;
            string P_Material2 = string.Empty;
            string P_MachineCode = string.Empty;
            string P_AllowPartialFlag = string.Empty;
            string P_PStatus = string.Empty;
            Int32 P_ShiftID;
            Int32 P_QtyOrder;
            Int32 P_QtyComplete;
            Int32? P_QtyNG;
            Int32? P_QtyNC;
            Int32 P_TOQtyComplete;
            Int32 P_TOQtyNG;
            Int32 P_PPQtyComplete;
            Int32 P_PPQtyNG;
            DateTime P_TransDate;
            DateTime P_CreateDate;
            string P_ModifyBy = string.Empty;

            //ReportingProcess rep;
            ViewBag.Id = ProcessText;

            //Get User Name
            var identity = (ClaimsIdentity)User.Identity;
            IEnumerable<Claim> claims = identity.Claims;
            var c = claims.FirstOrDefault();

            string ProcessCode = GetProcessCode(ProcessText);

            //get PreProcess
            string P_PreProcess = string.Empty;
            var p = _context.WoRouting.SingleOrDefault(x => x.BarCode == Barcode && x.ProcessCode == ProcessCode );
            if (p != null)
            {
                int OperationNo = p.OperationNo;
                var q = _context.WoRouting
                    .OrderByDescending(x => x.OperationNo)
                    .FirstOrDefault(x => x.BarCode == Barcode && x.OperationNo < OperationNo && x.MainProcessFlag == "Y" );
                if (q != null)
                {
                    P_PreProcess = q.ProcessCode;
                }
            }

            using (var cmd = _context.Database.GetDbConnection().CreateCommand())
            {
                try
                {
                    cmd.Parameters.Clear();
                    cmd.CommandText = "m_sp_WoRoutingMovement_select";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@Barcode", SqlDbType.NVarChar) { Value = Barcode });
                    cmd.Parameters.Add(new SqlParameter("@ProcessCode", SqlDbType.NVarChar) { Value = ProcessCode });
                    cmd.Parameters.Add(new SqlParameter("@ModiftBy", SqlDbType.NVarChar) { Value = c.Value });

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
                            if (dr["Barcode"] != System.DBNull.Value)
                            {
                                P_Barcode = (string)dr["Barcode"];
                            }

                            if (dr["OperationNo"] != System.DBNull.Value)
                            {
                                if (dr["OperationNo"].ToString() != "") P_OperationNo = int.Parse(dr["OperationNo"].ToString());
                                else P_OperationNo = 0;
                            }

                            if (dr["ProcessCode"] != System.DBNull.Value)
                            {
                                P_ProcessCode = (string)dr["ProcessCode"];
                            }

                            if (dr["Model"] != System.DBNull.Value)
                            {
                                P_Model = (string)dr["Model"];
                            }

                            if (dr["Material1"] != System.DBNull.Value)
                            {
                                P_Material1 = (string)dr["Material1"];
                            }

                            if (dr["Material2"] != System.DBNull.Value)
                            {
                                P_Material2 = (string)dr["Material2"];
                            }

                            if (dr["MachineCode"] != System.DBNull.Value)
                            {
                                P_MachineCode = (string)dr["MachineCode"];
                            }

                            if (dr["AllowPartialFlag"] != System.DBNull.Value)
                            {
                                P_AllowPartialFlag = (string)dr["AllowPartialFlag"];
                            }

                            if (dr["PStatus"] != System.DBNull.Value)
                            {
                                P_PStatus = (string)dr["PStatus"];
                            }

                            if (dr["ShiftID"].ToString() != "") P_ShiftID = int.Parse(dr["ShiftID"].ToString());
                            else P_ShiftID = 0;

                            if (dr["QtyOrder"].ToString() != "") P_QtyOrder = int.Parse(dr["QtyOrder"].ToString());
                            else P_QtyOrder = 0;

                            if (dr["QtyComplete"].ToString() != "") P_QtyComplete = int.Parse(dr["QtyComplete"].ToString());
                            else P_QtyComplete = 0;

                            if (dr["QtyNg"].ToString() != "") P_QtyNG = int.Parse(dr["QtyNG"].ToString());
                            else P_QtyNG = 0;

                            if (dr["QtyNC"].ToString() != "") P_QtyNC = int.Parse(dr["QtyNC"].ToString());
                            else P_QtyNC = 0;

                            if (dr["TOQtyComplete"].ToString() != "") P_TOQtyComplete = int.Parse(dr["TOQtyComplete"].ToString());
                            else P_TOQtyComplete = 0;

                            if (dr["TOQtyNG"].ToString() != "") P_TOQtyNG = int.Parse(dr["TOQtyNG"].ToString());
                            else P_TOQtyNG = 0;

                            if (dr["PPQtyComplete"].ToString() != "") P_PPQtyComplete = int.Parse(dr["PPQtyComplete"].ToString());
                            else P_PPQtyComplete = 0;

                            if (dr["PPQtyNG"].ToString() != "") P_PPQtyNG = int.Parse(dr["PPQtyNG"].ToString());
                            else P_PPQtyNG = 0;

                            P_TransDate = (DateTime)dr["TransDate"];
                            P_CreateDate = (DateTime)dr["CreateDate"];
                            P_ModifyBy = (string)dr["ModifyBy"];

                            Rep = new ReportingProcess
                            {
                                mProcessText = ProcessText,
                                Barcode = P_Barcode,
                                OperationNo = P_OperationNo,
                                ProcessCode = P_ProcessCode,
                                Model = P_Model,
                                Material1 = P_Material1,
                                Material2 = P_Material2,
                                MachineCode = P_MachineCode,
                                AllowPartialFlag = P_AllowPartialFlag,
                                PStatus = P_PStatus,
                                ShiftID = P_ShiftID,
                                QtyOrder = P_QtyOrder,
                                QtyComplete = P_QtyComplete,
                                QtyNG = P_QtyNG,
                                QtyNC = P_QtyNC,
                                TOQtyComplete = P_TOQtyComplete,
                                TOQtyNG = P_TOQtyNG,
                                PPQtyComplete = P_PPQtyComplete,
                                PPQtyNG = P_PPQtyNG,
                                TransDate = P_TransDate,
                                CreateDate = P_CreateDate,
                                ModifyBy = P_ModifyBy,
                                PreProcess = P_PreProcess,
                                SqlStatus = "OK",
                                SqlErrtext = ""
                            };

                        }
                    }
                    else
                    {
                        Rep = new ReportingProcess
                        {
                            mProcessText = ProcessText,
                            SqlStatus = "OK",
                            SqlErrtext = ""
                        };
                    }
                }

                catch (SqlException ex)
                {
                    //ViewBag.Message = ex.Message;
                    //ViewBag.RedirectController = "ReportingProcess/Index/" + ProcessText;
                    //return PartialView("ModalSqlException");
                    Rep = new ReportingProcess
                    {
                        mProcessText = ProcessText,
                        SqlStatus = "Error",
                        SqlErrtext = ex.Message
                    };
                }

            }
            return new JsonResult(Rep);
            //return RedirectToAction("Index", new RouteValueDictionary(new { controller = nameof(ReportingProcess), action = nameof(Index), Id = ProcessText }));
        }

        [HttpPost]
        public IActionResult Confirm(string ProcessText, string Barcode, Int32? OperationNo,
                                    Int32? ShiftID, string PStatus, Int32? QtyComplete, Int32? QtyNG,
                                    string MachineCode,
                                     [FromBody]ReportingProcessRequest request)
        {
            ProcessText = request.ProcessText;
            Barcode = request.Barcode;
            OperationNo = request.OperationNo;
            ShiftID = request.ShiftID;
            PStatus = request.PStatus;
            QtyComplete = request.QtyComplete;
            QtyNG = request.QtyNG;
            MachineCode = request.MachineCode;

            if (QtyComplete == null) { QtyComplete = 0; }
            if (QtyNG == null) { QtyNG = 0; }

            //ReportingProcess rep;
            ViewBag.Id = ProcessText;

            //Get User Name
            var identity = (ClaimsIdentity)User.Identity;
            IEnumerable<Claim> claims = identity.Claims;
            var c = claims.FirstOrDefault();

            string ProcessCode = GetProcessCode(ProcessText);

            using (var cmd = _context.Database.GetDbConnection().CreateCommand())
            {
                try
                {
                    cmd.Parameters.Clear();
                    cmd.CommandText = "m_sp_WoRoutingMovement_insert";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@Barcode", SqlDbType.NVarChar) { Value = Barcode });
                    cmd.Parameters.Add(new SqlParameter("@OperationNo", SqlDbType.Int) { Value = OperationNo });
                    cmd.Parameters.Add(new SqlParameter("@ShiftID", SqlDbType.Int) { Value = ShiftID });
                    cmd.Parameters.Add(new SqlParameter("@PStatus", SqlDbType.NVarChar) { Value = PStatus });
                    cmd.Parameters.Add(new SqlParameter("@QtyComplete", SqlDbType.Int) { Value = QtyComplete });
                    cmd.Parameters.Add(new SqlParameter("@QtyNG", SqlDbType.Int) { Value = QtyNG });
                    cmd.Parameters.Add(new SqlParameter("@ModifyBy", SqlDbType.NVarChar) { Value = c.Value });
                    cmd.Parameters.Add(new SqlParameter("@MachineCode", SqlDbType.NVarChar) { Value = MachineCode });

                    if (cmd.Connection.State != ConnectionState.Open)
                    {
                        cmd.Connection.Open();
                    }
                    cmd.ExecuteNonQuery();
                }
                catch (SqlException ex)
                {
                    //ViewBag.Message = ex.Message;
                    //ViewBag.RedirectController = "ReportingProcess/Index/" + ProcessText;
                    //return PartialView("ModalSqlException");
                    Rep = new ReportingProcess
                    {
                        mProcessText = ProcessText,
                        SqlStatus = "Error",
                        SqlErrtext = ex.Message
                    };

                    return new JsonResult(Rep);
                }
            }


            Rep = new ReportingProcess
            {
                mProcessText = ProcessText,
                SqlStatus = "OK",
                SqlErrtext = ""
            };

            //return RedirectToAction("Index", new RouteValueDictionary(new { controller = nameof(ReportingProcess), action = nameof(Index), Id = ProcessText }));
            return new JsonResult(Rep);
        }

        private string GetProcessCode(string ProcessText)
        {
            switch (ProcessText)
            {
                case "CUT OFF BAR":
                    return "CutOffBar";
                case "FRICTION":
                    return "Friction";
                case "FORGING":
                    return "Forging";
                case "STELLITE TIP":
                    return "StelliteTip";
                case "STELLITE SEAT":
                    return "StelliteSeat";
                case "STRAIGHTENING":
                    return "STRAIGHTENING";
                case "STEM ROUGH":
                    return "StemRough";
                case "STEM FINISH":
                    return "StemFinish";
                case "SEATFINISH":
                    return "SeatFinish";
                case "QCVISUAL":
                    return "QCVISUAL";
                default:
                    return "";
            }

        }

    }
}