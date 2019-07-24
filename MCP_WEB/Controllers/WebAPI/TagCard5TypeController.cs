using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using MCP_WEB.Data;
using MCP_WEB.Models;
using DevExtreme.AspNet.Mvc;
using DevExtreme.AspNet.Data;

using System.Dynamic;
using ZXing.QrCode;

namespace MCP_WEB.Controllers.WebAPI{
    public class TagCard5TypeController : Controller{
        private NittanDBcontext _context;
        public TagCard5TypeController(NittanDBcontext context) {
            this._context = context;
        }
        public IActionResult Index(){
            return View();
        }

        [HttpGet]
        public IActionResult Get(DataSourceLoadOptions loadOptions) {
            //var tag = (from wp in _context.WeeklyPlan
            //          join res in _context.m_Resource on wp.ItemCode equals res.ItemCode
            //          //where res.ItemCode == "14711-KPP-900"
            //          orderby res.ItemCode
            //          select new { res.ItemCode, res.ItemName, res.Model }).Distinct();

            var tag = _context.VW_MFC_RePrint_MoveTicket;
            return Json(DataSourceLoader.Load(tag, loadOptions));
        }

        public IActionResult T1(string[] id) {
            var detail = from v in _context.VW_MFC_T1HATCYamaha
                          select new VW_MFC_T1HATCYamaha {
                              Barcode = v.Barcode,
                              PartNo = v.PartNo,
                              PartName = v.PartName,
                              Date = v.Date,
                              Quantity = v.Quantity,
                              Weight = v.Weight,
                              OrderNo = v.OrderNo,
                              OrderQty = v.OrderQty,
                              PackType = v.PackType,
                              TotalPack = v.TotalPack,
                              ThisPackofTotal = v.ThisPackofTotal,
                              InspectionCode = v.InspectionCode,
                              InspectionBy = v.InspectionBy,
                              CheckBy = v.CheckBy,
                              LotNo = v.LotNo,
                              Images = v.Images == null ? "" : v.Images,
                              Model = v.Model,
                              QR = v.QR,
                              Customer = v.Customer,
                              Location = v.Location,
                              TagColor = v.TagColor,
                              BoxColor = v.BoxColor,
                              PartDesTag = v.PartDesTag,
                          };
            if (id != null) {
                detail = detail.Where(x => id.Contains(x.PartDesTag));
            }
            string CompareText = "";
            List<VW_MFC_T1HATCYamaha> header = new List<VW_MFC_T1HATCYamaha>() ; //= new VW_MFC_T1HATCYamaha();
            foreach (var dtl in detail.OrderBy(x => x.PartNo)) {
                //dtl.LotNoShow = dtl.LotNo.ToString().Substring(dtl.LotNo.Length - 8) + "=" + dtl.Quantity + " pc";
                dtl.LotNoShow = dtl.LotNo + "=" + dtl.Quantity + " pc"; //เปลี่ยนเป็น คำเต็มไม่ตัด string 
                if (CompareText != dtl.PartNo + dtl.QR) { //(part != dtl.PartNo && QR != dtl.QR) {
                    CompareText = dtl.PartNo + dtl.QR;
                    dtl.Customer = dtl.Customer.Substring(0,15); // Limit Text Length
                    header.Add(dtl);
                }
                else {
                    foreach(var h in header.Where(x => x.PartNo == dtl.PartNo && x.QR == dtl.QR)) {
                        h.Quantity += dtl.Quantity;
                        h.LotNo += dtl.LotNo;
                        h.LotNoShow += "," + dtl.LotNoShow;
                    }
                }
            }
            var headerList = header.ToList();
            return View(headerList);
        }

        public IActionResult T2(string[] id) {
            var detail = from v in _context.VW_MFC_T2_TR
                         select new VW_MFC_T2_TR {
                             BACKNO = v.BACKNO,
                             Remark = v.Remark,
                             SuppilerName = v.SuppilerName,
                             PartNo = v.PartNo,
                             PartName = v.PartName,
                             Model = v.Model,
                             LotControl = v.LotControl,
                             CheckBy = v.CheckBy,
                             DeliveryDate = v.DeliveryDate,
                             SupplierApproved = v.SupplierApproved,
                             Quantity = v.Quantity,
                             StoredBy = v.StoredBy,
                             ReceivedNo = v.ReceivedNo,
                             PartDesTag = v.PartDesTag,
                             QR = v.QR,
                             ModelCus = v.ModelCus,
                             Images = v.Images
                         };
            if (id != null) {
                detail = detail.Where(x => id.Contains(x.PartDesTag));
            }
            string CompareText = "";
            List<VW_MFC_T2_TR> header = new List<VW_MFC_T2_TR>();
            foreach (var dtl in detail.OrderBy(x => x.PartNo)) {//foreach (var dtl in detail.OrderBy(x => x.PartNo)) {
                //dtl.LotNoShow = dtl.LotControl.ToString().Substring(dtl.LotControl.Length - 8) + "=" + dtl.Quantity + " pc";
                dtl.LotNoShow = dtl.LotControl.ToString() + "=" + dtl.Quantity + " pc";
                if (CompareText != dtl.PartNo + dtl.QR) { //(part != dtl.PartNo && QR != dtl.QR) {
                    CompareText = dtl.PartNo + dtl.QR;
                    //dtl.Customer = dtl.Customer.Substring(0, 15);
                    header.Add(dtl);
                }
                else {
                    foreach (var h in header.Where(x => x.PartNo == dtl.PartNo && x.QR == dtl.QR)) {
                        h.Quantity += dtl.Quantity;
                        h.LotControl += dtl.LotControl;
                        h.LotNoShow += "," + dtl.LotNoShow;
                    }
                }
            }
            ViewBag.BP = _context.m_BPMaster.FirstOrDefault(f => f.BPCode == "100000");
            var headerList = header.ToList();
            return View(headerList);
        }// T2()

        public IActionResult T3(string[] id) {
            var detail = from v in _context.VW_MFC_T3_ZR
                         select new VW_MFC_T3_ZR {
                             PartNo = v.PartNo,
                             PartName = v.PartName,
                             SuppilerName = v.SuppilerName,
                             Model = v.Model,
                             LotControl = v.LotControl,
                             CheckBy = v.CheckBy,
                             DeliveryDate = v.DeliveryDate,
                             SupplierApproved = v.SupplierApproved,
                             Quantity = v.Quantity,
                             StoredBy = v.StoredBy,
                             ReceivedNo = v.ReceivedNo,
                             PartDesTag = v.PartDesTag,
                             QR = v.QR
                         };
            if (id != null) {
                detail = detail.Where(x => id.Contains(x.PartDesTag));
            }
            string CompareText = "";
            List<VW_MFC_T3_ZR> header = new List<VW_MFC_T3_ZR>();
            foreach (var dtl in detail.OrderBy(x => x.PartNo)) {
                dtl.LotNoShow = dtl.LotControl.ToString().Substring(dtl.LotControl.Length - 8) + "=" + dtl.Quantity + " pc";
                if (CompareText != dtl.PartNo + dtl.QR) { 
                    CompareText = dtl.PartNo + dtl.QR;
                    header.Add(dtl);
                }
                else {
                    foreach (var h in header.Where(x => x.PartNo == dtl.PartNo && x.QR == dtl.QR)) {
                        h.Quantity += dtl.Quantity;
                        h.LotControl += dtl.LotControl;
                        h.LotNoShow += "," + dtl.LotNoShow;
                    }
                }
            }
            ViewBag.BP = _context.m_BPMaster.FirstOrDefault(f => f.BPCode == "100000");
            var headerList = header.ToList();
            return View(headerList);
        }// T3()

        public IActionResult T4(string[] id) {
            var detail = from v in _context.VW_MFC_T4_THM_GD_TSM
                         select new VW_MFC_T4_THM_GD_TSM {
                             PartName = v.PartName,
                             PartNo = v.PartNo,
                             Model = v.Model,
                             OrderQty = v.OrderQty,
                             QuantityLot = v.QuantityLot,
                             Color = v.Color,
                             LotNo = v.LotNo,
                             Location = v.Location,
                             Customer = v.Customer,
                             PlantCode = v.PlantCode,
                             Timeofdelivery = v.Timeofdelivery,
                             _By = v._By,
                             Date0 = v.Date0,
                             PackBy = v.PackBy,
                             CheckBy = v.CheckBy,
                             StoreBy = v.StoreBy,
                             ReceiveNo = v.ReceiveNo,
                             Date1 = v.Date1,
                             Date2 = v.Date2,
                             Date3 = v.Date3,
                             Date4 = v.Date4,
                             PartDesTag = v.PartDesTag,
                             QR = v.QR
                         };
            if (id != null) {
                detail = detail.Where(x => id.Contains(x.PartDesTag));
            }
            string CompareText = "";
            List<VW_MFC_T4_THM_GD_TSM> header = new List<VW_MFC_T4_THM_GD_TSM>();
            foreach (var dtl in detail.OrderBy(x => x.PartNo)) {
                dtl.LotNoShow = dtl.LotNo.ToString().Substring(dtl.LotNo.Length - 8) + "=" + dtl.QuantityLot + " pc";
                if (CompareText != dtl.PartNo + dtl.QR) {
                    CompareText = dtl.PartNo + dtl.QR;
                    dtl.Customer = dtl.Customer.Substring(0, 13);
                    header.Add(dtl);
                }
                else {
                    foreach (var h in header.Where(x => x.PartNo == dtl.PartNo && x.QR == dtl.QR)) {
                        h.QuantityLot += dtl.QuantityLot;
                        h.OrderQty += dtl.OrderQty;
                        h.LotNo += dtl.LotNo;
                        h.LotNoShow += "," + dtl.LotNoShow;
                    }
                }
            }
            var headerList = header.ToList();
            return View(headerList);
        }// T4()

        public IActionResult T5(string[] id) {
            var detail = from v in _context.VW_MFC_T5_AAT
                         select new VW_MFC_T5_AAT {
                             PartNo = v.PartNo,
                             QR = v.QR,
                             Model = v.Model,
                             Color = v.Color,
                             Process = v.Process,
                             Suppiler = v.Suppiler,
                             Quantity = v.Quantity,
                             DeliveryDate = v.DeliveryDate,
                             ReceivedLoc = v.ReceivedLoc,
                             StorageLoc = v.StorageLoc,
                             SupplyLoc = v.SupplyLoc,
                             Remark = v.Remark,
                             PartName = v.PartName,
                             PartDesTag = v.PartDesTag
                         };
            if (id != null) {
                detail = detail.Where(x => id.Contains(x.PartDesTag));
            }
            string CompareText = "";
            List<VW_MFC_T5_AAT> header = new List<VW_MFC_T5_AAT>();
            foreach (var dtl in detail.OrderBy(x => x.PartNo)) {
                if (CompareText != dtl.PartNo + dtl.QR) {
                    CompareText = dtl.PartNo + dtl.QR;
                    header.Add(dtl);
                }
                else {
                    foreach (var h in header.Where(x => x.PartNo == dtl.PartNo && x.QR == dtl.QR)) {
                        h.Quantity += dtl.Quantity;
                    }
                }
            }
            var headerList = header.ToList();
            return View(headerList);
        }// T5()

    }
}