using System;
using System.Collections.Generic;
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
using System.Dynamic;
using ZXing.QrCode;
using MCP_WEB.Helper;
using System.Drawing;
using System.Security.Claims;

namespace MCP_WEB.Controllers.WebAPI
{
    //[Route("api/[controller]/[action]")]
    //[ApiController]
    public class PrintDeliveryNoteController : Controller
    {
        private NittanDBcontext _context;
        public PrintDeliveryNoteController(NittanDBcontext context)
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
        public IActionResult PrintDN(string deliverynote)
        {
            string[] code_dn = deliverynote.Split(",");
            
            var print = from dn in _context.VW_MFC_Deliverynote_Select
                        select new VW_MFC_Deliverynote_Select
                        {
                            DeliveryNote = dn.DeliveryNote,
                            StatusDelivery = dn.StatusDelivery,
                            MoveTicket = dn.MoveTicket,
                            StatusMT = dn.StatusMT,
                            FCode = dn.FCode,
                            Model = dn.Model,
                            MTQty = dn.MTQty,
                            BoxQty = dn.BoxQty,
                            ExcessQty = dn.ExcessQty,
                            Remarks = dn.Remarks
                        };
            ViewBag.codedn = code_dn;
            return View(print);
        }
    }
}