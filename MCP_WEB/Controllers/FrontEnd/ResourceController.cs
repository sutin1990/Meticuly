using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MCP_WEB.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MCP_WEB.Controllers.FrontEnd
{
    [Authorize]
    public class ResourceController : Controller
    {
        private readonly NittanDBcontext Db;

        public ResourceController(NittanDBcontext DB)
        {
            this.Db = DB;
        }
       
        public IActionResult Index()
        {
            ViewBag.format = Pram("DateTimeFormat");
            return View();
        }

        private string Pram(string options)
        {
            var p = Db.s_GlobalPams.SingleOrDefault(x => x.parm_key == options);
            return p.param_value;
        }
    }
}