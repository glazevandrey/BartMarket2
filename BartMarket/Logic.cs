using System.Data;
using System.Text.RegularExpressions;
using System;
using System.Linq;
using System.Xml;
using System.Text;
using System.Globalization;
using NLog;
using System.Collections.Generic;

namespace BartMarket
{
    public class Logic
    {
        public static Logger logger = LogManager.GetCurrentClassLogger();

        public static double CalculatePrice(int x, int type)
        {
            switch (type)
            {
                case 0:
                    return Convert.ToDouble(new DataTable().Compute(Program.formula1.Replace("x", x.ToString()), null));
                case 1:
                    return Convert.ToDouble(new DataTable().Compute(Program.formula2.Replace("x", x.ToString()), null));
                case 2:
                    return Convert.ToDouble(new DataTable().Compute(Program.formula3.Replace("x", x.ToString()), null));
                default:
                    return 0;
            }
        }
        public static string Reverse(string text)
        {
            string res = text;

            var regex1 = new Regex(@"([a-zA-Z0-9]+\s*\/*)+");
            Match m1 = regex1.Match(text);

            if (m1.Groups.Count == 0 || m1.Groups[0].Success == false)
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
        public static bool CheckBrand(Offer item)
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

            if (item.Param.FirstOrDefault(m => m.Name == "Производитель") != null)
            {
                var brand = item.Param.FirstOrDefault(m => m.Name == "Производитель");
                if (brand.Text == "Reccagni Angelo" || brand.Text == "Reccagni Angelo" || brand.Text == "Brillica")
                {
                    return false;
                }
            }

            return true;
        }

        public static int CheckMainPrise(Offer item)
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

        public static XmlElement CreateAndSetElement(XmlDocument docNew, string name, string text)
        {
            var el = docNew.CreateElement(name);
            var el_node = docNew.CreateTextNode(text);
            el.AppendChild(el_node);
            return el;
        }
        public static XmlElement CreateAndSetElementParam(XmlDocument docNew, string name, string text)
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
        public static double CheckWeight(Offer item)
        {
            //Encoding iso = Encoding.GetEncoding("ISO-8859-1");
            //Encoding utf8 = Encoding.UTF8;
            //byte[] utfBytes = iso.GetBytes("Коробка вес кг");
            //byte[] isoBytes = Encoding.Convert(iso, utf8, utfBytes);
            //string msg = iso.GetString(isoBytes);

            var raw = item.Param.FirstOrDefault(m => m.Name == "Коробка вес кг");

            if(raw == null)
            {
                raw = item.Param.FirstOrDefault(m => m.Name == "Коробка вес гр");
                if(raw == null)
                {
                    logger.Warn("No weight ITEM-" + item.Name);
                    return 0.0;
                }

            }
          
            double weight = 0.0;

            try
            {            
                if (raw.Name.Contains("гр"))
                {
                    var r = Convert.ToInt32(raw.Text);
                    weight = Convert.ToDouble(r/1000);
                }
                else if (raw.Name.Contains("кг"))
                {
                    weight = Convert.ToDouble(raw.Text);
                }
                else
                {
                    return weight;
                }
            }
            catch (Exception ex)
            {
                logger.Error(ex.Message);
            }

            logger.Info("final w" + weight);
            return weight;
        }
        public static int GetInst(List<Offer> item, int id)
        {
            var inst = item.FirstOrDefault(m => m.Id == id);
            int instInt = 0;
            if (inst != null)
            {
                instInt = Convert.ToInt32(inst.Quanity);
            }
            return instInt;
        }
        public static XmlAttribute CreateAndSetAttr(XmlDocument docNew, string name, string text)
        {
            var inst = docNew.CreateAttribute(name);
            var insnode = docNew.CreateTextNode(text);
            inst.AppendChild(insnode);

            return inst;
        }
        public static void StartParse(YmlCatalog catalog, YmlCatalog catalog2, XmlDocument docNew, XmlElement offers, string type)
        {
            int x = 1;
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

                offer.AppendChild(outlets);
                offers.AppendChild(offer);
                logger.Info("end offer: " + item.Name + $"({x}/{catalog.Shop.Offers.Offer.Count})");
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
