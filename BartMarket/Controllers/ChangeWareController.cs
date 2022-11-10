using BartMarket.Data;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Configuration;
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

            var model = new WarehouseModel();
            using (var db = new UserContext())
            {
                model = db.Warehouses.FirstOrDefault(m=>m.Name == oldware);
                var sett = db.WarehouseSettings.Where(m=>m.WarehouseId == model.Id);
                db.WarehouseSettings.RemoveRange(sett);
                db.SaveChanges();
            }

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
            model.Name = ware;

            var setts = new List<WarehouseSetting>();
            foreach (var item in h.Condition)
            {
                setts.Add(new WarehouseSetting()
                {
                    Filter = item,
                    WarehouseId = model.Id
                });
            }
            using (var db = new UserContext())
            {
                model = db.Warehouses.FirstOrDefault(m => m.Name == oldware);
                
                db.Warehouses.Update(model);
                db.WarehouseSettings.AddRange(setts);

                db.SaveChanges();
            }
            return Redirect("warehouses");
        }
    }
}
