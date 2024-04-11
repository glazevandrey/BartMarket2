using BartMarket.Data;
using Microsoft.AspNetCore.Mvc;
using NLog;
using System;
using System.Linq;

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
            try
            {
                var date = Program.Last["donplafon"].Date.AddHours(3);

                ViewData["SUCCESS2"] = Program.Last["donplafon"].Success;
                ViewData["ERROR"] = Program.Last["donplafon"].Error;
                ViewData["COUNT"] = Program.Last["donplafon"].Count;
                ViewData["DATE"] = date;
                ViewData["TIMELITE"] = Program.Last["donplafon"].ElapsedLite;
                ViewData["TIMEFULL"] = Program.Last["donplafon"].ElapsedFull;
                ViewData["AIR"] = Program.inAir;
                ViewData["OFFER"] = Program._DON;

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
            }
            catch (Exception ex)
            {

                throw ex;
            }
          

            return View();
        }
        [HttpPost]
        public IActionResult UpdateFormula([FromForm] string formula)
        {

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
            using (var db = new UserContext())
            {

                var f1 = db.Formulas.FirstOrDefault(m => m.Name == "formula1");
                f1.Value = data[0];

                var f2 = db.Formulas.FirstOrDefault(m => m.Name == "formula2");
                f2.Value = data[1];

                var f3 = db.Formulas.FirstOrDefault(m => m.Name == "formula3");
                f3.Value = data[2];

                db.SaveChanges();
            }

            return Redirect("donplafon_ozon?success=true");
        }
        
        [HttpPost("UpdateOffer", Name = "UpdateOffer")]
        public IActionResult UpdateOffer([FromForm] string offer)
        {

            if (offer == null)
            {
                return Redirect("donplafon_ozon");
            }

            Program._DON = offer;

            return Redirect("/donplafon_ozon?success=true");
        }
    }

}
