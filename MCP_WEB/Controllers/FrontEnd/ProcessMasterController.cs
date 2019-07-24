using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MCP_WEB.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MCP_WEB.Controllers.FrontEnd
{
    //[Authorize]
    public class ProcessMasterController : Controller
    {
        private NittanDBcontext _context;

        public ProcessMasterController(NittanDBcontext context)
        {
            _context = context;
        }
        public IActionResult Index()
        {

            //var s_GlobalPams = _context.s_GlobalPams.SingleOrDefault(g => g.parm_key == "DateTimeFormat");
            //ViewBag.s_GlobalPams = s_GlobalPams.param_value;
            var p = _context.s_GlobalPams.SingleOrDefault(x => x.parm_key == "DateTimeFormat");
            ViewBag.GlobalDtFormat = p.param_value.ToString();

            return View();
        }
    }
}