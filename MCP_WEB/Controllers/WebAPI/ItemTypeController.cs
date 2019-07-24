using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DevExtreme.AspNet.Data;
using DevExtreme.AspNet.Mvc;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MCP_WEB.Models;
namespace MCP_WEB.Controllers.WebAPI
{
    [Produces("application/json")]
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class ItemTypeController : Controller
    {

        [HttpGet]
        public IActionResult Get(DataSourceLoadOptions loadOptions)
        {
            List<ItemTypeModel> itemTypes = new List<ItemTypeModel>
            {
                new ItemTypeModel{
                    ItemTypeID="F",
                    ItemTypeName="Finished Goods"
                },
                 new ItemTypeModel{
                    ItemTypeID="R",
                    ItemTypeName="Raw Material"
                }
            };
            return Json(DataSourceLoader.Load(itemTypes, loadOptions));
        }

    }
}