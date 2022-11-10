using BartMarket.Data;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using NLog;
using Quartz.Logging;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;
using Logger = NLog.Logger;

namespace BartMarket
{
    public class Program
    {
        public static string formula1 = "(x + 1500) * 1.2";
        public static string formula2 = "(x + 1500) * 1.5";
        public static string formula3 = "(x + 1500) * 1.15";
        public static List<Warehouse> warehouses = new List<Warehouse>();
        public static string link_ozon_lite = "/content/liteozon.xml";
        public static string link_ozon_full = "/content/fullozon.xml";
        public static StatModel Last = new StatModel();

        private static Logger logger = LogManager.GetCurrentClassLogger();

        public static void Main(string[] args)
        {
            
            using (var db = new UserContext())
            {
                var l = db.LinkModels.ToList();
                if(l.Count == 0)
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

            using (var db = new UserContext())
            {
                var l = db.LinkModels.ToList();
                link_ozon_full = l.FirstOrDefault(m => m.Type == "Full").Link;
                link_ozon_lite = l.FirstOrDefault(m => m.Type == "Lite").Link;
            }


            var list = new List<WarehouseModel>();
            using (var db = new UserContext())
            {
                list = db.Warehouses.ToList();
                if(list.Count == 0)
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
            foreach (var item in list)
            {
                var n = new Warehouse();
                n.Name = item.Name;
                using (var db = new UserContext())
                {
                    var houses = db.Warehouses.ToList();

                    if(houses.Count == 0)
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

                        db.WarehouseSettings.AddRange(sett1,sett2, sett3);

                        db.SaveChanges();
                        break;
                    }



                    var set = db.WarehouseSettings.Where(m=>m.WarehouseId == item.Id).ToList();
                    if(set != null)
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
            //warehouses.Add(new Warehouse()
            //{
            //    Name = "DPN",
            //    Condition = null

            //});
            //warehouses.Add(new Warehouse()
            //{
            //    Name = "DPN2",
            //    Condition = new List<string>()
            //    {
            //        "weight < 30.0",
            //        "price > 3000",
            //        "price < 50000"
            //    }

            //});
            //warehouses.Add(new Warehouse()
            //{
            //    Name = "DPN3",
            //    Condition = null

            //});


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

