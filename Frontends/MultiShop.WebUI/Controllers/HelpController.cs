using Microsoft.AspNetCore.Mvc;

namespace MultiShop.WebUI.Controllers
{
    public class HelpController : Controller
    {
        public IActionResult Faq()
        {
            ViewBag.directory1 = "ÇarşıPazar";
            ViewBag.directory2 = "Yardım";
            ViewBag.directory3 = "Sıkça Sorulan Sorular";
            return View();
        }

        public IActionResult LiveSupport()
        {
            ViewBag.directory1 = "ÇarşıPazar";
            ViewBag.directory2 = "Yardım";
            ViewBag.directory3 = "Canlı Yardım";
            return View();
        }

        public IActionResult Refund()
        {
            ViewBag.directory1 = "ÇarşıPazar";
            ViewBag.directory2 = "Yardım";
            ViewBag.directory3 = "Nasıl İade Edebilirim";
            return View();
        }

        public IActionResult Guide()
        {
            ViewBag.directory1 = "ÇarşıPazar";
            ViewBag.directory2 = "Yardım";
            ViewBag.directory3 = "İşlem Rehberi";
            return View();
        }
    }
}
