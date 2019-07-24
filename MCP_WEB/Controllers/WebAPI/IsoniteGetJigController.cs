using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DevExtreme.AspNet.Data;
using DevExtreme.AspNet.Mvc;
using MCP_WEB.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace MCP_WEB.Controllers.WebAPI
{
    [Produces("application/json")]
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class IsoniteGetJigController : Controller
    {
        private readonly NittanDBcontext Db;

        public IsoniteGetJigController(NittanDBcontext db)
        {
            this.Db = db;
        }

        [HttpGet]
        public IActionResult GetJig(DataSourceLoadOptions loadOptions)
        {
            var m_jig = Db.m_Jig.Where(x => x.jig_isonite_status == "unUse").Select(i => new
            {
                i.JigID,
                i.JigNo
            });
            return Json(DataSourceLoader.Load(m_jig, loadOptions));
            // return new JsonResult(m_jig);
        }

        [HttpGet]
        public IActionResult GetJigSize(string options)
        {
            if (options != null)
            {
                var m_jig = Db.m_Jig.Where(n => n.JigNo == options).Select(i => new
                {
                    i.Column,
                    i.Row
                }).FirstOrDefault();

                return new JsonResult(new { column = m_jig.Column, row = m_jig.Row });
            }
            else
            {
                return new JsonResult("");
            }

        }

        [HttpGet]
        public IActionResult GetJigID(string options)
        {

            var m_jig = Db.m_Jig.Where(n => n.JigNo == options).Select(i => new
            {
                i.JigID
            }).FirstOrDefault();

            return new JsonResult(new { ID = m_jig.JigID });
        }

        [HttpGet]
        public JsonResult GetSumJig(string oldJig, string newJig)
        {
            if (oldJig != null && newJig != null)
            {

                var SizeOld = Db.m_Jig.Where(x => x.JigNo == oldJig).Select(i => new { size = i.Column * i.Row }).FirstOrDefault();

                var SizeNew = Db.m_Jig.Where(x => x.JigNo == newJig).Select(i => new { size = i.Column * i.Row }).FirstOrDefault();

                if (SizeOld != null && SizeNew != null)
                {
                    if (SizeOld.size != SizeNew.size)
                    {
                        return new JsonResult("ErrorJigSize");
                    }
                }


            }
            if (newJig != "0")
            {
                var checkJig = Db.m_Jig.Any(x => x.JigNo == newJig && x.jig_isonite_status == "Use");

                if (checkJig == true)
                {

                    return new JsonResult("JigActive");
                }

                var m_jig = Db.m_Jig.Where(n => n.JigNo == newJig).Select(i => new
                {
                    i.Column,
                    i.Row
                }).FirstOrDefault();

                return Json(new { Msg = "Ok", column = m_jig.Column, row = m_jig.Row });
            }
            else
            {
                return new JsonResult("Null");
            }

        }
    }
}