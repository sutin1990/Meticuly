using DevExtreme.AspNet.Data;
using DevExtreme.AspNet.Mvc;
using MCP_WEB.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
namespace MCP_WEB.Controllers.WebAPI
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class IsoniteReportController : Controller
    {
        private NittanDBcontext _context;
        public IsoniteReportController(NittanDBcontext context)
        {
            _context = context;
        }

        //private string GetUserName()
        //{
        //    var identity = (ClaimsIdentity)User.Identity;
        //    IEnumerable<Claim> claims = identity.Claims;
        //    var c = claims.FirstOrDefault();
        //    return c.Value;
        //}
        [HttpGet]
        public IActionResult Get(DataSourceLoadOptions loadOptions)
        {
            var isonitecode = from i in _context.Isonite
                              select new { i.IsoniteCode, i.DocStatus };
            return Json(DataSourceLoader.Load(isonitecode, loadOptions));
        }


    }
}