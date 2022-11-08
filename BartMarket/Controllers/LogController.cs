using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.IO;

namespace BartMarket.Controllers
{
    [ApiController]
    [Route("api/log")]
    [Authorize]
    public class LogController : Controller
    {
        public string Index()
        {
            using (StreamReader file = new StreamReader(AppDomain.CurrentDomain.BaseDirectory + "/logs/debug.log"))
            {
                return file.ReadToEnd();
            }
        }
    }
}
