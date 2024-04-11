using BartMarket.Data;
using Microsoft.AspNetCore.Mvc;
using System.Linq;

namespace BartMarket.Controllers
{
    [ApiController]
    [Route("arnika_ozon")]
    public class ArnikaController : Controller
    {
        public IActionResult Index(bool success)
        {
            var date = Program.Last["arnika"].Date.AddHours(3);

            ViewData["SUCCESS2"] = Program.Last["arnika"].Success;
            ViewData["ERROR"] = Program.Last["arnika"].Error;
            ViewData["COUNT"] = Program.Last["arnika"].Count;
            ViewData["DATE"] = date;
            ViewData["TIMELITE"] = Program.Last["arnika"].ElapsedLite;
            ViewData["TIMEFULL"] = Program.Last["arnika"].ElapsedFull;
            ViewData["AIR"] = Program.inAir;


            if (success)
            {
                ViewData["SUCCESS"] = true;
            }

            ViewData["URL1"] = Program.link_ozon_arnika_lite;
            ViewData["URL1F"] = "http://ovz1.j34469996.pxlzp.vps.myjino.ru" + Program.link_ozon_arnika_lite;

            ViewData["URL2"] = Program.link_ozon_arnika_full;

            ViewData["URL2F"] = "http://ovz1.j34469996.pxlzp.vps.myjino.ru" + Program.link_ozon_arnika_lite;
            ViewData["OFFER"] = Program._ARN;


            ViewData["URL4"] = Program.link_ozon_arnika_lite1;

            ViewData["URL4F"] = "http://ovz1.j34469996.pxlzp.vps.myjino.ru" + Program.link_ozon_arnika_lite1;





            ViewData["URL3"] = "http://ovz1.j34469996.pxlzp.vps.myjino.ru/content/arnikafid.xml";
            ViewData["FORMULA"] = $"{Program.formula1_ar};{Program.formula2_ar};{Program.formula3_ar};";    
            ViewData["FORMULADOP"] = $"{Program.formula1_ar_dop};{Program.formula2_ar_dop};{Program.formula3_ar_dop};";
           
            ViewData["FORMULA1"] = $"{Program.formula1_ar1};{Program.formula2_ar1};{Program.formula3_ar1};";
            ViewData["FORMULADOP1"] = $"{Program.formula1_ar1_dop};{Program.formula2_ar1_dop};{Program.formula3_ar1_dop};";

            return View();
        }
        [HttpPost("UpdateOfferArn", Name = "UpdateOfferArn")]
        public IActionResult UpdateOffer([FromForm] string offer)
        {

            if (offer == null)
            {
                return Redirect("arnika_ozon");
            }

            Program._ARN = offer;

            return Redirect("/arnika_ozon?success=true");
        }
        [HttpPost]
        public IActionResult UpdateFormula([FromForm] string formula, [FromForm] string type, [FromForm] string type1)
        {
            if (formula == null)
            {
                return Redirect("arnika_ozon");
            }
            using (var db = new UserContext())
            {
                if (type1 != "lite1")
                {
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

                        var f1 = db.Formulas.FirstOrDefault(m=>m.Name == "formula1_ar_dop");
                        f1.Value = data[0];
                        var f2 = db.Formulas.FirstOrDefault(m => m.Name == "formula2_ar_dop");
                        f2.Value = data[1];
                        var f3 = db.Formulas.FirstOrDefault(m => m.Name == "formula3_ar_dop");
                        f3.Value = data[2];
                        db.SaveChanges();
         
                    }
                    else
                    {
                        var data = formula.Split(";");
                        if (data.Length < 3)
                        {
                            return Redirect("arnika_ozon");
                        }

                        Program.formula1_ar = data[0];
                        Program.formula2_ar = data[1];
                        Program.formula3_ar = data[2];

                        var f1 = db.Formulas.FirstOrDefault(m => m.Name == "formula1_ar");
                        f1.Value = data[0];
                        var f2 = db.Formulas.FirstOrDefault(m => m.Name == "formula2_ar");
                        f2.Value = data[1];
                        var f3 = db.Formulas.FirstOrDefault(m => m.Name == "formula3_ar");
                        f3.Value = data[2];
                        db.SaveChanges();


                    }

                }
                else
                {
                    if (type == "dop")
                    {
                        var data = formula.Split(";");
                        if (data.Length < 3)
                        {
                            return Redirect("arnika_ozon");
                        }
                        Program.formula1_ar1_dop = data[0];
                        Program.formula2_ar1_dop = data[1];
                        Program.formula3_ar1_dop = data[2];
                        var f1 = db.Formulas.FirstOrDefault(m => m.Name == "formula1_ar1_dop");
                        f1.Value = data[0];
                        var f2 = db.Formulas.FirstOrDefault(m => m.Name == "formula2_ar1_dop");
                        f2.Value = data[1];
                        var f3 = db.Formulas.FirstOrDefault(m => m.Name == "formula3_ar1_dop");
                        f3.Value = data[2];
                        db.SaveChanges();

                    }
                    else
                    {
                        var data = formula.Split(";");
                        if (data.Length < 3)
                        {
                            return Redirect("arnika_ozon");
                        }
                        Program.formula1_ar1 = data[0];
                        Program.formula2_ar1 = data[1];
                        Program.formula3_ar1 = data[2];
                        var f1 = db.Formulas.FirstOrDefault(m => m.Name == "formula1_ar1");
                        f1.Value = data[0];
                        var f2 = db.Formulas.FirstOrDefault(m => m.Name == "formula2_ar1");
                        f2.Value = data[1];
                        var f3 = db.Formulas.FirstOrDefault(m => m.Name == "formula3_ar1");
                        f3.Value = data[2];
                        db.SaveChanges();


                    }

                }
            }
       

            return Redirect("arnika_ozon?success=true");

        }
    }
}
