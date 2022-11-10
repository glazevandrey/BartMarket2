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
            warehouses.Add(new Warehouse()
            {
                Name= "DPN", Condition = null

            });
            warehouses.Add(new Warehouse()
            {
                Name = "DPN2",
                Condition = new List<string>()
                {
                    "weight < 30.0",
                    "price > 3000",
                    "price < 50000"
                }

            });
            warehouses.Add(new Warehouse()
            {
                Name = "DPN3",
                Condition = null

            });

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

