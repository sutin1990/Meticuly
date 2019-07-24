using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using MCP_WEB.Helper;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace MCP_WEB.Controllers.FrontEnd
{
    public class UploadController : Controller
    {
        public IHostingEnvironment _appEnvironment { get; }

        public UploadController(IHostingEnvironment appEnvironment)
        {
            this._appEnvironment = appEnvironment;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost("UploadFiles")]
        public async Task<IActionResult> Post(List<IFormFile> files, string key)
        {
            long size = files.Sum(f => f.Length);


            // full path to file in temp location
            var filePath = Path.GetTempFileName();
            string webRootPath = _appEnvironment.WebRootPath;

            foreach (var formFile in files)
            {
                string extension = Path.GetExtension(formFile.FileName);

                if (formFile.Length > 0)
                {
                    using (var stream = new FileStream(Path.Combine(webRootPath + "/uploads/img", key + extension), FileMode.Create))
                    {
                        await formFile.CopyToAsync(stream);
                    }
                }
            }

            // process uploaded files
            // Don't rely on or trust the FileName property without validation.

            return Json(new { status = "success" });
        }
    }
}