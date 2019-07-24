using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using MCP_WEB.Data;
using MCP_WEB.Models;
using MCP_WEB.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace MCP_WEB.Controllers.FrontEnd
{
    [Authorize]
    public class IsoniteMaintenanceController : Controller
    {
        private readonly NittanDBcontext _context;
        public IsoniteMaintenanceController(NittanDBcontext context)
        {
            this._context = context;
        }

        [SessionTimeout]
        public IActionResult Index()
        {
            

            //var identity = (ClaimsIdentity)User.Identity;
            //IEnumerable<Claim> claims = identity.Claims;
            //var user = claims.FirstOrDefault();
            //username = user.ToString();

            //User_Session user1 = new User_Session();
            //user1.Name = username;
            //HttpContext.Session.SetComplexData("UserData", user1);


            //Get datetime format
            var p = _context.s_GlobalPams.SingleOrDefault(x => x.parm_key == "DateTimeFormat");
            ViewBag.GlobalDtFormat = p.param_value;

            return View();
        }

    }
}