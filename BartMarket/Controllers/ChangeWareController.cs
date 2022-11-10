using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;

namespace BartMarket.Controllers
{
    [ApiController]
    [Route("changeware")]
    public class ChangeWareController : Controller
    {
        public IActionResult Index([FromQuery]string ware)
        {
            return View(Program.warehouses.FirstOrDefault(m=>m.Name == ware));
        }
        [HttpPost]
        public IActionResult Save([FromForm] string cond, [FromForm] string ware, [FromForm] string oldware)
        {
            var h = Program.warehouses.FirstOrDefault(m => m.Name == oldware);
            h.Name = ware;
            var list = new List<string>();
            if(h == null)
            {
                return Redirect("Donplafon_Ozon");
            }
            
            var split = cond.Split(";");
            
            split = split.Where(x => !string.IsNullOrEmpty(x)).ToArray();
            for (int i = 0; i < split.Length; i++)
            {

                if (h.Condition.Count != i)
                {
                    if (h.Condition[i] == split[i])
                    {

                    }
                    else
                    {
                        h.Condition[i] = split[i];
                    }
                }
                else
                {
                    h.Condition.Add(split[i]);
                }
            }
            
            return Redirect("warehouses");
        }
    }
}
