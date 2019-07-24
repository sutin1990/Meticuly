using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DevExtreme.AspNet.Data;
using DevExtreme.AspNet.Mvc;
using MCP_WEB.Data;
using MCP_WEB.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Collections;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.Security.Claims;

namespace MCP_WEB.Controllers.WebAPI {

    [Route("api/[controller]/[action]")]
    public class m_JigDevXController : Controller {

        private NittanDBcontext _context;

        public m_JigDevXController(NittanDBcontext context) {
            this._context = context;
        }

        [HttpGet]
        public IActionResult Get(DataSourceLoadOptions loadOptions) {
            var m_Jig = _context.m_Jig.Select(i => new {
                i.JigID,
                i.JigNo,
                i.Column,
                i.Row,
                i.JigUserDef1,
                i.JigUserDef2,
                i.TransDate,
                i.CreateDate,
                i.ModifyBy
            });

            return Json(DataSourceLoader.Load(m_Jig, loadOptions));
        }

        [HttpPost] // Create
        public IActionResult Post(string values) {
            var model = new m_Jig();
            var _values = JsonConvert.DeserializeObject<IDictionary>(values);
            PopulateModel(model, _values);

            if (!TryValidateModel(model))
                return BadRequest(GetFullErrorMessage(ModelState));

            //checked duplicate
            if (_context.m_Jig.Any(o => o.JigNo == model.JigNo)) {
                ModelState.AddModelError("ModelError", string.Format("JigNo {0} is duplicate.", model.JigNo));
                return BadRequest(GetFullErrorMessage(ModelState));
            }

            int newJigID = 1 + _context.m_Jig.Max(p => p.JigID);
            model.JigID = newJigID;
            model.CreateDate = DateTime.Now;
            //// TODO edit 2 line
            //model.JigUserDef1 = DateTime.Today;
            model.ModifyBy = GetUserName(); // "FixIt";

            var result = _context.m_Jig.Add(model);
            _context.SaveChanges();

            return Json(result.Entity.JigID);
        }

        [HttpPut] // Update
        public IActionResult Put(int key, string values) {
            var model = _context.m_Jig.FirstOrDefault(item => item.JigID == key);
            if (model == null)
                return StatusCode(409, "m_Jig not found");

            var _values = JsonConvert.DeserializeObject<IDictionary>(values);
            PopulateModel(model, _values);
            
            if (!TryValidateModel(model))
                return BadRequest(GetFullErrorMessage(ModelState));

            model.TransDate = DateTime.Now; // jom add
            model.ModifyBy = GetUserName();

            _context.SaveChanges();
            return Ok();
        }

        [HttpDelete]
        public void Delete(int key) {
            var model = _context.m_Jig.FirstOrDefault(item => item.JigID == key);

            _context.m_Jig.Remove(model);
            _context.SaveChanges();
        }

        private void PopulateModel(m_Jig model, IDictionary values) {
            //string JigID = nameof(m_Jig.JigID);
            string JigNo = nameof(m_Jig.JigNo);
            string Column = nameof(m_Jig.Column);
            string Row = nameof(m_Jig.Row);

            string JigUserDef1 = nameof(m_Jig.JigUserDef1);
            string JigUserDef2 = nameof(m_Jig.JigUserDef2);
            //if (values.Contains(JigID)) {
            //    model.JigID = Convert.ToInt32(values[JigID]);
            //}
            if (values.Contains(JigNo)) {
                model.JigNo = Convert.ToString(values[JigNo]);
            }
            if (values.Contains(Column)) {
                model.Column = Convert.ToInt32(values[Column]);
            }
            if (values.Contains(Row)) {
                model.Row = Convert.ToInt32(values[Row]);
            }

            if (values.Contains(JigUserDef1)) {
                model.JigUserDef1 = Convert.ToString(values[JigUserDef1]);
            }
            if (values.Contains(JigUserDef2)) {
                model.JigUserDef2 = Convert.ToString(values[JigUserDef2]);
            }
        }

        private string GetFullErrorMessage(ModelStateDictionary modelState) {
            var messages = new List<string>();
            foreach (var entry in modelState) {
                foreach (var error in entry.Value.Errors)
                    messages.Add(error.ErrorMessage);
            }
            return String.Join(" ", messages);
        }

        private string GetUserName() {
            var identity = (ClaimsIdentity)User.Identity;
            IEnumerable<Claim> claims = identity.Claims;
            var c = claims.FirstOrDefault();

            return c.Value;
        }


    }
}