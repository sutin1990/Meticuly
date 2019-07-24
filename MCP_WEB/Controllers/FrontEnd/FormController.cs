using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MCP_WEB.Data;
using Microsoft.AspNetCore.Mvc;

namespace MCP_WEB.Controllers.FrontEnd
{
    public class FormController : Controller
    {
        private NittanDBcontext _context;
        public FormController(NittanDBcontext contex)
        {
            _context = contex;
        }
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Index(string firstName)
        {
            return Content($"Hello {firstName}");
        }

        [HttpGet]
        public IActionResult GetPackingLookUp(string id)
        {
            var model = _context.m_Package.Where(w => w.PackType == "Box").Select(i => new
            {
                PackageTypeId = i.PackID,
                PackageTypeName = i.PackDesc,
                PackageDisp = i.PackID + "-" + i.PackDesc
            });

            return new JsonResult(model);
        }

    }
}