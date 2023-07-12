using BartMarket.Data;
using Microsoft.AspNetCore.Mvc;
using NLog;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading;
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
            await StartDonplafon();

            Thread.Sleep(10000);
            GC.Collect();
            GC.WaitForPendingFinalizers();
            Thread.Sleep(10000);

            await StartArnika();

            Thread.Sleep(10000);
            GC.Collect();
            GC.WaitForPendingFinalizers();
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
                    try
                    {
                        x2 = Convert.ToInt32(new string(hh));

                    }
                    catch (Exception exx)
                    {
                        throw exx;
                    }

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
        public async static Task StartDonplafon()
        {
            if (Program.ExcelAir)
            {
                logger.Error("EXCEL IN AIR");
                return;
            }

            XmlSerializer serializer = new XmlSerializer(typeof(YmlCatalog));
            YmlCatalog catalog = new YmlCatalog();

            try
            {
                Program.inAir = true;
                Program.Last["donplafon"].Success = true;
                if(Program.list != null)
                {
                    Program.list.Clear();
                    Program.list = null;
                }

                GC.Collect();
                GC.WaitForPendingFinalizers();

                if (File.Exists($"{Environment.CurrentDirectory}/wwwroot/content/exmp2.xml"))
                {
                    if (File.Exists($"{Environment.CurrentDirectory}/wwwroot/content/exmp2_old.xml"))
                    {
                        File.Delete($"{Environment.CurrentDirectory}/wwwroot/content/exmp2_old.xml");
                    }

                    System.IO.File.Move($"{Environment.CurrentDirectory}/wwwroot/content/exmp2.xml", $"{Environment.CurrentDirectory}/wwwroot/content/exmp2_old.xml");

                    if (File.Exists($"{Environment.CurrentDirectory}/wwwroot/content/exmp2.xml"))
                    {
                        File.Delete($"{Environment.CurrentDirectory}/wwwroot/content/exmp2.xml");
                    }
                }
                if (File.Exists($"{Environment.CurrentDirectory}/wwwroot/content/exmp3.xml"))
                {
                    File.Delete($"{Environment.CurrentDirectory}/wwwroot/content/exmp3.xml");
                }
            }
            catch (Exception ex)
            {
                logger.Error("from litle " + ex.Message);
                Program.Last["donplafon"].Success = false;
                Program.Last["donplafon"].Error = ex.Message;
                Program.inAir = false;
                return;
            }
         
            
            try
            {
                using (var client = new HttpClient())
                {
                    using (var s = client.GetStreamAsync("https://partners.donplafon.ru/local/partners/BARTMARKET_XML_CONTENT/"))
                    {
                        try
                        {
                            using (var fs = new FileStream($"{Environment.CurrentDirectory}/wwwroot/content/exmp2.xml", FileMode.OpenOrCreate))
                            {
                                s.Result.CopyTo(fs);
                                logger.Info("success");

                            }
                        }
                        catch (Exception ex)
                        {
                            logger.Error("from download inside " + ex.Message);
                            Program.Last["donplafon"].Success = false;
                            Program.Last["donplafon"].Error = ex.Message;
                            Program.inAir = false;

                            return;
                        }
                       
                    }
                }

                using (var client = new HttpClient())
                {
                    using (var s = client.GetStreamAsync("https://partners.donplafon.ru/local/partners/BARTMARKET_XML_PRICES/"))
                    {
                        try
                        {
                            using (var fs = new FileStream($"{Environment.CurrentDirectory}/wwwroot/content/exmp3.xml", FileMode.OpenOrCreate))
                            {
                                s.Result.CopyTo(fs);
                                logger.Info("success");

                            }
                        }
                        catch (Exception ex)
                        {
                            logger.Error("from download inside " + ex.Message);
                            Program.Last["donplafon"].Success = false;
                            Program.Last["donplafon"].Error = ex.Message;
                            Program.inAir = false;

                            return;
                        }
                    
                    }
                }

            }
            catch (Exception ex)
            {
                logger.Error("from download " + ex.Message);
                Program.Last["donplafon"].Success = false;
                Program.Last["donplafon"].Error = ex.Message;
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
                Program.Last["donplafon"].Success = false;
                Program.Last["donplafon"].Error = ex.Message;
                Program.inAir = false;
                return;
            }
            var ofrs = new List<Offer>();

            logger.Info("DATE OF NEW ONLINE 1 FID IS: " + catalog.Date);
            logger.Info("DATE OF NEW ONLINE  2 FID IS: " + catalog2.Date);
            

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
                Program.Last["donplafon"].Success = false;
                Program.Last["donplafon"].Error = ex.Message;
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
                Program.Last["donplafon"].Success = false;
                Program.Last["donplafon"].Error = ex.Message;
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
                Program.Last["donplafon"].Success = false;
                Program.Last["donplafon"].Error = ex.Message;
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
            

            GC.Collect();
            GC.WaitForPendingFinalizers();

            XmlDocument docNew = new XmlDocument();
            XmlElement newRoot = docNew.CreateElement("yml_catalog");


            var attr = docNew.CreateAttribute("date");
            var date = docNew.CreateTextNode(catalog.Date);
            attr.AppendChild(date);

            newRoot.Attributes.Append(attr);
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

            Program.Last["donplafon"].ElapsedLite = elapsedTime;
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

            Program.Last["donplafon"].ElapsedFull = elapsedTime;

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


                Program.list = catalog3.Shop.Offers.Offer;
            }
            catch (Exception ex5)
            {
                logger.Error(ex5.Message);
            }
            GC.Collect();
            GC.WaitForPendingFinalizers();

            logger.Info($"progm list "  + Program.list.Count);

            Program.Last["donplafon"].Date = DateTime.Now;
            Program.inAir = false;

        }
        public async static Task StartArnika()
        {
            if (Program.ExcelAir)
            {
                logger.Error("EXCEL IN AIR");
                return;
            }

            XmlSerializer serializer = new XmlSerializer(typeof(Export));
            Export catalog = new Export();

            try
            {
                Program.inAir = true;
                Program.Last["arnika"].Success = true;
                if (Program.list != null)
                {
                    Program.list.Clear();
                    Program.list = null;
                }

                GC.Collect();
                GC.WaitForPendingFinalizers();

                if (File.Exists($"{Environment.CurrentDirectory}/wwwroot/content/arnikafid.xml"))
                {
                    if (File.Exists($"{Environment.CurrentDirectory}/wwwroot/content/arnikafid_old.xml"))
                    {
                        File.Delete($"{Environment.CurrentDirectory}/wwwroot/content/arnikafid_old.xml");
                    }

                    System.IO.File.Move($"{Environment.CurrentDirectory}/wwwroot/content/arnikafid.xml", $"{Environment.CurrentDirectory}/wwwroot/content/arnikafid_old.xml");

                    if (File.Exists($"{Environment.CurrentDirectory}/wwwroot/content/arnikafid.xml"))
                    {
                        File.Delete($"{Environment.CurrentDirectory}/wwwroot/content/arnikafid.xml");
                    }
                }
    
            }
            catch (Exception ex)
            {
                logger.Error("from litle " + ex.Message);
                Program.Last["arnika"].Success = false;
                Program.Last["arnika"].Error = ex.Message;
                Program.inAir = false;
                return;
            }


            try
            {
                using (var client = new HttpClient())
                {
                    using (var s = client.GetStreamAsync("https://td-arnika.ru/upload/acrit.exportproplus/arnika_agent.xml?1678614659"))
                    {
                        try
                        {
                            using (var fs = new FileStream($"{Environment.CurrentDirectory}/wwwroot/content/arnikafid.xml", FileMode.OpenOrCreate))
                            {
                                s.Result.CopyTo(fs);
                                logger.Info("success");

                            }
                        }
                        catch (Exception ex)
                        {
                            logger.Error("from download inside " + ex.Message);
                            Program.Last["arnika"].Success = false;
                            Program.Last["arnika"].Error = ex.Message;
                            Program.inAir = false;

                            return;
                        }

                    }
                }
            }
            catch (Exception ex)
            {
                logger.Error("from download " + ex.Message);
                Program.Last["arnika"].Success = false;
                Program.Last["arnika"].Error = ex.Message;
                Program.inAir = false;

                return;
            }

            Export catalog2 = new Export();


            try
            {
                var text = File.ReadAllText($"{Environment.CurrentDirectory}/wwwroot/content/arnikafid.xml");
                using (StringReader reader = new StringReader(text))
                {
                    var text2 = serializer.Deserialize(reader);
                    catalog = (Export)text2;
                }
            }
            catch (Exception ex)
            {
                logger.Error("from upload to disk " + ex.Message);
                Program.Last["arnika"].Success = false;
                Program.Last["arnika"].Error = ex.Message;
                Program.inAir = false;
                return;
            }
            var ofrs = new List<OfferArnika>();

      
            try
            {
                foreach (var item in catalog.Offers.Offer)
                {
                    if (item.Price == 0)
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
                Program.Last["arnika"].Success = false;
                Program.Last["arnika"].Error = ex.Message;
                Program.inAir = false;
                return;
            }

            catalog.Offers.Offer = ofrs;


            GC.Collect();
            GC.WaitForPendingFinalizers();

            XmlDocument docNew = new XmlDocument();
            XmlElement newRoot = docNew.CreateElement("yml_catalog");

            var attr = docNew.CreateAttribute("date");
            var date = docNew.CreateTextNode(catalog.Date);
            attr.AppendChild(date);

            newRoot.Attributes.Append(attr);
            docNew.AppendChild(newRoot);


            var cat = docNew.CreateElement("categories");

            foreach (var item in catalog.Categories.Category)
            {
                var catt = docNew.CreateElement("category");
                catt.InnerText = item.Text;
                var attrCatt = docNew.CreateAttribute("id");
                var text2 = docNew.CreateTextNode(item.Id.ToString());
                attrCatt.AppendChild(text2);

                if(item.ParentId != 0)
                {
                    var attrCatt2 = docNew.CreateAttribute("parentId");
                    var text3 = docNew.CreateTextNode(item.ParentId.ToString());
                    attrCatt2.AppendChild(text3);
                    catt.Attributes.Append(attrCatt2);

                }


                catt.Attributes.Append(attrCatt);

                cat.AppendChild(catt);

            }
            newRoot.AppendChild(cat);

            var shop = docNew.CreateElement("shop");
            var offers = docNew.CreateElement("offers");
            newRoot.AppendChild(shop);
            shop.AppendChild(offers);
            var startTime = System.Diagnostics.Stopwatch.StartNew();



            try
            {
                Logic.StartParse(catalog, docNew, offers, "lite");

            }
            catch (Exception ex)
            {
                throw ex;
            }


            startTime.Stop();
            var resultTime = startTime.Elapsed;
            string elapsedTime = String.Format("{0:00}:{1:00}:{2:00}.{3:000}",
    resultTime.Hours,
    resultTime.Minutes,
    resultTime.Seconds,
    resultTime.Milliseconds);

            Program.Last["arnika"].ElapsedLite = elapsedTime;
            logger.Info("-----SUCCESS ENDED LITE FORMATING FEED-----");
            logger.Info($"-----ELLAPSED: {elapsedTime}-----");
            GC.Collect();
            GC.WaitForPendingFinalizers();

            docNew = new XmlDocument();
            newRoot = docNew.CreateElement("yml_catalog");
            attr = docNew.CreateAttribute("date");
            date = docNew.CreateTextNode(catalog.Date);
            attr.AppendChild(date);

            newRoot.Attributes.Append(attr);
            docNew.AppendChild(newRoot);


            cat = docNew.CreateElement("categories");

            foreach (var item in catalog.Categories.Category)
            {
                var catt = docNew.CreateElement("category");
                catt.InnerText = item.Text;
                var attrCatt = docNew.CreateAttribute("id");
                var text2 = docNew.CreateTextNode(item.Id.ToString());
                attrCatt.AppendChild(text2);

                if (item.ParentId != 0)
                {
                    var attrCatt2 = docNew.CreateAttribute("parentId");
                    var text3 = docNew.CreateTextNode(item.ParentId.ToString());
                    attrCatt2.AppendChild(text3);
                    catt.Attributes.Append(attrCatt2);

                }


                catt.Attributes.Append(attrCatt);

                cat.AppendChild(catt);

            }
            newRoot.AppendChild(cat);

            docNew.AppendChild(newRoot);
            shop = docNew.CreateElement("shop");
            offers = docNew.CreateElement("offers");
            newRoot.AppendChild(shop);
            shop.AppendChild(offers);

            startTime = System.Diagnostics.Stopwatch.StartNew();
            Logic.StartParse(catalog, docNew, offers, "lite1");
            startTime.Stop();
            resultTime = startTime.Elapsed;
            elapsedTime = String.Format("{0:00}:{1:00}:{2:00}.{3:000}",
   resultTime.Hours,
   resultTime.Minutes,
   resultTime.Seconds,
   resultTime.Milliseconds);

            Program.Last["arnika"].ElapsedFull = elapsedTime;

            logger.Info("-----SUCCESS ENDED LITE1 FORMATING FEED-----");
            logger.Info($"-----ELLAPSED: {elapsedTime}-----");

            GC.Collect();
            GC.WaitForPendingFinalizers();








            docNew = new XmlDocument();
            newRoot = docNew.CreateElement("yml_catalog");
            attr = docNew.CreateAttribute("date");
            date = docNew.CreateTextNode(catalog.Date);
            attr.AppendChild(date);

            newRoot.Attributes.Append(attr);
            docNew.AppendChild(newRoot);


             cat = docNew.CreateElement("categories");

            foreach (var item in catalog.Categories.Category)
            {
                var catt = docNew.CreateElement("category");
                catt.InnerText = item.Text;
                var attrCatt = docNew.CreateAttribute("id");
                var text2 = docNew.CreateTextNode(item.Id.ToString());
                attrCatt.AppendChild(text2);

                if (item.ParentId != 0)
                {
                    var attrCatt2 = docNew.CreateAttribute("parentId");
                    var text3 = docNew.CreateTextNode(item.ParentId.ToString());
                    attrCatt2.AppendChild(text3);
                    catt.Attributes.Append(attrCatt2);

                }


                catt.Attributes.Append(attrCatt);

                cat.AppendChild(catt);

            }
            newRoot.AppendChild(cat);

            docNew.AppendChild(newRoot);
            shop = docNew.CreateElement("shop");
            offers = docNew.CreateElement("offers");
            newRoot.AppendChild(shop);
            shop.AppendChild(offers);

            startTime = System.Diagnostics.Stopwatch.StartNew();
            Logic.StartParse(catalog, docNew, offers, "full");
            startTime.Stop();
            resultTime = startTime.Elapsed;
            elapsedTime = String.Format("{0:00}:{1:00}:{2:00}.{3:000}",
   resultTime.Hours,
   resultTime.Minutes,
   resultTime.Seconds,
   resultTime.Milliseconds);

            Program.Last["arnika"].ElapsedFull = elapsedTime;

            logger.Info("-----SUCCESS ENDED FULL FORMATING FEED-----");
            logger.Info($"-----ELLAPSED: {elapsedTime}-----");

            GC.Collect();
            GC.WaitForPendingFinalizers();



            try
            {
                var text = File.ReadAllText($"{Environment.CurrentDirectory}/wwwroot/content/arnikafid.xml");
                using (StringReader reader = new StringReader(text))
                {
                    var text2 = serializer.Deserialize(reader);
                    catalog = (Export)text2;
                }
            }
            catch (Exception ex)
            {
                logger.Error("from upload to disk " + ex.Message);
                Program.Last["arnika"].Success = false;
                Program.Last["arnika"].Error = ex.Message;
                Program.inAir = false;
                return;
            }

             docNew = new XmlDocument();
             newRoot = docNew.CreateElement("yml_catalog");


             attr = docNew.CreateAttribute("date");
             date = docNew.CreateTextNode(catalog.Date);
            attr.AppendChild(date);

            newRoot.Attributes.Append(attr);

            cat = docNew.CreateElement("categories");

            foreach (var item in catalog.Categories.Category)
            {
                var catt = docNew.CreateElement("category");
                catt.InnerText = item.Text;
                var attrCatt = docNew.CreateAttribute("id");
                var text2 = docNew.CreateTextNode(item.Id.ToString());
                attrCatt.AppendChild(text2);

                if (item.ParentId != 0)
                {
                    var attrCatt2 = docNew.CreateAttribute("parentId");
                    var text3 = docNew.CreateTextNode(item.ParentId.ToString());
                    attrCatt2.AppendChild(text3);
                    catt.Attributes.Append(attrCatt2);

                }


                catt.Attributes.Append(attrCatt);

                cat.AppendChild(catt);

            }
            newRoot.AppendChild(cat);

            docNew.AppendChild(newRoot);
             shop = docNew.CreateElement("shop");
             offers = docNew.CreateElement("offers");
            newRoot.AppendChild(shop);
            shop.AppendChild(offers);
            //docNew = new XmlDocument();
            //newRoot = docNew.CreateElement("yml_catalog");
            //attr = docNew.CreateAttribute("date");
            //date = docNew.CreateTextNode(catalog.Date);
            //attr.AppendChild(date);

            //newRoot.Attributes.Append(attr);
            //docNew.AppendChild(newRoot);

            startTime = System.Diagnostics.Stopwatch.StartNew();
            
                Logic.StartParseAli(catalog, docNew, offers, "ali");
            
            startTime.Stop();
            resultTime = startTime.Elapsed;
            elapsedTime = String.Format("{0:00}:{1:00}:{2:00}.{3:000}",
   resultTime.Hours,
   resultTime.Minutes,
   resultTime.Seconds,
   resultTime.Milliseconds);

            Program.Last["arnika"].ElapsedAli = elapsedTime;

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


                Program.list = catalog3.Shop.Offers.Offer;
            }
            catch (Exception ex5)
            {
                logger.Error(ex5.Message);
            }

            GC.Collect();
            GC.WaitForPendingFinalizers();

            Program.Last["arnika"].Date = DateTime.Now;
            Program.inAir = false;

        }

    }
}
