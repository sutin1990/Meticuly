using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using MCP_WEB.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace MCP_WEB.Controllers.WebAPI
{
   
    [Produces("application/json")]
    [Route("api/[controller]/[action]")]
    [Authorize]
    [ApiController]
    public class MenuController : ControllerBase
    {
        private NittanDBcontext db;

        public MenuController(NittanDBcontext db)
        {
            this.db = db;

        }
        // GET: api/Menu

        [HttpGet]
        public IActionResult GetMenu()
        {
           
            var result = db.MenuMaster.Where(m => m.User_Roll == "Admin").ToList();
            return new JsonResult(result);

         
        }

       
    }
}
