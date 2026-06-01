using Microsoft.AspNetCore.Mvc;
using MultiShop.WebUI.Services.BasketServices;
using MultiShop.WebUI.Services.DiscountServices;

namespace MultiShop.WebUI.Controllers
{
    public class DiscountController : Controller
    {
        private readonly IDiscountService _discountService;
        private readonly IBasketService _basketService;
        public DiscountController(IDiscountService discountService, IBasketService basketService)
        {
            _discountService = discountService;
            _basketService = basketService;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var values = await _discountService.GetDiscountCouponsList();
            return View(values);
        }

        [HttpGet]
        public PartialViewResult ConfirmDiscountCoupon()
        {
            return PartialView();
        }

        [HttpPost]
        public async Task<IActionResult> ConfirmDiscountCoupon(string code)
        {
            var coupon = await _discountService.GetDiscountCode(code);
            
            if (coupon == null || string.IsNullOrEmpty(coupon.Code) || coupon.CouponId == 0 || !coupon.IsActive)
            {
                var basketValues = await _basketService.GetBasket();
                var totalPriceWithTax = basketValues.TotalPrice + basketValues.TotalPrice / 100 * 10;
                return RedirectToAction("Index", "ShoppingCart", new { code = code, discountRate = 0, totalNewPriceWithDiscount = totalPriceWithTax, isInvalid = true });
            }

            var values = coupon.Rate;

            var basketValuesForValid = await _basketService.GetBasket();
            var totalPriceWithTaxForValid = basketValuesForValid.TotalPrice + basketValuesForValid.TotalPrice / 100 * 10;

            var totalNewPriceWithDiscount = totalPriceWithTaxForValid - (totalPriceWithTaxForValid / 100 * values);

            return RedirectToAction("Index", "ShoppingCart", new { code = code, discountRate = values, totalNewPriceWithDiscount = totalNewPriceWithDiscount, isInvalid = false });
        }
    }
}
