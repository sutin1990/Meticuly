using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using MCP_WEB.Data;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace MCP_WEB.Controllers.WebAPI
{
    [Produces("application/json")]
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class UploadImgController : ControllerBase
    {
        private NittanDBcontext Db;
        private readonly IHostingEnvironment _appEnvironment;

        public UploadImgController(NittanDBcontext db, IHostingEnvironment appEnvironment)
        {
            this.Db = db;
            this._appEnvironment = appEnvironment;
        }
        [HttpPost]
        public async Task<IActionResult> UploadFile(List<IFormFile> myFile, string key)
        {
            string webRootPath = _appEnvironment.WebRootPath;
            string contentRootPath = _appEnvironment.ContentRootPath;

            //var path = AppDomain.CurrentDomain.BaseDirectory + "wwwroot\\uploads\\img\\" + key;

            foreach (var formFile in myFile)
            {
                if (formFile.Length > 0)
                {
                    
                    using (var fileStream = new FileStream(Path.Combine(webRootPath, key), FileMode.Create))
                    {
                        await formFile.CopyToAsync(fileStream);
                    }

                }
            }

            return Ok("File uploaded successfully");

        }
    }
}