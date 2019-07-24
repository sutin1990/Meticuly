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
using System.Collections.Generic;
using System;

namespace MCP_WEB.Controllers.WebAPI
{
    
        [Route("api/[controller]/[action]")]
        public class m_ProcessMasterController : Controller
        {

            private NittanDBcontext _context;

            public m_ProcessMasterController(NittanDBcontext context)
            {
                _context = context;
            }

            private string GetUserName()
            {
                var identity = (ClaimsIdentity)User.Identity;
                IEnumerable<Claim> claims = identity.Claims;
                var c = claims.FirstOrDefault();

                return c.Value;
            }

            [HttpGet]
            public IActionResult Get(DataSourceLoadOptions loadOptions)
            {
            var m_process = _context.m_ProcessMaster.ToList();

                return Json(DataSourceLoader.Load(m_process, loadOptions));
            }

            [HttpPost] // Create
            public IActionResult Post(string values)
            {
                
                var model = new m_ProcessMaster();
                var _values = JsonConvert.DeserializeObject<IDictionary>(values);
                var processcode = _values["ProcessCode"].ToString();
                var checkinsert = _context.m_ProcessMaster.Where(p => p.ProcessCode == processcode).ToList();
                if (checkinsert.Count > 0)
                {
                    ModelState.AddModelError("ModelError", string.Format("This processCode have already.", model.ProcessCode));
                    return BadRequest(GetFullErrorMessage(ModelState));
                }
                else
                {
                    PopulateModel(model, _values);
                    if (!TryValidateModel(model))
                        return BadRequest(GetFullErrorMessage(ModelState));

                    //model.CreateDate = DateTime.Now;
                    //model.TransDate = DateTime.Now;            
                    //model.ModifyBy = GetUserName();
                    model.SysemProcessFLag = "Y";
                    model.AllowPartialFlag = "N";
                    model.SeqNo = 99;
                    
                }
                var result = _context.m_ProcessMaster.Add(model);
                _context.SaveChanges();
                return Json(result.Entity.ProcessCode);
            }

            [HttpPut] // Update
            public IActionResult Put(string key, string values)
            {
                var model = _context.m_ProcessMaster.FirstOrDefault(item => item.ProcessCode == key);
                if (model == null)
                    return StatusCode(409, "ProcessMaster not found");

                var _values = JsonConvert.DeserializeObject<IDictionary>(values);
                PopulateModel(model, _values);

                if (!TryValidateModel(model))
                    return BadRequest(GetFullErrorMessage(ModelState));

                model.TransDate = DateTime.Now;
                model.ModifyBy = GetUserName();

                _context.SaveChanges();
                return Ok();
            }

            [HttpDelete]
            public IActionResult Delete(string key)
            {
                var checkdelete = _context.WoRouting.Where(w => w.ProcessCode == key).ToList();
                if(checkdelete.Count > 0){
                    var model = new m_ProcessMaster();
                    ModelState.AddModelError("ModelError", string.Format("This processCode being used can not be deleted.", model.ProcessCode));
                return BadRequest(GetFullErrorMessage(ModelState));
            }
            else
            {
                var modeldelete = _context.m_ProcessMaster.FirstOrDefault(item => item.ProcessCode == key);
                _context.m_ProcessMaster.Remove(modeldelete);
                _context.SaveChanges();
            }
                
            return Ok();

            }

            private void PopulateModel(m_ProcessMaster model, IDictionary values)
            {
                string Process_Code = nameof(m_ProcessMaster.ProcessCode);
                string Process_Name = nameof(m_ProcessMaster.ProcessName);
                string Sysem_ProcessFLag = nameof(m_ProcessMaster.SysemProcessFLag);
                string Allow_PartialFlag = nameof(m_ProcessMaster.AllowPartialFlag);
                string Trans_Date = nameof(m_ProcessMaster.TransDate);
                string Create_Date = nameof(m_ProcessMaster.CreateDate);
                string Modify_By = nameof(m_ProcessMaster.ModifyBy);
                string Seq_No = nameof( m_ProcessMaster.SeqNo);
                string Process_UserDef1 = nameof(m_ProcessMaster.ProcessUserDef1);
                string Process_UserDef2 = nameof(m_ProcessMaster.ProcessUserDef2);                
                
                if (values.Contains(Process_Code))
                {
                    model.ProcessCode = Convert.ToString(values[Process_Code]);
                }
                if (values.Contains(Process_Name))
                {
                    model.ProcessName = Convert.ToString(values[Process_Name]);
                }
                if (values.Contains(Sysem_ProcessFLag))
                {
                    model.SysemProcessFLag = Convert.ToString(values[Sysem_ProcessFLag]);
                }

                if (values.Contains(Allow_PartialFlag))
                {
                    model.AllowPartialFlag = Convert.ToString(values[Allow_PartialFlag]);
                }

                if (values.Contains(Trans_Date))
                {
                    model.TransDate = Convert.ToDateTime(values[Trans_Date]);
                }

                if (values.Contains(Create_Date))
                {
                    model.CreateDate = Convert.ToDateTime(values[Create_Date]);
                }

                if (values.Contains(Modify_By))
                {
                    model.ModifyBy = Convert.ToString(values[Modify_By]);
                }

                if (values.Contains(Seq_No))
                {
                    model.SeqNo = Convert.ToInt32(values[Seq_No]);
                }

                if (values.Contains(Process_UserDef1))
                {
                    model.ProcessUserDef1 = Convert.ToString(values[Process_UserDef1]);
                }

                if (values.Contains(Process_UserDef2))
                {
                    model.ProcessUserDef2 = Convert.ToString(values[Process_UserDef2]);
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

            //[HttpGet]
            //public IActionResult FormatDate()
            //{
            //    var s_GlobalPams = _context.s_GlobalPams.Where(g => g.parm_key == "DateTimeFormat").ToList();
            //    return Json(s_GlobalPams);
            //}
    }
}