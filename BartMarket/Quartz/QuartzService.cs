using Microsoft.Extensions.DependencyInjection;
using NLog;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;

namespace BartMarket.Quartz
{
    public class QuartzService : IQuartzService
    {
        private static Logger logger = LogManager.GetCurrentClassLogger();

        public QuartzService()
        {
        }
        public async Task MainParse()
        {
            await StartLite();
        }
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
                        using (var fs = new FileStream($"{Environment.CurrentDirectory}/wwwroot/content/content/exmp2.xml", FileMode.OpenOrCreate))
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
                        using (var fs = new FileStream($"{Environment.CurrentDirectory}/wwwroot/content/content/exmp3.xml", FileMode.OpenOrCreate))
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
                return;
            }
            YmlCatalog catalog2 = new YmlCatalog();

            try
            {
                var text = File.ReadAllText($"{Environment.CurrentDirectory}/wwwroot/content/exmp2.xml");
                using (StringReader reader = new StringReader(text))
                {
                    var text2 = serializer.Deserialize(reader);
                    catalog = (YmlCatalog)text2;
                }



                var text22 = File.ReadAllText($"{Environment.CurrentDirectory}/wwwroot/content/exmp3.xml");
                using (StringReader reader = new StringReader(text22))
                {
                    var text2 = serializer.Deserialize(reader);
                    catalog2 = (YmlCatalog)text2;
                }
            }
            catch (Exception ex)
            {
                logger.Error(ex.Message);
                return;
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
            }


            catalog.Shop.Offers.Offer = ofrs;

            XmlDocument docNew = new XmlDocument();
            XmlElement newRoot = docNew.CreateElement("yml_catalog");
            docNew.AppendChild(newRoot);
            var shop = docNew.CreateElement("shop");
            var offers = docNew.CreateElement("offers");
            newRoot.AppendChild(shop);
            shop.AppendChild(offers);
            var startTime = System.Diagnostics.Stopwatch.StartNew();


            Logic.StartParse(catalog, catalog2, docNew, offers, "lite");


            startTime.Stop();
            var resultTime = startTime.Elapsed;
            string elapsedTime = String.Format("{0:00}:{1:00}:{2:00}.{3:000}",
    resultTime.Hours,
    resultTime.Minutes,
    resultTime.Seconds,
    resultTime.Milliseconds);


            logger.Info("-----SUCCESS ENDED LITE FORMATING FEED-----");
            logger.Info($"-----ELLAPSED: {elapsedTime}-----");
            docNew = new XmlDocument();
            newRoot = docNew.CreateElement("yml_catalog");
            docNew.AppendChild(newRoot);
            shop = docNew.CreateElement("shop");
            offers = docNew.CreateElement("offers");
            newRoot.AppendChild(shop);
            shop.AppendChild(offers);

             startTime = System.Diagnostics.Stopwatch.StartNew();
            Logic.StartParse(catalog, catalog2, docNew, offers, "full");
            startTime.Stop();
             resultTime = startTime.Elapsed;
             elapsedTime = String.Format("{0:00}:{1:00}:{2:00}.{3:000}",
    resultTime.Hours,
    resultTime.Minutes,
    resultTime.Seconds,
    resultTime.Milliseconds);

            logger.Info("-----SUCCESS ENDED FULL FORMATING FEED-----");
            logger.Info($"-----ELLAPSED: {elapsedTime}-----");

        }

    }
}
