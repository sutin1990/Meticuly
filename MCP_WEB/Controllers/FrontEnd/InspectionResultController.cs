﻿using System;
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
    public class InspectionResultController : Controller
    {
        private NittanDBcontext _context;

        public InspectionResultController(NittanDBcontext context)
        {
            this._context = context;
        }
        public IActionResult Index()
        {
            var identity = (ClaimsIdentity)User.Identity;
            IEnumerable<Claim> claims = identity.Claims;
            var userLogin = claims.FirstOrDefault();
            var model1 = _context.UserTransaction.Where(x => x.UserName == userLogin.Value).FirstOrDefault();

            //int Minute = (DateTime.Now - model1.DateExprie).Minutes;
            ViewBag.MinuteTimeLogin = model1.DateExprie;
            return View();
        }
    }
}