using System.Data;
using System.Text.RegularExpressions;
using System;
using System.Linq;
using System.Xml;
using System.Text;
using System.Globalization;
using NLog;
using System.Collections.Generic;
using Microsoft.AspNetCore.WebUtilities;
using BartMarket.Quartz;
using System.IO;
using System.Threading;

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
            var raw = item.Param.FirstOrDefault(m => m.Name == "Коробка вес кг");

            if(raw == null)
            {
                raw = item.Param.FirstOrDefault(m => m.Name == "Коробка вес гр");
                if(raw == null)
                {
                    //logger.Warn("No weight ITEM-" + item.Name);
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

            //logger.Info("final w" + weight);
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
            int y = catalog.Shop.Offers.Offer.Count;
            Program.Last.Count = catalog.Shop.Offers.Offer.Count;
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

                var price = CreateAndSetElement(docNew, "price", QuartzService.MakePrice(Convert.ToInt32(CalculatePrice(Convert.ToInt32(mainPrice), 0)).ToString()).ToString());

                var name = CreateAndSetElement(docNew, "name", item.Name);
                
                var nameback = CreateAndSetElement(docNew, "name_back", Reverse(item.Name));

                var oldPrice = CreateAndSetElement(docNew, "oldprice", QuartzService.MakePrice(Convert.ToInt32(CalculatePrice(Convert.ToInt32(mainPrice), 1)).ToString()).ToString());

                var minPrice = CreateAndSetElement(docNew, "min_price", QuartzService.MakePrice(Convert.ToInt32(CalculatePrice(Convert.ToInt32(mainPrice), 2)).ToString()).ToString());

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


                var weight = CheckWeight(item);

                foreach (var ware in Program.warehouses)
                {
                    var outlet = docNew.CreateElement("outlet");

                    var instInt = GetInst(catalog2.Shop.Offers.Offer, item.Id).ToString();

                    if(ware.Condition == null || ware.Condition.Count == 0)
                    {
                        var instock = CreateAndSetAttr(docNew, "instock", instInt);
                        outlet.Attributes.Append(instock);
                    }
                    else
                    {
                        List<bool> bools = new List<bool>();


                        foreach (var cond in ware.Condition)
                        {
                            bool d = false;
                            if (cond.Contains("weight"))
                            {
                                d = (bool)new DataTable().Compute(cond.Replace("weight", weight.ToString()), null);
                                

                            }
                            else if (cond.Contains("price"))
                            {
                                d = (bool)new DataTable().Compute(cond.Replace("price", mainPrice.ToString()), null);

                            }
                            if(d == true)
                            {
                                bools.Add(true);
                            }
                            else
                            {
                                bools.Add(false);

                            }

                        }
                        if(bools.Where(m=>m == true).ToList().Count == ware.Condition.Count)
                        {
                            var instock = CreateAndSetAttr(docNew, "instock", instInt);
                            outlet.Attributes.Append(instock);
                        }
                        else
                        {
                            var instock = CreateAndSetAttr(docNew, "instock", "0");
                            outlet.Attributes.Append(instock);
                        }
                    }
                   
                    var w_name = CreateAndSetAttr(docNew, "warehouse_name", ware.Name);
                    outlet.Attributes.Append(w_name);
                    outlets.AppendChild(outlet);
                }

               // var warehouse_name = CreateAndSetAttr(docNew, "warehouse_name", "DPN");
              


                //if (weight < 30.0 && Convert.ToInt32(item.Price) > 3000 && Convert.ToInt32(item.Price) < 50000)
                //{
                    //var outlet2 = docNew.CreateElement("outlet");
                    //var instock2 = CreateAndSetAttr(docNew, "instock", instInt);
                    //outlet2.Attributes.Append(instock2);


                    //var warehouse_name2 = CreateAndSetAttr(docNew, "warehouse_name", "DPN2");
                    //outlet2.Attributes.Append(warehouse_name2);
                    //outlets.AppendChild(outlet2);

                //}

                //var outlet3 = docNew.CreateElement("outlet");

                //var instock3 = CreateAndSetAttr(docNew, "instock", instInt);
                //outlet3.Attributes.Append(instock3);

                //if(weight != 0.0)
                //{
                //    var warehouse_name3 = CreateAndSetAttr(docNew, "warehouse_name", "DPN3");
                //    outlet3.Attributes.Append(warehouse_name3);
                //    outlets.AppendChild(outlet3);
                //}

                offer.AppendChild(outlets);
                offers.AppendChild(offer);
                if(x%10000 == 0)
                {
                    logger.Info($"({x}/{y})");
                }
                x++;
            }

            if(type == "full")
            {
                
                var _1 = Program.link_ozon_full.TrimStart('/').Split("/")[0];
                if (_1.Contains(".xml"))
                {
                    docNew.Save("wwwroot" + Program.link_ozon_full);
                }
                else
                {
                    string path = "wwwroot/" + _1;
                    string subpath = "";
                    for (int i = 1; i < Program.link_ozon_full.TrimStart('/').Split("/").Length; i++)
                    {
                        if (Program.link_ozon_full.TrimStart('/').Split("/")[i].Contains(".xml"))
                        {
                            break;
                        }
                        subpath += Program.link_ozon_full.TrimStart('/').Split("/")[i] + "/";
                        
                    }
                    subpath.TrimEnd('/');

                    if (!Directory.Exists(path))
                    {
                        Directory.CreateDirectory(path);
                    }
                    Directory.CreateDirectory($"{path}/{subpath}");
                    try
                    {
                        docNew.Save("wwwroot" + Program.link_ozon_full);


                    }
                    catch (Exception ex)
                    {
                        logger.Error("from full:" + ex.Message);
                        Program.Last.Success = false;
                        Program.Last.Error = ex.Message;
                        return;

                    }
                }
         
            }
            else
            {
                var _1 = Program.link_ozon_lite.TrimStart('/').Split("/")[0];
                if (_1.Contains(".xml"))
                {
                    docNew.Save("wwwroot" + Program.link_ozon_lite);
                }
                else
                {
                    string path = "wwwroot/" + _1;
                    string subpath = "";
                    for (int i = 1; i < Program.link_ozon_lite.TrimStart('/').Split("/").Length; i++)
                    {
                        if (Program.link_ozon_lite.TrimStart('/').Split("/")[i].Contains(".xml"))
                        {
                            break;
                        }
                        subpath += Program.link_ozon_lite.TrimStart('/').Split("/")[i] + "/";

                    }
                    subpath.TrimEnd('/');

                    if (!Directory.Exists(path))
                    {
                        Directory.CreateDirectory(path);
                    }
                    Directory.CreateDirectory($"{path}/{subpath}");
                    try
                    {
                        docNew.Save("wwwroot" + Program.link_ozon_lite);


                    }
                    catch (Exception ex)
                    {
                        logger.Error("from full:" + ex.Message);
                        Program.Last.Success = false;
                        Program.Last.Error = ex.Message;
                        return;
                    }
                }
            }
        }
    }
}
