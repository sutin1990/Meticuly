using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MCP_WEB.Data;
using MCP_WEB.Models;
using OfficeOpenXml;

namespace MCP_WEB.Controllers.WebAPI {
    public class m_JigController : Controller {
        private readonly NittanDBcontext _context;

        public m_JigController(NittanDBcontext context) {
            _context = context;
        }

        // Export Excel =======================================================================
        public IActionResult ExportExcel() {
            byte[] result;
            var comlumHeadrs = new string[]{
                "JigNo",
                "Column",
                "Row"
            };
            using (var package = new ExcelPackage()) {
                var worksheet = package.Workbook.Worksheets.Add("Jig Master"); 
                //First add the headers
                for (var i = 0; i < comlumHeadrs.Count(); i++) {
                    worksheet.Cells[1, i + 1].Value = comlumHeadrs[i];
                }
                var jigs = from p in _context.m_Jig
                           select p;
                //Add values
                var j = 2;
                foreach (var jig in jigs) {
                    worksheet.Cells["A" + j].Value = jig.JigNo;
                    worksheet.Cells["B" + j].Value = jig.Column;
                    worksheet.Cells["C" + j].Value = jig.Row;
                    j++;
                }
                result = package.GetAsByteArray();
            }
            return File(result, "application/ms-excel", $"Jig.xlsx");
        }
        // =======================================================================

        // GET: m_Jig
        //public async Task<IActionResult> Index() {
        //    return View(await _context.m_Jig.ToListAsync());
        //}
        public async Task<IActionResult> Index(string sortOrder, string JigNoFilter, int? ColumnFilter, int? RowFilter) {
            // Sort
            ViewData["JigNoSortParm"] = sortOrder == "JigNo" ? "JigNo_desc" : "JigNo"; 
            ViewData["ColumnSortParm"] = sortOrder == "Column" ? "Column_desc" : "Column";
            ViewData["RowSortParm"] = sortOrder == "Row" ? "Row_desc" : "Row";
            // Filter
            ViewData["JigNoFilter"] = JigNoFilter;
            ViewData["ColumnFilter"] = ColumnFilter;
            ViewData["RowFilter"] = RowFilter;

            var jigs = from s in _context.m_Jig
                       select s;

            if (!String.IsNullOrEmpty(JigNoFilter)) {
                jigs = jigs.Where(s => s.JigNo.Contains(JigNoFilter));
            }
            if (ColumnFilter != null) {
                jigs = jigs.Where(s => s.Column == ColumnFilter);
            }
            if (RowFilter != null) {
                jigs = jigs.Where(s => s.Row == RowFilter);
            }

            ViewBag.JigNoSortType = "";
            ViewBag.ColumnType = "";
            ViewBag.RowType = "";
            switch (sortOrder) {
                ////case "name_desc":
                ////    jigs = jigs.OrderByDescending(s => s.JigNo);
                ////    break;
                case "JigNo":
                    jigs = jigs.OrderBy(s => s.JigNo);
                    ViewBag.JigNoSortType = "↓";
                    break;
                case "JigNo_desc":
                    jigs = jigs.OrderByDescending(s => s.JigNo);
                    ViewBag.JigNoSortType = "↑";
                    break;

                case "Column":
                    jigs = jigs.OrderBy(s => s.Column);
                    ViewBag.ColumnType = "↓";
                    break;
                case "Column_desc":
                    jigs = jigs.OrderByDescending(s => s.Column);
                    ViewBag.ColumnType = "↑";
                    break;

                case "Row":
                    jigs = jigs.OrderBy(s => s.Row);
                    ViewBag.RowType = "↓";
                    break;
                case "Row_desc":
                    jigs = jigs.OrderByDescending(s => s.Row);
                    ViewBag.RowType = "↑";
                    break;

                default:
                    jigs = jigs.OrderBy(s => s.JigID);
                    break;
            }

            //return View(await _context.m_Jig.ToListAsync());
            return View(await jigs.AsNoTracking().ToListAsync());
        }

        // GET: m_Jig/Details/5
        public async Task<IActionResult> Details(int? id) {
            if (id == null) {
                return NotFound();
            }

            var m_Jig = await _context.m_Jig
                .FirstOrDefaultAsync(m => m.JigID == id);
            if (m_Jig == null) {
                return NotFound();
            }

            return View(m_Jig);
        }

        // GET: m_Jig/Create
        public IActionResult Create() {
            return View();
        }

        // POST: m_Jig/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        //public async Task<IActionResult> Create([Bind("JigID,JigNo,Column,Row,JigUserDef1,JigUserDef2,TransDate,CreateDate,ModifyBy")] m_Jig m_Jig) {
        public async Task<IActionResult> Create([Bind("JigID,JigNo,Column,Row")] m_Jig m_Jig) {
            if (ModelState.IsValid) {

                int newJigID = 1 + _context.m_Jig.Max(p => p.JigID);
                m_Jig.JigID = newJigID;
                // TODO edit 2 line
                //m_Jig.JigUserDef1 = DateTime.Today;
                m_Jig.ModifyBy = "Jom";

                _context.Add(m_Jig);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(m_Jig);
        }

        // GET: m_Jig/Edit/5
        public async Task<IActionResult> Edit(int? id) {
            if (id == null) {
                return NotFound();
            }

            var m_Jig = await _context.m_Jig.FindAsync(id);
            if (m_Jig == null) {
                return NotFound();
            }
            return View(m_Jig);
        }

        // POST: m_Jig/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("JigID,JigNo,Column,Row")] m_Jig m_Jig) {
            if (id != m_Jig.JigID) {
                return NotFound();
            }

            if (ModelState.IsValid) {
                try {
                    // TODO edit
                    //m_Jig.JigUserDef1 = DateTime.Today;
                    
                    m_Jig.ModifyBy = "Jom";

                    _context.Update(m_Jig);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException) {
                    if (!m_JigExists(m_Jig.JigID)) {
                        return NotFound();
                    }
                    else {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(m_Jig);
        }

        // GET: m_Jig/Delete/5
        public async Task<IActionResult> Delete(int? id) {
            if (id == null) {
                return NotFound();
            }

            var m_Jig = await _context.m_Jig
                .FirstOrDefaultAsync(m => m.JigID == id);
            if (m_Jig == null) {
                return NotFound();
            }

            return View(m_Jig);
        }

        // POST: m_Jig/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id) {
            var m_Jig = await _context.m_Jig.FindAsync(id);
            _context.m_Jig.Remove(m_Jig);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool m_JigExists(int id) {
            return _context.m_Jig.Any(e => e.JigID == id);
        }
    }
}
