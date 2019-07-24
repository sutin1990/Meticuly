using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MCP_WEB.Data;
using MCP_WEB.Models;
using Microsoft.AspNetCore.Mvc;

namespace MCP_WEB.Controllers.FrontEnd
{
    public class UserMasterTestprintController : Controller
    {
        private NittanDBcontext _context;
        public UserMasterTestprintController(NittanDBcontext context)
        {
            _context = context;
        }
        public IActionResult Index()
        {
            //ViewData["m_user"] = (from u in _context.m_UserMaster 
            //                     where u.UserRoll == "USER"
            //                      select u).FirstOrDefault();
            List<m_UserMaster> user = _context.m_UserMaster.Where(u=>u.UserRoll=="USER").ToList();
            //List<m_UserMaster> m_usermaster = new List<m_UserMaster>();
           
            //m_usermaster.Add(new m_UserMaster() { UserId = user.UserId });
            //m_usermaster.Add(new m_UserMaster() { UserName = user.UserName });
            //m_usermaster.Add(new m_UserMaster() { FirstName = user.FirstName });

            ViewBag.user = user;
            //ViewBag.user = "user";
            return View();
        }
    }
}