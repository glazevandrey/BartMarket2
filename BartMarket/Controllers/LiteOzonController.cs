using Microsoft.AspNetCore.Mvc;
using NLog;
using System.Linq;

namespace BartMarket.Controllers
{
    [ApiController]
    [Route("Donplafon_Ozon")]
    public class LiteOzonController : Controller
    {
        private static Logger logger = LogManager.GetCurrentClassLogger();

        [HttpGet]
        public IActionResult Index(bool success)
        {

            ViewData["SUCCESS2"] = Program.Last.Success;
            ViewData["ERROR"] = Program.Last.Error;
            ViewData["COUNT"] = Program.Last.Count;
            ViewData["DATE"] = Program.Last.Date;
            ViewData["TIMELITE"] = Program.Last.ElapsedLite;
            ViewData["TIMEFULL"] = Program.Last.ElapsedFull;



            if (success)
            {
                ViewData["SUCCESS"] = true;
            }
            ViewData["URL1"] = Program.link_ozon_lite;
            ViewData["URL2"] = Program.link_ozon_full;
            ViewData["URL1F"] = "http://ovz1.j34469996.pxlzp.vps.myjino.ru" + Program.link_ozon_lite;

            ViewData["URL2F"] = "http://ovz1.j34469996.pxlzp.vps.myjino.ru" + Program.link_ozon_lite;

            ViewData["URL3"] = "http://ovz1.j34469996.pxlzp.vps.myjino.ru/content/exmp2.xml";
            ViewData["URL4"] = "http://ovz1.j34469996.pxlzp.vps.myjino.ru/content/exmp3.xml";

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
               return Redirect("Donplafon_Ozon");
            }
            var data = formula.Split(";");
            if (data.Length < 3)
            {
                return Redirect("Donplafon_Ozon");
            }
            Program.formula1 = data[0];
            Program.formula2 = data[1];
            Program.formula3 = data[2];

            return Redirect("Donplafon_Ozon?success=true") ;
        }
    }
    
}
