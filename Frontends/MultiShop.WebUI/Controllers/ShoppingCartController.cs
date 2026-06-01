using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MultiShop.DtoLayer.BasketDtos;
using MultiShop.WebUI.Services.BasketServices;
using MultiShop.WebUI.Services.CatalogServices.ProductServices;
using MultiShop.WebUI.Services.DiscountServices;

namespace MultiShop.WebUI.Controllers
{
    public class ShoppingCartController : Controller
    {
        private readonly IProductService _productService;
        private readonly IBasketService _basketService;
        public ShoppingCartController(IProductService productService, IBasketService basketService)
        {
            _productService = productService;
            _basketService = basketService;
        }
        public async Task<IActionResult> Index(string code,int discountRate,decimal totalNewPriceWithDiscount, bool isInvalid = false)
        {
            ViewBag.code = code;
            ViewBag.discountRate = discountRate;
            ViewBag.totalNewPriceWithDiscount = totalNewPriceWithDiscount;
            ViewBag.isInvalid = isInvalid;
            ViewBag.directory1 = "Ana Sayfa";
            ViewBag.directory2 = "Ürünler";
            ViewBag.directory3 = "Sepetim";
            var values = await _basketService.GetBasket();
            ViewBag.total = values.TotalPrice;
            var totalPriceWithTax = values.TotalPrice + values.TotalPrice / 100 * 10;
            var tax = values.TotalPrice / 100 * 10;
            ViewBag.totalPriceWithTax = totalPriceWithTax;
            ViewBag.tax = tax;
            if (totalNewPriceWithDiscount == 0 && values.TotalPrice > 0)
            {
                ViewBag.totalNewPriceWithDiscount = totalPriceWithTax;
            }
            else
            {
                ViewBag.totalNewPriceWithDiscount = totalNewPriceWithDiscount;
            }
            return View();
        }

        //[HttpPost]
        [Authorize]
        public async Task<IActionResult> AddBasketItem(string id)
        {
            var values = await _productService.GetByIdProductAsync(id);
            var items = new BasketItemDto
            {
                ProductId = values.ProductId,
                ProductName = values.ProductName,
                Price = values.ProductPrice,
                Quantity = 1,
                ProductImageUrl = values.ProductImageUrl
            };
            await _basketService.AddBasketItem(items);
            return RedirectToAction("Index");
        }

        [Authorize]
        public async Task<IActionResult> RemoveBasketItem(string id)
        {
            await _basketService.RemoveBasketItem(id);
            return RedirectToAction("Index");
        }

        [Authorize]
        public async Task<IActionResult> DecreaseBasketItem(string id)
        {
            var values = await _basketService.GetBasket();
            var item = values.BasketItems.FirstOrDefault(x => x.ProductId == id);
            if (item != null)
            {
                if (item.Quantity > 1)
                {
                    item.Quantity--;
                    await _basketService.SaveBasket(values);
                }
                else
                {
                    await _basketService.RemoveBasketItem(id);
                }
            }
            return RedirectToAction("Index");
        }
    }
}
