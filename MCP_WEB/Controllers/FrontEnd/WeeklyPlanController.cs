using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MCP_WEB.Data;
using MCP_WEB.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MCP_WEB.Controllers.FrontEnd
{
    [Authorize]
    public class WeeklyPlanController : Controller
    {
        private NittanDBcontext _context;
        public WeeklyPlanController(NittanDBcontext context)
        {
            _context = context;
        }

        public IActionResult Index(string ProcessId)
        {
            //Get datetime format
            var p = _context.s_GlobalPams.SingleOrDefault(x => x.parm_key == "DateTimeFormat");
            ViewBag.GlobalDtFormat = p.param_value;

            var processLog = (from i in _context.s_ProcessLog
                              where i.ProcessID == ProcessId
                              select new
                              {
                                  i.id,
                                  i.ProcessID,
                                  i.ProcessDate,
                                  i.Msg
                              });

            return View(processLog);

        }
    }
}