using BartMarket.Services;
using BartMarket.Template;
using Microsoft.AspNetCore.Mvc;

namespace BartMarket.Controllers
{
    [ApiController]
    [Route("excel")]
    public class ExcelParseController : Controller
    {
       
        [HttpGet]
        public IActionResult Index(string temp_path, string error)
        {
            ViewData["Templates"] = Program.ozonTemplates;
            if(temp_path != null)
            {
                if(error != null) 
                {
                    if (error.StartsWith("err"))
                    {
                        ViewData["Error"] = error.Split(":")[1].Trim();

                    }
                }
                ViewData["TempReady"]  = "http://ovz1.j34469996.pxlzp.vps.myjino/" + temp_path + "_ready.xlsx";
            }
            if (error != null)
            {
                ViewData["Error"] = "Сейчас идет обработка основного фида. Попробуйте через некоторое время.";
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

            var res = Program.excelService.OzonParse(tempate, count);
            if(res == null)
            {
                return RedirectToAction("Index", new { error = "error"});
            }else if (res.StartsWith("err"))
            {
                return RedirectToAction("Index", new { error = res });

            }

            return RedirectToAction("Index", new { temp_path = tempate.PathToTemplate }); 
        }
    }
}
