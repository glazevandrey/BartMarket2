using BartMarket.Services;
using BartMarket.Template;
using Microsoft.AspNetCore.Mvc;
using NLog;

namespace BartMarket.Controllers
{
    [ApiController]
    [Route("excel")]
    public class ExcelParseController : Controller
    {
        private static Logger logger = LogManager.GetCurrentClassLogger();

        [HttpGet]
        public IActionResult Index(string temp_path, string error)
        {
            ViewData["Templates"] = Program.ozonTemplates;
            logger.Info(temp_path + "    " + error);
            if(temp_path != null)
            {
                
                ViewData["TempReady"]  = "http://ovz1.j34469996.pxlzp.vps.myjino/" + temp_path + "_ready.xlsx";
            }
            else
            {
                if (error != null)
                {
                    if (error.StartsWith("err"))
                    {
                        ViewData["Error"] = error.Split(":")[1].Trim();                       
                    }
                    else
                    {
                        ViewData["Error"] = "Сейчас идет обработка основного фида. Попробуйте через некоторое время.";
                    }
                }
            }
         
            return View();
        }

        [HttpPost]
        public IActionResult Parse([FromForm] string temp, [FromForm] int count)
        {
            IBaseOzonTemplate tempate = null;

            switch (temp)
            {
                case "Светильник напольный":
                    tempate = new NapolnyTorsher();
                    break;
                default:
                    break;
            }
            logger.Info("template : " + tempate.Name);
            var res = Program.excelService.OzonParse(tempate, count);
            logger.Info( " res = " + res);

            if (res == null)
            {
                return RedirectToAction("Index", new { error = "error"});
            }
            else if (res.StartsWith("err"))
            {
                return RedirectToAction("Index", new { error = res });

            }

            return RedirectToAction("Index", new { temp_path = tempate.PathToTemplate }); 
        }
    }
}
