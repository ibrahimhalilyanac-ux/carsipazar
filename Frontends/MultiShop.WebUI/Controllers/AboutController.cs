using Microsoft.AspNetCore.Mvc;
using MultiShop.WebUI.Services.CatalogServices.AboutServices;
using System.Threading.Tasks;

namespace MultiShop.WebUI.Controllers
{
    public class AboutController : Controller
    {
        private readonly IAboutService _aboutService;
        public AboutController(IAboutService aboutService)
        {
            _aboutService = aboutService;
        }

        public async Task<IActionResult> Index()
        {
            ViewBag.directory1 = "ÇarşıPazar";
            ViewBag.directory2 = "Kurumsal";
            ViewBag.directory3 = "Hakkımızda";
            
            var values = await _aboutService.GetAllAboutAsync();
            return View(values);
        }
    }
}
