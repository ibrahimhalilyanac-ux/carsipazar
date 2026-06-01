using Microsoft.AspNetCore.Mvc;

namespace MultiShop.WebUI.Controllers
{
    public class InfoController : Controller
    {
        public IActionResult Career()
        {
            ViewBag.directory1 = "ÇarşıPazar";
            ViewBag.directory2 = "Kurumsal";
            ViewBag.directory3 = "Kariyer";
            return View();
        }

        public IActionResult Security()
        {
            ViewBag.directory1 = "ÇarşıPazar";
            ViewBag.directory2 = "Kurumsal";
            ViewBag.directory3 = "Güvenlik";
            return View();
        }
    }
}
