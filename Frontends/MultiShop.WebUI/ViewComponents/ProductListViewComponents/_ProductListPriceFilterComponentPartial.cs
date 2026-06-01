using Microsoft.AspNetCore.Mvc;
using MultiShop.WebUI.Services.CatalogServices.CategoryServices;

namespace MultiShop.WebUI.ViewComponents.ProductListViewComponents
{
    public class _ProductListPriceFilterComponentPartial : ViewComponent
    {
        private readonly ICategoryService _categoryService;
        public _ProductListPriceFilterComponentPartial(ICategoryService categoryService)
        {
            _categoryService = categoryService;
        }

        public async Task<IViewComponentResult> InvokeAsync(string id)
        {
            try
            {
                if (!string.IsNullOrEmpty(id))
                {
                    var category = await _categoryService.GetByIdCategoryAsync(id);
                    ViewBag.CategoryName = category?.CategoryName;
                }
                return View();
            }
            catch
            {
                return View();
            }
        }
    }
}
