using System;
using System.Collections.Generic;
using System.Data;
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
using Newtonsoft.Json.Linq;

namespace MCP_WEB.Controllers.WebAPI
{
    [Produces("application/json")]
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class IsoniteForSupplierController : Controller
    {
        private readonly NittanDBcontext _Context;

        public ReportingProcess Rep;

        public IsoniteForSupplierController(NittanDBcontext Context)
        {
            this._Context = Context;
        }

        [HttpGet]
        public IActionResult GetJigSupplier(DataSourceLoadOptions loadOptions)
        {
            var m_jig = _Context.m_Jig.Where(x => x.jig_isonite_status == "Use").Select(x => new
            {
                x.JigNo
            });

            return Json(DataSourceLoader.Load(m_jig, loadOptions));
        }

        [HttpGet]
        public IActionResult GetIsoniteSent(DataSourceLoadOptions loadOptions)
        {

            var Isonite_Line = _Context.Isonite_Line.Where(x => x.TransType == "Sent").Select(x => new
            {
                x.IsoniteCode
            }).GroupBy(x => x.IsoniteCode);

            return Json(DataSourceLoader.Load(Isonite_Line, loadOptions));

        }

        [HttpGet]
        public IActionResult GetSearchIsonite(string JigNo, string User)
        {
            if (JigNo != "")
            {
                var model = _Context.Isonite_Line_Temp.Where(x => x.ModifyBy == User).ToList();
                if (model != null)
                {
                    _Context.Isonite_Line_Temp.RemoveRange(model);
                    _Context.SaveChanges();
                }

                List<Isonite_Line_Temp> listIsonite = new List<Isonite_Line_Temp>();

                var woRoutings = (from detail in _Context.Isonite_Line
                                  join Weekly in _Context.WeeklyPlan
                                  on detail.BarCode equals Weekly.BarCode into gj
                                  from sub in gj.DefaultIfEmpty()
                                  where detail.JigNo == JigNo && detail.TransType == "Sent"
                                  select new
                                  {
                                      detail.IsoniteCode,
                                      detail.JigNo,
                                      detail.JigAddress,
                                      detail.BarCode,
                                      detail.TransType,
                                      detail.Qty,
                                      detail.RefIsoniteCode,
                                      detail.RefJigAddress,
                                      sub.Model,
                                  }).ToList();



                foreach (var row in woRoutings)
                {
                    listIsonite.Add(
                        new Isonite_Line_Temp
                        {
                            IsoniteCode = row.IsoniteCode,
                            JigNo = row.JigNo,
                            JigAddress = row.JigAddress,
                            BarCode = row.BarCode,
                            TransType = row.TransType,
                            Qty = row.Qty,
                            RefIsoniteCode = row.RefIsoniteCode,
                            RefJigAddress = row.RefJigAddress,
                            CreateDate = DateTime.Now,
                            TransDate = DateTime.Now,
                            Sentdate = DateTime.Now,
                            Receivedate = DateTime.Now,
                            ModifyBy = User
                        });
                }

                _Context.Isonite_Line_Temp.AddRange(listIsonite);
                _Context.SaveChanges();

                return new JsonResult(new { Msg = "OK", Isonite_Line = woRoutings });
            }
            else
            {
                return new JsonResult(new { Msg = "ERROR", des = "Message: Isonite Code is Null." });
            }


        }

        [HttpGet]
        public IActionResult GetIsoniteLineSave(DataSourceLoadOptions loadOptions)
        {
            var woRoutings = (from detail in _Context.Isonite_Line
                              join Weekly in _Context.WeeklyPlan
                              on detail.BarCode equals Weekly.BarCode into gj
                              from sub in gj.DefaultIfEmpty()
                              where detail.TransType == "Sent"

                              select new
                              {
                                  detail.RECID,
                                  detail.IsoniteCode,
                                  detail.JigNo,
                                  detail.JigAddress,
                                  detail.BarCode,
                                  detail.TransType,
                                  detail.Qty,
                                  detail.RefIsoniteCode,
                                  detail.RefJigAddress,
                                  sub.Model

                              }).ToList();

            var model = (from detail in woRoutings
                         where detail.TransType == "Sent" && !_Context.Isonite_Line_Temp.Any(x => x.BarCode == detail.BarCode && x.JigNo == detail.JigNo && x.JigAddress == detail.JigAddress)
                         select detail).ToList();

            return Json(DataSourceLoader.Load(model, loadOptions));

        }

        [HttpGet]
        public IActionResult GetIsoniteLine(DataSourceLoadOptions loadOptions)
        {
            var identity = (ClaimsIdentity)User.Identity;
            IEnumerable<Claim> claims = identity.Claims;
            var ID = claims.FirstOrDefault();


            var model1 = _Context.Isonite_Line_Temp.Where(x => x.ModifyBy == ID.Value).ToList();

            if (model1 != null)
            {

                _Context.Isonite_Line_Temp.RemoveRange(model1);
                _Context.SaveChanges();
            }

            var clare_isonite_line = _Context.Isonite_Line_Temp.Where(x => DateTime.Compare(x.TransDate.Value.Date, DateTime.Now) < 10).ToList();

            if (clare_isonite_line != null)
            {
                _Context.Isonite_Line_Temp.RemoveRange(clare_isonite_line);
                _Context.SaveChanges();
            }

            var countIsonite = (from i in _Context.Isonite_Line
                                where i.TransType == "Sent"
                                group i by i.IsoniteCode into Group
                                select new
                                {
                                    isoniteCode = Group.Key,
                                    Count = Group.Count(),
                                }).ToList();


            if (countIsonite != null)
            {

                var data = (from d in countIsonite
                            where d.Count > 0
                            select new
                            {
                                d.isoniteCode
                            });

                var model = (from detail in data
                             where !_Context.Isonite_Line_Temp.Any(x => x.IsoniteCode == detail.isoniteCode)
                             select detail).ToList();

                var query = model.GroupBy(x => x.isoniteCode).Select(x => new
                {
                    IsoniteCode = x.Key
                });

                var checkJig = from a in _Context.Isonite
                               join b in query on a.IsoniteCode equals b.IsoniteCode
                               select new
                               {
                                   a.IsoniteCode,
                                   a.JigNo1,
                                   a.JigNo2,
                                   a.JigNo3
                               };

                List<Isonite_Line> _Lines = new List<Isonite_Line>();

                foreach (var row in checkJig)
                {
                    if (_Context.m_Jig.Any(x => x.JigNo == row.JigNo1 && x.jig_isonite_status == "Use")
                        || _Context.m_Jig.Any(x => x.JigNo == row.JigNo2 && x.jig_isonite_status == "Use")
                        || _Context.m_Jig.Any(x => x.JigNo == row.JigNo3 && x.jig_isonite_status == "Use"))
                    {
                        _Lines.Add(new Isonite_Line
                        {
                            IsoniteCode = row.IsoniteCode
                        });
                    }
                }

                return Json(DataSourceLoader.Load(_Lines, loadOptions));
            }
            else
            {
                var query = countIsonite.GroupBy(x => x.isoniteCode).Select(x => new
                {
                    IsoniteCode = x.Key
                });

                return Json(DataSourceLoader.Load(query, loadOptions));
            }


        }

        [HttpGet]
        public IActionResult btnAdd(string IsoniteCode, string JigNo, string JigAddress, string BarCode, string Qty
            , string RefIsoniteCode, string RefJigAddress, string ModifyBy)
        {

            _Context.Isonite_Line_Temp.Add(new Isonite_Line_Temp
            {
                IsoniteCode = IsoniteCode,
                JigNo = JigNo,
                JigAddress = Convert.ToInt32(JigAddress),
                BarCode = BarCode,
                Qty = Convert.ToInt32(Qty),
                CreateDate = DateTime.Now,
                TransDate = DateTime.Now,
                Sentdate = DateTime.Now,
                Receivedate = DateTime.Now,
                ModifyBy = ModifyBy
            });

            _Context.SaveChanges();
            return new JsonResult(new { Msg = "OK" });

        }


        [HttpGet]
        public IActionResult btnDelete(string Barcode, string JigID, string JigAddress, string User)
        {
            var model = _Context.Isonite_Line_Temp.Where(x => x.ModifyBy == User && x.BarCode == Barcode && x.JigNo == JigID && x.JigAddress == Convert.ToInt32(JigAddress));
            if (model != null)
            {
                _Context.Isonite_Line_Temp.RemoveRange(model);
                _Context.SaveChanges();
                return new JsonResult(new { Msg = "OK" });
            }
            else
            {
                return new JsonResult(new { Msg = "Error", des = "Message : Model is null." });
            }

        }

        [HttpPost]
        public IActionResult BtnSum(JObject jsonResult)
        {
            var s1 = jsonResult.Values().ToList();

            var detail = ((Newtonsoft.Json.Linq.JValue)s1[0]).Value;

            var user = ((Newtonsoft.Json.Linq.JValue)s1[1]).Value;

            JObject _detail = JObject.Parse(detail.ToString());

            var Isonite_detail = _detail.GetValue("Detail").ToList();

            var objList = new List<IsoniteSum>();

            foreach (JToken _idetail in Isonite_detail)
            {
                objList.Add(new IsoniteSum()
                {
                    //IsoniteCode = CodeIsonite,
                    //JigNo = getjigNo(Convert.ToString(_idetail.SelectToken("JigNo"))),
                    JigAddress = Convert.ToInt32(_idetail.SelectToken("JigAddress")),
                    BarCode = Convert.ToString(_idetail.SelectToken("BarCode")),
                    Model = Convert.ToString(_idetail.SelectToken("Model")),
                    TransType = "Sent",
                    Qty = Convert.ToInt32(_idetail.SelectToken("Qty")),
                    CreateDate = DateTime.Now,
                    TransDate = DateTime.Now,
                    ModifyBy = Convert.ToString(user)
                });

            }

            var sumData = objList.GroupBy(d => d.Model)
             .Select(g => new
             {
                 Key = g.Key,
                 Value = g.Sum(s => s.Qty),
                 Model = g.First().Model
             });

            return new JsonResult(new { Msg = "OK", SumIsonite = sumData });
        }

        [HttpPost]
        public IActionResult ConfirmIsonite([FromBody]JObject jsonResult)
        {
            List<ReportingProcess> Data = new List<ReportingProcess>();

            var s1 = jsonResult.Values().ToList();

            var detail = ((Newtonsoft.Json.Linq.JValue)s1[0]).Value;

            var user = ((Newtonsoft.Json.Linq.JValue)s1[1]).Value;

            JObject _detail = JObject.Parse(detail.ToString());

            var Isonite_detail = _detail.GetValue("Detail").ToList();

            var objList = new List<Isonite_Line_Confirm>();

            foreach (JToken _idetail in Isonite_detail)
            {
                objList.Add(new Isonite_Line_Confirm()
                {
                    IsoniteCode = Convert.ToString(_idetail.SelectToken("IsoniteCode")),//isonite เดิม
                    JigNo = Convert.ToString(_idetail.SelectToken("JigNo")),//Jig ปัจจุบันที่เก็บ
                    JigAddress = Convert.ToInt32(_idetail.SelectToken("JigAddress")),//ตำแหน่ง ปัจจุบันที่เก็บ
                    BarCode = Convert.ToString(_idetail.SelectToken("BarCode")),
                    TransType = "Sent",
                    Qty = Convert.ToInt32(_idetail.SelectToken("Qty")),//
                    Confirm_Qty = Convert.ToInt32(_idetail.SelectToken("Confirm_Qty")),
                    NgQty = Convert.ToInt32(_idetail.SelectToken("NgQty")),
                    RefIsoniteCode = Convert.ToString(_idetail.SelectToken("RefIsoniteCode")),//Isonite ที่ รายงาน
                    RefJigAddress = Convert.ToInt32(_idetail.SelectToken("RefJigAddress")),//ตำแหน่งที่ เก็บเก่า
                    ReJigNo = Convert.ToString(_idetail.SelectToken("RefJigNo")),//ตำแหน่งที่เก็บ เก่า

                    ModifyBy = Convert.ToString(user)
                });

            }

            foreach (var row in objList)
            {
                if (_Context.Isonite_Line.Any(x => x.IsoniteCode == row.IsoniteCode
                && x.BarCode == row.BarCode && x.JigAddress == row.RefJigAddress && x.JigNo == row.ReJigNo))
                {
                    var _op = _Context.WoRouting.Where(x => x.BarCode == row.BarCode && x.ProcessCode == "ISONITE").FirstOrDefault();
                    if (row.Confirm_Qty == 0)
                    {
                        Data = Confirm(row.BarCode, _op.OperationNo, Convert.ToString(user), 1, "S", row.Qty, row.NgQty);
                    }
                    else
                    {
                        Data = Confirm(row.BarCode, _op.OperationNo, Convert.ToString(user), 1, "S", row.Confirm_Qty, row.NgQty);
                    }
                }
            }



            if (Data.Count > 0)
            {
                return new JsonResult(new { Msg = "Error", Error = Data });
            }
            else
            {
                foreach (var row in objList)
                {
                    if (_Context.Isonite_Line.Any(x => x.IsoniteCode == row.IsoniteCode
                    && x.BarCode == row.BarCode && x.JigAddress == row.RefJigAddress && x.JigNo == row.ReJigNo))
                    {
                        var model = _Context.Isonite_Line.Where(x => x.IsoniteCode == row.IsoniteCode
                                       && x.BarCode == row.BarCode && x.JigAddress == row.RefJigAddress && x.JigNo == row.ReJigNo).FirstOrDefault();

                        model.RefIsoniteCode = row.RefIsoniteCode;
                        model.RefJigAddress = row.JigAddress;
                        model.TransType = "Receive";
                        model.Receivedate = DateTime.Now;
                        model.ReceiveBy = row.ModifyBy;

                        _Context.SaveChanges();

                        var jig = _Context.m_Jig.Where(x => x.JigNo == row.JigNo).FirstOrDefault();
                        jig.jig_isonite_status = "unUse";
                        _Context.SaveChanges();

                        var countIsonite = (from i in _Context.Isonite_Line
                                            where i.IsoniteCode == row.IsoniteCode
                                            group i by i.IsoniteCode into Group
                                            select new
                                            {
                                                isoniteCode = Group.Key,
                                                Count = Group.Count(),
                                            }).ToList();

                        var CountResive = (from i in _Context.Isonite_Line
                                           where i.TransType == "Receive" && i.IsoniteCode == row.IsoniteCode
                                           group i by i.IsoniteCode into Group
                                           select new
                                           {
                                               isoniteCode = Group.Key,
                                               Count = Group.Count(),
                                           }).ToList();


                        if (CountResive.Count == countIsonite.Count)
                        {
                            var model_header = _Context.Isonite.Where(x => x.IsoniteCode == row.IsoniteCode).FirstOrDefault();
                            model_header.DocStatus = "C";
                            _Context.SaveChanges();
                        }

                    }
                }

            }


            var model1 = _Context.Isonite_Line_Temp.Where(x => x.ModifyBy == Convert.ToString(user));
            _Context.Isonite_Line_Temp.RemoveRange(model1);
            _Context.SaveChanges();

            return new JsonResult(new { Msg = "OK" });
        }

        [HttpGet]
        public IActionResult btn_cancel(string User)
        {
            var model1 = _Context.Isonite_Line_Temp.Where(x => x.ModifyBy == User);
            _Context.Isonite_Line_Temp.RemoveRange(model1);
            _Context.SaveChanges();

            return new JsonResult(new { Msg = "OK" });
        }

        [HttpPost]
        public List<ReportingProcess> Confirm(string Barcode, Int32? OperationNo, string User,
                                    Int32? ShiftID, string PStatus, Int32? QtyComplete, Int32? QtyNG)
        {
            List<ReportingProcess> error = new List<ReportingProcess>();
            using (var cmd = _Context.Database.GetDbConnection().CreateCommand())
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
                    cmd.Parameters.Add(new SqlParameter("@ModifyBy", SqlDbType.NVarChar) { Value = User });
                    cmd.Parameters.Add(new SqlParameter("@MachineCode", SqlDbType.NVarChar) { Value = "" });

                    if (cmd.Connection.State != ConnectionState.Open)
                    {
                        cmd.Connection.Open();
                    }
                    cmd.ExecuteNonQuery();

                    return error;
                }
                catch (SqlException ex)
                {

                    error.Add(Rep = new ReportingProcess
                    {
                        SqlStatus = "Error",
                        SqlErrtext = ex.Message
                    });

                    return error;
                }
            }

        }

    }
}