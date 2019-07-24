using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using MCP_WEB.Data;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging.AzureAppServices.Internal;

namespace MCP_WEB.Controllers.FrontEnd
{
    public class ImageUploadController : Controller
    {
        private NittanDBcontext Db;
        private readonly IHostingEnvironment _appEnvironment;

        public ImageUploadController(NittanDBcontext db, IHostingEnvironment appEnvironment)
        {
            this.Db = db;
            this._appEnvironment = appEnvironment;
        }
        public IActionResult Index()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> UploadFile(List<IFormFile> myFile, string key)
        {
            string webRootPath = _appEnvironment.WebRootPath;
            string contentRootPath = _appEnvironment.ContentRootPath;

            //var filePath = Path.GetTempFileName();

            try
            {
                //var path = AppDomain.CurrentDomain.BaseDirectory + "wwwroot\\uploads\\img\\" + key;

                foreach (var formFile in myFile)
                {
                    if (formFile.Length > 0)
                    {

                        var uploads = Path.Combine(webRootPath, "uploads");

                        using (var fileStream = new FileStream(Path.Combine(webRootPath, key), FileMode.Create))
                        {
                            await formFile.CopyToAsync(fileStream);
                        }

                    }
                }

            }
            catch
            {
                Response.StatusCode = 400;
            }



            //foreach (var formFile in myFile)
            //{
            //    if (formFile.Length > 0)
            //    {
            //        var file = formFile;
            //        //There is an error here
            //        var uploads = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "wwwroot\\uploads\\img");

            //        RollFiles(uploads, key);

            //        if (file.Length > 0)
            //        {
            //            //var fileName = Guid.NewGuid().ToString().Replace("-", "") + Path.GetExtension(file.FileName);
            //            using (var fileStream = new FileStream(Path.Combine(uploads, key), FileMode.Create))
            //            {
            //                await file.CopyToAsync(fileStream);
            //            }

            //        }
            //        //using (var stream = new FileStream(filePath, FileMode.Create))
            //        //{
            //        //    await formFile.CopyToAsync(stream);
            //        //}
            //    }
            //}

            // process uploaded files
            // Don't rely on or trust the FileName property without validation.

            return Ok(new { count = myFile.Count, myFile });
        }



        protected void RollFiles(string _path, string _fileName)
        {
            var files = new DirectoryInfo(_path)
                .GetFiles(_fileName + "*")
                .OrderByDescending(f => f.Name);

            foreach (var item in files)
            {
                item.Delete();
            }

        }

        void CheckFileExtensionValid(string fileName)
        {
            fileName = fileName.ToLower();
            string[] imageExtensions = { ".jpg", ".jpeg", ".gif", ".png" };

            var isValidExtenstion = imageExtensions.Any(ext =>
            {
                return fileName.LastIndexOf(ext) > -1;
            });
            if (!isValidExtenstion)
                throw new Exception("Not allowed file extension");
        }
        void CheckMaxFileSize(FileStream stream)
        {
            if (stream.Length > 4000000)
                throw new Exception("File is too large");
        }
        void ProcessUploadedFile(string tempFilePath, string fileName)
        {
            // Check if the uploaded file is a valid image
            var path = Path.Combine(_appEnvironment.WebRootPath, "uploads");
            System.IO.File.Copy(tempFilePath, Path.Combine(path, fileName));
        }
        void AppendContentToFile(string path, IFormFile content)
        {
            using (FileStream stream = new FileStream(path, FileMode.Append, FileAccess.Write))
            {
                content.CopyTo(stream);
                CheckMaxFileSize(stream);
            }

        }
    }
}