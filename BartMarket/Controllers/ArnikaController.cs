using BartMarket.Data;
using Microsoft.AspNetCore.Mvc;

namespace BartMarket.Controllers
{
    [ApiController]
    [Route("arnika_ozon")]
    public class ArnikaController : Controller
    {
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

            ViewData["URL1"] = Program.link_ozon_arnika_lite;
            ViewData["URL2"] = Program.link_ozon_arnika_full;
            ViewData["URL1F"] = "http://ovz1.j34469996.pxlzp.vps.myjino.ru" + Program.link_ozon_arnika_lite;

            ViewData["URL2F"] = "http://ovz1.j34469996.pxlzp.vps.myjino.ru" + Program.link_ozon_arnika_lite;

            ViewData["URL3"] = "http://ovz1.j34469996.pxlzp.vps.myjino.ru/content/arnikafid.xml";
            ViewData["FORMULA"] = $"{Program.formula1_ar};{Program.formula2_ar};{Program.formula3_ar};";
            ViewData["FORMULADOP"] = $"{Program.formula1_ar_dop};{Program.formula2_ar_dop};{Program.formula3_ar_dop};";

            return View();
        }
        [HttpPost]
        public IActionResult UpdateFormula([FromForm] string formula, [FromForm] string type)
        {
            if (formula == null)
            {
                return Redirect("arnika_ozon");
            }
            if (type == "dop")
            {
                var data = formula.Split(";");
                if (data.Length < 3)
                {
                    return Redirect("arnika_ozon");
                }

                Program.formula1_ar_dop = data[0];
                Program.formula2_ar_dop = data[1];
                Program.formula3_ar_dop = data[2];
            }
            else
            {
                var data = formula.Split(";");
                if (data.Length < 3)
                {
                    return Redirect("arnika_ozon");
                }
                Program.formula1_ar= data[0];
                Program.formula2_ar = data[1];
                Program.formula3_ar = data[2];

            }

            return Redirect("arnika_ozon?success=true");

        }
    }
}
