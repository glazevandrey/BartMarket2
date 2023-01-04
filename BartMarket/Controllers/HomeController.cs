using Microsoft.AspNetCore.Mvc;

namespace BartMarket.Controllers
{
    [ApiController]
    [Route("home")]
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public IActionResult ReturnProvider([FromForm] string type)
        {
            var list = Program.Providers[type];

            ViewData["providers"] = list;
            ViewData["type"] = type;

            return View();
        }
    }
}
