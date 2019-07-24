using DevExtreme.AspNet.Data;
using DevExtreme.AspNet.Mvc;
using MCP_WEB.Data;
using MCP_WEB.Helper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
namespace MCP_WEB.Models.Controllers
{
    [Route("api/[controller]/[action]")]
    public class m_DepController : Controller
    {
        private NittanDBcontext _context;
        

        public m_DepController(NittanDBcontext context) {
            this._context = context;
        }

        [HttpGet]
        public IActionResult Get(DataSourceLoadOptions loadOptions) {
            //var m_dep = _context.m_Dep.Select(i => new
            //{
            //    i.ID,
            //    i.DepID,
            //    i.DepDesc,               
            //    i.TransDate,
            //    i.CreateDate,
            //    i.ModifyBy
            //});
            var m_dep = from d in _context.m_Dep
                        select new
                        {
                            d.ID,
                            d.DepID,
                            d.DepDesc,
                            TransDate = Convert.ToDateTime(d.TransDate),
                            CreateDate = Convert.ToDateTime(d.CreateDate),
                            d.ModifyBy
                        };

            //var m_dep = (from d in _context.m_Dep
            //             join dm in _context.m_DepMenu on d.DepID equals dm.DepID
            //             join m in _context.MenuMaster on dm.MenuIdentity equals m.MenuIdentity
            //             orderby d.DepID ascending
            //             select new
            //             {
            //                 d.ID,
            //                 d.DepID,
            //                 d.DepDesc,
            //                 dm.MenuIdentity,
            //                 d.TransDate,
            //                 d.CreateDate,
            //                 d.ModifyBy,
            //                 m.MenuName

            //             });
            return Json(DataSourceLoader.Load(m_dep, loadOptions));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Post(string values) {
            if (ModelState.IsValid)
            {
                return View("SuccessValidation");
            }
            var model = new m_Dep();
            var _values = JsonConvert.DeserializeObject<IDictionary>(values);
            PopulateModel(model, _values);

            if(!TryValidateModel(model))
                return BadRequest(GetFullErrorMessage(ModelState));

            var result = _context.m_Dep.Add(model);
            _context.SaveChanges();

            return Json(result.Entity.ID);
        }

        [HttpPost]        
        public IActionResult insert(string DepDesc, DateTime CreateDate, string DepID, string[] getitem)
        {
            var identity = (ClaimsIdentity)User.Identity;
            IEnumerable<Claim> claims = identity.Claims;
            var userID = claims.FirstOrDefault();

            var insertdep = new m_Dep
            {
                DepID = DepID,
                DepDesc = DepDesc,
                TransDate = Convert.ToDateTime(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")),
                CreateDate = Convert.ToDateTime(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")),
                ModifyBy = userID.Value
            };
            _context.m_Dep.Add(insertdep);
            _context.SaveChanges();       
                

                for (var i = 0; i<getitem.Length; i++)
                {
                    var insertdepmenu = new m_DepMenu
                    {
                        DepID = DepID,
                        //DepDesc = DepDesc,
                        MenuIdentity = Convert.ToInt32(getitem[i].ToString()),
                        TransDate = Convert.ToDateTime(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")),
                        CreateDate = Convert.ToDateTime(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")),
                        ModifyBy = userID.Value
                    };
                    _context.m_DepMenu.Add(insertdepmenu);
                    _context.SaveChanges();
                }
    

                return Json(_context.m_Dep);

        }

        [HttpPut]
        public IActionResult Upsert(string DepDesc,DateTime CreateDate, string DepID, string[] getitem)
            {
            var identity = (ClaimsIdentity)User.Identity;
            IEnumerable<Claim> claims = identity.Claims;
            var userID = claims.FirstOrDefault();

            var menu_dep = _context.m_DepMenu.Where(m => m.DepID == DepID).ToList();
            
                _context.m_DepMenu.RemoveRange(menu_dep);//delete ออกไปให้หมดก่อน
                _context.SaveChanges();

                var department = _context.m_Dep.FirstOrDefault(d => d.DepID == DepID);
                department.DepDesc = DepDesc;
                department.TransDate = Convert.ToDateTime(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
                department.ModifyBy = userID.Value;
                _context.m_Dep.Update(department);
                _context.SaveChanges();

                for (var i = 0; i < getitem.Length; i++)
                {
                    var insertdepmenu = new m_DepMenu
                    {
                        DepID = DepID,
                        //DepDesc = DepDesc,
                        MenuIdentity = Convert.ToInt32(getitem[i].ToString()),
                        TransDate = Convert.ToDateTime(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")),
                        CreateDate = Convert.ToDateTime(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")),
                        ModifyBy = userID.Value
                    };
                    _context.m_DepMenu.Add(insertdepmenu);
                    _context.SaveChanges();
                }
           
            return Json(_context.m_DepMenu);
        }

        [HttpPost]
        public IActionResult deletemulti(string[] selectedID)
        {
            if (selectedID != null)
            {
                var list = selectedID.ToList();
                var m_dep = _context.m_Dep.Where(x => list.Contains(x.ID.ToString()));

                _context.m_Dep.RemoveRange(m_dep);

                _context.SaveChanges();
            }               
            
            return Json(_context.m_Dep);
        }

        [HttpGet]
        public IActionResult deleterowdep(string ID,string DepID)
        {
            // m_UserMaster result = new m_UserMaster();
            int result = 1;
            var userdep = _context.m_UserMaster.Where(item => item.DepID == DepID).ToList();
            if (userdep.Count > 0)
            {
                result = 1;
            }
            else
            {
                result = 0;
                if (ID != null)
                {
                    var removedep = _context.m_Dep.FirstOrDefault(item => item.ID.ToString() == ID);
                    _context.m_Dep.Remove(removedep);
                    _context.SaveChanges();

                    var removemenudep = _context.m_DepMenu.Where(item => item.DepID == DepID);
                    _context.m_DepMenu.RemoveRange(removemenudep);
                    _context.SaveChanges();
                }
            }
            
           
            return Json(result);
        }

        [HttpPut]
        public IActionResult Put(string key, string values) {
            var model = _context.m_Dep.FirstOrDefault(item => item.DepID == key);
            if(model == null)
                return StatusCode(409, "m_Dep not found");

            var _values = JsonConvert.DeserializeObject<IDictionary>(values);
            PopulateModel(model, _values);

            if(!TryValidateModel(model))
                return BadRequest(GetFullErrorMessage(ModelState));

            _context.SaveChanges();
            return Ok();
        }

        [HttpDelete]
        public void Delete(string key) {
            var model = _context.m_Dep.FirstOrDefault(item => item.DepID == key);

            _context.m_Dep.Remove(model);
            _context.SaveChanges();
        }
        [HttpGet]
        public IActionResult MenuMasterLookup2(DataSourceLoadOptions loadOptions, string DepID)
        {
            var lookup = from i in _context.MenuMaster
                         //join d in _context.m_Dep
                         //on i.MenuIdentity equals d.MenuIdentity into dep
                         //from de in dep.Where(d => d.DepID == DepID).DefaultIfEmpty()
                          //where i.Parent_MenuID == "*"
                         //orderby i.MenuIdentity ascending
                         select new
                         {
                             i.MenuID,
                             i.MenuIdentity,
                             i.MenuName,
                             i.Parent_MenuID,
                            
                         };

            
            return Json(DataSourceLoader.Load(lookup, loadOptions));
        }

        [HttpGet]
        public IActionResult MenuMasterLookup(DataSourceLoadOptions loadOptions,string DepID) {
            var lookup = from i in _context.MenuMaster
                         join d in _context.m_DepMenu
                         on i.MenuIdentity equals d.MenuIdentity  into dep
                         from de in dep.Where(d => d.DepID == DepID).DefaultIfEmpty()                         
                         where i.Parent_MenuID != "*"
                        //orderby i.MenuIdentity ascending
                         select new {
                                 i.MenuID,
                                 i.MenuIdentity,
                                i.MenuName,
                                i.Parent_MenuID,
                                de.DepID,
                                selected= de.DepID == DepID ? "selected": null
                         };
            return Json(DataSourceLoader.Load(lookup, loadOptions));
        }

        [HttpGet]
        public IActionResult Check_Have_DepID(DataSourceLoadOptions loadOptions, string DepID)
        {
            var lookup = from de in _context.m_Dep
                         where de.DepID == DepID                         
                         select new
                         {
                             de.DepID
                         };
            
            //    if (lookup!=null) {
            //    ModelState.AddModelError("ModelError", string.Format("DepID have already.", lookup));
            //    return BadRequest(GetFullErrorMessage(ModelState));
            //}
            
                
            return Json(DataSourceLoader.Load(lookup, loadOptions));
        }

        [HttpGet]
        public IActionResult FormatDate()
        {
            var s_GlobalPams = _context.s_GlobalPams.Where(g => g.parm_key == "DateTimeFormat").ToList();
            return Json(s_GlobalPams);
        }

        private void PopulateModel(m_Dep model, IDictionary values) {
            
            //string ID = nameof(m_Dep.ID);
            string DEP_ID = nameof(m_Dep.DepID);
            string DEP_DESC = nameof(m_Dep.DepDesc);
            //string MENU_IDENTITY = nameof(m_Dep.MenuIdentity);
            string TRANS_DATE = nameof(m_Dep.TransDate);
            string CREATE_DATE = nameof(m_Dep.CreateDate);
            string MODIFY_BY = nameof(m_Dep.ModifyBy);

           

            if(values.Contains(DEP_ID)) {
                model.DepID = Convert.ToString(values[DEP_ID]);
            }

            if(values.Contains(DEP_DESC)) {
                model.DepDesc = Convert.ToString(values[DEP_DESC]);
            }

            //if(values.Contains(MENU_IDENTITY)) {
            //    model.MenuIdentity = Convert.ToInt32(values[MENU_IDENTITY]);
            //}

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