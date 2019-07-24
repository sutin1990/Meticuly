﻿using System;
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
    public class BomLookupController : Controller
    {
        private NittanDBcontext Db;

        public BomLookupController(NittanDBcontext Db)
        {
            this.Db = Db;
        }
        

        [HttpGet]
        public IActionResult GetTypeF(DataSourceLoadOptions loadOptions)
        {
            var m_Resource = Db.m_Resource.Where(x => x.ItemType == "F").Select(i => new
            {
                i.ItemCode,
                i.ItemName,
                i.Model,
                i.Fcode
            });
            return Json(DataSourceLoader.Load(m_Resource, loadOptions));
        }

        [HttpGet]
        public IActionResult GetTypeR(DataSourceLoadOptions loadOptions)
        {
            var m_Resource = Db.m_Resource.Where(x => x.ItemType == "R").Select(i => new
            {
                i.ItemCode,
                i.ItemName
            });
            return Json(DataSourceLoader.Load(m_Resource, loadOptions));
        }


        [HttpGet]
        public IActionResult getItemResult(string options) {
            var m_Resource = Db.m_Resource.Where(x => x.ItemCode == options).Select(i => new
            {
                i.ItemName,
                i.Model,
                i.Fcode
            });
            return new JsonResult(m_Resource);
        }
    }
}