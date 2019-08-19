using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MCP_WEB.Data;
using MCP_WEB.Helper;
using MCP_WEB.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;

namespace MCP_WEB.Controllers.FrontEnd
{
    [Authorize]
    public class ResetPasswordController : Controller
    {
        private readonly NittanDBcontext context;

        public ResetPasswordController(NittanDBcontext context)
        {
            this.context = context;
        }

        public IActionResult Index(string returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;
            return View();
        }


        [HttpPost]
        public IActionResult Reset([FromBody] ResetPassword data)
        {
            var UserCheck = context.m_UserMaster.Where(b => b.UserName == data.user && b.UserPassword == HelperClass.EncodePassword(data.CurrentPassword, "P@ssw0rd") && b.Status != "I").ToList();

            if (UserCheck.Count == 1)
            {
                var model = context.m_UserMaster.Where(b => b.UserName == data.user && b.UserPassword == HelperClass.EncodePassword(data.CurrentPassword, "P@ssw0rd") && b.Status != "I").FirstOrDefault();

                model.UserPassword = HelperClass.EncodePassword(data.NewPassword, "P@ssw0rd");

                context.SaveChanges();

                return RedirectToAction("Index", "MainPage");
            }
            else
            {

                return new JsonResult(new { Msg = "Error", des = "Current Password do not match!" });
            }


        }
    }
}