using DevExtreme.AspNet.Data;
using DevExtreme.AspNet.Mvc;
using MCP_WEB.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using OfficeOpenXml.FormulaParsing.Excel.Functions.Text;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;

namespace MCP_WEB.Models.Controllers
{
    [Route("api/[controller]/[action]")]
    public class m_RoutingController : Controller
    {
        private NittanDBcontext _context;

        public m_RoutingController(NittanDBcontext context)
        {
            this._context = context;
        }

        [HttpGet]
        public IActionResult Get(DataSourceLoadOptions loadOptions)
        {
            var query = (from rt in _context.m_Routing
                        join rs in _context.m_Resource on rt.ItemCode equals rs.ItemCode into gj
                        from sub in gj.DefaultIfEmpty()
                        select new
                        {
                            rt.ItemCode,
                            PartName = sub.ItemName,
                            sub.Fcode,
                            sub.Model,
                            rt.OperationNo,
                            ProcessCode = rt.ProcessCode,
                            MachineCode = rt.MachineCode,
                            rt.PiecePerMin,
                            rt.TransDate,
                            rt.CreateDate,
                            rt.ModifyBy
                        });

            return Json(DataSourceLoader.Load(query.OrderBy(c => c.ItemCode), loadOptions));
        }

        [HttpGet]
        public IActionResult NewData(string loadOptions)
        {

            var query = from rt in _context.m_Routing.Where(r => r.ItemCode == loadOptions)
                        join rs in _context.m_Resource on rt.ItemCode equals rs.ItemCode into gj

                        from sub in gj.DefaultIfEmpty()
                        select new
                        {
                            rt.ItemCode,
                            PartName = sub.ItemName,
                            sub.Fcode,
                            sub.Model,
                            rt.OperationNo,
                            ProcessCode = rt.ProcessCode,
                            MachineCode = rt.MachineCode,
                            rt.PiecePerMin,
                            rt.TransDate,
                            rt.CreateDate,
                            rt.ModifyBy
                        };

            return new JsonResult(query);
        }


        [HttpPost]
        public IActionResult Post(string values)
        {
            var model = new m_Routing();
            var _values = JsonConvert.DeserializeObject<IDictionary>(values);
            PopulateModel(model, _values);

            if (model.OperationNo <= 0)
            {
                return BadRequest("OperationNo is negative please check data.");
            }

            if (model.ProcessCode == "")
            {
                return BadRequest("ProcessCode is empty please check data.");
            }

            if (model.MachineCode == "")
            {
                return BadRequest("MachineCode is empty please check data.");
            }

            if (model.PiecePerMin <= 0)
            {
                return BadRequest("PiecePerMin is not equal to 0 please check data.");
            }

            var ResourceCheck = _context.m_Routing.Where(b => b.ItemCode == model.ItemCode && b.OperationNo == model.OperationNo).Count();
            if (ResourceCheck > 0)
            {
                return BadRequest("Part No and OperationNo duplicate please check data.");
            }

            var ProcessCheck = _context.m_Routing.Where(b => b.ItemCode == model.ItemCode && b.ProcessCode == model.ProcessCode).Count();
            if (ProcessCheck > 0)
            {
                return BadRequest("Part No and ProcessCode duplicate please check data.");
            }

            var OperationCheck = _context.m_Routing.Where(b => b.ItemCode == model.ItemCode).Count();
            if (OperationCheck > 25)
            {
                return BadRequest("Operation greater than 25 please check data.");
            }

            if (model.ProcessCode == "Friction")
            {
                var Resource = _context.m_BOM.Where(b => b.ItemCode == model.ItemCode && b.Material1 == "" && b.Material2 == "").Count();
                if (Resource > 0)
                {
                    return BadRequest("Process Friction Material1 and Material2 is empty please check data.");
                }
            }


            if (!TryValidateModel(model))
                return BadRequest(GetFullErrorMessage(ModelState));

            var identity = (ClaimsIdentity)User.Identity;
            IEnumerable<Claim> claims = identity.Claims;
            var c = claims.FirstOrDefault();

            model.TransDate = DateTime.Now;
            model.CreateDate = DateTime.Now;
            model.ModifyBy = c.Value;

            var result = _context.m_Routing.Add(model);
            _context.SaveChanges();

            return Json(result.Entity.ItemCode);
        }

        [HttpPut]
        public IActionResult Put(string key, string values)
        {
            var resource = JObject.Parse(key);
            var itemCode = resource["ItemCode"];
            var Operation = resource["OperationNo"];

            var model = _context.m_Routing.FirstOrDefault(item => item.ItemCode == itemCode.ToString() && item.OperationNo == Int32.Parse(Operation.ToString()));
            if (model == null)
                return StatusCode(409, "m_Routing not found");


            var _values = JsonConvert.DeserializeObject<IDictionary>(values);

            if (_values[nameof(m_Routing.OperationNo)] != null && Convert.ToInt32(_values[nameof(m_Routing.OperationNo)]) < 0)
            {
                return BadRequest("OperationNo is negative please check data.");
            }

            if (_values[nameof(m_Routing.ProcessCode)] != null && Convert.ToString(_values[nameof(m_Routing.ProcessCode)]) == "")
            {
                return BadRequest("ProcessCode is empty please check data.");
            }

            if (_values[nameof(m_Routing.MachineCode)] != null && Convert.ToString(_values[nameof(m_Routing.MachineCode)]) == "")
            {
                return BadRequest("MachineCode is empty please check data.");
            }
            if (_values[nameof(m_Routing.PiecePerMin)] != null && Convert.ToDecimal(_values[nameof(m_Routing.PiecePerMin)]) <= 0)
            {
                return BadRequest("PiecePerMin is not equal to 0 please check data.");
            }

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
            var itemCode = resource["ItemCode"];
            var Operation = resource["OperationNo"];

            var CheckWeeklyPlan = _context.WeeklyPlan.Where(item => item.ItemCode == itemCode.ToString()).Count();

            if (CheckWeeklyPlan > 0)
            {
                return BadRequest("Item working on in WeeklyPlan please check data.");
            }

            var ds = (from c in _context.m_Routing
                      where c.ItemCode == itemCode.ToString() && c.OperationNo == Int32.Parse(Operation.ToString())
                      select c).ToList();

            _context.m_Routing.Remove(ds.FirstOrDefault());

            _context.SaveChanges();

            return Ok();
        }


        private void PopulateModel(m_Routing model, IDictionary values)
        {
            string ITEM_CODE = nameof(m_Routing.ItemCode);
            string OPERATION_NO = nameof(m_Routing.OperationNo);
            string PROCESS_CODE = nameof(m_Routing.ProcessCode);
            string MACHINE_CODE = nameof(m_Routing.MachineCode);
            string PIECE_PER_MIN = nameof(m_Routing.PiecePerMin);

            string TRANS_DATE = nameof(m_Routing.TransDate);
            string CREATE_DATE = nameof(m_Routing.CreateDate);
            string MODIFY_BY = nameof(m_Routing.ModifyBy);

            if (values.Contains(ITEM_CODE))
            {
                model.ItemCode = Convert.ToString(values[ITEM_CODE]);
            }

            if (values.Contains(OPERATION_NO))
            {
                model.OperationNo = Convert.ToInt32(values[OPERATION_NO]);
            }

            if (values.Contains(PROCESS_CODE))
            {
                model.ProcessCode = Convert.ToString(values[PROCESS_CODE]);
            }

            if (values.Contains(MACHINE_CODE))
            {
                model.MachineCode = Convert.ToString(values[MACHINE_CODE]);
            }

            if (values.Contains(PIECE_PER_MIN))
            {
                model.PiecePerMin = Convert.ToDecimal(values[PIECE_PER_MIN]);
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