using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MCP_WEB.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MCP_WEB.Models;
using System.Security.Claims;
using Microsoft.AspNetCore.Routing;
using System.Net;

namespace MCP_WEB.Controllers.WebAPI
{
    [Produces("application/json")]
    [Route("api/[controller]/[action]")]
    //[ApiController]
    public class ProductionDailyReport1_PrintController : Controller
    {
        private NittanDBcontext _context;
        public ProductionDailyReport1_PrintController(NittanDBcontext context)
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

        [HttpPost]
        public IActionResult PrintProductionDailyReport1(string RowNumber, string fromdate,string todate)
        {
            IPHostEntry heserver = Dns.GetHostEntry(Dns.GetHostName());
            var ip = heserver.AddressList[1].ToString();

            var Log_Select_Print = _context.Log_Select_Print.Where(w => w.opt == "PrintProduction1");

            var flag = false;
            if (RowNumber != null)
            { 
                flag = true;
                if (Log_Select_Print.Count() > 0)
                {
                    var oldipaddress = Log_Select_Print.Where(w => w.opt == "PrintProduction1" && w.ipaddress == ip);
                    if (oldipaddress.Count() > 0)
                    {
                        var pp_update = _context.Log_Select_Print.FirstOrDefault(f => f.opt == "PrintProduction1" && f.ipaddress == ip);
                        pp_update.name = RowNumber;
                        pp_update.ipaddress = ip;
                        _context.Log_Select_Print.Update(pp_update);
                    }
                    else
                    {
                        var pp_insert = new Log_Select_Print
                        {
                            opt = "PrintProduction1",
                            name = RowNumber,
                            ipaddress = ip
                        };
                        _context.Log_Select_Print.Add(pp_insert);

                    }

                    _context.SaveChanges();
                }
                else
                {
                    var pp_insert = new Log_Select_Print
                    {
                        opt = "PrintFGMovement",
                        name = RowNumber,
                        ipaddress = ip
                    };
                    _context.Log_Select_Print.Add(pp_insert);
                    _context.SaveChanges();
                }
            }
            //return View(pdr);
            return Json(flag);
            //return RedirectToAction("Index", "PrintProduction1");
            //return Content("window.open('PrintProduction1/Index')");

        }
    }
}