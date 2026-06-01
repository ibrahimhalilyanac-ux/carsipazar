using Microsoft.AspNetCore.Mvc;
using MultiShop.DtoLayer.CatalogDtos.CategoryDtos;
using MultiShop.DtoLayer.CatalogDtos.ProductDtos;
using MultiShop.WebUI.Services.CatalogServices.CategoryServices;
using MultiShop.WebUI.Services.CatalogServices.ProductServices;
using System.Linq;

namespace MultiShop.WebUI.ViewComponents.DefaultViewComponents
{
    public class _CategoriesDefaultComponentPartial : ViewComponent
    {
        private readonly ICategoryService _categoryService;
        private readonly IProductService _productService;
        
        public _CategoriesDefaultComponentPartial(ICategoryService categoryService, IProductService productService)
        {
            _categoryService = categoryService;
            _productService = productService;
        }
        
        public async Task<IViewComponentResult> InvokeAsync()
        {
            try 
            {
                var categories = await _categoryService.GetAllCategoryAsync();
                
                try 
                {
                    var products = await _productService.GetAllProductAsync();
                    var productCounts = (products ?? new List<ResultProductDto>())
                        .GroupBy(p => p.CategoryId)
                        .ToDictionary(g => g.Key, g => g.Count());
                    ViewBag.ProductCounts = productCounts;
                } 
                catch 
                {
                    ViewBag.ProductCounts = new Dictionary<string, int>();
                }
                
                return View(categories ?? new List<ResultCategoryDto>());
            }
            catch (Exception)
            {
                ViewBag.ProductCounts = new Dictionary<string, int>();
                return View(new List<ResultCategoryDto>());
            }
        }
    }
}
