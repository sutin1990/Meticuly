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
    public class BPCodeController : Controller
    {
        private readonly NittanDBcontext Db;

        public BPCodeController(NittanDBcontext Db)
        {
            this.Db = Db;
        }

        [HttpGet]
        public IActionResult Get(DataSourceLoadOptions loadOptions)
        {
            var BpCode = Db.m_BPMaster.Where(a => a.BPType == "C").ToList();
            return Json(DataSourceLoader.Load(BpCode, loadOptions));
        }

        [HttpGet]
        public IActionResult GetTypeB(DataSourceLoadOptions loadOptions)
        {
            var BpCode = Db.m_BPMaster.Where(a => a.BPType == "B").ToList();
            return Json(DataSourceLoader.Load(BpCode, loadOptions));
        }
    }
}