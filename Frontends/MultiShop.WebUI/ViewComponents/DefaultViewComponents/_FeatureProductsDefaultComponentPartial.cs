using Microsoft.AspNetCore.Mvc;
using MultiShop.DtoLayer.CatalogDtos.ProductDtos;
using MultiShop.WebUI.Services.CatalogServices.ProductServices;
using Newtonsoft.Json;

namespace MultiShop.WebUI.ViewComponents.DefaultViewComponents
{
    public class _FeatureProductsDefaultComponentPartial : ViewComponent
    {
        private readonly IProductService _productService;
        public _FeatureProductsDefaultComponentPartial(IProductService productService)
        {
            _productService = productService;
        }
        public async Task<IViewComponentResult> InvokeAsync()
        {
            try
            {
                var values = await _productService.GetAllProductAsync();
                return View(values ?? new List<ResultProductDto>());
            }
            catch (Exception)
            {
                return View(new List<ResultProductDto>());
            }
        }
    }
}
