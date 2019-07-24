using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MCP_WEB.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MCP_WEB.Models;
using System.Globalization;
using Newtonsoft.Json;
using System.Collections;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using OfficeOpenXml.FormulaParsing.Excel.Functions.DateTime;
using Newtonsoft.Json.Linq;
using DevExtreme.AspNet.Mvc;
using DevExtreme.AspNet.Data;

namespace MCP_WEB.Controllers.WebAPI
{
    [Produces("application/json")]
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class IsoniteMantController : Controller
    {
        private readonly NittanDBcontext _context;

        public IsoniteMantController(NittanDBcontext context)
        {
            this._context = context;
        }

        public IActionResult ScanBarCode(string Barcode, string User, string JigNO)
        {

            int? qtyCom;

            int? SumQtyLine = 0;
            //get Token

            var UserToken = _context.UserTransaction.FirstOrDefault(x => x.UserName == User);

            //Query WoRouting
            var BarCodeQuery = _context.WoRouting.Where(n => n.BarCode == Barcode.Trim()).OrderBy(x => x.BarCode).ThenBy(x => x.OperationNo).ToList();

            var isonite = BarCodeQuery.Where(x => x.ProcessCode == "ISONITE").FirstOrDefault();

            if (isonite == null)
            {
                return new JsonResult(new { Msg = "Error", des = "Process Error: " + Barcode + " is not Isonite process." });
            }

            //------------------------------------------------------------------------------------------------------------------------------------
            var LotSize = (from i in _context.WeeklyPlan
                           join x in _context.m_Resource
                           on i.ItemCode equals x.ItemCode
                           where i.BarCode == Barcode.Trim()
                           select new
                           {
                               size = x.ItemUserDef5,
                               stdLot = x.StdLotSize
                           }).FirstOrDefault();

            //------------------------------------------------------------------------------------------------------------------------------------------

            var Process = BarCodeQuery.Where(x => x.OperationNo < isonite.OperationNo && x.MainProcessFlag == "Y").OrderByDescending(x => x.OperationNo).FirstOrDefault();

            if (Process == null)
            {
                return new JsonResult(new { Msg = "Error", des = "Process Error: " + Barcode + ", You can not report for process [Isonite] due to prior process does not complete!" });
            }

            //-------------------หา Model ------------------------------------------------------------------------------------------------------------------

            var model = _context.WeeklyPlan.Where(x => x.BarCode == Barcode.Trim()).FirstOrDefault();

            //--------------------------------------------------------------------------------------------------------------------------------------
            if (Process.QtyComplete != null && Process.QtyComplete != 0)
            {

                if (_context.Isonite_Line.Any(x => x.BarCode == Barcode.Trim()))
                {
                    var isonite_line = _context.Isonite_Line.Where(x => x.BarCode == Barcode && x.TransType == "Sent").GroupBy(o => o.BarCode).Select(x => new
                    {
                        Barcode = x.Key,
                        SumQty = x.Sum(i => i.Qty)
                    }).FirstOrDefault();

                    SumQtyLine = isonite_line.SumQty;

                }

                //Check isonite in isonite temp
                if (_context.isonite_temp.Any(x => x.BarCode == Barcode.Trim()))
                {
                    var isonite_temp = _context.isonite_temp.Where(x => x.BarCode == Barcode).GroupBy(o => o.BarCode).Select(x => new
                    {
                        Barcode = x.Key,
                        SumQty = x.Sum(i => i.QtyComplete + i.QtyNG)
                    }).FirstOrDefault();

                    qtyCom = Process.QtyComplete - Process.QtyNG - (isonite.QtyComplete + isonite.QtyNG + isonite_temp.SumQty) - SumQtyLine;//มีข้อมูล ใน Temp

                    if (qtyCom == 0) return new JsonResult(new { Msg = "Error", des = "Process Error: " + Barcode + " is Qty Equal to 0." });

                }
                else
                {
                    qtyCom = Process.QtyComplete - Process.QtyNG - (isonite.QtyComplete + isonite.QtyNG) - SumQtyLine;//
                    if (qtyCom == 0) return new JsonResult(new { Msg = "Error", des = "Process Error: " + Barcode + " is Qty Equal to 0." });
                }
            }
            else
            {
                return new JsonResult(new { Msg = "Error", des = "Process Oparation : " + Process.OperationNo + "/" + Process.ProcessCode + " is 0. " });
            }

            if (_context.isonite_temp.Any(item => item.BarCode == Barcode.Trim()))
            {

                var model1 = _context.isonite_temp.Where(x => x.BarCode == Barcode.Trim() && x.user_create == User).FirstOrDefault();

                int Minute = (DateTime.Now - model1.CreateDate).Minutes;

                if (Minute > 10)
                {
                    var model_temp = _context.isonite_temp.Where(x => x.BarCode == Barcode.Trim() && x.user_create == User);
                    _context.isonite_temp.RemoveRange(model_temp);
                    _context.SaveChanges();

                    var WoRouting = _context.WoRouting.Where(x => x.BarCode == Barcode.Trim() && x.ProcessCode == "ISONITE").FirstOrDefault();
                    WoRouting.QTYinProcess = 0;
                    _context.SaveChanges();
                }

            }

            //----------------------------------------------------------------------------------------------------------------
            var style = NumberStyles.AllowDecimalPoint | NumberStyles.AllowThousands;
            var provider = new CultureInfo("fr-FR");
            if (LotSize.size == null)
            {
                return new JsonResult(new { Msg = "Error", des = "Process Error: " + Barcode + " is RackQty Equal to 0 Please check in itemuserdef5." });
            }
            var nQty = Decimal.Parse(LotSize.size, style, provider);//จำนวน lot

            if (LotSize.size == null || nQty == 0)
            {
                return new JsonResult(new { Msg = "Error", des = "Process Error: " + Barcode + " is RackQty Equal to 0 Please check in itemuserdef5." });
            }

            if (LotSize.stdLot == 0)
            {
                return new JsonResult(new { Msg = "Error", des = "Process Error: " + Barcode + " is LotSize Equal to 0 Please check in Std Lot size." });
            }
            //-----------------------------------------------------------------------------------------------------------------------

            if (_context.isonite_temp.Any(item => item.user_create == User && item.Token == UserToken.Token))
            {
                //--------------------หาค่าใน Temp ว่ามี Barcode ไหม------------------------------------------------------------------------------
                if (_context.isonite_temp.Any(item => item.BarCode == Barcode.Trim() && item.user_create == User && item.Token == UserToken.Token))
                {
                    //var temp = _context.isonite_temp.FirstOrDefault(item => item.BarCode == Barcode.Trim() && item.user_create == User);
                    //temp.QtyComplete = qtyCom.Value;
                    //temp.CreateDate = DateTime.Now;
                    //_context.SaveChanges();

                }
                else
                {

                    if (_context.isonite_temp.Any(x => x.BarCode == Barcode.Trim() && x.user_create == User))
                    {
                        var model_temp = _context.isonite_temp.Where(x => x.BarCode == Barcode.Trim() && x.user_create == User);
                        _context.isonite_temp.RemoveRange(model_temp);
                        _context.SaveChanges();
                    }
                    else
                    {

                        var temp = new Isonite_temp
                        {
                            BarCode = Barcode.Trim(),
                            QtyComplete = qtyCom.Value,
                            QtyNG = 0,
                            user_create = User,
                            Token = UserToken.Token,
                            CreateDate = DateTime.Now

                        };

                        _context.isonite_temp.Add(temp);
                        _context.SaveChanges();
                    }

                }
            }
            else
            {
                var model_del = _context.isonite_temp.Where(x => x.user_create == User && x.Token != UserToken.Token);
                _context.isonite_temp.RemoveRange(model_del);
                _context.SaveChanges();

                //var temp = new Isonite_temp
                //{
                //    BarCode = Barcode.Trim(),
                //    QtyComplete = qtyCom.Value,
                //    QtyNG = 0,
                //    user_create = User,
                //    Token = UserToken.Token,
                //    CreateDate = DateTime.Now

                //};

                //_context.isonite_temp.Add(temp);
                //_context.SaveChanges();

            }

            //----------------------------------------------------------------------------------------------------------


            if (isonite.QTYinProcess == null || isonite.QTYinProcess < 0)
            {
                // isonite.QTYinProcess = 0;
                var WoRouting = _context.WoRouting.Where(x => x.BarCode == Barcode.Trim() && x.ProcessCode == "ISONITE").FirstOrDefault();
                WoRouting.QTYinProcess = 0;
                _context.SaveChanges();
            }

            if (_context.isonite_temp.Any(x => x.BarCode == Barcode))
            {
                var sumTemp = _context.isonite_temp.Where(x => x.BarCode == Barcode).GroupBy(o => o.BarCode).Select(x => new
                {
                    Barcode = x.Key,
                    SumQty = x.Sum(i => i.QtyComplete + i.QtyNG)

                }).FirstOrDefault();

                if (sumTemp.SumQty != null)
                {
                    if (qtyCom.Value - isonite.QTYinProcess <= sumTemp.SumQty)
                    {
                        //isonite.QTYinProcess = sumTemp.SumQty;
                        //_context.SaveChanges();


                    }
                    else
                    {
                        var temp = _context.isonite_temp.FirstOrDefault(item => item.BarCode == Barcode.Trim() && item.user_create == User && item.Token == UserToken.Token);
                        temp.QtyComplete = temp.QtyComplete - qtyCom.Value;
                        _context.SaveChanges();

                        var WoRouting = _context.WoRouting.Where(x => x.BarCode == Barcode.Trim() && x.ProcessCode == "ISONITE").FirstOrDefault();
                        WoRouting.QTYinProcess = 0;
                        _context.SaveChanges();

                        return new JsonResult(new { Msg = "Error", des = "Process Error: " + Barcode + " is Qty in Process have more than Qty Complete." });
                    }
                }
            }

            decimal lotQty = qtyCom.Value / nQty;//หาจำนวนช่อง

            List<Isonie_PopupEdit> IsoniteResponse = new List<Isonie_PopupEdit>();

            int QtyGen = (int)nQty;

            var WIPQty = qtyCom.Value - (int)Math.Ceiling((lotQty * nQty));//คำนวนหาค่า WIPQty

            var m_jig = _context.m_Jig.Where(n => n.JigNo == JigNO).Select(i => new
            {
                size = i.Column * i.Row

            }).FirstOrDefault();

            if (m_jig.size != null)
            {
                if ((int)Math.Ceiling(lotQty) == 1)
                {
                    IsoniteResponse.Add(new Isonie_PopupEdit
                    {
                        IsoniteNo = "",
                        SerialNo = Barcode.Trim(),
                        Model = model.Model,
                        WIPQty = (int)WIPQty,
                        ConfirmedQty = qtyCom.Value,
                        NgQty = 0

                    });
                }
                else
                {
                    for (int i = 1; i <= (int)Math.Ceiling(lotQty); i++)
                    {
                        if (i <= m_jig.size)
                        {
                            if (i == (int)Math.Ceiling(lotQty) && (int)Math.Ceiling(lotQty) != lotQty)
                            {
                                QtyGen = qtyCom.Value - (((int)Math.Ceiling(lotQty) - 1) * (int)Math.Ceiling(nQty));
                            }

                            IsoniteResponse.Add(new Isonie_PopupEdit
                            {
                                IsoniteNo = i.ToString(),
                                SerialNo = Barcode.Trim(),
                                Model = model.Model,
                                WIPQty = (int)WIPQty,
                                ConfirmedQty = QtyGen,
                                NgQty = 0

                            });

                            if (i == (int)Math.Ceiling(lotQty)) {

                                var isonite_temp = _context.isonite_temp.Where(x => x.BarCode == Barcode && x.user_create == User && x.Token == UserToken.Token).GroupBy(o => o.BarCode).Select(x => new
                                {
                                    Barcode = x.Key,
                                    SumQty = x.Sum(j => j.QtyComplete + j.QtyNG)
                                }).FirstOrDefault();

                                var _model = _context.isonite_temp.FirstOrDefault(item => item.BarCode == Barcode);
                                _model.QtyComplete = isonite_temp.SumQty + qtyCom.Value;
                                _model.QtyNG = 0;
                                _context.SaveChanges();

                                var SumQTYinProcess = _context.isonite_temp.Where(x => x.BarCode == Barcode).GroupBy(o => o.BarCode).Select(x => new
                                {
                                    Barcode = x.Key,
                                    SumQty = x.Sum(c => c.QtyComplete + c.QtyNG)
                                }).FirstOrDefault();

                                var woRouting = _context.WoRouting.Where(item => item.BarCode == Barcode && item.ProcessCode == "ISONITE").FirstOrDefault();
                                woRouting.QTYinProcess = SumQTYinProcess.SumQty;

                                _context.SaveChanges();
                            }

                        }
                        else
                        {
                            if (i == (int)Math.Ceiling(lotQty) && (int)Math.Ceiling(lotQty) != lotQty)
                            {
                                QtyGen = qtyCom.Value - (((int)Math.Ceiling(lotQty) - 1) * (int)Math.Ceiling(nQty));
                            }

                            int n = QtyGen * m_jig.size;

                            UserToken = _context.UserTransaction.FirstOrDefault(x => x.UserName == User);

                            var isonite_temp = _context.isonite_temp.Where(x => x.BarCode == Barcode && x.user_create == User && x.Token == UserToken.Token).GroupBy(o => o.BarCode).Select(x => new
                            {
                                Barcode = x.Key,
                                SumQty = x.Sum(j => j.QtyComplete + j.QtyNG)
                            }).FirstOrDefault();

                            var _model = _context.isonite_temp.FirstOrDefault(item => item.BarCode == Barcode);
                            if (isonite_temp == null)
                            {
                                var temp = new Isonite_temp
                                {
                                    BarCode = Barcode.Trim(),
                                    QtyComplete = n,
                                    QtyNG = 0,
                                    user_create = User,
                                    Token = UserToken.Token,
                                    CreateDate = DateTime.Now

                                };

                                _context.isonite_temp.Add(temp);
                                _context.SaveChanges();

                                var woRouting = _context.WoRouting.Where(item => item.BarCode == Barcode && item.ProcessCode == "ISONITE").FirstOrDefault();
                                woRouting.QTYinProcess =  n;

                                _context.SaveChanges();
                            }
                            else
                            {
                                _model.QtyComplete = isonite_temp.SumQty + n;
                                _model.QtyNG = 0;
                                _context.SaveChanges();

                                var SumQTYinProcess = _context.isonite_temp.Where(x => x.BarCode == Barcode).GroupBy(o => o.BarCode).Select(x => new
                                {
                                    Barcode = x.Key,
                                    SumQty = x.Sum(c => c.QtyComplete + c.QtyNG)
                                }).FirstOrDefault();

                                var woRouting = _context.WoRouting.Where(item => item.BarCode == Barcode && item.ProcessCode == "ISONITE").FirstOrDefault();
                                woRouting.QTYinProcess = SumQTYinProcess.SumQty ;

                                _context.SaveChanges();
                            }

                            break;
                        }

                    }
                }

            }

            return new JsonResult(new { Msg = "ok", des = "", data = IsoniteResponse });

        }

        public IActionResult DeleteIsonniteTemp(string Barcode, int Qty, string User, string JigID)
        {
            //get Token

            var UserToken = _context.UserTransaction.FirstOrDefault(x => x.UserName == User);

            var isonite_temp = _context.isonite_temp.Where(x => x.BarCode == Barcode && x.user_create == User && x.Token == UserToken.Token).GroupBy(o => o.BarCode).Select(x => new
            {
                Barcode = x.Key,
                SumQty = x.Sum(i => i.QtyComplete + i.QtyNG)
            }).FirstOrDefault();

            var model = _context.isonite_temp.FirstOrDefault(item => item.BarCode == Barcode);
            if (isonite_temp == null)
            {
                model.QtyComplete = Qty;
            }
            else
            {
                model.QtyComplete = isonite_temp.SumQty - Qty;
            }
            model.QtyNG = 0;


            var SumQTYinProcess = _context.isonite_temp.Where(x => x.BarCode == Barcode).GroupBy(o => o.BarCode).Select(x => new
            {
                Barcode = x.Key,
                SumQty = x.Sum(i => i.QtyComplete + i.QtyNG)
            }).FirstOrDefault();

            var woRouting = _context.WoRouting.Where(item => item.BarCode == Barcode && item.ProcessCode == "ISONITE").FirstOrDefault();
            woRouting.QTYinProcess = SumQTYinProcess.SumQty;

            //_context.isonite_temp.Add(model);

            _context.SaveChangesAsync();

            return new JsonResult(new { Msg = "ok", des = "" });

        }


        public string getIsoniteID()
        {
            char[] alpha = "ABCDEFGHIJKLMNOPQRSTUVWXYZ".ToCharArray();

            var pattern = _context.s_GlobalPams.Where(x => x.parm_key == "Isonite").Select(x => new
            {
                x.param_value

            }).FirstOrDefault();

            var next_num = _context.m_NextNumber.Where(x => x.FieldName == "IsoniteCode").FirstOrDefault();

            var next_str = _context.m_NextNumber.Where(x => x.FieldName == "IsoniteCode_str").FirstOrDefault();

            string sub = pattern.param_value.ToString().Substring(0, 1);

            var isoniteCode = "";
            int n = next_str.NextNumber;

            if (next_num.NextNumber == 99999)
            {
                isoniteCode = sub + alpha[n + 1] + "-" + (1).ToString().PadLeft(5, '0');

                var model = _context.m_NextNumber.Where(x => x.FieldName == "IsoniteCode").FirstOrDefault();
                model.NextNumber = 1;
                _context.SaveChanges();

                var model_str = _context.m_NextNumber.Where(x => x.FieldName == "IsoniteCode_str").FirstOrDefault();
                model_str.NextNumber = n + 1;
                _context.SaveChanges();
            }
            else
            {
                isoniteCode = sub + alpha[n] + "-" + (next_num.NextNumber).ToString().PadLeft(5, '0');

            }

            return isoniteCode;
        }

        private string getjigNo(string JigID)
        {
            if (JigID != "")
            {
                var _Jig = _context.m_Jig.Where(x => x.JigID == Convert.ToInt32(JigID)).FirstOrDefault();
                return _Jig.JigNo;
            }
            else
            {
                return "";
            }

        }

        [HttpPost]
        public IActionResult SaveIsonite([FromBody] JObject jsonResult)
        {


            var s1 = jsonResult.Values().ToList();

            var header = ((Newtonsoft.Json.Linq.JValue)s1[0]).Value;

            var detail = ((Newtonsoft.Json.Linq.JValue)s1[1]).Value;

            var user = ((Newtonsoft.Json.Linq.JValue)s1[2]).Value;

            JObject _header = JObject.Parse(header.ToString());

            JObject _detail = JObject.Parse(detail.ToString());

            var CodeIsonite = getIsoniteID();

            var Isonite_herder = _header.GetValue("Header").ToList();

            List<Isonite> listIsoniteHeader = new List<Isonite>();
            List<Isonite_Line> listIsoniteLine = new List<Isonite_Line>();

            foreach (JToken herder in Isonite_herder)
            {
                if (Convert.ToString(herder.SelectToken("IsoniteCode")) == "")
                {
                    listIsoniteHeader.Add(new Isonite
                    {
                        //IsoniteCode = CodeIsonite,
                        BPCodeFrom = Convert.ToString(herder.SelectToken("BPCodeFrom")),
                        BPCodeTO = Convert.ToString(herder.SelectToken("BPCodeTO")),
                        JigNo1 = Convert.ToString(herder.SelectToken("JigNo1")),
                        JigNo2 = Convert.ToString(herder.SelectToken("JigNo2")),
                        JigNo3 = Convert.ToString(herder.SelectToken("JigNo3")),
                        DocDate = Convert.ToDateTime(herder.SelectToken("DocDate")),
                        DocStatus = "P",
                        CreateDate = DateTime.Now,
                        TransDate = DateTime.Now,
                        ModifyBy = Convert.ToString(user)
                    });
                }
                else
                {
                    listIsoniteHeader.Add(new Isonite
                    {
                        IsoniteCode = Convert.ToString(herder.SelectToken("IsoniteCode")),
                        BPCodeFrom = Convert.ToString(herder.SelectToken("BPCodeFrom")),
                        BPCodeTO = Convert.ToString(herder.SelectToken("BPCodeTO")),
                        JigNo1 = Convert.ToString(herder.SelectToken("JigNo1")),
                        JigNo2 = Convert.ToString(herder.SelectToken("JigNo2")),
                        JigNo3 = Convert.ToString(herder.SelectToken("JigNo3")),
                        DocDate = Convert.ToDateTime(herder.SelectToken("DocDate")),
                        DocStatus = "P",
                        CreateDate = DateTime.Now,
                        TransDate = DateTime.Now,
                        ModifyBy = Convert.ToString(user)
                    });

                }

            }

            var Isonite_detail = _detail.GetValue("Detail").ToList();

            foreach (JToken _idetail in Isonite_detail)
            {
                if (Convert.ToString(_idetail.SelectToken("IsoniteCode")) == "")
                {
                    if (Convert.ToString(_idetail.SelectToken("Confirm_Qty")) == "")
                    {
                        listIsoniteLine.Add(new Isonite_Line
                        {
                            //IsoniteCode = CodeIsonite,
                            JigNo = Convert.ToString(_idetail.SelectToken("JigNo")),
                            JigAddress = Convert.ToInt32(_idetail.SelectToken("JigAddress")),
                            BarCode = Convert.ToString(_idetail.SelectToken("BarCode")),
                            TransType = "Sent",
                            Qty = Convert.ToInt32(_idetail.SelectToken("Qty")),
                            CreateDate = DateTime.Now,
                            TransDate = DateTime.Now,
                            Sentdate = DateTime.Now,
                            SentBy = Convert.ToString(user)
                        });
                    }
                    else
                    {
                        listIsoniteLine.Add(new Isonite_Line
                        {
                            //IsoniteCode = CodeIsonite,
                            JigNo = Convert.ToString(_idetail.SelectToken("JigNo")),
                            JigAddress = Convert.ToInt32(_idetail.SelectToken("JigAddress")),
                            BarCode = Convert.ToString(_idetail.SelectToken("BarCode")),
                            TransType = "Sent",
                            Qty = Convert.ToInt32(_idetail.SelectToken("Confirm_Qty")),
                            CreateDate = DateTime.Now,
                            TransDate = DateTime.Now,
                            Sentdate = DateTime.Now,
                            SentBy = Convert.ToString(user)
                        });
                    }

                }
                else
                {
                    if (Convert.ToString(_idetail.SelectToken("Confirm_Qty")) == "")
                    {
                        listIsoniteLine.Add(new Isonite_Line
                        {
                            IsoniteCode = Convert.ToString(_idetail.SelectToken("IsoniteCode")),
                            JigNo = Convert.ToString(_idetail.SelectToken("JigNo")),
                            JigAddress = Convert.ToInt32(_idetail.SelectToken("JigAddress")),
                            BarCode = Convert.ToString(_idetail.SelectToken("BarCode")),
                            TransType = "Sent",
                            Qty = Convert.ToInt32(_idetail.SelectToken("Qty")),
                            CreateDate = DateTime.Now,
                            TransDate = DateTime.Now,
                            Sentdate = DateTime.Now,
                            SentBy = Convert.ToString(user)
                        });
                    }
                    else
                    {
                        listIsoniteLine.Add(new Isonite_Line
                        {
                            IsoniteCode = Convert.ToString(_idetail.SelectToken("IsoniteCode")),
                            JigNo = Convert.ToString(_idetail.SelectToken("JigNo")),
                            JigAddress = Convert.ToInt32(_idetail.SelectToken("JigAddress")),
                            BarCode = Convert.ToString(_idetail.SelectToken("BarCode")),
                            TransType = "Sent",
                            Qty = Convert.ToInt32(_idetail.SelectToken("Confirm_Qty")),
                            CreateDate = DateTime.Now,
                            TransDate = DateTime.Now,
                            Sentdate = DateTime.Now,
                            SentBy = Convert.ToString(user)
                        });
                    }
                }

            }

            foreach (var data in listIsoniteHeader)
            {
                //var model = _context.Isonite.Where(x => x.IsoniteCode == data.IsoniteCode).FirstOrDefault();

                var _Jig1 = _context.m_Jig.Where(x => x.JigNo == data.JigNo1).FirstOrDefault();

                if (_Jig1 != null)
                {
                    _Jig1.jig_isonite_status = "unUse";
                    _context.SaveChanges();
                }

                var _Jig2 = _context.m_Jig.Where(x => x.JigNo == data.JigNo2).FirstOrDefault();

                if (_Jig2 != null)
                {
                    _Jig2.jig_isonite_status = "unUse";
                    _context.SaveChanges();

                }

                var _Jig3 = _context.m_Jig.Where(x => x.JigNo == data.JigNo3).FirstOrDefault();
                if (_Jig3 != null)
                {
                    _Jig3.jig_isonite_status = "unUse";
                    _context.SaveChanges();
                }

            }

            foreach (var data in listIsoniteHeader)
            {
                if (_context.Isonite.Any(x => x.IsoniteCode == data.IsoniteCode))
                {
                    var CountResive = (from i in _context.Isonite_Line
                                       where i.TransType == "Receive" && i.IsoniteCode == data.IsoniteCode
                                       group i by i.IsoniteCode into Group
                                       select new
                                       {
                                           isoniteCode = Group.Key,
                                           Count = Group.Count(),
                                       }).ToList();

                    if (CountResive.Count > 0)
                    {
                        return new JsonResult(new { Msg = "Error", des = "Error: This Isonite cannot be edited, because material has been received!" });
                    }
                }
            }

            foreach (var data in listIsoniteHeader)
            {
                if (_context.Isonite.Any(x => x.IsoniteCode == data.IsoniteCode))
                {

                    if (_context.m_Jig.Any(x => x.JigNo == data.JigNo1
                    && x.jig_isonite_status == "Use"))
                    {
                        return new JsonResult(new { Msg = "Error", des = "Error: Jig " + data.JigNo1 + " already use." });
                    }
                    else if (_context.m_Jig.Any(x => x.JigNo == data.JigNo2
                    && x.jig_isonite_status == "Use"))
                    {
                        return new JsonResult(new { Msg = "Error", des = "Error: Jig " + data.JigNo2 + " already use." });
                    }
                    else if (_context.m_Jig.Any(x => x.JigNo == data.JigNo3
                    && x.jig_isonite_status == "Use"))
                    {
                        return new JsonResult(new { Msg = "Error", des = "Error: Jig " + data.JigNo3 + " already use." });
                    }

                    var model = _context.Isonite.Where(x => x.IsoniteCode == data.IsoniteCode).FirstOrDefault();
                    model.BPCodeFrom = data.BPCodeFrom;
                    model.BPCodeTO = data.BPCodeTO;
                    model.DocDate = data.DocDate;
                    model.JigNo1 = data.JigNo1;
                    model.JigNo2 = data.JigNo2;
                    model.JigNo3 = data.JigNo3;

                    _context.SaveChanges();

                    var model_line = _context.Isonite_Line.Where(x => x.IsoniteCode == data.IsoniteCode).ToList();
                    _context.Isonite_Line.RemoveRange(model_line);
                    _context.SaveChanges();

                    var _Jig1 = _context.m_Jig.Where(x => x.JigNo == data.JigNo1).FirstOrDefault();

                    if (_Jig1 != null)
                    {
                        _Jig1.jig_isonite_status = "Use";
                        _context.SaveChanges();
                    }

                    var _Jig2 = _context.m_Jig.Where(x => x.JigNo == data.JigNo2).FirstOrDefault();

                    if (_Jig2 != null)
                    {
                        _Jig2.jig_isonite_status = "Use";
                        _context.SaveChanges();

                    }

                    var _Jig3 = _context.m_Jig.Where(x => x.JigNo == data.JigNo2).FirstOrDefault();
                    if (_Jig3 != null)
                    {
                        _Jig3.jig_isonite_status = "Use";
                        _context.SaveChanges();
                    }

                }
                else
                {
                    if (_context.m_Jig.Any(x => x.JigNo == data.JigNo1
                   && x.jig_isonite_status == "Use"))
                    {
                        return new JsonResult(new { Msg = "Error", des = "Error: Jig " + data.JigNo1 + " already use." });
                    }
                    else if (_context.m_Jig.Any(x => x.JigNo == data.JigNo2
                    && x.jig_isonite_status == "Use"))
                    {
                        return new JsonResult(new { Msg = "Error", des = "Error: Jig " + data.JigNo2 + " already use." });
                    }
                    else if (_context.m_Jig.Any(x => x.JigNo == data.JigNo3
                    && x.jig_isonite_status == "Use"))
                    {
                        return new JsonResult(new { Msg = "Error", des = "Error: Jig " + data.JigNo3 + " already use." });
                    }

                    var row = new Isonite
                    {
                        IsoniteCode = CodeIsonite,
                        BPCodeFrom = data.BPCodeFrom,
                        BPCodeTO = data.BPCodeTO,
                        JigNo1 = data.JigNo1,
                        JigNo2 = data.JigNo2,
                        JigNo3 = data.JigNo3,
                        DocDate = data.DocDate,
                        DocStatus = data.DocStatus,
                        CreateDate = data.CreateDate,
                        TransDate = data.TransDate,
                        ModifyBy = data.ModifyBy
                    };

                    _context.Isonite.Add(row);
                    _context.SaveChanges();

                    var next_num = _context.m_NextNumber.Where(x => x.FieldName == "IsoniteCode").FirstOrDefault();

                    var model = _context.m_NextNumber.Where(x => x.FieldName == "IsoniteCode").FirstOrDefault();
                    model.NextNumber = next_num.NextNumber + 1;
                    _context.SaveChanges();

                    var _Jig1 = _context.m_Jig.Where(x => x.JigNo == data.JigNo1).FirstOrDefault();

                    if (_Jig1 != null)
                    {
                        _Jig1.jig_isonite_status = "Use";
                        _context.SaveChanges();
                    }

                    var _Jig2 = _context.m_Jig.Where(x => x.JigNo == data.JigNo2).FirstOrDefault();

                    if (_Jig2 != null)
                    {
                        _Jig2.jig_isonite_status = "Use";
                        _context.SaveChanges();

                    }

                    var _Jig3 = _context.m_Jig.Where(x => x.JigNo == data.JigNo3).FirstOrDefault();
                    if (_Jig3 != null)
                    {
                        _Jig3.jig_isonite_status = "Use";
                        _context.SaveChanges();
                    }
                }
            }

            var Code = "";

            foreach (var data in listIsoniteLine)
            {
                if (data.IsoniteCode == null)
                {
                    if (data.BarCode != null)
                    {
                        Code = CodeIsonite;
                        var row = new Isonite_Line
                        {
                            IsoniteCode = CodeIsonite,
                            JigNo = data.JigNo,
                            JigAddress = data.JigAddress,
                            BarCode = data.BarCode,
                            TransType = data.TransType,
                            Qty = data.Qty,
                            RefIsoniteCode = data.RefIsoniteCode,
                            RefJigAddress = data.RefJigAddress,
                            CreateDate = data.CreateDate,
                            TransDate = data.TransDate,
                            Sentdate = data.Sentdate,
                            Receivedate = data.Receivedate,
                            SentBy = data.SentBy,
                            ReceiveBy = data.ReceiveBy

                        };

                        _context.Isonite_Line.Add(row);
                        _context.SaveChanges();
                    }
                }
                else
                {
                    if (data.BarCode != null)
                    {
                        Code = data.IsoniteCode;
                        var row = new Isonite_Line
                        {
                            IsoniteCode = data.IsoniteCode,
                            JigNo = data.JigNo,
                            JigAddress = data.JigAddress,
                            BarCode = data.BarCode,
                            TransType = data.TransType,
                            Qty = data.Qty,
                            RefIsoniteCode = data.RefIsoniteCode,
                            RefJigAddress = data.RefJigAddress,
                            CreateDate = data.CreateDate,
                            TransDate = data.TransDate,
                            Sentdate = data.Sentdate,
                            Receivedate = data.Receivedate,
                            SentBy = data.SentBy,
                            ReceiveBy = data.ReceiveBy

                        };

                        _context.Isonite_Line.Add(row);
                        _context.SaveChanges();
                    }
                }

            }

            var User = _context.UserTransaction.FirstOrDefault(x => x.UserName == Convert.ToString(user));

            CancelIsonniteTemp(User.UserName);

            var DelTemp = _context.isonite_temp.Where(x => x.user_create == User.UserName && x.Token == User.Token);
            _context.isonite_temp.RemoveRange(DelTemp);
            _context.SaveChanges();

            return new JsonResult(new { Msg = "OK", des = "Save Isonite successful IsoniteNO:" + Code });

        }

        //public IActionResult CheckBarCode(string Barcode, string User)
        //{

        //    int? qtyCom;

        //    int? SumQtyLine = 0;
        //    //get Token

        //    var UserToken = _context.UserTransaction.FirstOrDefault(x => x.UserName == User);

        //    //Query WoRouting
        //    var BarCodeQuery = _context.WoRouting.Where(n => n.BarCode == Barcode.Trim()).OrderBy(x => x.BarCode).ThenBy(x => x.OperationNo).ToList();

        //    var isonite = BarCodeQuery.Where(x => x.ProcessCode == "ISONITE").FirstOrDefault();

        //    if (isonite == null)
        //    {
        //        return new JsonResult(new { Msg = "Error", des = "Process Error: " + Barcode + " is not Isonite process." });
        //    }

        //    //------------------------------------------------------------------------------------------------------------------------------------
        //    var LotSize = (from i in _context.WeeklyPlan
        //                   join x in _context.m_Resource
        //                   on i.ItemCode equals x.ItemCode
        //                   where i.BarCode == Barcode.Trim()
        //                   select new
        //                   {
        //                       size = x.ItemUserDef5
        //                   }).FirstOrDefault();

        //    //------------------------------------------------------------------------------------------------------------------------------------------

        //    var Process = BarCodeQuery.Where(x => x.OperationNo < isonite.OperationNo && x.MainProcessFlag == "Y").OrderByDescending(x => x.OperationNo).FirstOrDefault();

        //    if (Process == null)
        //    {
        //        return new JsonResult(new { Msg = "Error", des = "Process Error: " + Barcode + ", You can not report for process [Isonite] due to prior process does not complete!" });
        //    }

        //    //-------------------หา Model ------------------------------------------------------------------------------------------------------------------

        //    var model = _context.WeeklyPlan.Where(x => x.BarCode == Barcode.Trim()).FirstOrDefault();

        //    //--------------------------------------------------------------------------------------------------------------------------------------
        //    if (Process.QtyComplete != null && Process.QtyComplete != 0)
        //    {

        //        if (_context.Isonite_Line.Any(x => x.BarCode == Barcode.Trim()))
        //        {
        //            var isonite_line = _context.Isonite_Line.Where(x => x.BarCode == Barcode && x.TransType == "Sent").GroupBy(o => o.BarCode).Select(x => new
        //            {
        //                Barcode = x.Key,
        //                SumQty = x.Sum(i => i.Qty)
        //            }).FirstOrDefault();

        //            SumQtyLine = isonite_line.SumQty;

        //        }

        //        //Check isonite in isonite temp
        //        if (_context.isonite_temp.Any(x => x.BarCode == Barcode.Trim()))
        //        {
        //            var isonite_temp = _context.isonite_temp.Where(x => x.BarCode == Barcode).GroupBy(o => o.BarCode).Select(x => new
        //            {
        //                Barcode = x.Key,
        //                SumQty = x.Sum(i => i.QtyComplete + i.QtyNG)
        //            }).FirstOrDefault();

        //            qtyCom = Process.QtyComplete - Process.QtyNG - (isonite.QtyComplete + isonite.QtyNG + isonite_temp.SumQty) - SumQtyLine;//มีข้อมูล ใน Temp

        //            if (qtyCom == 0) return new JsonResult(new { Msg = "Error", des = "Process Error: " + Barcode + " is Qty Equal to 0." });

        //        }
        //        else
        //        {
        //            qtyCom = Process.QtyComplete - Process.QtyNG - (isonite.QtyComplete + isonite.QtyNG) - SumQtyLine;//
        //            if (qtyCom == 0) return new JsonResult(new { Msg = "Error", des = "Process Error: " + Barcode + " is Qty Equal to 0." });
        //        }
        //    }
        //    else
        //    {
        //        return new JsonResult(new { Msg = "Error", des = "Process Oparation : " + Process.OperationNo + "/" + Process.ProcessCode + " is 0. " });
        //    }

        //    if (_context.isonite_temp.Any(item => item.BarCode == Barcode.Trim()))
        //    {

        //        var model1 = _context.isonite_temp.Where(x => x.BarCode == Barcode.Trim() && x.user_create == User).FirstOrDefault();

        //        int Minute = (DateTime.Now - model1.CreateDate).Minutes;

        //        if (Minute > 3)
        //        {
        //            var model_temp = _context.isonite_temp.Where(x => x.BarCode == Barcode.Trim() && x.user_create == User);
        //            _context.isonite_temp.RemoveRange(model_temp);
        //            _context.SaveChanges();

        //            var WoRouting = _context.WoRouting.Where(x => x.BarCode == Barcode.Trim() && x.ProcessCode == "ISONITE").FirstOrDefault();
        //            WoRouting.QTYinProcess = 0;
        //            _context.SaveChanges();
        //        }

        //    }

        //    if (_context.isonite_temp.Any(item => item.user_create == User && item.Token == UserToken.Token))
        //    {
        //        //--------------------หาค่าใน Temp ว่ามี Barcode ไหม------------------------------------------------------------------------------
        //        if (_context.isonite_temp.Any(item => item.BarCode == Barcode.Trim() && item.user_create == User && item.Token == UserToken.Token))
        //        {
        //            var temp = _context.isonite_temp.FirstOrDefault(item => item.BarCode == Barcode.Trim() && item.user_create == User);
        //            temp.QtyComplete = temp.QtyComplete + qtyCom.Value;
        //            temp.CreateDate = DateTime.Now;
        //            _context.SaveChanges();

        //        }
        //        else
        //        {

        //            if (_context.isonite_temp.Any(x => x.BarCode == Barcode.Trim() && x.user_create == User))
        //            {
        //                var model_temp = _context.isonite_temp.Where(x => x.BarCode == Barcode.Trim() && x.user_create == User);
        //                _context.isonite_temp.RemoveRange(model_temp);
        //                _context.SaveChanges();
        //            }
        //            else
        //            {
        //                var temp = new Isonite_temp
        //                {
        //                    BarCode = Barcode.Trim(),
        //                    QtyComplete = qtyCom.Value,
        //                    QtyNG = 0,
        //                    user_create = User,
        //                    Token = UserToken.Token,
        //                    CreateDate = DateTime.Now

        //                };

        //                _context.isonite_temp.Add(temp);
        //                _context.SaveChanges();
        //            }

        //        }
        //    }
        //    else
        //    {
        //        var model_del = _context.isonite_temp.Where(x => x.user_create == User && x.Token != UserToken.Token);
        //        _context.isonite_temp.RemoveRange(model_del);
        //        _context.SaveChanges();

        //        var temp = new Isonite_temp
        //        {
        //            BarCode = Barcode.Trim(),
        //            QtyComplete = qtyCom.Value,
        //            QtyNG = 0,
        //            user_create = User,
        //            Token = UserToken.Token,
        //            CreateDate = DateTime.Now

        //        };

        //        _context.isonite_temp.Add(temp);
        //        _context.SaveChanges();

        //    }

        //    //----------------------------------------------------------------------------------------------------------
        //    var sumTemp = _context.isonite_temp.Where(x => x.BarCode == Barcode).GroupBy(o => o.BarCode).Select(x => new
        //    {
        //        Barcode = x.Key,
        //        SumQty = x.Sum(i => i.QtyComplete + i.QtyNG)

        //    }).FirstOrDefault();

        //    if (isonite.QTYinProcess == null)
        //    {
        //        isonite.QTYinProcess = 0;
        //    }

        //    if (sumTemp.SumQty <= qtyCom.Value - isonite.QTYinProcess)
        //    {
        //        isonite.QTYinProcess = sumTemp.SumQty;
        //        _context.SaveChanges();
        //    }
        //    else
        //    {
        //        var temp = _context.isonite_temp.FirstOrDefault(item => item.BarCode == Barcode.Trim() && item.user_create == User && item.Token == UserToken.Token);
        //        temp.QtyComplete = temp.QtyComplete - qtyCom.Value;
        //        _context.SaveChanges();

        //        return new JsonResult(new { Msg = "Error", des = "Process Error: " + Barcode + " is Qty in Process have more than Qty Complete." });
        //    }

        //    //----------------------------------------------------------------------------------------------------------------
        //    var style = NumberStyles.AllowDecimalPoint | NumberStyles.AllowThousands;
        //    var provider = new CultureInfo("fr-FR");

        //    var nQty = Decimal.Parse(LotSize.size, style, provider);//จำนวน lot


        //    if (LotSize.size == null || nQty == 0)
        //    {
        //        return new JsonResult(new { Msg = "Error", des = "Process Error: " + Barcode + " is LotSize Equal to 0." });
        //    }

        //    //-----------------------------------------------------------------------------------------------------------------------
        //    decimal lotQty = qtyCom.Value / nQty;//หาจำนวนช่อง

        //    List<Isonie_PopupEdit> IsoniteResponse = new List<Isonie_PopupEdit>();

        //    int QtyGen = (int)nQty;

        //    var WIPQty = qtyCom.Value - (int)Math.Ceiling((lotQty * nQty));//คำนวนหาค่า WIPQty

        //    if ((int)Math.Ceiling(lotQty) == 1)
        //    {
        //        IsoniteResponse.Add(new Isonie_PopupEdit
        //        {
        //            IsoniteNo = "",
        //            SerialNo = Barcode.Trim(),
        //            Model = model.Model,
        //            WIPQty = (int)WIPQty,
        //            ConfirmedQty = qtyCom.Value,
        //            NgQty = 0

        //        });
        //    }
        //    else
        //    {
        //        for (int i = 1; i <= (int)Math.Ceiling(lotQty); i++)
        //        {

        //            if (i == (int)Math.Ceiling(lotQty) && (int)Math.Ceiling(lotQty) != lotQty)
        //            {
        //                QtyGen = qtyCom.Value - (((int)Math.Ceiling(lotQty) - 1) * (int)Math.Ceiling(nQty));
        //            }

        //            IsoniteResponse.Add(new Isonie_PopupEdit
        //            {
        //                IsoniteNo = i.ToString(),
        //                SerialNo = Barcode.Trim(),
        //                Model = model.Model,
        //                WIPQty = (int)WIPQty,
        //                ConfirmedQty = QtyGen,
        //                NgQty = 0

        //            });
        //        }
        //    }

        //    return new JsonResult(new { Msg = "ok", des = "", data = IsoniteResponse });

        //}

        [HttpPut]
        public IActionResult EditIsonite(string key, string values)
        {
            var model = _context.Isonite.FirstOrDefault(item => item.IsoniteCode == key);
            if (model == null)
                return StatusCode(409, "Isonite not found");

            var _values = JsonConvert.DeserializeObject<IDictionary>(values);
            PopulateModel(model, _values);

            if (!TryValidateModel(model))
                return BadRequest(GetFullErrorMessage(ModelState));

            _context.SaveChanges();
            return Ok();
        }

        [HttpDelete]
        public void DeleteIsonite(string key)
        {
            var model = _context.Isonite.FirstOrDefault(item => item.IsoniteCode == key);

            var model_line = _context.Isonite_Line.FirstOrDefault(item => item.IsoniteCode == key);

            _context.Isonite.Remove(model);
            _context.Isonite_Line.Remove(model_line);

            _context.SaveChanges();
        }

        private void PopulateModel(Isonite model, IDictionary values)
        {
            string IsoniteCode = nameof(Isonite.IsoniteCode);
            string BPCodeFrom = nameof(Isonite.BPCodeFrom);
            string BPCodeTO = nameof(Isonite.BPCodeTO);
            string JigNo1 = nameof(Isonite.JigNo1);
            string JigNo2 = nameof(Isonite.JigNo2);
            string JigNo3 = nameof(Isonite.JigNo3);
            string DocDate = nameof(Isonite.DocDate);
            string DocStatus = nameof(Isonite.DocStatus);
            string CreateDate = nameof(Isonite.CreateDate);
            string TransDate = nameof(Isonite.TransDate);
            string ModifyBy = nameof(Isonite.ModifyBy);
            string IsoniteUserDef1 = nameof(Isonite.IsoniteUserDef1);
            string IsoniteUserDef2 = nameof(Isonite.IsoniteUserDef2);
            string IsoniteUserDef3 = nameof(Isonite.IsoniteUserDef3);


            if (values.Contains(IsoniteCode))
            {
                model.IsoniteCode = Convert.ToString(values[IsoniteCode]);
            }
            if (values.Contains(BPCodeFrom))
            {
                model.BPCodeFrom = Convert.ToString(values[BPCodeFrom]);
            }

            if (values.Contains(BPCodeTO))
            {
                model.BPCodeTO = Convert.ToString(values[BPCodeTO]);
            }

            if (values.Contains(JigNo1))
            {
                model.JigNo1 = Convert.ToString(values[JigNo1]);
            }

            if (values.Contains(JigNo2))
            {
                model.JigNo2 = Convert.ToString(values[JigNo2]);
            }

            if (values.Contains(JigNo3))
            {
                model.JigNo3 = Convert.ToString(values[JigNo3]);
            }

            if (values.Contains(DocDate))
            {
                model.DocDate = Convert.ToDateTime(values[DocDate]);
            }

            if (values.Contains(DocStatus))
            {
                model.DocStatus = Convert.ToString(values[DocStatus]);
            }

            if (values.Contains(CreateDate))
            {
                model.CreateDate = Convert.ToDateTime(values[CreateDate]);
            }

            if (values.Contains(TransDate))
            {
                model.TransDate = Convert.ToDateTime(values[TransDate]);
            }

            if (values.Contains(ModifyBy))
            {
                model.ModifyBy = Convert.ToString(values[ModifyBy]);
            }

            if (values.Contains(IsoniteUserDef1))
            {
                model.IsoniteUserDef1 = Convert.ToString(values[IsoniteUserDef1]);
            }

            if (values.Contains(IsoniteUserDef2))
            {
                model.IsoniteUserDef2 = Convert.ToString(values[IsoniteUserDef2]);
            }

            if (values.Contains(IsoniteUserDef3))
            {
                model.IsoniteUserDef3 = Convert.ToString(values[IsoniteUserDef3]);
            }
        }

        //private void PopulateModel_IsoniteLine(Isonite_Line model, IDictionary values)
        //{

        //    string IsoniteCode = nameof(Isonite_Line.IsoniteCode);
        //    string JigNo = nameof(Isonite_Line.JigNo);
        //    string JigAddress = nameof(Isonite_Line.JigAddress);
        //    string BarCode = nameof(Isonite_Line.BarCode);
        //    string TransType = nameof(Isonite_Line.TransType);
        //    string Qty = nameof(Isonite_Line.Qty);
        //    string RefIsoniteCode = nameof(Isonite_Line.RefIsoniteCode);
        //    string RefJigAddress = nameof(Isonite_Line.RefJigAddress);
        //    string CreateDate = nameof(Isonite_Line.CreateDate);
        //    string TransDate = nameof(Isonite_Line.TransDate);
        //    string ModifyBy = nameof(Isonite_Line.ModifyBy);
        //    string IsoniteLineUserDef1 = nameof(Isonite_Line.IsoniteLineUserDef1);
        //    string IsoniteLineUserDef2 = nameof(Isonite_Line.IsoniteLineUserDef2);
        //    string IsoniteLineUserDef3 = nameof(Isonite_Line.IsoniteLineUserDef3);


        //    if (values.Contains(IsoniteCode))
        //    {
        //        model.IsoniteCode = Convert.ToString(values[IsoniteCode]);
        //    }

        //    if (values.Contains(JigNo))
        //    {
        //        model.JigNo = Convert.ToString(values[JigNo]);
        //    }

        //    if (values.Contains(JigAddress))
        //    {
        //        model.JigAddress = Convert.ToInt32(values[JigAddress]);
        //    }

        //    if (values.Contains(BarCode))
        //    {
        //        model.BarCode = Convert.ToString(values[BarCode]);
        //    }

        //    if (values.Contains(TransType))
        //    {
        //        model.TransType = Convert.ToString(values[TransType]);
        //    }

        //    if (values.Contains(Qty))
        //    {
        //        model.Qty = Convert.ToInt32(values[Qty]);
        //    }

        //    if (values.Contains(RefIsoniteCode))
        //    {
        //        model.RefIsoniteCode = Convert.ToString(values[RefIsoniteCode]);
        //    }

        //    if (values.Contains(RefJigAddress))
        //    {
        //        model.RefJigAddress = Convert.ToInt32(values[RefJigAddress]);
        //    }

        //    if (values.Contains(CreateDate))
        //    {
        //        model.CreateDate = Convert.ToDateTime(values[CreateDate]);
        //    }

        //    if (values.Contains(TransDate))
        //    {
        //        model.TransDate = Convert.ToDateTime(values[TransDate]);
        //    }

        //    if (values.Contains(ModifyBy))
        //    {
        //        model.ModifyBy = Convert.ToString(values[ModifyBy]);
        //    }

        //    if (values.Contains(IsoniteLineUserDef1))
        //    {
        //        model.IsoniteLineUserDef1 = Convert.ToString(values[IsoniteLineUserDef1]);
        //    }

        //    if (values.Contains(IsoniteLineUserDef2))
        //    {
        //        model.IsoniteLineUserDef2 = Convert.ToString(values[IsoniteLineUserDef2]);
        //    }

        //    if (values.Contains(IsoniteLineUserDef3))
        //    {
        //        model.IsoniteLineUserDef3 = Convert.ToString(values[IsoniteLineUserDef3]);
        //    }
        //}


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

        [HttpGet]
        public void DeleteDataRefresh(string User)
        {

            //get Token
            if (_context.isonite_temp.Any(x => x.user_create == User))
            {
                var UserToken = _context.UserTransaction.FirstOrDefault(x => x.UserName == User);

                var model = _context.isonite_temp.Where(item => item.user_create == User && item.Token == UserToken.Token);

                var sumTemp = _context.isonite_temp.Where(x => x.user_create == User).GroupBy(o => o.BarCode).Select(x => new
                {
                    Barcode = x.Key,
                    SumQty = x.Sum(i => i.QtyComplete + i.QtyNG)

                }).ToList();

                foreach (var row in sumTemp)
                {
                    var woRouting = _context.WoRouting.FirstOrDefault(item => item.BarCode == row.Barcode && item.ProcessCode == "ISONITE");
                    woRouting.QTYinProcess = woRouting.QTYinProcess - row.SumQty;
                }


                if (model != null)
                {
                    _context.isonite_temp.RemoveRange(model);
                    _context.SaveChanges();
                }

            }

        }

        [HttpGet]
        public IActionResult GetSearchIsonite(string Isonite)
        {
            if (Isonite != "")
            {
                var Isonite_Header = _context.Isonite.Where(x => x.IsoniteCode == Isonite).ToList();

                return new JsonResult(new { Msg = "OK", Header = Isonite_Header });
            }
            else
            {
                return new JsonResult(new { Msg = "ERROR", des = "Message: Isonite Code is Null." });
            }


        }

        [HttpGet]
        public IActionResult GetIsoniteDetail(string Isonite, string JigNo)
        {
            if (Isonite != "")
            {
                //var Isonite_Detail = _context.Isonite_Line.Where(x => x.IsoniteCode == Isonite && x.JigNo == JigNo).ToList();

                var Isonite_Detail = (from detail in _context.Isonite_Line
                                      join Weekly in _context.WeeklyPlan
                                      on detail.BarCode equals Weekly.BarCode
                                      where detail.IsoniteCode == Isonite && detail.JigNo == JigNo
                                      && detail.TransType == "Sent"
                                      select new
                                      {
                                          detail.IsoniteCode,
                                          detail.JigNo,
                                          detail.JigAddress,
                                          detail.BarCode,
                                          Weekly.Model,
                                          detail.Qty
                                      }).ToList();


                return new JsonResult(new { Msg = "OK", Detail = Isonite_Detail });
            }
            else
            {
                return new JsonResult(new { Msg = "ERROR", des = "Message: Isonite Code is Null." });
            }
        }

        [HttpGet]
        public IActionResult BtnCancel(string User)
        {

            DeleteDataRefresh(User);
            return new JsonResult(new { Msg = "OK", des = "Message: Cancel Isonite successful." });
        }

        [HttpGet]
        public IActionResult BtnPrint(string IsoniteNo)
        {
            return new JsonResult("");
        }

        [HttpGet]
        public IActionResult BtnNew(string User)
        {
            DeleteDataRefresh(User);
            return new JsonResult(new { Msg = "OK", des = "" });
        }

        [HttpGet]
        public IActionResult BtnClear(string User)
        {
            DeleteDataRefresh(User);
            return new JsonResult(new { Msg = "OK", des = "" });
        }

        [HttpGet]
        public IActionResult btnDelete(string BarCode, string WipQty, string User)
        {
            var UserToken = _context.UserTransaction.FirstOrDefault(x => x.UserName == User);

            if (WipQty != "")
            {
                if (_context.isonite_temp.Any(x => x.BarCode == BarCode && x.user_create == User && x.Token == UserToken.Token))
                {
                    var model = _context.isonite_temp.Where(x => x.BarCode == BarCode && x.user_create == User && x.Token == UserToken.Token).FirstOrDefault();
                    model.QtyComplete = model.QtyComplete - Convert.ToInt32(WipQty);
                    _context.SaveChanges();
                }
                else
                {
                    var temp = new Isonite_temp
                    {
                        BarCode = BarCode.Trim(),
                        QtyComplete = Convert.ToInt32(WipQty) * (-1),
                        QtyNG = 0,
                        user_create = User,
                        Token = UserToken.Token,
                        CreateDate = DateTime.Now

                    };

                    _context.isonite_temp.Add(temp);
                    _context.SaveChanges();
                }

            }

            return new JsonResult(new { Msg = "OK", des = "" });
        }

        [HttpPost]
        public IActionResult BtnSum([FromBody]JObject jsonResult)
        {
            var s1 = jsonResult.Values().ToList();

            var detail = ((Newtonsoft.Json.Linq.JValue)s1[0]).Value;

            var user = ((Newtonsoft.Json.Linq.JValue)s1[1]).Value;


            JObject _detail = JObject.Parse(detail.ToString());

            //var CodeIsonite = getIsoniteID();

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

        [HttpGet]
        public IActionResult GetWoRouting(DataSourceLoadOptions loadOptions)
        {

            var isonite = _context.WoRouting.Where(x => x.ProcessCode == "ISONITE").ToList();

            var model1 = _context.isonite_temp.GroupBy(a => a.BarCode).ToList();

            var Process_temp = model1.SelectMany(a => a.Where(b => b.CreateDate.Minute <= (DateTime.Now - b.CreateDate).TotalMinutes)).ToList();

            var Process = (from detail in _context.WoRouting
                           join i in isonite
                           on detail.BarCode equals i.BarCode
                           where detail.OperationNo < i.OperationNo
                           && detail.MainProcessFlag == "Y"
                           orderby detail.OperationNo descending
                           select new
                           {
                               detail.BarCode,
                               detail.OperationNo,
                               SerialLot = detail.BarCode.Substring(9),
                               detail.QtyComplete,
                               detail.QtyNG

                           }).ToList();

            var groups = Process.GroupBy(a => (a.BarCode));

            var ProcessMax = groups.SelectMany(a => a.Where(b => b.OperationNo == a.Max(c => c.OperationNo))).ToList();

            var woRoutings = (from detail in ProcessMax
                              join Weekly in _context.WeeklyPlan
                              on detail.BarCode equals Weekly.BarCode into gj
                              from sub in gj.DefaultIfEmpty()
                              select new
                              {
                                  detail.BarCode,
                                  SerialLot = detail.BarCode.Substring(9),
                                  sub.Model,
                                  WIP_Qty = Convert.ToInt32(CheckWIPQty(detail.BarCode, Convert.ToInt32(detail.QtyComplete), Convert.ToInt32(detail.QtyNG)))

                              }).Where(x => x.WIP_Qty > 0).ToList();

            return Json(DataSourceLoader.Load(woRoutings, loadOptions));

        }

        public string CheckWIPQty(string Barcode, int QtyComplete, int QtyNG)
        {

            int? qtyCom = 0;

            int? SumQtyLine = 0;

            var isonite = _context.WoRouting.Where(x => x.ProcessCode == "ISONITE" && x.BarCode == Barcode).FirstOrDefault();

            if (_context.isonite_temp.Any(item => item.BarCode == Barcode.Trim()))
            {
                var model1 = _context.isonite_temp.Where(x => x.BarCode == Barcode.Trim()).FirstOrDefault();

                double Minute = (DateTime.Now - model1.CreateDate).TotalMinutes;

                if (Minute > 10)
                {
                    var model_temp = _context.isonite_temp.Where(x => x.BarCode == Barcode.Trim());
                    _context.isonite_temp.RemoveRange(model_temp);
                    _context.SaveChanges();

                    var WoRouting = _context.WoRouting.Where(x => x.BarCode == Barcode.Trim() && x.ProcessCode == "ISONITE").FirstOrDefault();
                    WoRouting.QTYinProcess = 0;
                    _context.SaveChanges();
                }

            }
            //--------------------------------------------------------------------------------------------------------------------------------------
            if (QtyComplete != 0)
            {

                if (_context.Isonite_Line.Any(x => x.BarCode == Barcode.Trim()))
                {
                    var isonite_line = _context.Isonite_Line.Where(x => x.BarCode == Barcode && x.TransType == "Sent").GroupBy(o => o.BarCode).Select(x => new
                    {
                        Barcode = x.Key,
                        SumQty = x.Sum(i => i.Qty)
                    }).FirstOrDefault();
                    if (isonite_line != null)
                    {
                        SumQtyLine = isonite_line.SumQty;

                    }
                }

                //Check isonite in isonite temp
                if (_context.isonite_temp.Any(x => x.BarCode == Barcode.Trim()))
                {
                    var isonite_temp = _context.isonite_temp.Where(x => x.BarCode == Barcode).GroupBy(o => o.BarCode).Select(x => new
                    {
                        Barcode = x.Key,
                        SumQty = x.Sum(i => i.QtyComplete + i.QtyNG)
                    }).FirstOrDefault();

                    qtyCom = QtyComplete - QtyNG - (isonite.QtyComplete + isonite_temp.SumQty) - SumQtyLine;//มีข้อมูล ใน Temp

                }
                else
                {
                    qtyCom = QtyComplete - QtyNG - (isonite.QtyComplete + isonite.QtyNG) - SumQtyLine;//

                }
            }

            return qtyCom.ToString();

        }

        public IActionResult CancelIsonniteTemp(string User)
        {
            //get Token

            var UserToken = _context.UserTransaction.FirstOrDefault(x => x.UserName == User);

            var isonite_temp = _context.isonite_temp.Where(x => x.user_create == User && x.Token == UserToken.Token).ToList();

            var isonite = _context.WoRouting.Where(x => x.ProcessCode == "ISONITE").ToList();

            var Process = (from t in isonite_temp
                           join i in isonite
                           on t.BarCode equals i.BarCode
                           where i.MainProcessFlag == "Y"
                           select i
                         ).ToList();

            foreach (var r in Process)
            {
                r.QTYinProcess = 0;
            }
            _context.isonite_temp.RemoveRange(isonite_temp);
            _context.SaveChanges();

            return new JsonResult(new { Msg = "ok", des = "" });

        }

    }
}