using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;

namespace BartMarket.Controllers
{
    [ApiController]
    [Route("donplafon_aliexpress")]
    public class DonAliController : Controller
    {
       
        public IActionResult Index(bool success)
        {
            var date = Program.Last["donplafon"].Date.AddHours(3);
            List<string> result = new List<string>();
            string str = "";
           
            ViewData["SUCCESS2"] = Program.Last["donplafon"].Success;
            ViewData["ERROR"] = Program.Last["donplafon"].Error;
            ViewData["COUNT"] = Program.Last["donplafon"].Count;
            ViewData["DATE"] = date;
            ViewData["TIMELITE"] = Program.Last["donplafon"].ElapsedAli;
            ViewData["AIR"] = Program.inAir;


            if (success)
            {
                ViewData["SUCCESS"] = true;
            }
          

            ViewData["URL1"] = Program.link_aliexpress_donplafon;
            ViewData["URL1F"] = "http://ovz1.j34469996.pxlzp.vps.myjino.ru" + Program.link_aliexpress_donplafon;

            string d = "";
            foreach (var item in Program.ZeroVendors)
            {
                d += item + ";";
            }
            d = d.TrimEnd(';');

            ViewData["Vendors"] = d;
            ViewData["URL2"] = "http://ovz1.j34469996.pxlzp.vps.myjino.ru/content/exmp2.xml";
            ViewData["FORMULA1"] = $"{Program.formula1_dp_ali};{Program.formula2_dp_ali}";
            ViewData["FORMULA2"] = $"{Program.formula3_dp_ali};{Program.formula4_dp_ali}";

            return View();
        }

        [HttpPost]
        public IActionResult UpdateFormula([FromForm] string formula1, [FromForm] string formula2, [FromForm] string type, [FromForm] string type1, [FromForm] string vendors)
        {
            var split = vendors.Split(";");
            if (split.Length == 0)
            {
            }


            Program.ZeroVendors.Clear();

            foreach (var item in split)
            {
                Program.ZeroVendors.Add(item);
            }


            string formula = formula1+";" + formula2;
            if (formula == null)
            {
                return Redirect("donplafon_aliexpress");
            }
      
            var data = formula.Split(";");
            if (data.Length < 3)
            {
                return Redirect("donplafon_aliexpress");
            }
            Program.formula1_ar_ali = data[0];
            Program.formula2_ar_ali = data[1];
            Program.formula3_ar_ali = data[2];
            Program.formula4_ar_ali = data[3];
    

            return Redirect("donplafon_aliexpress?success=true");

        }
       
    }
}
