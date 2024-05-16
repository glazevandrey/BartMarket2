using BartMarket.Data;
using BartMarket.Template;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using NLog;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading;
using Logger = NLog.Logger;

namespace BartMarket
{
    public class Program
    {
        public static string  _ARN = "_ARN";
        public static string _DON = "_DONP";

        public static List<string> ZeroVendors = new List<string>();
        public static bool ExcelAir = false;
        public static string formula1 = "";
        public static string formula2 = "";
        public static string formula3 = "";

        public static string formula1_ar1 = "";
        public static string formula2_ar1 = "";
        public static string formula3_ar1 = "";


        public static string formula1_ar1_dop = "";
        public static string formula2_ar1_dop = "";
        public static string formula3_ar1_dop = "";

        public static string formula1_ar = "";
        public static string formula2_ar = "";
        public static string formula3_ar = "";

        public static string formula1_ar_dop = "";
        public static string formula2_ar_dop = "";
        public static string formula3_ar_dop = "";

        public static string formula1_ar_ali = "";
        public static string formula2_ar_ali = "";
        public static string formula5_ar_ali = "";
        public static string formula6_ar_ali = "";
        public static string formula3_ar_ali = "";
        public static string formula4_ar_ali = "";


        public static string formula1_dp_ali = "";
        public static string formula2_dp_ali = "";
        public static string formula3_dp_ali = "";
        public static string formula4_dp_ali = "";
        


        public static List<Warehouse> warehouses = new List<Warehouse>();
        public static string link_ozon_lite = "/content/liteozon.xml";
        public static string link_ozon_full = "/content/fullozon.xml";

        public static string content = "/content/csv1.csv";
        public static string prices = "/content/csv2.csv";

        public static string link_ozon_arnika_lite = "/content/liteozon_arnika.xml";
        public static string link_ozon_arnika_lite1 = "/content/liteozon1_arnika.xml";

        public static string link_ozon_arnika_full = "/content/fullozon_arnika.xml";

        public static string link_aliexpress_arnika = "/content/ali_arnika.xml";
        public static string link_aliexpress_donplafon = "/content/ali_donplafon.xml";


        public static Dictionary<string, StatModel> Last = new Dictionary<string, StatModel>() { {"arnika", new StatModel() },{"donplafon", new StatModel() }   }; 
        public static bool inAir = false;
        public static IEnumerable<Offer2> list;
        public static List<Offer> deleted = new List<Offer>();
        public static List<OfferArnika> deletedAr = new List<OfferArnika>();

        public static List<Offer2> need_add = new List<Offer2>();
        public static IBaseOzonTemplate lastTemplate;
        public static StringBuilder lastIds = new StringBuilder();
        public static List<IBaseOzonTemplate> ozonTemplates = new List<IBaseOzonTemplate>();
        public static Dictionary<string, List<string>> Providers = new Dictionary<string, List<string>>()
        {
            { "ozon", new List<string>(){ "donplafon", "arnika"} } ,
            { "aliexpress", new List<string>(){ "donplafon", "arnika" } },
            { "ym", new List<string>(){ ""} },

        };

        public static void Main(string[] args)
        {
            IronXL.License.LicenseKey = "IRONXL.TONI70020.25513-B58A105DFA-E5J5E2-KN5QIEVOS6QV-25KF2LLZS664-ZUPCPC43GEYK-LNIVESSFL4KX-CPN2OAJ5HCAT-YQA6BE-THDZ3GN3FCOJEA-DEPLOYMENT.TRIAL-T6FH55.TRIAL.EXPIRES.02.MAR.2023"; 
            ozonTemplates.Add(new NapolnyTorsher());
            using (var db = new UserContext())
            {
                var formuls = db.Formulas.ToList();
                if(formuls.Count == 0)
                {
                    db.Formulas.Add(new Formulas() { Name = "formula1", Value = "(x + 1500) * 1.2"});
                    db.Formulas.Add(new Formulas() { Name = "formula2", Value = "(x + 1500) * 1.5" });
                    db.Formulas.Add(new Formulas() { Name = "formula3", Value = "(x + 1500) * 1.15" });

                    db.Formulas.Add(new Formulas() { Name = "formula1_ar1", Value = "(x + 2200) * 4.2" });
                    db.Formulas.Add(new Formulas() { Name = "formula2_ar1", Value = "(x + 2200) * 6.3" });
                    db.Formulas.Add(new Formulas() { Name = "formula3_ar1", Value = "(x + 2200) * 2.6" });

                    db.Formulas.Add(new Formulas() { Name = "formula1_ar1_dop", Value = "(x + 3500) * 4.2" });
                    db.Formulas.Add(new Formulas() { Name = "formula2_ar1_dop", Value = "(x + 3500) * 6.3" });
                    db.Formulas.Add(new Formulas() { Name = "formula3_ar1_dop", Value = "(x + 3500) * 2.6" });

                    db.Formulas.Add(new Formulas() { Name = "formula1_ar", Value = "(x + 2200) * 1.17" });
                    db.Formulas.Add(new Formulas() { Name = "formula2_ar", Value = "(x + 2200) * 1.8" });
                    db.Formulas.Add(new Formulas() { Name = "formula3_ar", Value = "(x + 2200) * 1.14" });

                    db.Formulas.Add(new Formulas() { Name = "formula1_ar_dop", Value = "(x + 3500) * 1.25" });
                    db.Formulas.Add(new Formulas() { Name = "formula2_ar_dop", Value = "(x + 3500) * 1.8" });
                    db.Formulas.Add(new Formulas() { Name = "formula3_ar_dop", Value = "(x + 3500) * 1.2" });

                    db.Formulas.Add(new Formulas() { Name = "formula1_ar_ali", Value = "(x + 2200) * 1.15" });
                    db.Formulas.Add(new Formulas() { Name = "formula2_ar_ali", Value = "(x + 2200) * 1.1" });
                    db.Formulas.Add(new Formulas() { Name = "formula5_ar_ali", Value = "(x + 2200) * 1.15" });
                    db.Formulas.Add(new Formulas() { Name = "formula6_ar_ali", Value = "(x + 2200) * 1.1" });
                    db.Formulas.Add(new Formulas() { Name = "formula3_ar_ali", Value = "(x + 3500) * 1.15" });
                    db.Formulas.Add(new Formulas() { Name = "formula4_ar_ali", Value = "(x + 3500) * 1.1" });

                    db.Formulas.Add(new Formulas() { Name = "formula1_dp_ali", Value = "(x + 1500) * 1.25" });
                    db.Formulas.Add(new Formulas() { Name = "formula2_dp_ali", Value = "(x + 1500) * 1.15" });
                    db.Formulas.Add(new Formulas() { Name = "formula3_dp_ali", Value = "(x + 500) * 1.25" });
                    db.Formulas.Add(new Formulas() { Name = "formula4_dp_ali", Value = "(x + 500) * 1.15" });
                    db.SaveChanges();
                }
                var fs = db.Formulas.ToList();

                formula1 = fs.FirstOrDefault(m=>m.Name == "formula1").Value;
                formula2 = fs.FirstOrDefault(m => m.Name == "formula2").Value;
                formula3 = fs.FirstOrDefault(m => m.Name == "formula3").Value;

                formula1_ar1 = fs.FirstOrDefault(m => m.Name == "formula1_ar1").Value;
                formula2_ar1 = fs.FirstOrDefault(m => m.Name == "formula2_ar1").Value;
                formula3_ar1 = fs.FirstOrDefault(m => m.Name == "formula3_ar1").Value;


                formula1_ar1_dop = fs.FirstOrDefault(m => m.Name == "formula1_ar1_dop").Value;
                formula2_ar1_dop = fs.FirstOrDefault(m => m.Name == "formula2_ar1_dop").Value;
                formula3_ar1_dop = fs.FirstOrDefault(m => m.Name == "formula3_ar1_dop").Value;

                formula1_ar = fs.FirstOrDefault(m => m.Name == "formula1_ar").Value;
                formula2_ar = fs.FirstOrDefault(m => m.Name == "formula2_ar").Value;
                formula3_ar = fs.FirstOrDefault(m => m.Name == "formula3_ar").Value;

                formula1_ar_dop = fs.FirstOrDefault(m => m.Name == "formula1_ar_dop").Value;
                formula2_ar_dop = fs.FirstOrDefault(m => m.Name == "formula2_ar_dop").Value;
                formula3_ar_dop = fs.FirstOrDefault(m => m.Name == "formula3_ar_dop").Value;

                formula1_ar_ali = fs.FirstOrDefault(m => m.Name == "formula1_ar_ali").Value;
                formula2_ar_ali = fs.FirstOrDefault(m => m.Name == "formula2_ar_ali").Value;
                formula5_ar_ali = fs.FirstOrDefault(m => m.Name == "formula5_ar_ali").Value;
                formula6_ar_ali = fs.FirstOrDefault(m => m.Name == "formula6_ar_ali").Value;
                formula3_ar_ali = fs.FirstOrDefault(m => m.Name == "formula3_ar_ali").Value;
                formula4_ar_ali = fs.FirstOrDefault(m => m.Name == "formula4_ar_ali").Value;


                formula1_dp_ali = fs.FirstOrDefault(m => m.Name == "formula1_dp_ali").Value;
                formula2_dp_ali = fs.FirstOrDefault(m => m.Name == "formula2_dp_ali").Value;
                formula3_dp_ali = fs.FirstOrDefault(m => m.Name == "formula3_dp_ali").Value;
                formula4_dp_ali = fs.FirstOrDefault(m => m.Name == "formula4_dp_ali").Value;
       

            }

            using (var db = new UserContext())
            {
                var fg = db.UploadedOzonIds.ToList();
                var l = db.LinkModels.ToList();
                if (l.Count == 0)
                {
                    var lite = new LinkModel();
                    lite.Link = link_ozon_lite;
                    lite.Type = "Lite";

                    var lite2 = new LinkModel();
                    lite2.Link = link_ozon_full;
                    lite2.Type = "Full";

                    db.LinkModels.Add(lite);
                    db.LinkModels.Add(lite2);
                    db.SaveChanges();


                }
            }

            var list = new List<WarehouseModel>();
            using (var db = new UserContext())
            {
                list = db.Warehouses.ToList();
                var _4 = new WarehouseModel();
                _4.Name = "APH";

                if (list.Count == 0)
                {
                    var _1 = new WarehouseModel();
                    var _2 = new WarehouseModel();
                    var _3 = new WarehouseModel();
                    _1.Name = "DPN";
                    _2.Name = "DPN2";
                    _3.Name = "DPN3";

                    db.Warehouses.Add(_1);
                    db.Warehouses.Add(_2);
                    db.Warehouses.Add(_3);
                    db.Warehouses.Add(_4);

                    db.SaveChanges();
                }

                if (list.FirstOrDefault(m=>m.Name == "APH") == null)
                {
                    db.Warehouses.Add(_4);
                    db.SaveChanges();

                }

                db.SaveChanges();

            }

            using (var db = new UserContext())
            {
                list = db.Warehouses.ToList();

            }
            foreach (var item in list)
            {
                var n = new Warehouse();
                n.Name = item.Name;
                using (var db = new UserContext())
                {
                    var houses = db.WarehouseSettings.ToList();

                    if (houses.Count == 0)
                    {

                        var sett = new List<WarehouseSetting>();

                        var sett1 = new WarehouseSetting();
                        sett1.WarehouseId = 2;
                        sett1.Filter = "price > 3000";

                        var sett2 = new WarehouseSetting();
                        sett2.WarehouseId = 2;
                        sett2.Filter = "price < 50000";

                        var sett3 = new WarehouseSetting();
                        sett3.WarehouseId = 2;
                        sett3.Filter = "weight < 30";

                        db.WarehouseSettings.AddRange(sett1, sett2, sett3);

                        db.SaveChanges();
                    }
                }
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
                warehouses.Add(n);

            }

            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });


    }
}

