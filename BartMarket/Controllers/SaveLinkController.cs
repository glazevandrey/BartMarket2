using BartMarket.Data;
using Microsoft.AspNetCore.Mvc;
using System.Linq;

namespace BartMarket.Controllers
{
    [ApiController]
    [Route("savelink")]
    public class SaveLinkController : Controller
    {
        [HttpGet]
        public IActionResult Index()
        {
            return Redirect("Donplafon_Ozon");
        }

        [HttpPost]
        public IActionResult Save([FromForm] string link_lite, [FromForm] string link_full)
        {
            using (var db = new UserContext())
            {
                if(link_full == null)
                {
                    var l = db.LinkModels.FirstOrDefault(m => m.Type == "Lite");
                    l.Link = link_lite;
                }
                else
                {
                    var l = db.LinkModels.FirstOrDefault(m => m.Type == "Full");
                    l.Link = link_full;
                }
          
                db.SaveChanges();
            }

            if (link_lite != null)
            {
                Program.link_ozon_lite = link_lite;
            }
            else if (link_full != null)
            {
                Program.link_ozon_full = link_full;

            }

            return Redirect("Donplafon_Ozon");
        }
    }
}
