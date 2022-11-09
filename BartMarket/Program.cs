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
        public static double CalculatePrice(int x, int type)
        {
            switch (type)
            {
                case 0:
                    return Convert.ToDouble(new DataTable().Compute(formula1.Replace("x", x.ToString()), null));
                case 1:
                    return Convert.ToDouble(new DataTable().Compute(formula2.Replace("x", x.ToString()), null));
                case 2:
                    return Convert.ToDouble(new DataTable().Compute(formula3.Replace("x", x.ToString()), null));
                default:
                    return 0;
            }
        }
        public static string Reverse(string text)
        {
            string res = text;

            var regex1 = new Regex(@"([a-zA-Z0-9]+\s*\/*)+");
            Match m1 = regex1.Match(text);

            if(m1.Groups.Count == 0 || m1.Groups[0].Success == false)
            {
                return text;
            }
            var item = m1.Groups[0];
                var old = item.Value;

                var chars = old.ToCharArray();
                Array.Reverse(chars);

                res = res.Replace(old, new string(chars));
                
            
            return res;
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
                logger.Info(File.ReadAllText("exmp3.xml"));

            }
            else
            {
                logger.Error("No acceess");
            }
            var ofrs = new List<Offer>();
            foreach (var item in catalog.Shop.Offers.Offer)
            {
                if(item.Price == null)
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
            var shop =  docNew.CreateElement("shop");
            var offers = docNew.CreateElement("offers");
            newRoot.AppendChild(shop);
            shop.AppendChild(offers);
            

            StartParse(catalog, catalog2, docNew, offers, "light");
            docNew = new XmlDocument();
            newRoot = docNew.CreateElement("yml_catalog");
            docNew.AppendChild(newRoot);
            shop = docNew.CreateElement("shop");
            offers = docNew.CreateElement("offers");
            newRoot.AppendChild(shop);
            shop.AppendChild(offers);

            StartParse(catalog, catalog2, docNew, offers, "full");


        }
        private static bool CheckBrand(Offer item)
        {
            if (item.Param.FirstOrDefault(m => m.Name.Contains("Brillica")) != null ||
                   item.Param.FirstOrDefault(m => m.Name.Contains("DesignLed")) != null ||
                   item.Param.FirstOrDefault(m => m.Name.Contains("Reccagni Angelo")) != null)

            {
                return false;
            }

            if (item.Param.FirstOrDefault(m => m.Name == "Бренд") != null)
            {
                var brand = item.Param.FirstOrDefault(m => m.Name == "Бренд");
                if (brand.Text == "Reccagni Angelo" || brand.Text == "Reccagni Angelo" || brand.Text == "Brillica")
                {
                    return false;
                }
            }
            
            return true;
        }

        private static int CheckMainPrise(Offer item)
        {
            var mainPrice = 0;

            if (item.OldPrice != null)
            {
                mainPrice = Convert.ToInt32(item.OldPrice);
            }
            else
            {
                mainPrice = Convert.ToInt32(item.Price);
            }
            return mainPrice;
        }

        private static XmlElement CreateAndSetElement(XmlDocument docNew, string name, string text)
        {
            var el = docNew.CreateElement(name);
            var el_node = docNew.CreateTextNode(text);
            el.AppendChild(el_node);
            return el;
        }
        private static XmlElement CreateAndSetElementParam(XmlDocument docNew, string name, string text)
        {
            var el = docNew.CreateElement("param");
            var atr = docNew.CreateAttribute("name");
            var node2 = docNew.CreateTextNode(name);
            atr.AppendChild(node2);

            el.Attributes.Append(atr);
            var el_node = docNew.CreateTextNode(text);
            el.AppendChild(el_node);
            return el;
        }
        private static double CheckWeight(Offer item)
        {
            double weight = 0.0;
            var f = CultureInfo.CurrentCulture;
            logger.Info(f);
            try
            {
                //var raw = item.Param.FirstOrDefault(m => m.Name.Trim() == "Коробка вес кг");
                Param raw = new Param();

                foreach (var item2 in item.Param)
                {
                    if(item2.Name.Equals("Коробка вес кг", StringComparison.OrdinalIgnoreCase))
                    {   
                        raw = item2;
                        logger.Info("find weigh! = " + item2.Text);
                        break;  
                    }
                    if(item2.Text == "0.578")
                    {
                        logger.Info("FIND 0 578 " + item2.Name);
                    }

                    logger.Info($"{item2.Name} : {item2.Text}");
                }
           
                if (raw != null && raw.Name != "" && raw.Name != null)
                {
                    logger.Info(raw.Text);
                    weight = Convert.ToDouble(raw.Text.Replace(".", ","));
                    if (weight == 0.0)
                    {
                        weight = Convert.ToDouble(raw.Text);

                    }
                }
            }
            catch (Exception ex)
            {

                logger.Error(ex.Message);
            }
          

            logger.Info(weight);
            return weight;
        }
        private static int GetInst(List<Offer> item, int id)
        {
            var inst = item.FirstOrDefault(m => m.Id == id);
            int instInt = 0;
            if (inst != null)
            {
                instInt = Convert.ToInt32(inst.Quanity);
            }
            return instInt;
        }
        private static XmlAttribute CreateAndSetAttr(XmlDocument docNew, string name, string text)
        {
            var inst = docNew.CreateAttribute(name);
            var insnode = docNew.CreateTextNode(text);
            inst.AppendChild(insnode);

            return inst;
        }
        private static void StartParse(YmlCatalog catalog, YmlCatalog catalog2, XmlDocument docNew, XmlElement offers, string type)
        {
            foreach (var item in catalog.Shop.Offers.Offer)
            {
                if(CheckBrand(item) == false)
                {
                    continue;
                }

                var mainPrice = CheckMainPrise(item);

                var offer = docNew.CreateElement("offer");
                XmlAttribute idAttr = docNew.CreateAttribute("id");
                offer.Attributes.Append(idAttr);
                offer.Attributes.Item(0).Value = item.Id.ToString() + "_DPN";

                var price = CreateAndSetElement(docNew, "price", Convert.ToInt32(CalculatePrice(Convert.ToInt32(mainPrice), 0)).ToString());

                var name = CreateAndSetElement(docNew, "name", item.Name);
                
                var nameback = CreateAndSetElement(docNew, "name_back", Reverse(item.Name));

                var oldPrice = CreateAndSetElement(docNew, "oldprice", Convert.ToInt32(CalculatePrice(Convert.ToInt32(mainPrice), 1)).ToString());

                var minPrice = CreateAndSetElement(docNew, "min_price", Convert.ToInt32(CalculatePrice(Convert.ToInt32(mainPrice), 2)).ToString());

                var formula = CreateAndSetElement(docNew, "formula", $"{Program.formula1.Replace("x", mainPrice.ToString())};{Program.formula2.Replace("x", mainPrice.ToString())};{Program.formula3.Replace("x", mainPrice.ToString())};");

                if (type == "full")
                {
                    foreach (var pm in item.Param)
                    {
                        var node = CreateAndSetElementParam(docNew, pm.Name, pm.Text);
                        offer.AppendChild(node);
                    }
                }

                offer.AppendChild(name);
                offer.AppendChild(nameback);
                offer.AppendChild(price);
                offer.AppendChild(oldPrice);
                offer.AppendChild(minPrice);
                offer.AppendChild(formula);

                var outlets = docNew.CreateElement("outlets");
                var outlet = docNew.CreateElement("outlet");

                var instInt = GetInst(catalog2.Shop.Offers.Offer, item.Id).ToString();
                var instock = CreateAndSetAttr(docNew, "instock", instInt);

                outlet.Attributes.Append(instock);


                var warehouse_name = CreateAndSetAttr(docNew, "warehouse_name", "DPN");
                outlet.Attributes.Append(warehouse_name);
                outlets.AppendChild(outlet);

                var weight = CheckWeight(item);

                if (weight < 30.0 && Convert.ToInt32(item.Price) > 3000 && Convert.ToInt32(item.Price) < 50000)
                {
                    var outlet2 = docNew.CreateElement("outlet");
                    var instock2 = CreateAndSetAttr(docNew, "instock", instInt);
                    outlet2.Attributes.Append(instock2);


                    var warehouse_name2 = CreateAndSetAttr(docNew, "warehouse_name", "DPN2");
                    outlet2.Attributes.Append(warehouse_name2);
                    outlets.AppendChild(outlet2);

                }

                var outlet3 = docNew.CreateElement("outlet");

                var instock3 = CreateAndSetAttr(docNew, "instock", instInt);
                outlet3.Attributes.Append(instock3);

                if(weight != 0.0)
                {
                    var warehouse_name3 = CreateAndSetAttr(docNew, "warehouse_name", "DPN3");
                    outlet3.Attributes.Append(warehouse_name3);
                    outlets.AppendChild(outlet3);
                }
                logger.Info(weight);

                offer.AppendChild(outlets);
                offers.AppendChild(offer);
            }

            if(type == "full")
            {
                docNew.Save("wwwroot/content/fullozon.xml");
            }
            else
            {
                docNew.Save("wwwroot/content/liteozon.xml");
            }
        }
    }
}
