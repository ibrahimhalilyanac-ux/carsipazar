using Microsoft.AspNetCore.Mvc;

namespace MultiShop.WebUI.Controllers
{
    public class CampaignController : Controller
    {
        public IActionResult Index()
        {
            ViewBag.directory1 = "ÇarşıPazar";
            ViewBag.directory2 = "Kampanyalar";
            ViewBag.directory3 = "Aktif Kampanyalar";
            return View();
        }

        public IActionResult Credit()
        {
            ViewBag.directory1 = "ÇarşıPazar";
            ViewBag.directory2 = "Kampanyalar";
            ViewBag.directory3 = "Alışveriş Kredisi";
            return View();
        }

        public IActionResult Elite()
        {
            ViewBag.directory1 = "ÇarşıPazar";
            ViewBag.directory2 = "Kampanyalar";
            ViewBag.directory3 = "Elit Üyelik";
            return View();
        }

        public IActionResult Gift()
        {
            ViewBag.directory1 = "ÇarşıPazar";
            ViewBag.directory2 = "Kampanyalar";
            ViewBag.directory3 = "Hediye Fikirleri";
            return View();
        }
    }
}
