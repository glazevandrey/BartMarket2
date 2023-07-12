using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;

namespace BartMarket.Controllers
{
    [ApiController]
    [Route("arnika_aliexpress")]
    public class ArnikaAliController : Controller
    {
       
        public IActionResult Index(bool success)
        {
            var date = Program.Last["arnika"].Date.AddHours(3);
            List<string> result = new List<string>();
            string str = "";
           
                ViewData["SUCCESS2"] = Program.Last["arnika"].Success;
            ViewData["ERROR"] = Program.Last["arnika"].Error;
            ViewData["COUNT"] = Program.Last["arnika"].Count;
            ViewData["DATE"] = date;
            ViewData["TIMELITE"] = Program.Last["arnika"].ElapsedAli;
            ViewData["AIR"] = Program.inAir;


            if (success)
            {
                ViewData["SUCCESS"] = true;
            }
          

            ViewData["URL1"] = Program.link_aliexpress_arnika;
            ViewData["URL1F"] = "http://ovz1.j34469996.pxlzp.vps.myjino.ru" + Program.link_aliexpress_arnika;



            ViewData["URL2"] = "http://ovz1.j34469996.pxlzp.vps.myjino.ru/content/arnikafid.xml";
            ViewData["FORMULA"] = $"{Program.formula1_ar_ali};{Program.formula2_ar_ali};{Program.formula5_ar_ali};{Program.formula6_ar_ali};{Program.formula3_ar_ali};{Program.formula4_ar_ali}";
            return View();
        }
        [HttpPost]
        public IActionResult UpdateFormula([FromForm] string formula, [FromForm] string type, [FromForm] string type1)
        {
            if (formula == null)
            {
                return Redirect("arnika_aliexpress");
            }

           
                
            var data = formula.Split(";");
            if (data.Length < 3)
            {
                return Redirect("arnika_aliexpress");
            }
            Program.formula1_ar_ali = data[0];
            Program.formula2_ar_ali = data[1];
            Program.formula5_ar_ali = data[2];
            Program.formula6_ar_ali = data[3];
            Program.formula3_ar_ali = data[4];
            Program.formula4_ar_ali = data[5];


            return Redirect("arnika_aliexpress?success=true");

        }
    }
}
