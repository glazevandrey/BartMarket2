using BartMarket.Data;
using IronXL;
using NLog;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace BartMarket.Template
{
    public class NapolnyTorsher : IBaseOzonTemplate
    {
        private static Logger logger = LogManager.GetCurrentClassLogger();

        public string Name { get; set; } = "Светильник напольный";
        public string PathToTemplate { get; set; } = "SvetilnikNapolny";

        public List<string> KeyWords { get; set; } = new List<string>() { "Торшер" };

        public string Parse(int count)
        {
            return GetExcel(count);
        }

        public string Prepare()
        {
            var text = File.ReadAllText("wwwroot" + Program.link_ozon_full);
            if (text == null || text == "") { logger.Error("text == null"); return null; }


            XmlSerializer serializer = new XmlSerializer(typeof(YmlCatalog2));
            YmlCatalog2 catalog = new YmlCatalog2();

            using (StringReader reader = new StringReader(text))
            {
                var text2 = serializer.Deserialize(reader);
                catalog = (YmlCatalog2)text2;
            }
            var used = new List<UploadedOzonId>();

            using (var db = new UserContext())
            {
                used = db.UploadedOzonIds.ToList();
            }
            var list = new List<Offer2>();
            foreach (var item in catalog.Shop.Offers.Offer)
            {
                logger.Info(item.Id);
                if (used.FirstOrDefault(m => m.OzonId == item.Id) != null)
                {
                    continue;
                }

                list.Add(item);
            }
            Program.list = list.Where(m=>m.Name.ToLower().Contains(KeyWords[0].ToLower())).ToList();
            return "ok";
        }

        private string GetExcel(int count)
        {
            string path = "wwwroot/" + PathToTemplate + "_ready.xlsx";

            try
            {
                if (Program.inAir)
                {
                    return null;
                }

                WorkBook workbook = new WorkBook();

                try
                {
                    workbook = WorkBook.Load("wwwroot/" + PathToTemplate + ".xlsx");

                }
                catch (Exception ex)
                {
                    logger.Error(ex.Message);
                    return null;

                }

                logger.Info(PathToTemplate);
                if (workbook == null) { logger.Error("workbook == null"); return null; }
                var sheets = workbook.WorkSheets;
                var sheet = sheets.First(m => m.Name == "Шаблон для поставщика");
                var s = sheet["C2"].Value;

                var link_to_full = "http://ovz1.j34469996.pxlzp.vps.myjino.ru" + Program.link_ozon_full;
 
                var list = new List<Offer2>();

                if (Program.list == null) { logger.Error("catalog == null"); return null; }

                int x = 4;
                int y = 1;
                int z = 0;
                var list_id = new List<string>();
                var colors = new List<string>()
            {
                "белый","бежевый","бирюзовый",
                "бордовый","бронза","голубой","горчичный",
                "желтый","зеленый","зеркальный","золотой",
                "коралловый","коричнево-красный","коричневый",
                "красный","кремовый","лазурный","лиловый",
                "малиновый","медь","оливковый","оранжевый",
                "перламутровый","прозрачный","пурпурный","разноцветный",
                "розовый","салатовый","светло-бежевый","светло-желтый",
                "светло-зеленый","светло-коричневый","светло-розовый","светло-серый",
                "светло-синий","серебристый","серый","серый металлик",
                "синий","сиреневый","слоновая кость","темно-бежевый","темно-бордовый",
                "темно-зеленый","темно-коричневый","темно-розовый","темно-серый",
                "темно-синий","фиолетовый","фуксия","хаки",
                "хром","черно-серый","черный","черный матовый","шоколадный"
            };
                var colors2 = new List<string>()
            {
                "бежевый","белый","бирюзовый",
                "бордовый","бронза","голубой",
                "желтый","зеленый","холодный белый","золотой",
                "коралловый","коричнево-красный","коричневый",
                "красный","медь","оливковый","оранжевый", "пудровый",
                "прозрачный","пурпурный","разноцветный",
                "розовый","светло-бежевый","светло-зеленый","светло-коричневый","светло-розовый","светло-серый",
                "серебристый","серый","серый металлик",
                "синий","сиреневый","слоновая кость","темно-бежевый",
                "теплый белый", "темно-зеленый","темно-коричневый","темно-серый",
                "темно-синий","фиолетовый","фуксия","черный","шоколадный"
            };

                if (list == null)
                {
                    logger.Error("list == null");
                    return null;
                }
                logger.Info("start foreach");
                GC.Collect();
                GC.WaitForPendingFinalizers();
                GC.Collect();
                foreach (var item in list)
                {
                    if (!item.Name.ToLower().Contains(KeyWords[0].ToLower()))
                    {
                        continue;
                    }

                    sheet["A" + x].Value = y;
                    sheet["B" + x].Value = item.Id;
                    sheet["C" + x].Value = item.NameBack;
                    sheet["D" + x].Value = item.Price;
                    sheet["E" + x].Value = item.Oldprice;
                    sheet["F" + x].Value = "Не облагается";
                    sheet["H" + x].Value = "Светильник напольный";
                    sheet["I" + x].Value = item.Param.FirstOrDefault(m => m.Name.ToLower() == "штрих-код (служебное)")?.Text;

                    var weight = item.Param.FirstOrDefault(m => m.Name.ToLower() == "коробка вес гр")?.Text;
                    string weightString = "";
                    if (weight != null && weight != "")
                    {

                        if (weight.Contains("."))
                        {
                            weight = weight.Replace(".", "");
                        }
                        int xy = Convert.ToInt32(weight);
                        if (xy < 500)
                        {
                            weightString = "500";
                        }
                        else
                        {
                            weightString = weight.ToString();
                        }
                    }
                    else
                    {
                        weightString = "500";
                    }
                    sheet["J" + x].Value = weightString;


                    var shirina = item.Param.FirstOrDefault(m => m.Name.ToLower() == "коробка ширина")?.Text;
                    string shirinaString = "";
                    if (shirina != null && shirina != "")
                    {
                        if (shirina.Contains("."))
                        {
                            shirina = shirina.Replace(".", "");
                        }

                        int xy = Convert.ToInt32(shirina);
                        if (xy < 50)
                        {
                            shirinaString = "150";
                        }
                        else
                        {
                            shirinaString = shirina.ToString();
                        }
                    }
                    else
                    {
                        shirinaString = "150";
                    }
                    sheet["K" + x].Value = shirinaString;

                    var height = item.Param.FirstOrDefault(m => m.Name.ToLower() == "коробка высота")?.Text;
                    string heightString = "";
                    if (height != null && height != "")
                    {
                        if (height.Contains("."))
                        {
                            height = height.Replace(".", "");
                        }
                        int xy = Convert.ToInt32(height);
                        if (xy < 50)
                        {
                            heightString = "200";
                        }
                        else
                        {
                            heightString = height.ToString();
                        }
                    }
                    else
                    {
                        heightString = "200";
                    }
                    sheet["L" + x].Value = heightString;

                    var lenght = item.Param.FirstOrDefault(m => m.Name.ToLower() == "коробка длина")?.Text;
                    string lenghtString = "";
                    if (lenght != null && lenght != "")
                    {
                        if (lenght.Contains("."))
                        {
                            lenght = lenght.Replace(".", "");
                        }
                        int xy = Convert.ToInt32(lenght);
                        if (xy < 50)
                        {
                            lenghtString = "200";
                        }
                        else
                        {
                            lenghtString = lenght.ToString();
                        }
                    }
                    else
                    {
                        lenghtString = "200";
                    }
                    sheet["M" + x].Value = lenghtString;

                    sheet["N" + x].Value = item.Pictures.FirstOrDefault();

                    var list_pic = new List<string>();

                    if (item.Pictures.Count > 1)
                    {
                        for (int i = 1; i < item.Pictures.Count; i++)
                        {
                            list_pic.Add(item.Pictures[i]);
                        }

                        foreach (var pic in list_pic)
                        {
                            sheet["O" + x].Value = pic + "\n";
                        }
                        sheet["O" + x].Value = sheet["O" + x].Value.ToString().TrimEnd('\n');

                    }

                    sheet["R" + x].Value = "BartMarket";
                    sheet["S" + x].Value = item.NameBack;

                    string c1 = item.Param.FirstOrDefault(m => m.Name.ToLower() == "цвет плафона/абажура")?.Text;
                    string c2 = item.Param.FirstOrDefault(m => m.Name.ToLower() == "цвет арматуры")?.Text;
                    if (!colors.Contains(c1?.ToLower()))
                    {
                        c1 = null;
                    }
                    if (!colors.Contains(c2?.ToLower()))
                    {
                        c2 = null;
                    }


                    if (c1?.ToLower() != c2?.ToLower())
                    {
                        sheet["U" + x].Value = (c1 + ";" + c2).TrimEnd(';').TrimStart(';');
                    }
                    else
                    {
                        sheet["U" + x].Value = c1?.TrimEnd(';').TrimStart(';');
                    }

                    sheet["V" + x].Value = (item.Param.FirstOrDefault(m => m.Name.ToLower() == "цвет плафона/абажура")?.Text + "/" + item.Param.FirstOrDefault(m => m.Name.ToLower() == "цвет арматуры")?.Text).TrimEnd('/').TrimStart('/');

                    var type_lamp = item.Param.FirstOrDefault(m => m.Name.ToLower() == "вид ламп")?.Text;
                    if (type_lamp?.ToLower() == "галогеновая")
                    {
                        sheet["W" + x].Value = "Галогенная";
                    }
                    else if (type_lamp?.ToLower() == "люминесцентные" || type_lamp?.ToLower() == "люминесцентная")
                    {
                        sheet["W" + x].Value = "Люминисцентная (энергосберегающая)";

                    }
                    else if (type_lamp?.ToLower() == "накаливания" || type_lamp?.ToLower() == "светодиодная")
                    {
                        sheet["W" + x].Value = type_lamp;

                    }



                    sheet["X" + x].Value = "1";
                    sheet["Y" + x].Value = item.Param.FirstOrDefault(m => m.Name.ToLower() == "высота, мм")?.Text;
                    sheet["Z" + x].Value = "Напольный светильник";
                    sheet["AF" + x].Value = "Торшер";
                    sheet["AH" + x].Value = "Взрослая;Детская";
                    sheet["AJ" + x].Value = item.Param.FirstOrDefault(m => m.Name.ToLower() == "цоколь")?.Text;
                    sheet["AK" + x].Value = item.Param.FirstOrDefault(m => m.Name.ToLower() == "количество ламп")?.Text;
                    sheet["AL" + x].Value = "Нет";
                    sheet["AM" + x].Value = item.Param.FirstOrDefault(m => m.Name.ToLower() == "общая мощность, w")?.Text;
                    sheet["AN" + x].Value = item.Param.FirstOrDefault(m => m.Name.ToLower() == "s освещ.(м2)")?.Text;
                    sheet["AR" + x].Value = "Балкон;Баня и сауна;Бар;Бильярдный зал;Ванная;Веранда;Гараж;Гардероб;Гостиная;Детская;Домашний кинотеатр;Душевая;Зимний сад;Кабинет;Кладовка;Коридор;Котельная;Крыльцо;Кухня;Кухня-столовая;Мансарда;Мастер-спальня;Мастерская;Офис;Прачечная;Прохожая;Санузел;Спальня;Столовая;Студия;Студия-кухня;Терраса;Торговый зал;Холл";

                    string c3 = item.Param.FirstOrDefault(m => m.Name.ToLower() == "цвет плафона/абажура")?.Text;
                    if (!colors2.Contains(c3?.ToLower()))
                    {
                        c3 = null;
                    }

                    sheet["AS" + x].Value = c3;
                    sheet["AT" + x].Value = "Американский;Ампир;Английский;Ар-деко;Африканский;Борокко;Бохо;Викторианский;Винтаж;Восточный;Готический стиль;Детский;Итальянский;Кантри;Китайский;Китч;Классический;Лофт;Минимализм;Модерн;Молодежный;Морской;Неоклассический;Поп-арт;Прованс;Ретро-стиль;Рококо;Романтика;Русский;Рустик;Скандинавский стиль;Современная классика;Современный;Средиземноморский;Техно;Тиффани;Французский;Хай-тек;Шале;Шебби-шик;Эклектика;Эко-стиль;Этнический;Японский";

                    string mat = "";
                    var mat_plafon = "";
                    mat_plafon = item.Param.FirstOrDefault(m => m.Name.ToLower() == "материал плафона/абажура")?.Text;

                    if (mat_plafon?.ToLower() == "silicon" || mat_plafon?.ToLower() == "silicone" || mat_plafon?.ToLower() == "силикон")
                    {
                        mat = "Силикон";
                    }
                    else if (mat_plafon?.ToLower() == "полимер")
                    {
                        mat = "Полимерный материал";
                    }
                    else
                    {
                        if (mat_plafon?.ToLower() == "камень" && mat_plafon?.ToLower() == "кожа" && mat_plafon?.ToLower() == "органза")
                        {
                            mat = null;
                        }
                        else
                        {
                            var color4 = new List<string>()
                        {
                            "abs пластик","авиационный алюминий","акрил","алюминий","бетон","биопластик",
                            "бумага","гипоаллергенный пластик","гипс","дерево","искусственный камень","искусственный мех","каменная соль",
                            "керамика","кристаллы","кружево","культивированный жемчуг пресноводный","металл","натуральный камень","нейлон",
                            "нержавеющая сталь","органическое стекло","пвх (поливинилхлорид)","пвх, пластик, пенопласт, ткань","перо","пластик","поликарбонат",
                            "полимерный материал","полипропилен","полирезин","полистоун","полиэстр","резина",
                            "резина-пластик","силикон","скорлупа","соль","сталь","стекло","стеклопластик",
                            "текстиль","термопластик (tpu)","термопластичная резина (тпр)","ткань","углеродистая сталь","фарфор","хрусталь","хрустальное стекло","ювелирное стекло"
                        };
                            if (color4.Contains(mat_plafon?.ToLower()))
                            {
                                mat = mat_plafon;
                            }
                            else
                            {
                                mat = null;
                            }
                        }

                    }


                    sheet["AU" + x].Value = mat;
                    sheet["AY" + x].Value = item.Param.FirstOrDefault(m => m.Name.ToLower() == "напряжение")?.Text;
                    sheet["AZ" + x].Value = "IP20";

                    var color5 = new List<string>()
                {
                    "акрил", "алюминий", "биопластик","керамика","металл","пластик"
                };

                    var mat_arm = item.Param.FirstOrDefault(m => m.Name.ToLower() == "материал арматуры")?.Text;

                    if (!color5.Contains(mat_arm?.ToLower()))
                    {
                        mat_arm = null;
                    }

                    sheet["BD" + x].Value = mat_arm;

                    string country = item.Param.FirstOrDefault(m => m.Name.ToLower() == "страна")?.Text;
                    if (country?.ToLower() != "китай")
                    {
                        country += ";Китай";
                    }
                    sheet["BF" + x].Value = country;
                    sheet["BG" + x].Value = "Стандартная";
                    sheet["BH" + x].Value = "Картонная коробка";
                    sheet["BI" + x].Value = item.Param.FirstOrDefault(m => m.Name.ToLower() == "коробка вес гр")?.Text;
                    sheet["BJ" + x].Value = "1";
                    sheet["BB" + x].Value = item.Param.FirstOrDefault(m => m.Name.ToLower() == "ширина, мм")?.Text;


                    x++;
                    y++;
                    z++;
                    list_id.Add(item.Id);
                    if (z == count)
                    {
                        break;
                    }

                }
                logger.Info("end foreach");
                logger.Info("start upload ids");

                using (var db = new UserContext())
                {
                    foreach (var item in list_id)
                    {
                        db.UploadedOzonIds.Add(new UploadedOzonId() { OzonId = item });

                    }

                    db.SaveChanges();
                }
                logger.Info("end upload ids");


                if (File.Exists(path))
                {
                    File.Delete(path);
                }
                logger.Info("save path: " + path);

                workbook.SaveAs(path);
            }
            catch (Exception ex)
            {
                logger.Error(ex);
                return "err:" + ex.Message;
            }
            return path;
        }
    }
}
