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
using System.Data.SqlClient;
using System.Linq;
using System.Security.Claims;

namespace MCP_WEB.Models.Controllers
{
    [Route("api/[controller]/[action]")]
    public class m_UserMasterController : Controller
    {
        private NittanDBcontext _context;

        public m_UserMasterController(NittanDBcontext context)
        {
            this._context = context;
        }

        [HttpGet]
        public IActionResult Get(DataSourceLoadOptions loadOptions)
        {
            
                var countuser = _context.m_UserMaster.ToList();
            var VW_MFC_UserMasters = (from u in _context.VW_MFC_UserMasters
                                     select new
                                     {
                                         u.DepID,
                                         u.UserId,
                                         u.UserName,
                                         u.UserPassword,
                                         u.FirstName,
                                         u.LastName,
                                         u.UserEmail,
                                         u.ClusterCode,
                                         u.ShiftID,
                                         Status = u.Status == "A" ? "Active" : "Inactive",
                                         u.EmployeeCode,
                                         u.CompanyCode,
                                         u.LocationCode,
                                         u.UserLocationId,
                                         u.UserImage,
                                         u.UserExpireDate,
                                         u.CreateDate,
                                         u.UserCreated,
                                         u.TransDate,
                                         u.LastSignedin,
                                         u.ModifyBy,
                                         u.MenuName
                                     }).ToList();

                //var m_UserMaster = from u in _context.m_UserMaster
                //                   where u.UserRoll == "USER"
                //                   group u by new
                //                   {
                //                       u.DepID,
                //                       u.UserId,
                //                       u.UserName,
                //                       u.UserPassword,
                //                       u.FirstName,
                //                       u.LastName,
                //                       u.UserEmail,
                //                       u.ClusterCode,
                //                       u.ShiftID,
                //                       Status = u.Status,// == "A" ? "Active" : "Inactive",
                //                       u.EmployeeCode,
                //                       u.CompanyCode,
                //                       u.LocationCode,
                //                       u.UserLocationId,
                //                       u.UserImage,
                //                       u.UserExpireDate,
                //                       u.CreateDate,
                //                       u.UserCreated,
                //                       u.TransDate,
                //                       u.LastSignedin,
                //                       u.ModifyBy,
                //                   } into gcs

            //                   join p in _context.m_UserPermiss on gcs.FirstOrDefault().UserId equals Convert.ToInt32(p.UserId)
            //                   into p1
            //                   from p2 in p1.DefaultIfEmpty()
            //                   join m in _context.MenuMaster on p2.MenuIdentity equals m.MenuIdentity
            //                   into m1
            //                   from m2 in m1.DefaultIfEmpty()
            //                   //where gcs.FirstOrDefault().UserName == "fsdfsf"
            //                   select new
            //                   {
            //                       gcs.FirstOrDefault().UserId,
            //                       gcs.FirstOrDefault().UserName,
            //                       gcs.FirstOrDefault().UserPassword,
            //                       gcs.FirstOrDefault().FirstName,
            //                       gcs.FirstOrDefault().LastName,
            //                       gcs.FirstOrDefault().UserEmail,
            //                       gcs.FirstOrDefault().ClusterCode,
            //                       gcs.FirstOrDefault().DepID,
            //                       gcs.FirstOrDefault().ShiftID,
            //                       Status = gcs.FirstOrDefault().Status == "A" ? "Active" : "Inactive",
            //                       gcs.FirstOrDefault().EmployeeCode,
            //                       gcs.FirstOrDefault().CompanyCode,
            //                       gcs.FirstOrDefault().LocationCode,
            //                       gcs.FirstOrDefault().UserLocationId,
            //                       gcs.FirstOrDefault().UserImage,
            //                       UserExpireDate = Convert.ToDateTime(gcs.FirstOrDefault().UserExpireDate),
            //                       CreateDate = Convert.ToDateTime(gcs.FirstOrDefault().CreateDate),
            //                       gcs.FirstOrDefault().UserCreated,
            //                       TransDate = Convert.ToDateTime(gcs.FirstOrDefault().TransDate),
            //                       LastSignedin = Convert.ToDateTime(gcs.FirstOrDefault().LastSignedin),
            //                       gcs.FirstOrDefault().ModifyBy,
            //                       //m2.MenuName,
            //                       //count = countuser.Count()
            //                   };


            try
            {
            return Json(DataSourceLoader.Load(VW_MFC_UserMasters, loadOptions));
            }
            catch (SqlException ex)
            {
                return Json(DataSourceLoader.Load(ex.Message, loadOptions));
                
            }
        }
        [HttpGet]
        public IActionResult FormatDate()
        {
            var s_GlobalPams = _context.s_GlobalPams.Where(g => g.parm_key == "DateTimeFormat").ToList();
            return Json(s_GlobalPams);
        }

        [HttpPost]
        public IActionResult CheckRepeatedlyUsername(string UserName)
        {
            var m_UM = _context.m_UserMaster.Where(u => u.UserName == UserName).ToList();//เช็คusername ก่อน insert m_UserMaster

            return Json(m_UM);
        }

        [HttpPost]
        public IActionResult Insert(string[] getitem, string UserName, string UserPassword, string FirstName, string LastName, string UserEmail, string ShiftID, string DepID, string Status)
        {
            var identity = (ClaimsIdentity)User.Identity;
            IEnumerable<Claim> claims = identity.Claims;
            var userLogin = claims.FirstOrDefault();
            //List<m_UserMaster> result = new List<m_UserMaster>();
            var result = 0;
            var UserId = "";

            switch (Status)
            {
                default:
                    Status = "I";
                    break;

                case "Active":
                    Status = "A";
                    break;

                case "Inactive":
                    Status = "I";
                    break;
            }

            //check insert  m_UserMaster

            var m_UM = _context.m_UserMaster.Where(u => u.UserName == UserName).ToList();//เช็คusername ก่อน insert m_UserMaster
            if (m_UM.Count > 0) //username ซํ้า
            {
                result = 0;
            }
            else
            {
                var insertdep = new m_UserMaster //ถ้าไม่ซํ้าให้ insert User
                {
                    UserName = UserName,
                    UserPassword = HelperClass.EncodePassword(UserPassword, "P@ssw0rd"),
                    FirstName = FirstName,
                    LastName = LastName,
                    UserEmail = UserEmail,
                    ShiftID = Convert.ToInt32(ShiftID),
                    DepID = DepID,
                    Status = Status,
                    TransDate = Convert.ToDateTime(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")),
                    CreateDate = Convert.ToDateTime(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")),
                    LastSignedin = Convert.ToDateTime(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")),
                    ModifyBy = userLogin.Value,
                    UserRoll = "USER"
                };
                _context.m_UserMaster.Add(insertdep);
                _context.SaveChanges();
                UserId = Convert.ToString(insertdep.UserId);
                m_UM = _context.m_UserMaster.Where(u => u.UserId == Convert.ToInt32(UserId)).ToList();
                result = 1;
                //end check insert m_UserMaster

                //default insert m_UserPermiss
                var dashboard = _context.MenuMaster.SingleOrDefault(d => d.MenuID == "DashBoard" && d.Parent_MenuID == "*");
                var defaultinsertper = new m_UserPermiss //default view dashboard
                {
                    UserId = UserId,
                    MenuIdentity = dashboard.MenuIdentity,
                    TransDate = Convert.ToDateTime(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")),
                    CreateDate = Convert.ToDateTime(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")),
                    ModifyBy = userLogin.Value
                };
                _context.m_UserPermiss.Add(defaultinsertper);
                _context.SaveChanges();

                //check insert m_UserPermiss
                for (var a = 0; a < getitem.Length; a++)
                {
                    var insertper = new m_UserPermiss
                    {
                        UserId = UserId,
                        MenuIdentity = Convert.ToInt32(getitem[a].ToString()),
                        TransDate = Convert.ToDateTime(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")),
                        CreateDate = Convert.ToDateTime(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")),
                        ModifyBy = userLogin.Value
                    };
                    _context.m_UserPermiss.Add(insertper);
                    _context.SaveChanges();

                }
            }//end insert m_UserPermiss


            return Json(result);
        }


        [HttpPost]
        public IActionResult update(string[] getitem, int getuserid, string UserName, string UserPassword, string FirstName, string LastName, string UserEmail, string ShiftID, string DepID, string Status)
        {
            var identity = (ClaimsIdentity)User.Identity;
            IEnumerable<Claim> claims = identity.Claims;
            var userLogin = claims.FirstOrDefault();
            var result = 1;
            var UserId = "";
            if (DepID == null)
            {
                result = 0;
            }
            else
            {
                switch (Status)
                {
                    default:
                        Status = "I";
                        break;

                    case "Active":
                        Status = "A";
                        break;

                    case "Inactive":
                        Status = "I";
                        break;
                }
                //check  edit m_UserMaster         
                var usermaster = _context.m_UserMaster.FirstOrDefault(u => u.UserId == getuserid);
                usermaster.UserName = UserName;
                //usermaster.UserPassword = HelperClass.EncodePassword(UserPassword, "P@ssw0rd");
                usermaster.FirstName = FirstName;
                usermaster.LastName = LastName;
                usermaster.UserEmail = UserEmail;
                usermaster.ShiftID = Convert.ToInt32(ShiftID);
                usermaster.DepID = DepID;
                usermaster.Status = Status;
                usermaster.TransDate = Convert.ToDateTime(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
                usermaster.LastSignedin = Convert.ToDateTime(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
                usermaster.ModifyBy = userLogin.Value;
                _context.m_UserMaster.Update(usermaster);
                _context.SaveChanges();
                var m_UM = _context.m_UserMaster.Where(u => u.UserId == getuserid).ToList();
                //result = m_UM;
                UserId = Convert.ToString(getuserid);
                //end check edit m_UserMaster

                //check edit UserPermiss
                var permissquery = _context.m_UserPermiss.Where(m => m.UserId == UserId).ToList();

                _context.m_UserPermiss.RemoveRange(permissquery);//delete ออกไปให้หมดก่อน
                _context.SaveChanges();

                for (var i = 0; i < getitem.Length; i++) //Loop insert m_UserPermiss
                {
                    var insertper = new m_UserPermiss
                    {
                        UserId = UserId,
                        MenuIdentity = Convert.ToInt32(getitem[i].ToString()),
                        TransDate = Convert.ToDateTime(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")),
                        CreateDate = Convert.ToDateTime(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")),
                        ModifyBy = userLogin.Value
                    };
                    _context.m_UserPermiss.Add(insertper);
                    _context.SaveChanges();
                }
                result = 1;
            }


            return Json(result);
        }

        [HttpGet]
        public IActionResult MenuMasterLookup(DataSourceLoadOptions loadOptions, string UserId)
        {
            var lookup = from m in _context.MenuMaster
                         join u in _context.m_UserPermiss
                         on m.MenuIdentity equals u.MenuIdentity into per
                         from permiss in per.Where(d => d.UserId == UserId).DefaultIfEmpty()
                         where m.Parent_MenuID != "*"
                         orderby m.MenuIdentity ascending
                         select new
                         {
                             m.MenuIdentity,
                             m.MenuName,
                             m.Parent_MenuID,
                             selected = permiss.UserId == UserId ? "selected" : null
                         };
            return Json(DataSourceLoader.Load(lookup, loadOptions));
        }

        [HttpGet]
        public IActionResult ShiftLookup(DataSourceLoadOptions loadOptions)
        {
            var ShiftLookup = from s in _context.m_Shift
                              group s by new
                              {
                                  s.ShiftID,
                                  s.ShiftType
                              } into gcs

                              select new
                              {
                                  gcs.Key.ShiftID,
                                  gcs.Key.ShiftType,
                                  Shift = gcs.ToList()

                              };
            return Json(DataSourceLoader.Load(ShiftLookup, loadOptions));
        }

        [HttpGet]
        public IActionResult DepartmentLookup(DataSourceLoadOptions loadOptions)
        {
            var DepartmentLookup = from d in _context.m_Dep
                                   group d by new
                                   {
                                       d.DepID,
                                       d.DepDesc
                                   } into gcs
                                   select new
                                   {
                                       gcs.Key.DepID,
                                       gcs.Key.DepDesc,
                                       Depart = gcs.ToList()

                                   };

            return Json(DataSourceLoader.Load(DepartmentLookup, loadOptions));
        }

        [HttpGet]
        public IActionResult deleterowuser(string UserId)
        {
            int result = 1;

            if (UserId != null)
            {
                var removedep = _context.m_UserMaster.FirstOrDefault(item => item.UserId.ToString() == UserId);
                _context.m_UserMaster.Remove(removedep);
                _context.SaveChanges();

                var removemenudep = _context.m_UserPermiss.Where(item => item.UserId == UserId);
                _context.m_UserPermiss.RemoveRange(removemenudep);
                _context.SaveChanges();
            }
            else
            {
                result = 0;
            }

            return Json(result);
        }

        [HttpGet]
        public IActionResult CloneDepID(DataSourceLoadOptions loadOptions, string DepID)
        {

            var depMenus = from i in _context.MenuMaster
                           join d in _context.m_DepMenu
                           on i.MenuIdentity equals d.MenuIdentity into dep
                           from de in dep.Where(d => d.DepID == DepID).DefaultIfEmpty()//left join
                           where i.Parent_MenuID != "*"
                           orderby i.MenuIdentity ascending
                           select new
                           {
                               i.MenuIdentity,
                               i.MenuName,
                               i.Parent_MenuID,
                               selected = de.DepID == DepID ? "selected" : null,
                               isnulldepid = DepID
                           };
            return Json(DataSourceLoader.Load(depMenus, loadOptions));
        }

    }
}