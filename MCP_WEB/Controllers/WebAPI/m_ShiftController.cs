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
using System.Security.Claims;

namespace MCP_WEB.Models.Controllers
{
    [Route("api/[controller]/[action]")]
    public class m_ShiftController : Controller
    {
        private NittanDBcontext _context;

        public m_ShiftController(NittanDBcontext context) {
            this._context = context;
        }

        [HttpGet]
        public IActionResult Get(DataSourceLoadOptions loadOptions) {
            var m_shifts = _context.m_Shift.Select(i => new {
                i.ShiftID,
                i.ShiftType,
                i.ShiftDesc,
                i.StartTime,
                i.EndTime,
                i.TransDate,
                i.CreateDate,
                i.ModifyBy
            });
            return Json(DataSourceLoader.Load(m_shifts, loadOptions));
        }

        [HttpPost]
        public IActionResult Post(string values) {
            var model = new m_Shift();
            var _values = JsonConvert.DeserializeObject<IDictionary>(values);
            PopulateModel(model, _values);

            if(!TryValidateModel(model))
                return BadRequest(GetFullErrorMessage(ModelState));

            //checked duplicate
            //if (_context.m_Shift.Any(o => o.ShiftID == model.ShiftID))
            //{
            //    ModelState.AddModelError("ModelError", string.Format("Shift Id {0} is duplicate key.", model.ShiftID));
            //    return BadRequest(GetFullErrorMessage(ModelState));
            //}

            var maxValue = _context.m_Shift.Max(x => x.ShiftID);
            var maxRow = _context.m_Shift.First(x => x.ShiftID == maxValue);
            if (maxRow == null)
            {
                maxRow.ShiftID = 0;
            }

            model.ShiftID = maxRow.ShiftID + 1;
            //Add Create date
            model.CreateDate = DateTime.Now;
            model.TransDate = DateTime.Now;

            var result = _context.m_Shift.Add(model);
            _context.SaveChanges();

            return Json(result.Entity.ShiftID);
        }

        [HttpPut]
        public IActionResult Put(int key, string values) {
            var model = _context.m_Shift.FirstOrDefault(item => item.ShiftID == key);
            if(model == null)
                return StatusCode(409, "m_Shift not found");

            var _values = JsonConvert.DeserializeObject<IDictionary>(values);
            PopulateModel(model, _values);

            if(!TryValidateModel(model))
                return BadRequest(GetFullErrorMessage(ModelState));
            
            //Add user id
            var identity = (ClaimsIdentity)User.Identity;
            IEnumerable<Claim> claims = identity.Claims;
            var c = claims.FirstOrDefault();
            model.ModifyBy = c.Value;
            _context.Entry(model).Property(x => x.ModifyBy).IsModified = true;

            //Change trans date
            model.TransDate = DateTime.Now;
            _context.Entry(model).Property(x => x.TransDate).IsModified = true;

            _context.SaveChanges();
            return Ok();
        }

        [HttpDelete]
        public IActionResult Delete(int key) {
            var model = _context.m_Shift.FirstOrDefault(item => item.ShiftID == key);

            //check WoRoutingMovement
            if (_context.WoRoutingMovement.Any(o => o.ShiftID == model.ShiftID))
            {
                ModelState.AddModelError("ModelError", string.Format("Can not delete, Shift Type {0} is use by WoRoutingMovement.", model.ShiftType));
                return BadRequest(GetFullErrorMessage(ModelState));
            }

            _context.m_Shift.Remove(model);
            _context.SaveChanges();
            return Ok();
        }


        private void PopulateModel(m_Shift model, IDictionary values) {
            string SHIFT_ID = nameof(m_Shift.ShiftID);
            string SHIFT_TYPE = nameof(m_Shift.ShiftType);
            string SHIFT_DESC = nameof(m_Shift.ShiftDesc);
            string START_TIME = nameof(m_Shift.StartTime);
            string END_TIME = nameof(m_Shift.EndTime);
            string TRANS_DATE = nameof(m_Shift.TransDate);
            string CREATE_DATE = nameof(m_Shift.CreateDate);
            string MODIFY_BY = nameof(m_Shift.ModifyBy);

            if(values.Contains(SHIFT_ID)) {
                model.ShiftID = Convert.ToInt32(values[SHIFT_ID]);
            }

            if(values.Contains(SHIFT_TYPE)) {
                model.ShiftType = Convert.ToString(values[SHIFT_TYPE]);
            }

            if(values.Contains(SHIFT_DESC)) {
                model.ShiftDesc = Convert.ToString(values[SHIFT_DESC]);
            }

            if(values.Contains(START_TIME)) {
                model.StartTime = Convert.ToDateTime(values[START_TIME]);
            }

            if(values.Contains(END_TIME)) {
                model.EndTime = Convert.ToDateTime(values[END_TIME]);
            }

            if(values.Contains(TRANS_DATE)) {
                model.TransDate = Convert.ToDateTime(values[TRANS_DATE]);
            }

            if(values.Contains(CREATE_DATE)) {
                model.CreateDate = Convert.ToDateTime(values[CREATE_DATE]);
            }

            if(values.Contains(MODIFY_BY)) {
                model.ModifyBy = Convert.ToString(values[MODIFY_BY]);
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