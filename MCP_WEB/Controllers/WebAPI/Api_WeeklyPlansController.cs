using DevExtreme.AspNet.Data;
using DevExtreme.AspNet.Mvc;
using MCP_WEB.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace MCP_WEB.Models.Controllers
{
    [Route("api/[controller]/[action]")]
    public class Api_WeeklyPlansController : Controller
    {
        private NittanDBcontext _context;

        public Api_WeeklyPlansController(NittanDBcontext context) {
            this._context = context;
        }

        [HttpGet]
        public IActionResult Get(DataSourceLoadOptions loadOptions) {
            var weeklyplan = (from i in _context.WeeklyPlan
                join x in _context.m_Resource
                on i.ItemCode equals x.ItemCode
                select new
                {
                    i.BarCode,
                    i.ItemCode,
                    x.Model,
                    i.QtyOrder,
                    i.SeriesLot,
                    i.StdLotSize,
                    i.WStatus,
                    i.CreateDate,
                    i.UpdateDate,
                    i.CreateBy,
                    i.UpdateBy,
                    i.QtyOrderAll,
                    i.PlanUserDef1,
                    i.PlanUserDef2,
                    x.Fcode
                });

            return Json(DataSourceLoader.Load(weeklyplan, loadOptions));
        }

        [HttpPost]
        public IActionResult Post(string values) {
            var model = new WeeklyPlan();
            var _values = JsonConvert.DeserializeObject<IDictionary>(values);
            PopulateModel(model, _values);

            if(!TryValidateModel(model))
                return BadRequest(GetFullErrorMessage(ModelState));

            var result = _context.WeeklyPlan.Add(model);
            _context.SaveChanges();

            return Json(result.Entity.BarCode);
        }

        [HttpPut]
        public IActionResult Put(string key, string values) {
            var model = _context.WeeklyPlan.FirstOrDefault(item => item.BarCode == key);
            if(model == null)
                return StatusCode(409, "WeeklyPlan not found");

            var _values = JsonConvert.DeserializeObject<IDictionary>(values);
            PopulateModel(model, _values);

            if(!TryValidateModel(model))
                return BadRequest(GetFullErrorMessage(ModelState));

            _context.SaveChanges();
            return Ok();
        }

        [HttpDelete]
        public void Delete(string key) {
            var model = _context.WeeklyPlan.FirstOrDefault(item => item.BarCode == key);

            _context.WeeklyPlan.Remove(model);
            _context.SaveChanges();
        }


        private void PopulateModel(WeeklyPlan model, IDictionary values) {
            string BAR_CODE = nameof(WeeklyPlan.BarCode);
            string ITEM_CODE = nameof(WeeklyPlan.ItemCode);

            string QTY_ORDER = nameof(WeeklyPlan.QtyOrder);
            string SERIES_LOT = nameof(WeeklyPlan.SeriesLot);
            string WSTATUS = nameof(WeeklyPlan.WStatus);
            string CREATE_DATE = nameof(WeeklyPlan.CreateDate);
            string UPDATE_DATE = nameof(WeeklyPlan.UpdateDate);
            string CREATE_BY = nameof(WeeklyPlan.CreateBy);
            string UPDATE_BY = nameof(WeeklyPlan.UpdateBy);

            if(values.Contains(BAR_CODE)) {
                model.BarCode = Convert.ToString(values[BAR_CODE]);
            }

            if(values.Contains(ITEM_CODE)) {
                model.ItemCode = Convert.ToString(values[ITEM_CODE]);
            }

            if(values.Contains(QTY_ORDER)) {
                model.QtyOrder = Convert.ToInt32(values[QTY_ORDER]);
            }

            if(values.Contains(SERIES_LOT)) {
                model.SeriesLot = Convert.ToString(values[SERIES_LOT]);
            }

            if(values.Contains(WSTATUS)) {
                model.WStatus = Convert.ToString(values[WSTATUS]);
            }

            if(values.Contains(CREATE_DATE)) {
                model.CreateDate = Convert.ToDateTime(values[CREATE_DATE]);
            }

            if(values.Contains(UPDATE_DATE)) {
                model.UpdateDate = Convert.ToDateTime(values[UPDATE_DATE]);
            }

            if(values.Contains(CREATE_BY)) {
                model.CreateBy = Convert.ToString(values[CREATE_BY]);
            }

            if(values.Contains(UPDATE_BY)) {
                model.UpdateBy = Convert.ToString(values[UPDATE_BY]);
            }
        }

        private string GetFullErrorMessage(ModelStateDictionary modelState) {
            var messages = new List<string>();

            foreach(var entry in modelState) {
                foreach(var error in entry.Value.Errors)
                    messages.Add(error.ErrorMessage);
            }

            return String.Join(" ", messages);
        }
    }
}