using Microsoft.Extensions.DependencyInjection;
using NLog;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Runtime.InteropServices;
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
        public static int MakePrice(string s1)
        {
            string ss = s1.ToString();
            char[] hh = ss.ToCharArray();


            char[] d = s1.ToString().ToCharArray();
            Array.Reverse(d);
            var __3 = Convert.ToInt32(d[2].ToString());
            int g1 = 0;
            List<int> ints = new List<int>();

            int x2 = 0;
            if (__3 == 9)
            {
                var fist = s1.ToString().IndexOf('9');
                char[] newhh = new char[hh.Length + 1];
                for (int i = hh.Length; i > fist; i--)
                {
                    hh[i - 1] = '0';
                }
                try
                {
                    hh[fist - 1] = Convert.ToChar((Convert.ToInt32(hh[fist - 1].ToString()) + 1).ToString());

                }
                catch (Exception ex)
                {
                    newhh = new char[hh.Length + 1];
                    for (int i = 1; i < newhh.Length; i++)
                    {
                        newhh[i] = hh[i - 1];
                    }
                    newhh[0] = '1';
                }
                try
                {
                    x2 = Convert.ToInt32(new string(newhh));

                }
                catch (Exception)
                {
                    x2 = Convert.ToInt32(new string(hh));

                }

            }
            else
            {
                g1 = __3 + 1;
                char _1 = '0';
                char _2 = _1;
                char _3 = Convert.ToChar(g1.ToString());
                d[0] = _1;
                d[1] = _2;
                d[2] = _3;
                Array.Reverse(d);
                x2 = Convert.ToInt32(new string(d));

            }
            return x2;
        }
        public async static Task StartLite()
        {


            XmlSerializer serializer = new XmlSerializer(typeof(YmlCatalog));
            YmlCatalog catalog = new YmlCatalog();

            try
            {
                using (var client = new HttpClient())
                {
                    using (var s = client.GetStreamAsync("https://partners.donplafon.ru/local/partners/BARTMARKET_XML_CONTENT/"))
                    {
                        using (var fs = new FileStream($"{Environment.CurrentDirectory}/wwwroot/content/exmp2.xml", FileMode.OpenOrCreate))
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
                        using (var fs = new FileStream($"{Environment.CurrentDirectory}/wwwroot/content/exmp3.xml", FileMode.OpenOrCreate))
                        {
                            s.Result.CopyTo(fs);
                            logger.Info("success");

                        }
                    }
                }

            }
            catch (Exception ex)
            {
                logger.Error("from download" + ex.Message);
                Program.Last.Success = false;
                Program.Last.Error = ex.Message;

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


                //var text = File.ReadAllText($"Example1.xml");
                //using (StringReader reader = new StringReader(text))
                //{
                //    var text2 = serializer.Deserialize(reader);
                //    catalog = (YmlCatalog)text2;
                //}



                //var text22 = File.ReadAllText($"Example22.xml");
                //using (StringReader reader = new StringReader(text22))
                //{
                //    var text2 = serializer.Deserialize(reader);
                //    catalog2 = (YmlCatalog)text2;
                //}
            }
            catch (Exception ex)
            {
                logger.Error("from upload to disk" + ex.Message);
                Program.Last.Success = false;
                Program.Last.Error = ex.Message;
                return;
            }
          
         
            var ofrs = new List<Offer>();

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

            Program.Last.ElapsedLite = elapsedTime;
            logger.Info("-----SUCCESS ENDED LITE FORMATING FEED-----");
            logger.Info($"-----ELLAPSED: {elapsedTime}-----");
            GC.Collect();
            GC.WaitForPendingFinalizers();
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
            Program.Last.ElapsedFull = elapsedTime;

            logger.Info("-----SUCCESS ENDED FULL FORMATING FEED-----");
            logger.Info($"-----ELLAPSED: {elapsedTime}-----");
            GC.Collect();
            GC.WaitForPendingFinalizers();

            Program.Last.Date = DateTime.Now;
            
        }

    }
}
