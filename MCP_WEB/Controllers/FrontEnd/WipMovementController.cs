﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MCP_WEB.Data;
using Microsoft.AspNetCore.Mvc;

namespace MCP_WEB.Controllers.FrontEnd
{
    public class WipMovementController : Controller
    {
        private NittanDBcontext _context;
        public WipMovementController(NittanDBcontext context)
        {
            _context = context;
        }
        public IActionResult Index()
        {
            var p = _context.s_GlobalPams.SingleOrDefault(x => x.parm_key == "DateTimeFormat");
            ViewBag.wipmovementFormat = p.param_value;
            return View();
        }
    }
}