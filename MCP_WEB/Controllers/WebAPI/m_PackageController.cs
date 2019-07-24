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
    public class m_PackageController : Controller
    {
        private NittanDBcontext _context;

        public m_PackageController(NittanDBcontext context) {
            this._context = context;
        }

        [HttpGet]
        public IActionResult Get(DataSourceLoadOptions loadOptions) {
            var m_package = _context.m_Package.Select(i => new {
                i.PackID,
                i.PackDesc,
                i.PackType,
                i.PackQty,
                i.TransDate,
                i.CreateDate,
                i.ModifyBy
            });
            return Json(DataSourceLoader.Load(m_package, loadOptions));
        }

        [HttpPost]
        public IActionResult Post(string values) {
            var model = new m_Package();
            var _values = JsonConvert.DeserializeObject<IDictionary>(values);
            PopulateModel(model, _values);

            if(!TryValidateModel(model))
                return BadRequest(GetFullErrorMessage(ModelState));

            ////checked duplicate
            if (_context.m_Package.Any(o => o.PackID == model.PackID))
            {
                ModelState.AddModelError("ModelError", string.Format("Pack ID {0} is duplicate key.", model.PackID));
                return BadRequest(GetFullErrorMessage(ModelState));
            }

            //Add Create date
            model.CreateDate = DateTime.Now;
            model.TransDate = DateTime.Now;

            var result = _context.m_Package.Add(model);
            _context.SaveChanges();

            return Json(result.Entity.PackID);
        }

        [HttpPut]
        public IActionResult Put(string key, string values) {
            var model = _context.m_Package.FirstOrDefault(item => item.PackID == key);
            if(model == null)
                return StatusCode(409, "m_Package not found");

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
            var model = _context.m_Package.FirstOrDefault(item => item.PackID == key);

            //checked m_BPMaster
            if (_context.m_BPMaster.Any(o => o.PackingID == key))
            {
                ModelState.AddModelError("ModelError", string.Format("Can not delete, Pack ID {0} is use by BPMaster.", key));
                return BadRequest(GetFullErrorMessage(ModelState));
            }

            var cnt = _context.m_BPMaster.Where(x => x.PackingID == key.ToString()).Count();

            if (cnt > 0)
            {
                return BadRequest(string.Format("Can not delete, Pack ID {0} is use by BPMaster.", key));
            }

            ////check WoRoutingMovement
            //if (_context.WoRoutingMovement.Any(o => o. == model.PackID))
            //{
            //    ModelState.AddModelError("ModelError", string.Format("Can not delete, Pack ID {0} is use by BPMaster.", model.PackID));
            //    return BadRequest(GetFullErrorMessage(ModelState));
            //}

            _context.m_Package.Remove(model);
            _context.SaveChanges();
            return Ok();
        }


        private void PopulateModel(m_Package model, IDictionary values) {
            string PACK_ID = nameof(m_Package.PackID);
            string PACK_DESC = nameof(m_Package.PackDesc);
            string PACK_TYPE = nameof(m_Package.PackType);
            string PACK_QTY = nameof(m_Package.PackQty);
            string TRANS_DATE = nameof(m_Package.TransDate);
            string CREATE_DATE = nameof(m_Package.CreateDate);
            string MODIFY_BY = nameof(m_Package.ModifyBy);

            if(values.Contains(PACK_ID)) {
                model.PackID = Convert.ToString(values[PACK_ID]);
            }

            if(values.Contains(PACK_DESC)) {
                model.PackDesc = Convert.ToString(values[PACK_DESC]);
            }

            if(values.Contains(PACK_TYPE)) {
                model.PackType = Convert.ToString(values[PACK_TYPE]);
            }

            if(values.Contains(PACK_QTY)) {
                model.PackQty = Convert.ToInt32(values[PACK_QTY]);
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

        [HttpGet]
        public IActionResult GetPackageTypeLookUp(DataSourceLoadOptions loadOptions)
        {
            List<m_packageType> PackageType = new List<m_packageType>();
            PackageType.Add(new m_packageType { PackageTypeTd = "Box", PackageTypeName = "Box" });
            PackageType.Add(new m_packageType { PackageTypeTd = "Tag", PackageTypeName = "Tag" });

            return Json(DataSourceLoader.Load(PackageType, loadOptions));
        }

    }
}