using MCP_WEB.Data;
using MCP_WEB.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MCP_WEB.Service
{
    public static class LastChangedValidator
    {
        public static async Task ValidateAsync(CookieValidatePrincipalContext context)
        {
            // Pull database from registered DI services.
            var userRepository = context.HttpContext.RequestServices.GetRequiredService<IUserRepository>();
            var userPrincipal = context.Principal;

            var _context = context.HttpContext.RequestServices.GetRequiredService<NittanDBcontext>();

            // Look for the last changed claim.
            string lastChanged;
            lastChanged = (from c in userPrincipal.Claims
                           where c.Type == "ContactName"
                           select c.Value).FirstOrDefault();


            if (string.IsNullOrEmpty(lastChanged))
            {
                context.RejectPrincipal();

                await context.HttpContext.SignOutAsync(
              CookieAuthenticationDefaults.AuthenticationScheme);

            }
            else
            {
                if (_context.UserTransaction.Any(x => x.UserName == lastChanged))
                {

                    var model1 = _context.UserTransaction.Where(x => x.UserName == lastChanged).FirstOrDefault();

                    int Minute = (DateTime.Now - model1.DateExprie).Minutes;

                    if (Minute <= 10)
                    {
                        var model_1 = _context.UserTransaction.FirstOrDefault(x => x.UserName == lastChanged);
                        model1.DateExprie = DateTime.Now;
                        _context.SaveChanges();
                    }
                    else
                    {

                        var model_1 = _context.UserTransaction.FirstOrDefault(x => x.UserName == lastChanged);
                        _context.UserTransaction.Remove(model_1);
                        _context.SaveChanges();

                        DeleteDataRefresh(lastChanged, _context);


                        var ToKen = Guid.NewGuid();

                        var Model_Tran = new UserTransaction
                        {
                            UserName = lastChanged,
                            Token = ToKen.ToString(),
                            SessionKey = "",
                            DateExprie = DateTime.Now
                        };
                        _context.UserTransaction.AddRange(Model_Tran);
                        _context.SaveChanges();
                    }
                }
                else
                {

                    context.RejectPrincipal();

                    await context.HttpContext.SignOutAsync(
                  CookieAuthenticationDefaults.AuthenticationScheme);

                }

            }
        }

        public static void DeleteDataRefresh(string User, NittanDBcontext _context)
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

                var isonite_line_temp = _context.Isonite_Line_Temp.Where(x => x.ModifyBy == User);

                if (isonite_line_temp != null)
                {
                    _context.Isonite_Line_Temp.RemoveRange(isonite_line_temp);
                    _context.SaveChanges();
                }


                if (sumTemp != null)
                {
                    var woRouting = _context.WoRouting.FirstOrDefault(item => item.BarCode == sumTemp.Barcode && item.ProcessCode == "ISONITE");
                    woRouting.QTYinProcess = woRouting.QTYinProcess - sumTemp.SumQty;

                    _context.SaveChanges();
                }
                if (model != null)
                {
                    _context.isonite_temp.Remove(model);
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
    }
}
