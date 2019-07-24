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
    public class m_BPMasterController : Controller
    {
        private NittanDBcontext _context;

        public m_BPMasterController(NittanDBcontext context) {
            this._context = context;
        }

        [HttpGet]
        public IActionResult Get(DataSourceLoadOptions loadOptions) {
            var m_bpmasters = _context.m_BPMaster.Select(i => new {
                i.BPCode,
                i.BPName,
                i.BPType,
                i.BPAddress1,
                i.BPAddress2,
                i.BPAddress3,
                i.BPAddress4,
                i.BPAddress5,
                i.TransDate,
                i.CreateDate,
                i.ModifyBy,
                i.BPAddress6,
                i.TagFormat,
                i.PackingID
            });
            return Json(DataSourceLoader.Load(m_bpmasters, loadOptions));
        }

        [HttpPost]
        public IActionResult Post(string values) {
            var model = new m_BPMaster();
            var _values = JsonConvert.DeserializeObject<IDictionary>(values);
            PopulateModel(model, _values);

            if(!TryValidateModel(model))
                return BadRequest(GetFullErrorMessage(ModelState));

            ////checked duplicate
            if (_context.m_BPMaster.Any(o => o.BPCode == model.BPCode))
            {
                ModelState.AddModelError("ModelError", string.Format("BP Code {0} is duplicate key.", model.BPCode));
                return BadRequest(GetFullErrorMessage(ModelState));
            }

            //Add Create date
            model.CreateDate = DateTime.Now;
            model.TransDate = DateTime.Now;

            var result = _context.m_BPMaster.Add(model);
            _context.SaveChanges();

            return Json(result.Entity.BPCode);
        }

        [HttpPut]
        public IActionResult Put(string key, string values) {
            var model = _context.m_BPMaster.FirstOrDefault(item => item.BPCode == key);
            if(model == null)
                return StatusCode(409, "m_BPMaster not found");
                     

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
            var model = _context.m_BPMaster.FirstOrDefault(item => item.BPCode == key);

            //checked in use m_Resource
            if (_context.m_Resource.Any(o => o.BPCode == key))
            {
                ModelState.AddModelError("ModelError", string.Format("Can not delete, BP CODE {0} is use by Resource.", key));
                return BadRequest(GetFullErrorMessage(ModelState));
            }

            _context.m_BPMaster.Remove(model);
            _context.SaveChanges();
            return Ok();
        }


        private void PopulateModel(m_BPMaster model, IDictionary values) {
            string BPCODE = nameof(m_BPMaster.BPCode);
            string BPNAME = nameof(m_BPMaster.BPName);
            string BPTYPE = nameof(m_BPMaster.BPType);
            string BPADDRESS1 = nameof(m_BPMaster.BPAddress1);
            string BPADDRESS2 = nameof(m_BPMaster.BPAddress2);
            string BPADDRESS3 = nameof(m_BPMaster.BPAddress3);
            string BPADDRESS4 = nameof(m_BPMaster.BPAddress4);
            string BPADDRESS5 = nameof(m_BPMaster.BPAddress5);
            string TRANS_DATE = nameof(m_BPMaster.TransDate);
            string CREATE_DATE = nameof(m_BPMaster.CreateDate);
            string MODIFY_BY = nameof(m_BPMaster.ModifyBy);
            string BPADDRESS6 = nameof(m_BPMaster.BPAddress6);
            string TAG_FORMAT = nameof(m_BPMaster.TagFormat);
            string PACKING_ID = nameof(m_BPMaster.PackingID);

            if(values.Contains(BPCODE)) {
                model.BPCode = Convert.ToString(values[BPCODE]);
            }

            if(values.Contains(BPNAME)) {
                model.BPName = Convert.ToString(values[BPNAME]);
            }

            if(values.Contains(BPTYPE)) {
                model.BPType = Convert.ToString(values[BPTYPE]);
            }

            if(values.Contains(BPADDRESS1)) {
                model.BPAddress1 = Convert.ToString(values[BPADDRESS1]);
            }

            if(values.Contains(BPADDRESS2)) {
                model.BPAddress2 = Convert.ToString(values[BPADDRESS2]);
            }

            if(values.Contains(BPADDRESS3)) {
                model.BPAddress3 = Convert.ToString(values[BPADDRESS3]);
            }

            if(values.Contains(BPADDRESS4)) {
                model.BPAddress4 = Convert.ToString(values[BPADDRESS4]);
            }

            if(values.Contains(BPADDRESS5)) {
                model.BPAddress5 = Convert.ToString(values[BPADDRESS5]);
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

            if(values.Contains(BPADDRESS6)) {
                model.BPAddress6 = Convert.ToString(values[BPADDRESS6]);
            }

            if(values.Contains(TAG_FORMAT)) {
                model.TagFormat = Convert.ToString(values[TAG_FORMAT]);
            }

            if(values.Contains(PACKING_ID)) {
                model.PackingID = Convert.ToString(values[PACKING_ID]);
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
        public IActionResult GetBPTypeLookUp(DataSourceLoadOptions loadOptions)
        {
            List<m_BPType> PackageType = new List<m_BPType>();
            PackageType.Add(new m_BPType { TypeId = "C", TypeName = "C" });
            PackageType.Add(new m_BPType { TypeId = "B", TypeName = "B" });
            PackageType.Add(new m_BPType { TypeId = "S", TypeName = "S" });

            return Json(DataSourceLoader.Load(PackageType, loadOptions));
        }

        [HttpGet]
        public IActionResult GetPackingIdLookUp(DataSourceLoadOptions loadOptions)
        {
             var model = _context.m_Package.Where(w => w.PackType == "Box").Select(i => new
            {
                PackageTypeTd = i.PackID,
                PackageTypeName = i.PackDesc,
                PackageDisp = i.PackID + "-" + i.PackDesc
            });

            return Json(DataSourceLoader.Load(model, loadOptions));
        }

        [HttpGet]
        public IActionResult GetTagFormatLookUp(DataSourceLoadOptions loadOptions)
        {
            var model = _context.m_Package.Where(w => w.PackType == "Tag").Select(i => new
            {
                PackageTypeTd = i.PackID,
                PackageTypeName = i.PackDesc,
                PackageDisp = i.PackID + "-" + i.PackDesc
            });

            return Json(DataSourceLoader.Load(model, loadOptions));
        }
    }
}