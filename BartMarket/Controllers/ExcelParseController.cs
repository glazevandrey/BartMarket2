using BartMarket.Template;
using Microsoft.AspNetCore.Mvc;
using NLog;
using System;
using System.Threading;

namespace BartMarket.Controllers
{
    [ApiController]
    [Route("excel")]
    public class ExcelParseController : Controller
    {
        private static Logger logger = LogManager.GetCurrentClassLogger();

        [HttpGet]
        public IActionResult Index(string temp_path, string error, string stage)
        {
            try
            {
                if(stage == "ready")
                {
                    ViewData["stage"] = "go";
                }
                if (stage == "go")
                {
                    ViewData["stage"] = "ready";
                }
                logger.Info(" END go to index = " + temp_path);

                ViewData["Templates"] = Program.ozonTemplates;
                logger.Info(temp_path + "    " + error);
                if (temp_path != null)
                {

                    ViewData["TempReady"] = "http://ovz1.j34469996.pxlzp.vps.myjino.ru/" + temp_path + "_ready.xlsx";
                    Program.lastTemplate = null;
                }
                else
                {
                    if (error != null)
                    {
                        if (error == "error")
                        {
                            ViewData["Error"] = "Сейчас идет обработка основного фида. Попробуйте через некоторое время.";
                        }
                        else
                        {
                            try
                            {
                                ViewData["Error"] = error.Split(":")[1].Trim();
                            }
                            catch (System.Exception ex)
                            {
                                logger.Error(ex.Message);
                            }

                        }
                    }
                }
            }
            catch (System.Exception ex)
            {
                logger.Error(ex.Message);
            }

            return View();
        }

        [HttpPost]
        public IActionResult Parse([FromForm] string temp, [FromForm] int count, [FromForm] string step, [FromForm] bool ostatok)
        {  
            if(step == "ready")
            {
                IBaseOzonTemplate tempate = null;

                switch (temp)
                {
                    case "Светильник напольный":
                        tempate = new NapolnyTorsher();
                        break;
                    default:
                        break;
                }
                logger.Info("template : " + tempate.Name);

                Program.ExcelAir = true;
                Program.lastTemplate = tempate;
                string res = "";

                try
                {
                    res = tempate.Prepare();
                    logger.Info("res2 = " + res);
                    Program.ExcelAir = false;

                    if (res == null)
                    {
                        return RedirectToAction("Index", new { error = "error" });

                    }else if (res == "ok")
                    {
                        return Redirect("excel?stage=ready");

                    }
                    else
                    {
                        return RedirectToAction("Index", new { error = res });
                    }
                }
                catch (Exception ex)
                {
                    logger.Error(ex.Message);
                    Program.ExcelAir = false;

                    return RedirectToAction("Index", new { error = res });

                }
            }
            else
            {
                Program.ExcelAir = true;
                var res = Program.lastTemplate.Parse(count, ostatok);
                Program.ExcelAir = false;

                logger.Info(" res = " + res);

                if (res == null)
                {
                    return RedirectToAction("Index", new { error = "error" });
                }
                else if (res.StartsWith("err"))
                {
                    return RedirectToAction("Index", new { error = res });

                }
                logger.Info(" go to index = " + Program.lastTemplate.PathToTemplate);
                return Redirect("excel?temp_path=" + Program.lastTemplate.PathToTemplate + "&stage=go");
            }
           
        }
    }
}
