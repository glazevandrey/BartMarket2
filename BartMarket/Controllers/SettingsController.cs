using Microsoft.AspNetCore.Mvc;
using System.IO;
using System.Text.RegularExpressions;
using System;

namespace BartMarket.Controllers
{
    [ApiController]
    [Route("api/settings")]
    public class SettingsController : Controller
    {
        public const long MaxFileSize = 10L * 1024L * 1024L * 1024L; // 10GB, adjust to your need

        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }

        [RequestSizeLimit(10L * 1024L * 1024L * 1024L)]
        [RequestFormLimits(MultipartBodyLengthLimit = 10L * 1024L * 1024L * 1024L)]
          [HttpPost]
        public string Save()
        {
            var path = "wwwroot/content/";
            var uploadedFile = Request.Form.Files;
            string name = "";
            foreach (var item in uploadedFile)
            {
                try
                {
                    Regex reg = new Regex(@"\\(\w+).xml");
                Match m = reg.Match(item.FileName);
                string format = ".xml";
                    name = item.FileName;
                    path += "fullozon" + format;
                    using (var fileStream = new FileStream(path, FileMode.Create))
                    {
                        item.CopyToAsync(fileStream);
                    }
                }
                catch (Exception ex)
                {
                    return ex.Message;
                }
            }
            return path + " " + name;
        }
    }

  
    
}
