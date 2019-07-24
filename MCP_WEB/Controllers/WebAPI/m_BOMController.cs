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
    public class m_BOMController : Controller
    {
        private NittanDBcontext _context;

        public m_BOMController(NittanDBcontext context)
        {
            this._context = context;
        }

        [HttpGet]
        public IActionResult Get(DataSourceLoadOptions loadOptions)
        {
            var Query = (from i in _context.m_BOM
                         join x in _context.m_Resource
                         on i.ItemCode equals x.ItemCode
                         select new
                         {
                             i.ItemCode,
                             x.Fcode,
                             x.ItemName,
                             x.Model,
                             i.Material1,
                             i.Material2,
                             i.BOMUserDef1,
                             i.BOMUserDef2,
                             i.BOMUserDef3,
                             i.BOMUserDef4,
                             i.TransDate,
                             i.CreateDate,
                             i.ModifyBy
                         });
            return Json(DataSourceLoader.Load(Query, loadOptions));
        }

        [HttpPost]
        public IActionResult Post(string values)
        {
            var model = new m_BOM();
            var _values = JsonConvert.DeserializeObject<IDictionary>(values);
            PopulateModel(model, _values);

            var ResourceCheck = _context.m_BOM.Where(b => b.ItemCode == model.ItemCode).Count();
            if (ResourceCheck > 0)
            {
                return BadRequest("Part No duplicate please check data.");
            }

            if (model.Material1 == model.Material2)
            {
                return BadRequest("Material#1 duplicate Material#2 please check data.");
            }

            if (!TryValidateModel(model))
                return BadRequest(GetFullErrorMessage(ModelState));

            var result = _context.m_BOM.Add(model);
            _context.SaveChanges();

            return Json(result.Entity.ItemCode);
        }

        [HttpPut]
        public IActionResult Put(string key, string values)
        {
            var model = _context.m_BOM.FirstOrDefault(item => item.ItemCode == key);
            if (model == null)
                return StatusCode(409, "m_BOM not found");

            var _values = JsonConvert.DeserializeObject<IDictionary>(values);
            PopulateModel(model, _values);

            if (model.Material1 == model.Material2)
            {
                return BadRequest("Material#1 duplicate Material#2 please check data.");
            }


            if (!TryValidateModel(model))
                return BadRequest(GetFullErrorMessage(ModelState));

            _context.SaveChanges();
            return Ok();
        }

        [HttpDelete]
        public IActionResult Delete(string key)
        {

            var Query = (from i in _context.m_BOM
                         join x in _context.m_Resource
                         on i.ItemCode equals x.ItemCode
                         where i.ItemCode == key
                         select new
                         {
                             i.ItemCode,
                             x.Fcode,
                             x.ItemName,
                             x.Model,
                             i.Material1,
                             i.Material2,
                             i.BOMUserDef1,
                             i.BOMUserDef2,
                             i.BOMUserDef3,
                             i.BOMUserDef4,
                             i.TransDate,
                             i.CreateDate,
                             i.ModifyBy
                         }).FirstOrDefault();

            //var Query = _context.m_BOM.Where(x => x.ItemCode == key).FirstOrDefault();

            if (!_context.WeeklyPlan.Any(x => x.ItemCode == key && x.Model == Query.Model && x.Fcode == Query.Fcode))
            {
                var model = _context.m_BOM.FirstOrDefault(item => item.ItemCode == key);

                _context.m_BOM.Remove(model);
                _context.SaveChanges();

                return Ok();

            }
            else {
                return BadRequest("ItemCode Used by WeeklyPlan please check data.");
            }

        }

        private void PopulateModel(m_BOM model, IDictionary values)
        {
            string PART_NO = nameof(m_BOM.ItemCode);

            string MATERIAL1 = nameof(m_BOM.Material1);
            string MATERIAL2 = nameof(m_BOM.Material2);
            string BOMUSER_DEF1 = nameof(m_BOM.BOMUserDef1);
            string BOMUSER_DEF2 = nameof(m_BOM.BOMUserDef2);
            string BOMUSER_DEF3 = nameof(m_BOM.BOMUserDef3);
            string BOMUSER_DEF4 = nameof(m_BOM.BOMUserDef4);
            string TRANS_DATE = nameof(m_BOM.TransDate);
            string CREATE_DATE = nameof(m_BOM.CreateDate);
            string MODIFY_BY = nameof(m_BOM.ModifyBy);

            if (values.Contains(PART_NO))
            {
                model.ItemCode = Convert.ToString(values[PART_NO]);
            }


            if (values.Contains(MATERIAL1))
            {
                model.Material1 = Convert.ToString(values[MATERIAL1]);
            }

            if (values.Contains(MATERIAL2))
            {
                model.Material2 = Convert.ToString(values[MATERIAL2]);
            }

            if (values.Contains(BOMUSER_DEF1))
            {
                model.BOMUserDef1 = Convert.ToString(values[BOMUSER_DEF1]);
            }

            if (values.Contains(BOMUSER_DEF2))
            {
                model.BOMUserDef2 = Convert.ToString(values[BOMUSER_DEF2]);
            }

            if (values.Contains(BOMUSER_DEF3))
            {
                model.BOMUserDef3 = Convert.ToString(values[BOMUSER_DEF3]);
            }

            if (values.Contains(BOMUSER_DEF4))
            {
                model.BOMUserDef4 = Convert.ToString(values[BOMUSER_DEF4]);
            }

            if (values.Contains(TRANS_DATE))
            {
                model.TransDate = Convert.ToDateTime(values[TRANS_DATE]);
            }

            if (values.Contains(CREATE_DATE))
            {
                model.CreateDate = Convert.ToDateTime(values[CREATE_DATE]);
            }

            if (values.Contains(MODIFY_BY))
            {
                model.ModifyBy = Convert.ToString(values[MODIFY_BY]);
            }
        }

        private string GetFullErrorMessage(ModelStateDictionary modelState)
        {
            var messages = new List<string>();

            foreach (var entry in modelState)
            {
                foreach (var error in entry.Value.Errors)
                    messages.Add(error.ErrorMessage);
            }

            return String.Join(" ", messages);
        }
    }
}