using BartMarket.Data;
using Microsoft.AspNetCore.Identity;
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
            ViewData["FORMULA1"] = $"{Program.formula1_ar_ali};{Program.formula2_ar_ali}";
            ViewData["FORMULA2"] = $"{Program.formula5_ar_ali};{Program.formula6_ar_ali}";
            ViewData["FORMULA3"] = $"{Program.formula3_ar_ali};{Program.formula4_ar_ali}";
            return View();
        }
        [HttpPost]
        public IActionResult UpdateFormula([FromForm] string formula1, [FromForm] string formula2, [FromForm] string formula3, [FromForm] string type, [FromForm] string type1)
        {
            string formula = formula1+";" + formula2 +";"+ formula3;
            if (formula == null)
            {
                return Redirect("arnika_aliexpress");
            }
      
            var data = formula.Split(";");
            if (data.Length < 3)
            {
                return Redirect("arnika_aliexpress");
            }

            using (var db = new UserContext())
            {
                var f1  = db.Formulas.FirstOrDefault(m=>m.Name == "formula1_ar_ali");
                f1.Value = data[0];

                var f2 = db.Formulas.FirstOrDefault(m => m.Name == "formula2_ar_ali");
                f2.Value = data[1];

                var f3 = db.Formulas.FirstOrDefault(m => m.Name == "formula5_ar_ali");
                f3.Value = data[2];

                var f4 = db.Formulas.FirstOrDefault(m => m.Name == "formula6_ar_ali");
                f4.Value = data[3];

                var f5 = db.Formulas.FirstOrDefault(m => m.Name == "formula3_ar_ali");
                f5.Value = data[4];

                var f6 = db.Formulas.FirstOrDefault(m => m.Name == "formula4_ar_ali");
                f6.Value = data[5];

                db.SaveChanges();

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
