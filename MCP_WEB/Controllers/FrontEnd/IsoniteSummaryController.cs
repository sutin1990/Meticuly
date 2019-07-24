using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MCP_WEB.Controllers.FrontEnd
{
    [Authorize]
    public class IsoniteSummaryController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}