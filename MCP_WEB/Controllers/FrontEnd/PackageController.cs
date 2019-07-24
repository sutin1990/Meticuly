using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using MCP_WEB.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MCP_WEB.Controllers.FrontEnd
{
    [Authorize]
    public class PackageController : Controller
    {
        private NittanDBcontext _context;
        public PackageController(NittanDBcontext context)
        {
            _context = context;
        }
        public IActionResult Index()
        {
            //Screen Name
            //string actionName = this.ControllerContext.RouteData.Values["action"].ToString();
            //string controllerName = this.ControllerContext.RouteData.Values["controller"].ToString();
            //var n = _context.MenuMaster.SingleOrDefault(x => x.MenuURL == controllerName);
            //ViewBag.Title = n.MenuName;

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