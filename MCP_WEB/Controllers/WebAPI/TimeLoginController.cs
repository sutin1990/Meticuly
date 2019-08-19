using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using MCP_WEB.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace MCP_WEB.Controllers.WebAPI
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class TimeLoginController : Controller
    {
        private NittanDBcontext _context;

        public TimeLoginController(NittanDBcontext context)
        {
            _context = context;
        }
        public IActionResult GetTime()
        {
            var identity = (ClaimsIdentity)User.Identity;
            IEnumerable<Claim> claims = identity.Claims;
            var userLogin = claims.FirstOrDefault();
            var model1 = _context.UserTransaction.Where(x => x.UserName == userLogin.Value).FirstOrDefault();
            //int Minute = (DateTime.Now - model1.DateExprie).Minutes;
            return Json((DateTime.Now - model1.DateExprie).Minutes);
        }
    }
}