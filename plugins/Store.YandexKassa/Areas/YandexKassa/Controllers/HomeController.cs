using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace Store.YandexKassa.Areas.YandexKassa.Controllers
{
    [Area("YandexKassa")]
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
        // /YandexKassa/Home/Callback
        public IActionResult Callback()
        {
            return View();
        }
    }
}
