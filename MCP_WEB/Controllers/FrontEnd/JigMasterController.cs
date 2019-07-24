using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MCP_WEB.Data;
using Microsoft.AspNetCore.Mvc;

namespace MCP_WEB.Controllers.FrontEnd
{
    public class JigMasterController : Controller
    {
        private NittanDBcontext _context;

        public JigMasterController(NittanDBcontext context) {
            this._context = context;
        }
        public IActionResult Index()
        {

            // Date format
            var p = _context.s_GlobalPams.SingleOrDefault(x => x.parm_key == "DateTimeFormat");
            ViewBag.GlobalDtFormat = p.param_value; // "dd-MM-YYYY";

            return View();
        }
    }
}