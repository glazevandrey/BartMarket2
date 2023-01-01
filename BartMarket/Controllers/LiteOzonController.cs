using Microsoft.AspNetCore.Mvc;
using NLog;

namespace BartMarket.Controllers
{
    [ApiController]
    [Route("donplafon_ozon")]
    public class LiteOzonController : Controller
    {
        private static Logger logger = LogManager.GetCurrentClassLogger();

        [HttpGet]
        public IActionResult Index(bool success)
        {

            var date = Program.Last.Date.AddHours(3);

            ViewData["SUCCESS2"] = Program.Last.Success;
            ViewData["ERROR"] = Program.Last.Error;
            ViewData["COUNT"] = Program.Last.Count;
            ViewData["DATE"] = date;
            ViewData["TIMELITE"] = Program.Last.ElapsedLite;
            ViewData["TIMEFULL"] = Program.Last.ElapsedFull;
            ViewData["AIR"] = Program.inAir;


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
                return Redirect("donplafon_ozon");
            }
            var data = formula.Split(";");
            if (data.Length < 3)
            {
                return Redirect("donplafon_ozon");
            }
            Program.formula1 = data[0];
            Program.formula2 = data[1];
            Program.formula3 = data[2];

            return Redirect("donplafon_ozon?success=true");
        }
    }

}
