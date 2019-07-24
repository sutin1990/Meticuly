using MCP_WEB.Data;
using MCP_WEB.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.EntityFrameworkCore.Internal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;

namespace MCP_WEB.Service
{
    [AttributeUsage(AttributeTargets.Method, Inherited = true, AllowMultiple = false)]
    public class SessionTimeoutAttribute : ActionFilterAttribute
    {
        NittanDBcontext _context;
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            var user = (((Microsoft.AspNetCore.Http.DefaultHttpContext)filterContext.HttpContext).User.Claims).FirstOrDefault();


            string s = user.Value;

            ////DbFunctions dfunc = null;
            //if (_context.UserTransaction.Any(x => x.UserName == user.Value))
            //{
            //    //         var model1 = _context.UserTransaction.Where(x => SqlServerDbFunctionsExtensions
            //    //.DateDiffMinute(dfunc, Convert.ToDateTime(x.DateExprie), Convert.ToDateTime(DateTime.Now)) <= 10 && x.UserName == model.Username).FirstOrDefault();
            //    var model1 = _context.UserTransaction.Where(x => x.UserName == user.Value).FirstOrDefault();

            //    int Minute = (DateTime.Now - model1.DateExprie).Minutes;

            //    if (Minute <= 10)
            //    {
            //        //return RedirectToAction("Index", "Login");
            //    }
            //    else
            //    {

            //        var model_1 = _context.UserTransaction.FirstOrDefault(x => x.UserName == user.Value);
            //        _context.UserTransaction.Remove(model_1);
            //        _context.SaveChanges();

            //        DeleteDataRefresh(user.Value);

            //        var ToKen = Guid.NewGuid();

            //        var Model_Tran = new UserTransaction
            //        {
            //            UserName = user.Value,
            //            Token = ToKen.ToString(),
            //            SessionKey = "",
            //            DateExprie = DateTime.Now
            //        };
            //        _context.UserTransaction.AddRange(Model_Tran);
            //        _context.SaveChanges();
            //    }
            //}
            //else
            //{
            //    var ToKen = Guid.NewGuid();

            //    var Model_Tran = new UserTransaction
            //    {
            //        UserName = user.Value,
            //        Token = ToKen.ToString(),
            //        SessionKey = "",
            //        DateExprie = DateTime.Now
            //    };
            //    _context.UserTransaction.AddRange(Model_Tran);
            //    _context.SaveChanges();

            //}
            //var user = filterContext.HttpContext.Session.Get("Username");
            //if (user == null) { }
            //filterContext.Result = new RedirectResult(string.Format("/User/Login?targetUrl={0}", filterContext.HttpContext.Request.Url.AbsolutePath));

        }
        public void DeleteDataRefresh(string User)
        {
            //get Token

            var UserToken = _context.UserTransaction.FirstOrDefault(x => x.UserName == User);

            if (UserToken != null)
            {
                var model = _context.isonite_temp.FirstOrDefault(item => item.user_create == User && item.Token == UserToken.Token);

                var sumTemp = _context.isonite_temp.Where(x => x.user_create == User).GroupBy(o => o.BarCode).Select(x => new
                {
                    Barcode = x.Key,
                    SumQty = x.Sum(i => i.QtyComplete + i.QtyNG)

                }).FirstOrDefault();

                if (sumTemp != null)
                {
                    var woRouting = _context.WoRouting.FirstOrDefault(item => item.BarCode == sumTemp.Barcode);
                    woRouting.QTYinProcess = woRouting.QTYinProcess - sumTemp.SumQty;

                    _context.SaveChanges();
                }
                if (model != null)
                {
                    _context.isonite_temp.Remove(model);
                    _context.SaveChanges();
                }
            }

        }
    }

}
