﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace MCP_WEB.Controllers.FrontEnd
{
    public class LotInMoveticketController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}