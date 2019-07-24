using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MCP_WEB.Helper;
using MCP_WEB.Data;

namespace MCP_WEB.Controllers.FrontEnd
{
    [Authorize]
    public class BomController : Controller
    {
        private readonly NittanDBcontext Db;

        public BomController(NittanDBcontext DB)
        {
            this.Db = DB;
        }
        public IActionResult Index()
        {
            ViewBag.format = Pram("DateTimeFormat");
            return View();
        }
        private string Pram(string options) {
            var p = Db.s_GlobalPams.SingleOrDefault(x => x.parm_key == options);
            return p.param_value;
        }
    }
}