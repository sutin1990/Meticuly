using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MCP_WEB.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MCP_WEB.Models;
using System.Security.Claims;

namespace MCP_WEB.Controllers.WebAPI
{

    //[Produces("application/json")]
    [Route("api/[controller]/[action]")]
    //[ApiController]
    public class IsoniteController : Controller
    {
        private NittanDBcontext _context;
        public IsoniteController(NittanDBcontext context)
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

        //[HttpGet]
        public IActionResult PrintIsonite(string isonitecode,string TransType)
        {
            string[] ison;
            if (isonitecode!=null)
            {
                ison = isonitecode.Split(",");
            }
            else//กรณีไม่ระบุ isonite มา ใน url
            {
                //var IsoniteCode = from i in _context.Isonite select new { i.IsoniteCode };
                var IsoniteCode = _context.Isonite.Select(s => s.IsoniteCode).ToArray();
                ison = IsoniteCode;
            }
            List<IsoniteViewModel> isonite = new List<IsoniteViewModel>();
             
            //if (isonitecode.Length==0)
            //{
            //    ison = new string[] { };
            //}
            List<string> code  =new List<string>() ;

            var Isonite_head = from i in _context.Isonite // header
                                   join b1 in _context.m_BPMaster on i.BPCodeFrom equals b1.BPCode 
                                   join b2 in _context.m_BPMaster on i.BPCodeTO equals b2.BPCode
                                   group new { i, b1, b2 } by new
                                   {
                                       i.IsoniteCode,
                                       i.BPCodeFrom,
                                       i.BPCodeTO,
                                       b1.BPName,
                                       b1.BPAddress1,
                                       b1.BPAddress2,
                                       b1.BPAddress6,
                                       b2BPName = b2.BPName,
                                       b2BPAddress1 = b2.BPAddress1,
                                       b2BPAddress2 = b2.BPAddress2,

                                   } into gcs                                   

                                   select new IsoniteViewModel
                                   {
                                       namefrom = gcs.Key.BPName,
                                       addrfrom = gcs.Key.BPAddress1 + gcs.Key.BPAddress2,
                                       nameto = gcs.Key.b2BPName,
                                       addrto = gcs.Key.b2BPAddress1 + gcs.Key.b2BPAddress2,
                                       IsoniteCode = gcs.Key.IsoniteCode,
                                       addr6 = gcs.Key.BPAddress6
                                   };

                    ViewBag.head = Isonite_head.ToList();
            if(TransType == null)
            {
                var Isonite = from i in _context.Isonite // detial
                              join l in _context.VW_MFC_Isonite_Line on i.IsoniteCode equals l.IsoniteCode into t
                              from nt in t.DefaultIfEmpty()
                              join we in _context.WeeklyPlan on nt.BarCode equals we.BarCode
                              join wo in _context.WoRouting on we.BarCode equals wo.BarCode into we_wo
                              from wo in we_wo.Where(w => w.ProcessCode == "ISONITE").DefaultIfEmpty()
                              join b1 in _context.m_BPMaster on i.BPCodeFrom equals b1.BPCode
                              join b2 in _context.m_BPMaster on i.BPCodeTO equals b2.BPCode
                              join j in _context.m_Jig on nt.JigNo equals j.JigNo
                              //where l.TransType == TransType
                              select new IsoniteViewModel
                              {
                                  IsoniteVM = i,
                                  Isonite_LineVM = nt,
                                  WeeklyPlanVM = we,
                                  WoRoutingVM = wo,
                                  m_BPMaster1VM = b1,
                                  m_BPMaster2VM = b2,
                                  m_JigVM = j
                              };
                isonite = Isonite.ToList();

                for (var i = 0; i < ison.Length; i++)
                {
                    var check = isonite.Where(w => w.IsoniteVM.IsoniteCode == ison[i].ToString()).ToList();
                    if (check.Count() > 0)
                    {
                        code.Add(ison[i]);
                    }

                }
                ViewBag.code = code;

                //var jig_no = from l in _context.VW_MFC_Isonite_Line //order by jig
                //             join we in _context.WeeklyPlan on l.BarCode equals we.BarCode
                //             join wo in _context.WoRouting on we.BarCode equals wo.BarCode into we_wo
                //             from wo in we_wo.Where(w => w.ProcessCode == "ISONITE").DefaultIfEmpty()
                //             group new { l } by new { l.JigNo, l.IsoniteCode} into gcs
                //             //where gcs.Key.TransType == TransType 
                //             select new IsoniteViewModel
                //             {
                //                 jigno = gcs.Key.JigNo,
                //                 jigIsoniteCode = gcs.Key.IsoniteCode

                //             };

                var jig_no = from j in _context.VW_MFC_JigonIsonite
                             select new IsoniteViewModel { jigno = j.JigNo, jigIsoniteCode = j.IsoniteCode };
                //var jig_no = _context.VW_MFC_JigonIsonite;

                //var jig_no = _context.Isonite_Line.Where(j => j.IsoniteCode==isonitecode).GroupBy(g=>new { g.JigNo});

                //ViewBag.jigno = jig_no.Select(s => s.jigno).ToList();
                ViewBag.jigno = jig_no.ToList();
                //ViewBag.isonitecode = ison[code];
            }
            else
            {
                var Isonite = from i in _context.Isonite // detial
                              join l in _context.VW_MFC_Isonite_Line on i.IsoniteCode equals l.IsoniteCode
                              join we in _context.WeeklyPlan on l.BarCode equals we.BarCode
                              join wo in _context.WoRouting on we.BarCode equals wo.BarCode into we_wo
                              from wo in we_wo.Where(w => w.ProcessCode == "ISONITE").DefaultIfEmpty()
                              join b1 in _context.m_BPMaster on i.BPCodeFrom equals b1.BPCode
                              join b2 in _context.m_BPMaster on i.BPCodeTO equals b2.BPCode
                              join j in _context.m_Jig on l.JigNo equals j.JigNo
                              where l.TransType == TransType
                              select new IsoniteViewModel
                              {
                                  IsoniteVM = i,
                                  Isonite_LineVM = l,
                                  WeeklyPlanVM = we,
                                  WoRoutingVM = wo,
                                  m_BPMaster1VM = b1,
                                  m_BPMaster2VM = b2,
                                  m_JigVM = j
                              };
                isonite = Isonite.ToList();

                for (var i = 0; i < ison.Length; i++)
                {
                    var check = isonite.Where(w => w.IsoniteVM.IsoniteCode == ison[i].ToString()).ToList();
                    if (check.Count() > 0)
                    {
                        code.Add(ison[i]);
                    }

                }
                ViewBag.code = code;

                //var jig_no = from l in _context.VW_MFC_Isonite_Line //order by jig
                //             join we in _context.WeeklyPlan on l.BarCode equals we.BarCode
                //             join wo in _context.WoRouting on we.BarCode equals wo.BarCode into we_wo
                //             from wo in we_wo.Where(w => w.ProcessCode == "ISONITE").DefaultIfEmpty()
                //             group new { l } by new { l.JigNo, l.IsoniteCode, l.TransType } into gcs
                //             where gcs.Key.TransType == TransType
                //             select new IsoniteViewModel
                //             {
                //                 jigno = gcs.Key.JigNo,
                //                 jigIsoniteCode = gcs.Key.IsoniteCode

                //             };
                var jig_no = from j in _context.VW_MFC_JigonIsonite
                             select new IsoniteViewModel { jigno = j.JigNo, jigIsoniteCode = j.IsoniteCode };

                //var jig_no = _context.Isonite_Line.Where(j => j.IsoniteCode==isonitecode).GroupBy(g=>new { g.JigNo});

                //ViewBag.jigno = jig_no.Select(s => s.jigno).ToList();
                ViewBag.jigno = jig_no.ToList();
                //ViewBag.isonitecode = ison[code];
            }


            return View(isonite);
        }
    }
}