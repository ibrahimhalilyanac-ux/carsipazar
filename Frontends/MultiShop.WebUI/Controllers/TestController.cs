using Microsoft.AspNetCore.Mvc;
using MultiShop.DtoLayer.CatalogDtos.CategoryDtos;
using MultiShop.WebUI.Services.CatalogServices.CategoryServices;

namespace MultiShop.WebUI.Controllers
{
    public class TestController : Controller
    {
        private readonly ICategoryService _categoryService;
        public TestController(ICategoryService categoryService)
        {
            _categoryService = categoryService;
        }
        public async Task<IActionResult> Index()
        {
            var values = await _categoryService.GetAllCategoryAsync();
            return View(values);
        }
        public IActionResult Deneme1()
        {
            return View();
        }

        public async Task<IActionResult> Deneme2()
        {
            var values = await _categoryService.GetAllCategoryAsync();
            return View(values);
        }
    }
}