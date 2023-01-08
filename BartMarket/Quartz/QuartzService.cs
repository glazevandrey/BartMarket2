using BartMarket.Data;
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
            if (Program.ExcelAir)
            {
                return;
            }
            Program.inAir = true;
            Program.Last.Success = true;
            Program.list.Clear();
            Program.list = null;
            XmlSerializer serializer = new XmlSerializer(typeof(YmlCatalog));
            YmlCatalog catalog = new YmlCatalog();
            GC.Collect();
            GC.WaitForPendingFinalizers();
            if (File.Exists($"{Environment.CurrentDirectory}/wwwroot/content/exmp2.xml"))
            {
                File.Delete($"{Environment.CurrentDirectory}/wwwroot/content/exmp2.xml");
            }
            if (File.Exists($"{Environment.CurrentDirectory}/wwwroot/content/exmp3.xml"))
            {
                File.Delete($"{Environment.CurrentDirectory}/wwwroot/content/exmp3.xml");
            }
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
                logger.Error("from download " + ex.Message);
                Program.Last.Success = false;
                Program.Last.Error = ex.Message;
                Program.inAir = false;

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
                logger.Error("from upload to disk " + ex.Message);
                Program.Last.Success = false;
                Program.Last.Error = ex.Message;
                Program.inAir = false;
                return;
            }
            var ofrs = new List<Offer>();

            try
            {
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
            }
            catch (Exception ex)
            {
                logger.Error("offers " + ex.Message);
                Program.Last.Success = false;
                Program.Last.Error = ex.Message;
                Program.inAir = false;
                return;

            }



            try
            {
                using (var db = new UserContext())
                {
                    var l = db.LinkModels.ToList();
                    Program.link_ozon_full = l.FirstOrDefault(m => m.Type == "Full").Link;
                    Program.link_ozon_lite = l.FirstOrDefault(m => m.Type == "Lite").Link;
                }
            }
            catch (Exception ex)
            {
                logger.Error("from link " + ex.Message);
                Program.Last.Success = false;
                Program.Last.Error = ex.Message;
                Program.inAir = false;
                return;


            }

            try
            {
                var whs = new List<WarehouseModel>();
                using (var db = new UserContext())
                {
                    whs = db.Warehouses.ToList();
                }

                foreach (var item in whs)
                {
                    var setts = new List<WarehouseSetting>();
                    using (var db = new UserContext())
                    {
                        setts = db.WarehouseSettings.Where(m => m.WarehouseId == item.Id).ToList();
                    }

                    if (setts.Count == 0)
                    {
                        continue;
                    }

                    if (setts.First().Filter == "DELETED")
                    {
                        using (var db = new UserContext())
                        {
                            if (setts.ToList().Count == 3)
                            {
                                db.Warehouses.Remove(item);
                                db.WarehouseSettings.RemoveRange(setts);
                                Program.warehouses.Remove(Program.warehouses.FirstOrDefault(m => m.Name == item.Name));
                            }
                            else
                            {
                                db.WarehouseSettings.Add(new WarehouseSetting()
                                {
                                    WarehouseId = db.Warehouses.FirstOrDefault(m => m.Name == item.Name).Id,
                                    Filter = "DELETED"
                                });
                            }
                        }
                    }
                }


            }
            catch (Exception ex)
            {

                logger.Error("from warehouse " + ex.Message);
                Program.Last.Success = false;
                Program.Last.Error = ex.Message;
                Program.inAir = false;
                return;

            }


            var list = new List<WarehouseModel>();
            using (var db = new UserContext())
            {
                list = db.Warehouses.ToList();

            }

            logger.Info("remove old warehouses");

            Program.warehouses = new List<Warehouse>();

            foreach (var item in list)
            {

                var n = new Warehouse();
                n.Name = item.Name;
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

                Program.warehouses.Add(n);

                logger.Info("add new warehouse: " + item.Name + " cond.count: " + Program.warehouses.FirstOrDefault(m => m.Name == item.Name).Condition.Count);

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

            try
            {
                var text = File.ReadAllText("wwwroot" + Program.link_ozon_full);


                XmlSerializer serializer2 = new XmlSerializer(typeof(YmlCatalog2));
                YmlCatalog2 catalog3 = new YmlCatalog2();

                using (StringReader reader = new StringReader(text))
                {
                    var text2 = serializer2.Deserialize(reader);
                    catalog3 = (YmlCatalog2)text2;
                }

                logger.Info($"zaro list.count " + catalog3.Shop.Offers.Offer.Count);

                Program.list = catalog3.Shop.Offers.Offer;
            }
            catch (Exception ex5)
            {
                logger.Error(ex5.Message);
            }
            GC.Collect();
            GC.WaitForPendingFinalizers();

            logger.Info($"progm list "  + Program.list.Count);

            Program.Last.Date = DateTime.Now;
            Program.inAir = false;

        }

    }
}
