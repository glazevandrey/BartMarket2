using Microsoft.AspNetCore.Mvc;

namespace BartMarket.Controllers
{
    [ApiController]
    [Route("warehouses")]
    public class WarehousesController : Controller
    {
        public IActionResult Index()
        {
            return View(Program.warehouses);
        }
        [HttpPost]
        public IActionResult Save([FromForm] string ware, [FromForm] string havecond, [FromForm] string cond)
        {
            var warehouse = new Warehouse();

            warehouse.Name = ware.Trim();
            if(havecond == "false" || havecond == "off")
            {
                warehouse.Condition = null;
                Program.warehouses.Add(warehouse);
                return Redirect("Donplafon_Ozon");
            }
            else
            {
                warehouse.Condition = new System.Collections.Generic.List<string>();
              
            }

            var split = cond.Split(";");
            for (int i = 0; i < split.Length; i++)
            {
                warehouse.Condition.Add(split[i].Trim());
            }

            Program.warehouses.Add(warehouse);
            return Redirect("Donplafon_Ozon");
        }
    }
}
