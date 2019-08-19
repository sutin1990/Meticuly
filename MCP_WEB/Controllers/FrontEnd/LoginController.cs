using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using MCP_WEB.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using MCP_WEB.Models;
using MCP_WEB.Helper;
using Microsoft.AspNetCore.Http;
using MCP_WEB.Service;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Authentication.Cookies;

namespace MCP_WEB.Controllers.FrontEnd
{
    public class LoginController : Controller
    {
        public readonly NittanDBcontext _context;
        public HttpContext _session;
        
        public LoginController(NittanDBcontext context)
        {
            _context = context;
        }
        public IActionResult Index()
        {
            //foreach (var cookieKey in Request.Cookies.Keys)
            //{
            //    //Response.Cookies.Delete(cookieKey);
            //}

            var identity = (ClaimsIdentity)User.Identity;

            string lastChanged;
            lastChanged = (from c in identity.Claims
                           where c.Type == "ContactName"
                           select c.Value).FirstOrDefault();

            int a = Request.Cookies.Keys.Count();
            //if (a <= 1)
            if (string.IsNullOrEmpty(lastChanged))
            {
                return View();
            }
            else
            {
                return RedirectToAction("Index", "MainPage");
            }

        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Index(LoginViewModel model, string returnUrl = null)
        {

            ViewData["ReturnUrl"] = returnUrl;
            try
            {
                string sessionID = HttpContext.Session.Id.ToString();
                if (ModelState.IsValid)
                {
                    ModelState.Remove("FirstName");
                    ModelState.Remove("LastName");

                    using (var context = _context)
                    {
                        // Query for all blogs with names starting with B 
                        var UserCheck = context.m_UserMaster.Where(b => b.UserName == model.Username && b.UserPassword == HelperClass.EncodePassword(model.Password, "P@ssw0rd") && b.Status != "I").Count();
                        if (UserCheck == 0)
                        {

                            ViewBag.UserLoginFailed = "Login Failed.Please enter correct credentials";
                            return View(model);
                        }
                        else
                        {

                            var identity = (ClaimsIdentity)User.Identity;

                            string lastChanged;
                            lastChanged = (from c in identity.Claims
                                           where c.Type == "ContactName"
                                           select c.Value).FirstOrDefault();

                            if (_context.UserTransaction.Any(x => x.UserName == model.Username)) //มี user ในtable UserTransaction
                            {
                                // var model1 = _context.UserTransaction.Where(x => SqlServerDbFunctionsExtensions
                                //.DateDiffMinute(dfunc, Convert.ToDateTime(x.DateExprie), Convert.ToDateTime(DateTime.Now)) <= 10 && x.UserName == model.Username).FirstOrDefault();
                                var model1 = _context.UserTransaction.Where(x => x.UserName == model.Username).FirstOrDefault();

                                double Minute = (DateTime.Now - model1.DateExprie).Days;//login นานเท่าไหร่
                                if(Minute <= 1)//เวลายังไม่หมด
                                {
                                    //if (string.IsNullOrEmpty(lastChanged))//เข้ามาในระบบ อีก browser หรือ อีกเครื่อง แต่มี login ค้างอยู่อีก browser
                                    //{
                                    //ViewBag.UserLoginFailed = "time:" + Minute.ToString();
                                    //    return View();
                                    //}
                                    //else
                                    //{
                                    //    return RedirectToAction("Index", "MainPage");
                                    //}

                                    ClaimsPrincipal user = new ClaimsPrincipal(new ClaimsIdentity(new[]
                                   {new Claim("ContactName", model.Username ?? "")}, CookieAuthenticationDefaults.AuthenticationScheme));

                                    await HttpContext.SignInAsync(
                                    CookieAuthenticationDefaults.AuthenticationScheme,
                                    new ClaimsPrincipal(user),
                                    new AuthenticationProperties
                                    {
                                        //CookiePath = new PathString("/Login/Logout"),
                                        IsPersistent = true,
                                        ExpiresUtc = DateTime.UtcNow.AddDays(1)
                                    });

                                    var model_1 = _context.UserTransaction.FirstOrDefault(x => x.UserName == model.Username);
                                    model1.DateExprie = DateTime.Now;
                                    _context.SaveChanges();
                                    return RedirectToAction("Index", "MainPage");
                                }
                                else
                                {
                                    ClaimsPrincipal user = new ClaimsPrincipal(new ClaimsIdentity(new[]
                                   {new Claim("ContactName", model.Username ?? "")}, CookieAuthenticationDefaults.AuthenticationScheme));

                                    await HttpContext.SignInAsync(
                                    CookieAuthenticationDefaults.AuthenticationScheme,
                                    new ClaimsPrincipal(user),
                                    new AuthenticationProperties
                                    {
                                    //CookiePath = new PathString("/Login/Logout"),
                                    IsPersistent = true,
                                        ExpiresUtc = DateTime.UtcNow.AddDays(1)
                                    });

                                    var model_2 = _context.UserTransaction.FirstOrDefault(x => x.UserName == model.Username);
                                    _context.UserTransaction.Remove(model_2);
                                    _context.SaveChanges();

                                    DeleteDataRefresh(model.Username);

                                    TempData["UserName"] = model.Username;
                                    var ToKen1 = Guid.NewGuid();

                                    var Model_Tran1 = new UserTransaction
                                    {
                                        UserName = model.Username,
                                        Token = ToKen1.ToString(),
                                        SessionKey = "",
                                        DateExprie = DateTime.Now
                                    };
                                    _context.UserTransaction.AddRange(Model_Tran1);
                                    _context.SaveChanges();
                                    return RedirectToAction("Index", "MainPage");
                                }                                

                            }
                            else // ไม่มี user ในtable UserTransaction
                            {

                                var ToKen = Guid.NewGuid();

                                TempData["UserName"] = model.Username;

                                var Model_Tran = new UserTransaction
                                {
                                    UserName = model.Username,
                                    Token = ToKen.ToString(),
                                    SessionKey = "",
                                    DateExprie = DateTime.Now
                                };
                                _context.UserTransaction.AddRange(Model_Tran);
                                _context.SaveChanges();


                                ClaimsPrincipal user = new ClaimsPrincipal(new ClaimsIdentity(new[]
                                {new Claim("ContactName", model.Username ?? "")}, CookieAuthenticationDefaults.AuthenticationScheme));

                                await HttpContext.SignInAsync(
                                CookieAuthenticationDefaults.AuthenticationScheme,
                                new ClaimsPrincipal(user),
                                new AuthenticationProperties
                                {
                                        //CookiePath = new PathString("/Login/Logout"),
                                    IsPersistent = true,
                                    ExpiresUtc = DateTime.UtcNow.AddDays(1)
                                });
                                //var identity1 = (ClaimsIdentity)User.Identity;
                                //string lastChanged1;
                                //lastChanged1 = (from c in identity1.Claims
                                //                where c.Type == "ContactName"
                                //                select c.Value).FirstOrDefault();

                                return RedirectToAction("Index", "MainPage");

                            }                            

                            
                        }
                    }

                }
                else
                {
                    return View(model);
                }
            }
            catch (DbUpdateConcurrencyException)
            {
                return View(model);
            }

        }


        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> Logout()
        {
            //var identity = (ClaimsIdentity)User.Identity;
            //IEnumerable<Claim> claims = identity.Claims;
            //var user = claims.FirstOrDefault();

            var user = HttpContext.User;
            if (user?.Identity.IsAuthenticated == true)
            {
                var userName = user.Claims.FirstOrDefault().Value;
                var model = _context.UserTransaction.FirstOrDefault(x => x.UserName == userName);
                if (model != null)
                {
                    DeleteDataRefresh(userName);
                    _context.UserTransaction.Remove(model);
                    _context.SaveChanges();

                }
                // delete local authentication cookie
                await HttpContext.SignOutAsync();

            }

            return RedirectToAction("Index", "Login");
        }

        public void DeleteDataRefresh(string User)
        {
            //get Token

            var UserToken = _context.UserTransaction.FirstOrDefault(x => x.UserName == User);

            if (UserToken != null)
            {
                var model = _context.isonite_temp.Where(item => item.user_create == User && item.Token == UserToken.Token);

                var sumTemp = _context.isonite_temp.Where(x => x.user_create == User).GroupBy(o => o.BarCode).Select(x => new
                {
                    Barcode = x.Key,
                    SumQty = x.Sum(i => i.QtyComplete + i.QtyNG)

                }).ToList();

                if (sumTemp != null)
                {
                    foreach (var row in sumTemp)
                    {
                        var woRouting = _context.WoRouting.FirstOrDefault(item => item.BarCode == row.Barcode && item.ProcessCode == "ISONITE");
                        woRouting.QTYinProcess = woRouting.QTYinProcess - row.SumQty;

                        _context.SaveChanges();
                    }
                }

                if (model != null)
                {
                    _context.isonite_temp.RemoveRange(model);
                    _context.SaveChanges();
                }
                else
                {
                    var model_1 = _context.isonite_temp.Where(item => item.user_create == User);
                    _context.isonite_temp.RemoveRange(model_1);
                    _context.SaveChanges();
                }

            }


        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult Register(string returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterViewModel model, string returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;
            try
            {
                if (ModelState.IsValid)
                {
                    using (var context = _context)
                    {
                        // Query for all blogs with names starting with B 
                        var UserCheck = context.m_UserMaster
                                    .Where(b => b.UserName == model.UserName)
                                    .Count();
                        if (UserCheck > 0)
                        {
                            return View(model);
                        }
                        else
                        {
                            var userModel = new m_UserMaster { UserName = model.UserName, UserPassword = HelperClass.EncodePassword(model.Password, "P@ssw0rd"), FirstName = model.FirstName, LastName = model.LastName, ClusterCode = "MCP", UserEmail = model.Email };
                            context.m_UserMaster.AddRange(userModel);
                            context.SaveChanges();

                            //var claims = new List<Claim> { new Claim("ContactName", model.UserName ?? "") };
                            //ClaimsIdentity userIdentity = new ClaimsIdentity(claims, "login");
                            //ClaimsPrincipal principal = new ClaimsPrincipal(userIdentity);

                            ClaimsPrincipal user = new ClaimsPrincipal(new ClaimsIdentity(new[]
                          { new Claim("ContactName", model.UserName ?? "")}, CookieAuthenticationDefaults.AuthenticationScheme));
                            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, user);

                            // await HttpContext.SignInAsync(principal);

                            return RedirectToAction("Index", "MainPage");
                        }

                    }

                }
                else

                {
                    return View(model);
                }

            }
            catch (DbUpdateConcurrencyException)
            {
                return View(model);

            }

        }


    }
}