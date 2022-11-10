using Microsoft.AspNetCore.Mvc;

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

            if (link_lite != null)
            {
                Program.link_ozon_lite = link_lite;
            }
            else if (link_full != null)
            {
                Program.link_ozon_full = link_full;

            }
            return Redirect("Donplafon_Ozon");

            //return RedirectToAction("Donplafon_Ozon");
        }
    }
}
