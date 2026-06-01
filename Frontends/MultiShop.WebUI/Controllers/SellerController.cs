using Microsoft.AspNetCore.Mvc;

namespace MultiShop.WebUI.Controllers
{
    public class SellerController : Controller
    {
        public IActionResult BecomeSeller()
        {
            ViewBag.directory1 = "ÇarşıPazar";
            ViewBag.directory2 = "Satıcı";
            ViewBag.directory3 = "Satış Yap";
            return View();
        }

        public IActionResult Concepts()
        {
            ViewBag.directory1 = "ÇarşıPazar";
            ViewBag.directory2 = "Satıcı";
            ViewBag.directory3 = "Temel Kavramlar";
            return View();
        }

        public IActionResult Training()
        {
            ViewBag.directory1 = "ÇarşıPazar";
            ViewBag.directory2 = "Satıcı";
            ViewBag.directory3 = "Eğitimler";
            return View();
        }
    }
}
