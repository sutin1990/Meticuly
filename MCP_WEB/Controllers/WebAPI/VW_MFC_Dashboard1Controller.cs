using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DevExtreme.AspNet.Data;
using DevExtreme.AspNet.Mvc;
using MCP_WEB.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace MCP_WEB.Controllers.WebAPI
{
    [Produces("application/json")]
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class VW_MFC_Dashboard1Controller : Controller
    {
        private readonly NittanDBcontext _Context;

        public VW_MFC_Dashboard1Controller(NittanDBcontext context)
        {
            this._Context = context;
        }

        [HttpGet]
        public IActionResult GetData(DataSourceLoadOptions loadOptions)
        {
            var view = _Context.VW_MFC_Dashboard1.Select(x => new
            {
                x.ProcessCode,
                x.All_Work,
                x.Between_Work,
                x.Close_Work
            });
            
            return Json(DataSourceLoader.Load(view, loadOptions));

        }
    }
}