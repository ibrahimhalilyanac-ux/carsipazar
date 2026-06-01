using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MultiShop.WebUI.Models;
using MultiShop.WebUI.Services.FavoriteServices;
using MultiShop.WebUI.Services.CatalogServices.ProductServices;
using MultiShop.WebUI.Services.BasketServices;
using MultiShop.DtoLayer.BasketDtos;

namespace MultiShop.WebUI.Controllers
{
    [Authorize]
    public class FavoriteController : Controller
    {
        private readonly IFavoriteService _favoriteService;
        private readonly IProductService _productService;
        private readonly IBasketService _basketService;

        public FavoriteController(IFavoriteService favoriteService, IProductService productService, IBasketService basketService)
        {
            _favoriteService = favoriteService;
            _productService = productService;
            _basketService = basketService;
        }

        public IActionResult Index()
        {
            ViewBag.directory1 = "Ana Sayfa";
            ViewBag.directory2 = "Ürünler";
            ViewBag.directory3 = "Favorilerim";
            var values = _favoriteService.GetFavorites();
            return View(values);
        }

        public async Task<IActionResult> AddFavorite(string id)
        {
            var product = await _productService.GetByIdProductAsync(id);
            if (product != null)
            {
                var favoriteItem = new FavoriteItemModel
                {
                    ProductId = product.ProductId,
                    ProductName = product.ProductName,
                    ProductPrice = product.ProductPrice,
                    ProductImageUrl = product.ProductImageUrl,
                    CategoryName = "" // Opsiyonel: Kategoriyi de çekebilirsiniz
                };
                _favoriteService.AddFavorite(favoriteItem);
            }
            return RedirectToAction("Index");
        }

        public IActionResult RemoveFavorite(string id)
        {
            _favoriteService.RemoveFavorite(id);
            return RedirectToAction("Index");
        }

        public async Task<IActionResult> AddToCartAndRemoveFromFavorite(string id)
        {
            var product = await _productService.GetByIdProductAsync(id);
            if (product != null)
            {
                var basketItem = new BasketItemDto
                {
                    ProductId = product.ProductId,
                    ProductName = product.ProductName,
                    Price = product.ProductPrice,
                    Quantity = 1,
                    ProductImageUrl = product.ProductImageUrl
                };
                await _basketService.AddBasketItem(basketItem);
                _favoriteService.RemoveFavorite(id);
            }
            return RedirectToAction("Index", "ShoppingCart");
        }

        [AllowAnonymous]
        [HttpGet]
        public async Task<IActionResult> ToggleFavoriteAjax(string id)
        {
            if (_favoriteService.IsFavorite(id))
            {
                _favoriteService.RemoveFavorite(id);
                return Json(new { success = true, isFavorite = false, count = _favoriteService.GetFavoriteCount() });
            }
            else
            {
                var product = await _productService.GetByIdProductAsync(id);
                if (product != null)
                {
                    var favoriteItem = new FavoriteItemModel
                    {
                        ProductId = product.ProductId,
                        ProductName = product.ProductName,
                        ProductPrice = product.ProductPrice,
                        ProductImageUrl = product.ProductImageUrl,
                        CategoryName = ""
                    };
                    _favoriteService.AddFavorite(favoriteItem);
                    return Json(new { success = true, isFavorite = true, count = _favoriteService.GetFavoriteCount() });
                }
            }
            return Json(new { success = false });
        }
    }
}
