using Microsoft.AspNetCore.Mvc;
using NLog;
using System.Linq;

namespace BartMarket.Controllers
{
    [ApiController]
    [Route("ozon")]
    public class LiteOzonController : Controller
    {
        private static Logger logger = LogManager.GetCurrentClassLogger();

        [HttpGet]
        public IActionResult Index(bool success)
        {
            if (success)
            {
                ViewData["SUCCESS"] = true;
            }
            ViewData["URL1"] = "http://ovz1.j34469996.pxlzp.vps.myjino.ru/content/liteozon.xml";
            ViewData["URL2"] = "http://ovz1.j34469996.pxlzp.vps.myjino.ru/content/fullozon.xml";

            ViewData["FORMULA"] = $"{Program.formula1};{Program.formula2};{Program.formula3};";

            return View();
        }
        [HttpPost]
        public IActionResult UpdateFormula([FromForm] string formula)
        {
            logger.Info(Request.HttpContext.Connection.RemoteIpAddress);
            logger.Info(Request.HttpContext.Connection.LocalIpAddress);

            if (formula == null)
            {
               return Redirect("ozon");
            }
            var data = formula.Split(";");
            if (data.Length < 3)
            {
                return Redirect("ozon");
            }
            Program.formula1 = data[0];
            Program.formula2 = data[1];
            Program.formula3 = data[2];

            return Redirect("ozon?success=true") ;
        }
    }
    
}
