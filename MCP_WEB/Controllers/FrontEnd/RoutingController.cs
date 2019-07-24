using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using MCP_WEB.Data;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MCP_WEB.Controllers.FrontEnd
{
    [Authorize]
    public class RoutingController : Controller
    {

        private NittanDBcontext _Context;
        public RoutingController(NittanDBcontext context)
        {
            this._Context = context;
           
        }

        public IActionResult Index()
        {
            ViewBag.format = Pram("DateTimeFormat");
            return View();
        }

        private string Pram(string options)
        {
            var p = _Context.s_GlobalPams.SingleOrDefault(x => x.parm_key == options);
            return p.param_value;
        }
    }
}