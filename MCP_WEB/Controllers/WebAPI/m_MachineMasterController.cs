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
    public class m_MachineMasterController : Controller
    {
        private NittanDBcontext _context;

        public m_MachineMasterController(NittanDBcontext context) {
            this._context = context;
        }

        [HttpGet]
        public IActionResult Get(DataSourceLoadOptions loadOptions) {
            var m_machinemaster = _context.m_MachineMaster.Select(i => new {
                i.MachineCode,
                i.MachineName,
                i.MachineUserDef1,
                i.MachineUserDef2,
                i.TransDate,
                i.CreateDate,
                i.ModifyBy
            });
            return Json(DataSourceLoader.Load(m_machinemaster, loadOptions));
        }

        [HttpPost]
        public IActionResult Post(string values) {
            var model = new m_MachineMaster();
            var _values = JsonConvert.DeserializeObject<IDictionary>(values);
            PopulateModel(model, _values);

            if(!TryValidateModel(model))
                return BadRequest(GetFullErrorMessage(ModelState));

            //checked duplicate
            if (_context.m_MachineMaster.Any(o => o.MachineCode == model.MachineCode))
            {
                ModelState.AddModelError("ModelError", string.Format("Machine code {0} is duplicate key.", model.MachineCode));
                return BadRequest(GetFullErrorMessage(ModelState));
            }

            //Add Create date
            model.CreateDate = DateTime.Now;
            model.TransDate = DateTime.Now;

            var result = _context.m_MachineMaster.Add(model);
            _context.SaveChanges();

            return Json(result.Entity.MachineCode);
        }

        [HttpPut]
        public IActionResult Put(string key, string values) {
            var model = _context.m_MachineMaster.FirstOrDefault(item => item.MachineCode == key);
            if(model == null)
                return StatusCode(409, "m_MachineMaster not found");

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
        public IActionResult Delete(string key) {
            var model = _context.m_MachineMaster.FirstOrDefault(item => item.MachineCode == key);

            //checked routing
            if (_context.m_Routing.Any(o => o.MachineCode == model.MachineCode))
            {
                ModelState.AddModelError("ModelError", string.Format("Can not delete, Machine code {0} is use by routing.", model.MachineCode));
                return BadRequest(GetFullErrorMessage(ModelState));
            }

            //checked wo routing
            if (_context.WoRouting.Any(o => o.MachineCode == model.MachineCode))
            {
                ModelState.AddModelError("ModelError", string.Format("Can not delete, Machine code {0} is use by wo routing.", model.MachineCode));
                return BadRequest(GetFullErrorMessage(ModelState));
            }

            _context.m_MachineMaster.Remove(model);
            _context.SaveChanges();

            return Ok();
        }


        private void PopulateModel(m_MachineMaster model, IDictionary values) {
            string MACHINE_CODE = nameof(m_MachineMaster.MachineCode);
            string MACHINE_NAME = nameof(m_MachineMaster.MachineName);
            string MACHINE_USER_DEF1 = nameof(m_MachineMaster.MachineUserDef1);
            string MACHINE_USER_DEF2 = nameof(m_MachineMaster.MachineUserDef2);
            string TRANS_DATE = nameof(m_MachineMaster.TransDate);
            string CREATE_DATE = nameof(m_MachineMaster.CreateDate);
            string MODIFY_BY = nameof(m_MachineMaster.ModifyBy);

            if(values.Contains(MACHINE_CODE)) {
                model.MachineCode = Convert.ToString(values[MACHINE_CODE]);
            }

            if(values.Contains(MACHINE_NAME)) {
                model.MachineName = Convert.ToString(values[MACHINE_NAME]);
            }

            if(values.Contains(MACHINE_USER_DEF1)) {
                model.MachineUserDef1 = Convert.ToString(values[MACHINE_USER_DEF1]);
            }

            if(values.Contains(MACHINE_USER_DEF2)) {
                model.MachineUserDef2 = Convert.ToString(values[MACHINE_USER_DEF2]);
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