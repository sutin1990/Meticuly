using DevExtreme.AspNet.Data;
using DevExtreme.AspNet.Mvc;
using MCP_WEB.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace MCP_WEB.Models.Controllers
{
    [Route("api/[controller]/[action]")]
    public class m_ResourceController : Controller
    {
        private NittanDBcontext _context;

        public m_ResourceController(NittanDBcontext context)
        {
            this._context = context;
        }

        [HttpGet]
        public IActionResult Get(DataSourceLoadOptions loadOptions)
        {
            var m_resource = _context.m_Resource.OrderBy(n => n.ItemCode).Select(i => new
            {
                i.ItemCode,
                i.ItemName,
                i.Model,
                i.Fcode,
                i.BPCode,
                i.ItemType,
                i.StdLotSize,
                i.Tolerance,
                i.Status,
                i.Dimension1,
                i.Dimension2,
                i.Color,
                i.Length,
                i.Batch1,
                i.Batch2,
                i.Uom,
                i.HeatNo,
                i.LengthUom,
                i.ItemUserDef1,
                i.ItemUserDef2,
                i.ItemUserDef3,
                i.ItemUserDef4,
                i.ItemUserDef5,
                i.TransDate,
                i.CreateDate,
                i.ModifyBy,
                i.ItemImage
            });
            return Json(DataSourceLoader.Load(m_resource, loadOptions));
        }

        [HttpPost]
        public IActionResult Post(string values)
        {
            var model = new m_Resource();
            var _values = JsonConvert.DeserializeObject<IDictionary>(values);
            PopulateModel(model, _values);


            if (_context.m_Resource.Any(b => b.ItemCode == model.ItemCode))
            {
                return BadRequest("Item no." + model.ItemCode + " is duplicate key please check data.");
            }

            if (model.ItemType == "F" && model.StdLotSize == null)
            {
                return BadRequest("Std. Lot Size Required.");
            }

            var ResourceCheck = _context.m_Resource.Where(b => b.ItemCode == model.ItemCode && b.Fcode == model.Fcode && b.Model == model.Model).Count();
            if (ResourceCheck > 0)
            {
                return BadRequest("Resource is duplicate please check data.");
            }
            if (!TryValidateModel(model))
                return BadRequest(GetFullErrorMessage(ModelState));

            var result = _context.m_Resource.Add(model);
            _context.SaveChanges();

            return Json(result.Entity.ItemCode);
        }

        [HttpPut]
        public IActionResult Put(string key, string values)
        {
            var resource = JObject.Parse(key);
            var Code = resource["ItemCode"];

            var model = _context.m_Resource.FirstOrDefault(item => item.ItemCode == Code.ToString());
            if (model == null)
                return StatusCode(409, "m_Resource not found.");

            var _values = JsonConvert.DeserializeObject<IDictionary>(values);
            PopulateModel(model, _values);

            if (!TryValidateModel(model))
                return BadRequest(GetFullErrorMessage(ModelState));

            _context.SaveChanges();
            return Ok();
        }

        [HttpDelete]
        public IActionResult Delete(string key)
        {
            var resource = JObject.Parse(key);
            var Code = resource["ItemCode"];

            if (_context.m_Routing.Any(x => x.ItemCode == Code.ToString()))
            {
                ModelState.AddModelError("ModelError", string.Format("Can not delete, ItemCode {0} is use by Resource.", key));
                return BadRequest(GetFullErrorMessage(ModelState));
            }

            var r = (from a in _context.m_Resource
                     join b in _context.WeeklyPlan on a.ItemCode equals b.ItemCode
                     join c in _context.WoRouting on b.BarCode equals c.BarCode
                     where a.ItemCode == Code.ToString()
                     select new
                     {
                         ItemCode = a.ItemCode
                     });

            if (r.Any(s => s.ItemCode == Code.ToString()))
            {
                ModelState.AddModelError("ModelError", string.Format("Can not delete, ItemCode {0} is use by Resource.", Code.ToString()));
                return BadRequest(GetFullErrorMessage(ModelState));
            }

            var model = _context.m_Resource.FirstOrDefault(item => item.ItemCode == Code.ToString());

            _context.m_Resource.Remove(model);
            _context.SaveChanges();

            return Ok();
        }


        private void PopulateModel(m_Resource model, IDictionary values)
        {
            string ITEM_CODE = nameof(m_Resource.ItemCode);
            string ITEM_NAME = nameof(m_Resource.ItemName);
            string MODEL = nameof(m_Resource.Model);
            string FCODE = nameof(m_Resource.Fcode);
            string BPCODE = nameof(m_Resource.BPCode);
            string ITEM_TYPE = nameof(m_Resource.ItemType);
            string STD_LOT_SIZE = nameof(m_Resource.StdLotSize);
            string TOLERANCE = nameof(m_Resource.Tolerance);
            string STATUS = nameof(m_Resource.Status);
            string DIMENSION1 = nameof(m_Resource.Dimension1);
            string DIMENSION2 = nameof(m_Resource.Dimension2);
            string COLOR = nameof(m_Resource.Color);
            string LENGTH = nameof(m_Resource.Length);
            string BATCH1 = nameof(m_Resource.Batch1);
            string BATCH2 = nameof(m_Resource.Batch2);
            string UOM = nameof(m_Resource.Uom);
            string HEAT_NO = nameof(m_Resource.HeatNo);
            string LENGTH_UOM = nameof(m_Resource.LengthUom);
            string ITEM_USER_DEF1 = nameof(m_Resource.ItemUserDef1);
            string ITEM_USER_DEF2 = nameof(m_Resource.ItemUserDef2);
            string ITEM_USER_DEF3 = nameof(m_Resource.ItemUserDef3);
            string ITEM_USER_DEF4 = nameof(m_Resource.ItemUserDef4);
            string ITEM_USER_DEF5 = nameof(m_Resource.ItemUserDef5);
            string TRANS_DATE = nameof(m_Resource.TransDate);
            string CREATE_DATE = nameof(m_Resource.CreateDate);
            string MODIFY_BY = nameof(m_Resource.ModifyBy);
            string ITEMIMAGE = nameof(m_Resource.ItemImage);

            if (values.Contains(ITEM_CODE))
            {
                model.ItemCode = Convert.ToString(values[ITEM_CODE]);
            }

            if (values.Contains(ITEM_NAME))
            {
                model.ItemName = Convert.ToString(values[ITEM_NAME]);
            }

            if (values.Contains(MODEL))
            {
                model.Model = Convert.ToString(values[MODEL]);
            }

            if (values.Contains(FCODE))
            {
                model.Fcode = Convert.ToString(values[FCODE]);
            }

            if (values.Contains(BPCODE))
            {
                model.BPCode = Convert.ToString(values[BPCODE]);
            }

            if (values.Contains(ITEM_TYPE))
            {
                model.ItemType = Convert.ToString(values[ITEM_TYPE]);
            }

            if (values.Contains(STD_LOT_SIZE))
            {
                model.StdLotSize = Convert.ToInt32(values[STD_LOT_SIZE]);
            }

            if (values.Contains(TOLERANCE))
            {
                model.Tolerance = Convert.ToInt32(values[TOLERANCE]);
            }

            if (values.Contains(STATUS))
            {
                model.Status = Convert.ToString(values[STATUS]);
            }

            if (values.Contains(DIMENSION1))
            {
                model.Dimension1 = Convert.ToDecimal(values[DIMENSION1]);
            }

            if (values.Contains(DIMENSION2))
            {
                model.Dimension2 = Convert.ToDecimal(values[DIMENSION2]);
            }

            if (values.Contains(COLOR))
            {
                model.Color = Convert.ToString(values[COLOR]);
            }

            if (values.Contains(LENGTH))
            {
                model.Length = Convert.ToInt32(values[LENGTH]);
            }

            if (values.Contains(BATCH1))
            {
                model.Batch1 = Convert.ToString(values[BATCH1]);
            }

            if (values.Contains(BATCH2))
            {
                model.Batch2 = Convert.ToString(values[BATCH2]);
            }

            if (values.Contains(UOM))
            {
                model.Uom = Convert.ToString(values[UOM]);
            }

            if (values.Contains(HEAT_NO))
            {
                model.HeatNo = Convert.ToString(values[HEAT_NO]);
            }

            if (values.Contains(LENGTH_UOM))
            {
                model.LengthUom = Convert.ToString(values[LENGTH_UOM]);
            }

            if (values.Contains(ITEM_USER_DEF1))
            {
                model.ItemUserDef1 = Convert.ToString(values[ITEM_USER_DEF1]);
            }

            if (values.Contains(ITEM_USER_DEF2))
            {
                model.ItemUserDef2 = Convert.ToString(values[ITEM_USER_DEF2]);
            }

            if (values.Contains(ITEM_USER_DEF3))
            {
                model.ItemUserDef3 = Convert.ToString(values[ITEM_USER_DEF3]);
            }

            if (values.Contains(ITEM_USER_DEF4))
            {
                model.ItemUserDef4 = Convert.ToString(values[ITEM_USER_DEF4]);
            }

            if (values.Contains(ITEM_USER_DEF5))
            {
                model.ItemUserDef5 = Convert.ToString(values[ITEM_USER_DEF5]);
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

            if (values.Contains(ITEMIMAGE))
            {
                model.ItemImage = Convert.ToString(values[ITEMIMAGE]);
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