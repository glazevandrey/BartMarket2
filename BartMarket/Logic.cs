using BartMarket.Quartz;
using NLog;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Xml;
using System.Xml.Serialization;

namespace BartMarket
{
    public class Logic
    {
        public static Logger logger = LogManager.GetCurrentClassLogger();

        
        public static double CalculatePriceArnika(int x,  int type, bool _default)
        {
            if (_default)
            {
                switch (type)
                {
                    case 0:
                        return Convert.ToDouble(new DataTable().Compute(Program.formula1_ar.Replace("x", x.ToString()), null));
                    case 1:
                        return Convert.ToDouble(new DataTable().Compute(Program.formula2_ar.Replace("x", x.ToString()), null));
                    case 2:
                        return Convert.ToDouble(new DataTable().Compute(Program.formula3_ar.Replace("x", x.ToString()), null));
                    default:
                        return 0;
                }
            }
            else
            {
                switch (type)
                {
                    case 0:
                        return Convert.ToDouble(new DataTable().Compute(Program.formula1_ar1.Replace("x", x.ToString()), null));
                    case 1:
                        return Convert.ToDouble(new DataTable().Compute(Program.formula2_ar1.Replace("x", x.ToString()), null));
                    case 2:
                        return Convert.ToDouble(new DataTable().Compute(Program.formula3_ar1.Replace("x", x.ToString()), null));
                    default:
                        return 0;
                }
            }
          
        }
        public static double CalculatePriceDopArnika(int x, int type, bool _default )
        {
            if (_default)
            {
                switch (type)
                {
                    case 0:
                        return Convert.ToDouble(new DataTable().Compute(Program.formula1_ar_dop.Replace("x", x.ToString()), null));
                    case 1:
                        return Convert.ToDouble(new DataTable().Compute(Program.formula2_ar_dop.Replace("x", x.ToString()), null));
                    case 2:
                        return Convert.ToDouble(new DataTable().Compute(Program.formula3_ar_dop.Replace("x", x.ToString()), null));
                    default:
                        return 0;
                }
            }
            else
            {
                switch (type)
                {
                    case 0:
                        return Convert.ToDouble(new DataTable().Compute(Program.formula1_ar1_dop.Replace("x", x.ToString()), null));
                    case 1:
                        return Convert.ToDouble(new DataTable().Compute(Program.formula2_ar1_dop.Replace("x", x.ToString()), null));
                    case 2:
                        return Convert.ToDouble(new DataTable().Compute(Program.formula3_ar1_dop.Replace("x", x.ToString()), null));
                    default:
                        return 0;
                }
            }
           
        }
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

            var regex1 = new Regex(@"([a-zA-Z0-9]+\s*\/*)+", RegexOptions.CultureInvariant);
            Match m1 = regex1.Match(text);
            if (text.Contains("Wi-Fi"))
            {
                m1 = regex1.Match(text.Replace("Wi-Fi", string.Empty));
            }

            if (m1.Groups.Count == 0 || m1.Groups[0].Success == false)
            {
                return text;
            }
            var item = m1.Groups[0];
            var old = item.Value.Trim();

            var chars = old.ToCharArray();
            Array.Reverse(chars);

            res = res.Replace(old, new string(chars));

            for (int i = 0; i < 10; i++)
            {
                m1 = m1.NextMatch();
                if (!m1.Success)
                {
                    return res;
                }

                var chars2 = m1.Value.Trim().ToCharArray();
                Array.Reverse(chars2);

                res = res.Replace(m1.Value.Trim(), new string(chars2));

            }

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

        public static int CheckMainPrise(Offer item, OfferArnika item_ar)
        {
            var mainPrice = 0;
            if(item != null)
            {
                if (item.OldPrice != null)
                {
                    mainPrice = Convert.ToInt32(item.OldPrice);
                }
                else
                {
                    mainPrice = Convert.ToInt32(item.Price);
                }
            }
            else
            {
                
                if (item_ar.OldPrice != null)
                {
                    try
                    {
                        mainPrice = Convert.ToInt32(item_ar.OldPrice);

                    }
                    catch (Exception ex)
                    {
                        var gg = Convert.ToDouble(item_ar.OldPrice.Replace('.', ','));
                        mainPrice = Convert.ToInt32(gg);

                    }
                }
                else
                {
                    mainPrice = Convert.ToInt32(item_ar.Price);
                }
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
        public static double CheckWeight(Offer item, OfferArnika item_ar)
        {
            if(item_ar != null)
            {
                var raw_w = item_ar.Param.FirstOrDefault(m => m.Name == "Вес");
                if (raw_w == null)
                {
                    return 0.0;
                }
                try
                {
                    var w = Convert.ToDouble(raw_w.Text.Replace('.',','));
                    return w;
                }
                catch (Exception ex)
                {
                    var w = Convert.ToDouble(raw_w.Text);
                    return w;
                }
               

            }
            var raw = item.Param.FirstOrDefault(m => m.Name == "Коробка вес кг");

            if (raw == null)
            {
                raw = item.Param.FirstOrDefault(m => m.Name == "Коробка вес гр");
                if (raw == null)
                {
                    return 0.0;
                }

            }

            double weight = 0.0;

            try
            {
                if (raw.Name.Contains("гр"))
                {
                    try
                    {
                        var r = Convert.ToInt32(raw.Text);
                        weight = Convert.ToDouble(r / 1000);
                    }
                    catch (Exception)
                    {
                        weight = Convert.ToDouble(raw.Text) / 1000;
                    }

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
                try
                {
                    weight = Convert.ToDouble(raw.Text.Replace(".", ","));
                }
                catch (Exception)
                {
                    logger.Error(ex.Message + " : " + item.Name + " : " + raw);

                }
            }

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

            if (Program.Last.Count > catalog.Shop.Offers.Offer.Count)
            {
                try
                {
                    var split = Program.lastIds.ToString().TrimEnd(';').Split(";");
                    List<string> ids = new List<string>();
                    foreach (var item in split)
                    {
                        if (catalog.Shop.Offers.Offer.FirstOrDefault(m => m.Id.ToString() == item) != null)
                        {
                            continue;
                        }

                        ids.Add(item);
                        logger.Info("deleted id: " + item);
                    }

                    var old = new YmlCatalog();

                    var text = File.ReadAllText($"{Environment.CurrentDirectory}/wwwroot/content/exmp2_old.xml");
                    using (StringReader reader = new StringReader(text))
                    {
                        XmlSerializer serializer = new XmlSerializer(typeof(YmlCatalog));
                        var text2 = serializer.Deserialize(reader);
                        old = (YmlCatalog)text2;
                    }

                    foreach (var item in ids)
                    {
                        var curr = old.Shop.Offers.Offer.FirstOrDefault(m => m.Id.ToString() == item);
                        if (curr == null)
                        {
                            curr = Program.deleted.FirstOrDefault(m => m.Id.ToString() == item);
                            if (curr == null)
                            {
                                logger.Warn("why null " + item);
                                continue;
                            }
                        }

                        if (catalog.Shop.Offers.Offer.FirstOrDefault(m => m.Id == curr.Id) == null)
                        {
                            if (Program.deleted.FirstOrDefault(m => m.Id == curr.Id) == null)
                            {
                                Program.deleted.Add(curr);
                            }

                            catalog.Shop.Offers.Offer.Add(curr);

                        }
                    }

                    logger.Info("added to this parse : " +  ids.Count + " штук");

                    text = null;
                    old = null;
                }
                catch (Exception ex)
                {
                    logger.Error(ex.Message);

                }
                GC.Collect();
                GC.WaitForPendingFinalizers();

            }

            Program.Last["donplafon"].Count = catalog.Shop.Offers.Offer.Count;

            foreach (var item in catalog.Shop.Offers.Offer)
            {
                if (CheckBrand(item) == false)
                {
                    continue;
                }

                var mainPrice = CheckMainPrise(item, null);

                var offer = docNew.CreateElement("offer");
                XmlAttribute idAttr = docNew.CreateAttribute("id");
                offer.Attributes.Append(idAttr);
                offer.Attributes.Item(0).Value = item.Id.ToString() + "_DPN";
                if (!Program.lastIds.ToString().Contains(item.Id.ToString()))
                {
                    Program.lastIds.Append(item.Id.ToString() + ";");
                }
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

                    foreach (var pic in item.Pictures)
                    {
                        var pEl = CreateAndSetElement(docNew, "picture", pic.Trim());
                        offer.AppendChild(pEl);
                    }
                }

                offer.AppendChild(name);
                offer.AppendChild(nameback);
                offer.AppendChild(price);
                offer.AppendChild(oldPrice);
                offer.AppendChild(minPrice);
                offer.AppendChild(formula);

                var outlets = docNew.CreateElement("outlets");


                var weight = CheckWeight(item, null) ;
                try
                {

                    foreach (var ware in Program.warehouses)
                    {
                        if (!ware.Name.Contains("DPN"))
                        {
                            continue;
                        }

                        var outlet = docNew.CreateElement("outlet");

                        var instInt = GetInst(catalog2.Shop.Offers.Offer, item.Id).ToString();

                        if (ware.Condition == null || ware.Condition.Count == 0)
                        {
                            var instock = CreateAndSetAttr(docNew, "instock", instInt);
                            outlet.Attributes.Append(instock);
                        }
                        else
                        {
                            List<bool> bools = new List<bool>();

                            if (ware.Condition[0] == "DELETED")
                            {

                                foreach (var ii in ware.Condition)
                                {
                                    bools.Add(false);
                                }

                            }

                            foreach (var cond in ware.Condition)
                            {
                                bool d = false;
                                if (cond == "DELETED")
                                {
                                    break;
                                }

                                if (cond.Contains("weight"))
                                {
                                    try
                                    {
                                        d = (bool)new DataTable().Compute(cond.Replace("weight", weight.ToString()), null);

                                    }
                                    catch (Exception ex)
                                    {
                                        try
                                        {
                                            d = (bool)new DataTable().Compute(cond.Replace("weight", Convert.ToInt32(weight).ToString()), null);

                                        }
                                        catch (Exception ex2)
                                        {
                                            try
                                            {
                                                d = (bool)new DataTable().Compute(cond.Replace("weight", Convert.ToInt32(weight.ToString().Replace(".", ",")).ToString()), null);

                                            }
                                            catch (Exception ex3)
                                            {

                                                throw ex3;
                                            }
                                        }
                                    }


                                }
                                else if (cond.Contains("price"))
                                {
                                    d = (bool)new DataTable().Compute(cond.Replace("price", mainPrice.ToString()), null);

                                }
                                else if (cond.Contains("glass"))
                                {
                                    var yes = cond.Contains("yes");
                                    var no = cond.Contains("no");

                                    var material = item.Param.FirstOrDefault(m => m.Name == "Материал плафона/абажура");


                                    if (material == null)
                                    {
                                        // logger.Error(item.Name + " has zero param Material");
                                        d = true;
                                    }
                                    else
                                    {
                                        if (material.Text.ToLower().Contains("хрусталь") || material.Text.ToLower().Contains("стекло"))
                                        {
                                            if (yes)
                                            {
                                                d = true;
                                            }
                                            else
                                            {
                                                d = false;
                                            }

                                        }
                                    }
                                }
                                if (d == true)
                                {
                                    bools.Add(true);
                                }
                                else
                                {
                                    bools.Add(false);

                                }

                            }
                            if (bools.Where(m => m == true).ToList().Count == ware.Condition.Count)
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


                }
                catch (Exception ex)
                {
                    logger.Error("Неверно задан фильтр у одного из складов. Нужна проверка");
                    Program.inAir = false;
                    Program.Last["donplafon"].Success = false;
                    Program.Last["donplafon"].Error = "Неверно задан фильтр у одного из складов. Нужна проверка";
                    return;
                }
                offer.AppendChild(outlets);
                offers.AppendChild(offer);
                if (x % 10000 == 0)
                {
                    logger.Info($"({x}/{y})");
                }
                x++;

            }

            if (type == "full")
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
                        logger.Error(docNew.InnerXml);

                        Program.Last["donplafon"].Success = false;
                        Program.Last["donplafon"].Error = ex.Message;
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
                        logger.Error(docNew.InnerXml);

                        Program.Last["donplafon"].Success = false;
                        Program.Last["donplafon"].Error = ex.Message;
                        return;
                    }
                }
            }
        }

        public static void StartParse(Export export, XmlDocument docNew, XmlElement offers, string type)
        {
            int x = 1;
            int y = export.Offers.Offer.Count;

            if (Program.Last.Count > export.Offers.Offer.Count)
            {
                try
                {
                    var split = Program.lastIds.ToString().TrimEnd(';').Split(";");
                    List<string> ids = new List<string>();
                    foreach (var item in split)
                    {
                        if (export.Offers.Offer.FirstOrDefault(m => m.Id.ToString() == item) != null)
                        {
                            continue;
                        }

                        ids.Add(item);
                        logger.Info("deleted id: " + item);
                    }

                    var old = new Export();

                    var text = File.ReadAllText($"{Environment.CurrentDirectory}/wwwroot/content/arnikafid_old.xml");
                    using (StringReader reader = new StringReader(text))
                    {
                        XmlSerializer serializer = new XmlSerializer(typeof(Export));
                        var text2 = serializer.Deserialize(reader);
                        old = (Export)text2;
                    }

                    foreach (var item in ids)
                    {
                        var curr = old.Offers.Offer.FirstOrDefault(m => m.Id.ToString() == item);
                        if (curr == null)
                        {
                            curr = Program.deletedAr.FirstOrDefault(m => m.Id.ToString() == item);
                            if (curr == null)
                            {
                                logger.Warn("why null " + item);
                                continue;
                            }
                        }

                        if (export.Offers.Offer.FirstOrDefault(m => m.Id == curr.Id) == null)
                        {
                            if (Program.deletedAr.FirstOrDefault(m => m.Id == curr.Id) == null)
                            {
                                Program.deletedAr.Add(curr);
                            }

                            export.Offers.Offer.Add(curr);

                        }
                    }

                    logger.Info("added to this parse : " + ids.Count + " штук");

                    text = null;
                    old = null;
                }
                catch (Exception ex)
                {
                    logger.Error(ex.Message);

                }
                GC.Collect();
                GC.WaitForPendingFinalizers();

            }

            Program.Last["arnika"].Count = export.Offers.Offer.Count;

            foreach (var item in export.Offers.Offer)
            {
                var mainPrice = CheckMainPrise(null,item);

                var offer = docNew.CreateElement("offer");
                XmlAttribute idAttr = docNew.CreateAttribute("id");
                offer.Attributes.Append(idAttr);
                offer.Attributes.Item(0).Value = item.Id.ToString() + "_ARN";
                if (!Program.lastIds.ToString().Contains(item.Id.ToString()))
                {
                    Program.lastIds.Append(item.Id.ToString() + ";");
                }

                var name = CreateAndSetElement(docNew, "name", item.Name);

                var nameback = CreateAndSetElement(docNew, "name_back", Reverse(item.Name));

                var desk = CreateAndSetElement(docNew, "description", item.Description);
                var url = CreateAndSetElement(docNew, "url", item.Url);
                var article = CreateAndSetElement(docNew, "article", item.Article);
                var vendor = CreateAndSetElement(docNew, "vendor", item.Vendor);

                var availible = CreateAndSetElement(docNew, "availible", item.Available.ToString());

                var currency = CreateAndSetElement(docNew, "currencyId", item.CurrencyId);
                var country = CreateAndSetElement(docNew, "country_of_origin", item.CountryOfOrigin);

                var weight = CheckWeight(null, item);

                var main_cat = item.MainCategory;

                XmlElement price;
                XmlElement oldPrice;
                XmlElement minPrice;

                XmlElement formula;
                bool _default = true;
                if(type == "lite1")
                {
                    _default = false;
                }
                if(weight > 20 && (main_cat == 1644 || main_cat == 1625))
                {
                    price = CreateAndSetElement(docNew, "price", QuartzService.MakePrice(Convert.ToInt32(CalculatePriceDopArnika(Convert.ToInt32(mainPrice), 0, _default)).ToString()).ToString());
                    oldPrice = CreateAndSetElement(docNew, "oldprice", QuartzService.MakePrice(Convert.ToInt32(CalculatePriceDopArnika(Convert.ToInt32(mainPrice), 1, _default)).ToString()).ToString());
                    minPrice = CreateAndSetElement(docNew, "min_price", QuartzService.MakePrice(Convert.ToInt32(CalculatePriceDopArnika(Convert.ToInt32(mainPrice), 2, _default)).ToString()).ToString());

                    if (_default)
                    {
                        formula = CreateAndSetElement(docNew, "formula", $"{Program.formula1_ar_dop.Replace("x", mainPrice.ToString())};{Program.formula2_ar_dop.Replace("x", mainPrice.ToString())};{Program.formula3_ar_dop.Replace("x", mainPrice.ToString())};");

                    }
                    else
                    {
                        formula = CreateAndSetElement(docNew, "formula", $"{Program.formula1_ar1_dop.Replace("x", mainPrice.ToString())};{Program.formula2_ar1_dop.Replace("x", mainPrice.ToString())};{Program.formula3_ar1_dop.Replace("x", mainPrice.ToString())};");

                    }
                }
                else
                {
                    price = CreateAndSetElement(docNew, "price", QuartzService.MakePrice(Convert.ToInt32(CalculatePriceArnika(Convert.ToInt32(mainPrice), 0, _default)).ToString()).ToString());
                    oldPrice = CreateAndSetElement(docNew, "oldprice", QuartzService.MakePrice(Convert.ToInt32(CalculatePriceArnika(Convert.ToInt32(mainPrice), 1, _default)).ToString()).ToString());
                    minPrice = CreateAndSetElement(docNew, "min_price", QuartzService.MakePrice(Convert.ToInt32(CalculatePriceArnika(Convert.ToInt32(mainPrice), 2, _default)).ToString()).ToString());
                    if (_default)
                    {
                        formula = CreateAndSetElement(docNew, "formula", $"{Program.formula1_ar.Replace("x", mainPrice.ToString())};{Program.formula2_ar.Replace("x", mainPrice.ToString())};{Program.formula3_ar.Replace("x", mainPrice.ToString())};");

                    }
                    else
                    {
                        formula = CreateAndSetElement(docNew, "formula", $"{Program.formula1_ar1.Replace("x", mainPrice.ToString())};{Program.formula2_ar1.Replace("x", mainPrice.ToString())};{Program.formula3_ar1.Replace("x", mainPrice.ToString())};");

                    }
                }


                if (type == "full")
                {
                    foreach (var pm in item.Param)
                    {
                        var node = CreateAndSetElementParam(docNew, pm.Name, pm.Text);
                        offer.AppendChild(node);
                    }

                    foreach (var pic in item.Pictures)
                    {
                        var pEl = CreateAndSetElement(docNew, "picture", pic.Trim());
                        offer.AppendChild(pEl);
                    }
                }

                offer.AppendChild(name);
                offer.AppendChild(currency);
                offer.AppendChild(country);
                offer.AppendChild(article);
                offer.AppendChild(vendor);
                offer.AppendChild(availible);

                offer.AppendChild(url);
                offer.AppendChild(nameback);
                offer.AppendChild(desk);
                offer.AppendChild(price);
                offer.AppendChild(oldPrice);

                offer.AppendChild(minPrice);
                offer.AppendChild(formula);

                var outlets = docNew.CreateElement("outlets");


                
                try
                {

                    
                        var outlet = docNew.CreateElement("outlet");

                        var instInt = item.Count.ToString();

                        
                        var instock = CreateAndSetAttr(docNew, "instock", instInt);
                        outlet.Attributes.Append(instock);
                        
                        var w_name = CreateAndSetAttr(docNew, "warehouse_name", "APH");
                        outlet.Attributes.Append(w_name);
                        outlets.AppendChild(outlet);
                    
                }
                catch (Exception ex)
                {
                    logger.Error("Неверно задан фильтр у одного из складов. Нужна проверка");
                    Program.inAir = false;
                    Program.Last["arnika"].Success = false;
                    Program.Last["arnika"].Error = "Неверно задан фильтр у одного из складов. Нужна проверка";
                    return;
                }
                offer.AppendChild(outlets);
                offers.AppendChild(offer);
                if (x % 1000 == 0)
                {
                    logger.Info($"({x}/{y})");
                }
                x++;

            }

            if (type == "full")
            {
                var _1 = Program.link_ozon_arnika_full.TrimStart('/').Split("/")[0];
                if (_1.Contains(".xml"))
                {
                    docNew.Save("wwwroot" + Program.link_ozon_arnika_full);
                }
                else
                {
                    string path = "wwwroot/" + _1;
                    string subpath = "";
                    for (int i = 1; i < Program.link_ozon_arnika_full.TrimStart('/').Split("/").Length; i++)
                    {
                        if (Program.link_ozon_arnika_full.TrimStart('/').Split("/")[i].Contains(".xml"))
                        {
                            break;
                        }
                        subpath += Program.link_ozon_arnika_full.TrimStart('/').Split("/")[i] + "/";

                    }
                    subpath.TrimEnd('/');

                    if (!Directory.Exists(path))
                    {
                        Directory.CreateDirectory(path);
                    }
                    Directory.CreateDirectory($"{path}/{subpath}");
                    try
                    {
                        docNew.Save("wwwroot" + Program.link_ozon_arnika_full);


                    }
                    catch (Exception ex)
                    {
                        logger.Error("from full:" + ex.Message);
                        logger.Error(docNew.InnerXml);

                        Program.Last["arnika"].Success = false;
                        Program.Last["arnika"].Error = ex.Message;
                        return;

                    }
                }
            }
            else if (type == "lite1")
            {
                var _1 = Program.link_ozon_arnika_lite1.TrimStart('/').Split("/")[0];
                if (_1.Contains(".xml"))
                {
                    docNew.Save("wwwroot" + Program.link_ozon_arnika_lite1);
                }
                else
                {
                    string path = "wwwroot/" + _1;
                    string subpath = "";
                    for (int i = 1; i < Program.link_ozon_arnika_lite1.TrimStart('/').Split("/").Length; i++)
                    {
                        if (Program.link_ozon_arnika_lite1.TrimStart('/').Split("/")[i].Contains(".xml"))
                        {
                            break;
                        }
                        subpath += Program.link_ozon_arnika_lite1.TrimStart('/').Split("/")[i] + "/";

                    }
                    subpath.TrimEnd('/');

                    if (!Directory.Exists(path))
                    {
                        Directory.CreateDirectory(path);
                    }
                    Directory.CreateDirectory($"{path}/{subpath}");
                    try
                    {
                        docNew.Save("wwwroot" + Program.link_ozon_arnika_lite1);


                    }
                    catch (Exception ex)
                    {
                        logger.Error("from full:" + ex.Message);
                        logger.Error(docNew.InnerXml);

                        Program.Last["arnika"].Success = false;
                        Program.Last["arnika"].Error = ex.Message;
                        return;
                    }
                }
            }
            else
            {
                var _1 = Program.link_ozon_arnika_lite.TrimStart('/').Split("/")[0];
                if (_1.Contains(".xml"))
                {
                    docNew.Save("wwwroot" + Program.link_ozon_arnika_lite);
                }
                else
                {
                    string path = "wwwroot/" + _1;
                    string subpath = "";
                    for (int i = 1; i < Program.link_ozon_arnika_lite.TrimStart('/').Split("/").Length; i++)
                    {
                        if (Program.link_ozon_arnika_lite.TrimStart('/').Split("/")[i].Contains(".xml"))
                        {
                            break;
                        }
                        subpath += Program.link_ozon_arnika_lite.TrimStart('/').Split("/")[i] + "/";

                    }
                    subpath.TrimEnd('/');

                    if (!Directory.Exists(path))
                    {
                        Directory.CreateDirectory(path);
                    }
                    Directory.CreateDirectory($"{path}/{subpath}");
                    try
                    {
                        docNew.Save("wwwroot" + Program.link_ozon_arnika_lite);


                    }
                    catch (Exception ex)
                    {
                        logger.Error("from full:" + ex.Message);
                        logger.Error(docNew.InnerXml);

                        Program.Last["arnika"].Success = false;
                        Program.Last["arnika"].Error = ex.Message;
                        return;
                    }
                }
            }
        }

    }
}
