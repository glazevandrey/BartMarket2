using BartMarket.Data;
using BartMarket.Template;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using NLog;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using Logger = NLog.Logger;

namespace BartMarket
{
    public class Program
    {
        public static bool ExcelAir = false;
        public static string formula1 = "(x + 1500) * 1.2";
        public static string formula2 = "(x + 1500) * 1.5";
        public static string formula3 = "(x + 1500) * 1.15";
        public static List<Warehouse> warehouses = new List<Warehouse>();
        public static string link_ozon_lite = "/content/liteozon.xml";
        public static string link_ozon_full = "/content/fullozon.xml";
        public static StatModel Last = new StatModel();
        public static bool inAir = false;
        public static List<Offer2> list = new List<Offer2>();
        public static List<Offer> deleted = new List<Offer>();
        public static List<Offer2> need_add = new List<Offer2>();
        public static IBaseOzonTemplate lastTemplate;
        public static StringBuilder lastIds = new StringBuilder();
        public static List<IBaseOzonTemplate> ozonTemplates = new List<IBaseOzonTemplate>();
        public static Dictionary<string, List<string>> Providers = new Dictionary<string, List<string>>()
        {
            { "ozon", new List<string>(){ "donplafon"} } ,
            { "wb", new List<string>(){ ""} },
            { "ym", new List<string>(){ ""} },

        };
        public static void Main(string[] args)
        {

            //IronXL.License.LicenseKey = "IRONXL.ANDREIGLAZEV.6127-1437388117-BCLHSJ3L73OL3VJ-HXZNWRL5SQV5-XN3DONZDABSQ-4P5K3IVW22T2-NZNCZUGHI6HD-TXFJE3-TQN7K64XGTWIUA-DEPLOYMENT.TRIAL-2ZETOW.TRIAL.EXPIRES.01.FEB.2023";
            //NEXT
            IronXL.License.LicenseKey = "IRONXL.TONI70020.25513-B58A105DFA-E5J5E2-KN5QIEVOS6QV-25KF2LLZS664-ZUPCPC43GEYK-LNIVESSFL4KX-CPN2OAJ5HCAT-YQA6BE-THDZ3GN3FCOJEA-DEPLOYMENT.TRIAL-T6FH55.TRIAL.EXPIRES.02.MAR.2023"; 
            ozonTemplates.Add(new NapolnyTorsher());

            using (var db = new UserContext())
            {
                var fg = db.UploadedOzonIds.ToList();
                var l = db.LinkModels.ToList();
                if (l.Count == 0)
                {
                    var lite = new LinkModel();
                    lite.Link = link_ozon_lite;
                    lite.Type = "Lite";

                    var lite2 = new LinkModel();
                    lite2.Link = link_ozon_full;
                    lite2.Type = "Full";

                    db.LinkModels.Add(lite);
                    db.LinkModels.Add(lite2);
                    db.SaveChanges();


                }
            }

            var list = new List<WarehouseModel>();
            using (var db = new UserContext())
            {
                list = db.Warehouses.ToList();
                if (list.Count == 0)
                {
                    var _1 = new WarehouseModel();
                    var _2 = new WarehouseModel();
                    var _3 = new WarehouseModel();

                    _1.Name = "DPN";
                    _2.Name = "DPN2";
                    _3.Name = "DPN3";

                    db.Warehouses.Add(_1);
                    db.Warehouses.Add(_2);
                    db.Warehouses.Add(_3);

                    db.SaveChanges();
                }


            }

            using (var db = new UserContext())
            {
                list = db.Warehouses.ToList();

            }
            foreach (var item in list)
            {
                var n = new Warehouse();
                n.Name = item.Name;
                using (var db = new UserContext())
                {
                    var houses = db.WarehouseSettings.ToList();

                    if (houses.Count == 0)
                    {

                        var sett = new List<WarehouseSetting>();

                        var sett1 = new WarehouseSetting();
                        sett1.WarehouseId = 2;
                        sett1.Filter = "price > 3000";

                        var sett2 = new WarehouseSetting();
                        sett2.WarehouseId = 2;
                        sett2.Filter = "price < 50000";

                        var sett3 = new WarehouseSetting();
                        sett3.WarehouseId = 2;
                        sett3.Filter = "weight < 30";

                        db.WarehouseSettings.AddRange(sett1, sett2, sett3);

                        db.SaveChanges();
                    }
                }
                using (var db = new UserContext())
                {

                    var set = db.WarehouseSettings.Where(m => m.WarehouseId == item.Id).ToList();
                    if (set != null)
                    {
                        n.Condition = new List<string>();
                        foreach (var item2 in set)
                        {
                            n.Condition.Add(item2.Filter.Trim());
                        }
                    }
                    else
                    {
                        n.Condition = null;
                    }
                }
                warehouses.Add(n);

            }

            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });


    }
}

