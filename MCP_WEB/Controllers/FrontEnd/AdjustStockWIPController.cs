using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using MCP_WEB.Data;
using Microsoft.AspNetCore.Mvc;

namespace MCP_WEB.Controllers.FrontEnd
{
    public class AdjustStockWIPController : Controller
    {
        private NittanDBcontext _context;
        public AdjustStockWIPController(NittanDBcontext context)
        {
            _context = context;
        }
        public IActionResult Index()
        {
            //Get User
            var identity = (ClaimsIdentity)User.Identity;
            IEnumerable<Claim> claims = identity.Claims;
            var c = claims.FirstOrDefault();
            ViewBag.UserName = c.Value;

            //Get datetime format
            var p = _context.s_GlobalPams.SingleOrDefault(x => x.parm_key == "DateTimeFormat");
            ViewBag.GlobalDtFormat = p.param_value;
            return View();
        }
    }
}