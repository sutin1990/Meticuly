using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MCP_WEB.Models;
using DevExtreme.AspNet.Mvc;
using DevExtreme.AspNet.Data;

namespace MCP_WEB.Controllers.WebAPI
{
    [Produces("application/json")]
    [Route("api/[controller]/[action]")]
    
    [ApiController]
    public class StatusController : Controller
    {
        [HttpGet]
        public IActionResult Get(DataSourceLoadOptions loadOptions)
        {
            List<StatusModel> statusModels = new List<StatusModel>
            {
                new StatusModel{
                    StatusID="N",
                    StatusName="Active"
                },
                 new StatusModel{
                    StatusID="N",
                    StatusName="Inactive"
                }
            };
            return Json(DataSourceLoader.Load(statusModels, loadOptions));
        }
    }
}