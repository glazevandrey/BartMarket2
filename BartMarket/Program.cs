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
        private static Logger logger = LogManager.GetCurrentClassLogger();

        public static void Main(string[] args)
        {
            // StartLite();

            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });

        public async static Task StartLite()
        {
            XmlSerializer serializer = new XmlSerializer(typeof(YmlCatalog));
            YmlCatalog catalog = new YmlCatalog();

            if (File.Exists("exmp2.xml"))
            {
                File.Delete("exml2.xml");
            }
            if (File.Exists("exmp3.xml"))
            {
                File.Delete("exml3.xml");
            }
            try
            {
                using (var client = new HttpClient())
                {
                    using (var s = client.GetStreamAsync("https://partners.donplafon.ru/local/partners/BARTMARKET_XML_CONTENT/"))
                    {
                        using (var fs = new FileStream("exmp2.xml", FileMode.OpenOrCreate))
                        {
                            s.Result.CopyTo(fs);
                            logger.Info("success");

                        }
                    }
                }

                using (var client = new HttpClient())
                {
                    using (var s = client.GetStreamAsync("https://partners.donplafon.ru/local/partners/BARTMARKET_XML_PRICES/"))
                    {
                        using (var fs = new FileStream("exmp3.xml", FileMode.OpenOrCreate))
                        {
                            s.Result.CopyTo(fs);
                            logger.Info("success");

                        }
                    }
                }

            }
            catch (Exception ex)
            {
                logger.Error(ex.Message);
            }


            var text = File.ReadAllText("exmp2.xml");
            using (StringReader reader = new StringReader(text))
            {
                var text2 = serializer.Deserialize(reader);
                catalog = (YmlCatalog)text2;
            }



            YmlCatalog catalog2 = new YmlCatalog();
            var text22 = File.ReadAllText("exmp3.xml");
            using (StringReader reader = new StringReader(text22))
            {
                var text2 = serializer.Deserialize(reader);
                catalog2 = (YmlCatalog)text2;
            }
            if (File.Exists("exmp3.xml"))
            {

            }
            else
            {
                logger.Error("No acceess");
            }
            var ofrs = new List<Offer>();
            var ofrs2 = new List<Offer>();

            foreach (var item in catalog.Shop.Offers.Offer)
            {
                if (item.Price == null)
                {
                    if (Convert.ToInt32(item.OldPrice) > 1000)
                    {
                        ofrs.Add(item);
                    }
                }
                else
                {
                    if (Convert.ToInt32(item.Price) > 1000)
                    {
                        ofrs.Add(item);
                    }
                }


                //}

                //
                //YmlCatalog catalog2 = new YmlCatalog();

                //var text4 = File.ReadAllText("Example1.xml", Encoding.UTF8);
                //using (StringReader reader = new StringReader(text4))
                //{
                //    var text2 = serializer.Deserialize(reader);
                //    catalog = (YmlCatalog)text2;
                //}
                //var text3 = File.ReadAllText("Example22.xml", Encoding.UTF8);
                //using (StringReader reader = new StringReader(text3))
                //{
                //    var text2 = serializer.Deserialize(reader);
                //    catalog2 = (YmlCatalog)text2;
                //}
            }


            catalog.Shop.Offers.Offer = ofrs;

            XmlDocument docNew = new XmlDocument();
                XmlElement newRoot = docNew.CreateElement("yml_catalog");
                docNew.AppendChild(newRoot);
                var shop = docNew.CreateElement("shop");
                var offers = docNew.CreateElement("offers");
                newRoot.AppendChild(shop);
                shop.AppendChild(offers);


                Logic.StartParse(catalog, catalog2, docNew, offers, "light");
                docNew = new XmlDocument();
                newRoot = docNew.CreateElement("yml_catalog");
                docNew.AppendChild(newRoot);
                shop = docNew.CreateElement("shop");
                offers = docNew.CreateElement("offers");
                newRoot.AppendChild(shop);
                shop.AppendChild(offers);

            var startTime = System.Diagnostics.Stopwatch.StartNew();
            startTime.Stop();
            var resultTime = startTime.Elapsed;
            string elapsedTime = String.Format("{0:00}:{1:00}:{2:00}.{3:000}",
    resultTime.Hours,
    resultTime.Minutes,
    resultTime.Seconds,
    resultTime.Milliseconds);
            Logic.StartParse(catalog, catalog2, docNew, offers, "full");

                logger.Info("-----SUCCESS ENDED FORMATING FEED-----");
            logger.Info($"-----ELLAPSED: {elapsedTime}-----");

        }

    }
    }

