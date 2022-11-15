using BartMarket.Data;
using Microsoft.AspNetCore.Mvc;
using System.Linq;

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
            var model = new WarehouseModel();
            model.Name = ware;
            warehouse.Name = ware.Trim();
            var id = 0;
            using (var db = new UserContext())
            {
                db.Warehouses.Add(model);
                db.SaveChanges();


            }

            using (var db = new UserContext())
            {
                id = db.Warehouses.FirstOrDefault(m => m.Name == model.Name).Id;
            }

            if (havecond == "false" || havecond == "off" || cond == null)
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
            using (var db = new UserContext())
            {

                for (int i = 0; i < split.Length; i++)
                {
                    warehouse.Condition.Add(split[i].Trim());
                    db.WarehouseSettings.Add(new WarehouseSetting()
                    {
                        WarehouseId = id,
                        Filter = split[i].Trim(),

                    });
                    db.SaveChanges();
                }
            }


            Program.warehouses.Add(warehouse);
            return Redirect("Donplafon_Ozon");
        }
    }
}
