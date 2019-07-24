using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DevExtreme.AspNet.Data;
using DevExtreme.AspNet.Mvc;
using MCP_WEB.Data;
using MCP_WEB.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Collections;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.Dynamic;
using ZXing.QrCode;
using MCP_WEB.Helper;
using System.Drawing;

namespace MCP_WEB.Controllers.WebAPI {
    [Route("api/[controller]/[action]")]
    public class LotControlTicketController : Controller {
        private NittanDBcontext _context;

        public LotControlTicketController(NittanDBcontext context) {
            this._context = context;
        }

        [HttpGet]
        public IActionResult Get(DataSourceLoadOptions loadOptions) {
            var Lot = from wp in _context.WeeklyPlan
                      join wb in _context.WoBOM on wp.BarCode equals wb.BarCode
                      join res in _context.m_Resource on wp.ItemCode equals res.ItemCode
                      join mb in _context.m_BOM on wp.ItemCode equals mb.ItemCode
                      orderby wp.BarCode
                      select new { wp.BarCode, wp.ItemCode, wb.Material1, wb.Material2, wp.QtyOrder, res.Fcode, res.Model, wp.WStatus };
            return Json(DataSourceLoader.Load(Lot, loadOptions));
        }

        [HttpGet]
        public object GetDetails(string id, DataSourceLoadOptions loadOptions) {
            var woRoute = from wr in _context.WoRouting.Where(x => x.BarCode == id)
                          select new { wr.OperationNo, wr.ProcessCode, wr.PStatus };
            //var woRoute = from wr in _context.WoRouting
            //              select new { wr.BarCode, wr.OperationNo, wr.ProcessCode, wr.PStatus};
            //woRoute = woRoute.Where(x => x.BarCode == id);
            return DataSourceLoader.Load(woRoute, loadOptions);
        }
        [HttpGet]
        public IActionResult PrintLotControl(string[] id) { // PrintSelected
            var Lot = from wp in _context.WeeklyPlan
                      join wb in _context.WoBOM on wp.BarCode equals wb.BarCode
                      join res in _context.m_Resource on wp.ItemCode equals res.ItemCode
                      join bp in _context.m_BPMaster on res.BPCode equals bp.BPCode
                      join mb in _context.m_BOM on wp.ItemCode equals mb.ItemCode
                      orderby wp.BarCode
                      select new LotControlViewModel {
                          WeeklyPlanVM = wp,
                          wobomVM = wb,
                          m_resourceVM = res,
                          m_BomVM = mb,
                          PageNum = 0,
                          WoRoutingVM = null,
                          BPAddress6 = "",
                          m_BPMasterVM = bp
                      };
            if (id != null) {
                Lot = Lot.Where(x => id.Contains(x.WeeklyPlanVM.BarCode));
            }
            var lotList = Lot.ToList();

            var mbp = _context.m_BPMaster.Where(m => m.BPCode == "100000").First();
            string BPAddress6 = "";
            if (mbp != null) {
                BPAddress6 = mbp.BPAddress6;
            }

            double i = 0;
            int p = 0;
            int PageCount = 0;
            foreach (var row in lotList.OrderBy(x => x.WeeklyPlanVM.BarCode)) {
                i += 1;
                row.RowNum = Convert.ToInt32(i);
                // if i = 1 to 3 then PageNum = 1
                // if i = 4 to 6 then PageNum = 2
                p = Convert.ToInt32(Math.Ceiling(i / 3));
                row.PageNum = p;
                PageCount = p;

                row.BPAddress6 = BPAddress6;

                var detail = (from wr in _context.WoRouting
                              where wr.BarCode.Equals(row.WeeklyPlanVM.BarCode) // .Where(x => x.BarCode == "F011009521810/013")
                              select new WoRouting { OperationNo = wr.OperationNo, ProcessCode = wr.ProcessCode });
                int idx = 0;
                var dtl = detail.ToList();

                // check can Insert before StemRough
                bool canCheckStemRough = true;

                foreach (var d in detail) {
                    if (d.ProcessCode.ToUpper() == "MATERIAL INSPECTION".ToUpper()) {
                        if (row.m_resourceVM.ItemCode.ToUpper() == "13715-0E010-00".ToUpper()
                        && row.m_resourceVM.Fcode.ToUpper() == "F0510012A".ToUpper()
                        && row.m_resourceVM.Model.ToUpper() == "GD EXH".ToUpper()) {
                            canCheckStemRough = false;
                        }
                        if (row.m_resourceVM.ItemCode.ToUpper() == "13711-0E010-00".ToUpper()
                        && row.m_resourceVM.Fcode.ToUpper() == "F0510011A".ToUpper()
                        && row.m_resourceVM.Model.ToUpper() == "GD INT".ToUpper()) {
                            canCheckStemRough = false;
                        }
                    }
                }

                foreach (var d in detail) {
                    // add empty gray line before STEMROUGH
                    if (canCheckStemRough == true) {
                        if (d.ProcessCode.ToUpper() == "StemRough".ToUpper()) { // "COLOR CHECK" 
                            WoRouting wor = new WoRouting();
                            wor.OperationNo = 0;
                            dtl.Insert(idx, wor);
                            idx++;
                        }
                    }

                    // add empty gray line before 13715-0E010-00, F0510012A, GD EXH
                    if (row.m_resourceVM.ItemCode.ToUpper() == "13715-0E010-00".ToUpper()
                        && row.m_resourceVM.Fcode.ToUpper() == "F0510012A".ToUpper()
                        && row.m_resourceVM.Model.ToUpper() == "GD EXH".ToUpper()) {
                        if (d.ProcessCode.ToUpper() == "MATERIAL INSPECTION".ToUpper()) {
                            WoRouting wor = new WoRouting();
                            wor.OperationNo = 0;
                            dtl.Insert(idx, wor);
                            idx++;
                        }
                    }
                    // add empty gray line before 13711-0E010-00, F0510011A, GD INT
                    if (row.m_resourceVM.ItemCode.ToUpper() == "13711-0E010-00".ToUpper()
                        && row.m_resourceVM.Fcode.ToUpper() == "F0510011A".ToUpper()
                        && row.m_resourceVM.Model.ToUpper() == "GD INT".ToUpper()) {
                        if (d.ProcessCode.ToUpper() == "MATERIAL INSPECTION".ToUpper()) {
                            WoRouting wor = new WoRouting();
                            wor.OperationNo = 0;
                            dtl.Insert(idx, wor);
                            idx++;
                        }
                    }

                    idx++;
                }

                row.WoRoutingVM = dtl; // detail;
            }
            ViewBag.PageCount = PageCount;
            lotList.OrderBy(x => x.WeeklyPlanVM.BarCode);
            return View(lotList);
        }//PrintLotControl

    }
}