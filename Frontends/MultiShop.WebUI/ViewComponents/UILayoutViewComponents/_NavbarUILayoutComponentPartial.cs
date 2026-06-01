using Microsoft.AspNetCore.Mvc;
using MultiShop.DtoLayer.CatalogDtos.CategoryDtos;
using MultiShop.WebUI.Services.CatalogServices.CategoryServices;
using MultiShop.WebUI.Services.FavoriteServices;
using MultiShop.WebUI.Services.BasketServices;
using System.Net.Http.Headers;

namespace MultiShop.WebUI.ViewComponents.UILayoutViewComponents
{
    public class _NavbarUILayoutComponentPartial : ViewComponent
    {
        private readonly ICategoryService _categoryService;
        private readonly IFavoriteService _favoriteService;
        private readonly IBasketService _basketService;
        public _NavbarUILayoutComponentPartial(ICategoryService categoryService, IFavoriteService favoriteService, IBasketService basketService)
        {
            _categoryService = categoryService;
            _favoriteService = favoriteService;
            _basketService = basketService;
        }
        public async Task<IViewComponentResult> InvokeAsync()
        {
            try
            {
                var values = await _categoryService.GetAllCategoryAsync();
                ViewBag.FavoriteCount = _favoriteService.GetFavoriteCount();
                var basket = await _basketService.GetBasket();
                ViewBag.BasketCount = basket?.BasketItems?.Count ?? 0;
                return View(values);
            }
            catch (Exception)
            {
                ViewBag.FavoriteCount = 0;
                ViewBag.BasketCount = 0;
                return View(new List<ResultCategoryDto>());
            }
        }
    }
}