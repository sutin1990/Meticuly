using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DevExtreme.AspNet.Data;
using DevExtreme.AspNet.Mvc;
using MCP_WEB.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
namespace MCP_WEB.Controllers.WebAPI
{
    [Produces("application/json")]
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class VW_MFC_Isonite_SummaryController : Controller
    {
        private readonly NittanDBcontext _Context;

        public VW_MFC_Isonite_SummaryController(NittanDBcontext context)
        {
            this._Context = context;
        }

        [HttpGet]
        public IActionResult GetIsonite_Summary(DataSourceLoadOptions loadOptions)
        {
            var view = _Context.VW_MFC_Isonite_Summary.Select(x => new
            {
                x.IsoniteCode,
                x.JigNo,
                x.Column,
                x.Row,
                x.BarCode,
                x.SeriesLot,
                x.Model,
                x.WStatus,
                x.QtyOrder,
                x.WIPLeftQty,
                x.IsoniteQty
            });

            return Json(DataSourceLoader.Load(view, loadOptions));

        }
    }
}