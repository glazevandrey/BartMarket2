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
                ViewData["TempReady"]  = "https://localhost:44368/" + temp_path + "_ready.xlsx";
            }
            if (error != null)
            {
                ViewData["Error"] = "Сейчас идет обработка основного фида. Попробуйте через некоторое время.";
            }
            return View();
        }

        [HttpPost]
        public IActionResult Parse([FromForm] string temp)
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

            var res = Program.excelService.OzonParse(tempate);
            if(res == null)
            {
                return RedirectToAction("Index", new { error = "error"});
            }

            return RedirectToAction("Index", new { temp_path = tempate.PathToTemplate }); 
        }
    }
}
