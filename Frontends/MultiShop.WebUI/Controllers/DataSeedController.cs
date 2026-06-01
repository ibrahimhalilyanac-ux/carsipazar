using Microsoft.AspNetCore.Mvc;
using MultiShop.DtoLayer.CatalogDtos.CategoryDtos;
using MultiShop.DtoLayer.CatalogDtos.ProductDtos;
using MultiShop.WebUI.Services.CatalogServices.CategoryServices;
using MultiShop.WebUI.Services.CatalogServices.ProductServices;
using System.Diagnostics;

namespace MultiShop.WebUI.Controllers
{
    public class DataSeedController : Controller
    {
        private readonly ICategoryService _categoryService;
        private readonly IProductService _productService;

        public DataSeedController(ICategoryService categoryService, IProductService productService)
        {
            _categoryService = categoryService;
            _productService = productService;
        }

        public async Task<IActionResult> Seed()
        {
            var resultMsg = new System.Text.StringBuilder();
            resultMsg.AppendLine("--- VERİ DOLDURMA İŞLEMİ BAŞLATILDI ---");

            try
            {
                // 1. Step: Fetching Products
                resultMsg.AppendLine("1. Mevcut ürünler çekiliyor...");
                var products = await _productService.GetAllProductAsync();
                resultMsg.AppendLine($" - {products.Count} adet mevcut ürün bulundu.");

                // 2. Step: Deleting Products
                resultMsg.AppendLine("2. Eski ürünler siliniyor...");
                foreach (var product in products)
                {
                    try { await _productService.DeleteProductAsync(product.ProductId); } catch { }
                }
                resultMsg.AppendLine(" - Ürün temizleme tamamlandı.");

                // 3. Step: Deleting Categories
                resultMsg.AppendLine("3. Eski kategoriler siliniyor...");
                var categories = await _categoryService.GetAllCategoryAsync();
                foreach (var category in categories)
                {
                    try { await _categoryService.DeleteCategoryAsync(category.CategoryID); } catch { }
                }
                resultMsg.AppendLine(" - Kategori temizleme tamamlandı.");

                // 4. Step: Creating Categories
                resultMsg.AppendLine("4. 10 yeni kategori oluşturuluyor...");
                var categoryData = new List<(string Name, string Image)>
                {
                    ("Giyim & Aksesuar", "clothes.png"),
                    ("Elektronik & Teknoloji", "laptop.png"),
                    ("Ev & Dekorasyon", "furniture.jpg"),
                    ("Ayakkabı & Çanta", "shoes.png"),
                    ("Kozmetik & Bakım", "cleanthings.jpg"),
                    ("Spor & Outdoor", "electronics.jpg"),
                    ("Kitap & Hobi", "toy.png"),
                    ("Anne & Bebek", "clothes2.png"),
                    ("Süpermarket", "fruit.png"),
                    ("Mutfak Gereçleri", "kitchen.jpg")
                };

                foreach (var cat in categoryData)
                {
                    await _categoryService.CreateCategoryAsync(new CreateCategoryDto
                    {
                        CategoryName = cat.Name,
                        ImageUrl = $"/images/categoryimages/{cat.Image}"
                    });
                }
                resultMsg.AppendLine(" - Kategoriler eklendi.");

                // 5. Step: Fetching new categories
                var addedCategories = await _categoryService.GetAllCategoryAsync();

                // 6. Step: Adding Products (Limiting to 10 products per category for stability)
                resultMsg.AppendLine("6. Ürünler ekleniyor (Her kategoriye 10 adet)...");
                var productImages = new string[] { "laptop.jpg", "jumper.jpg", "soap.jpg", "detergant.jpg", "toothpaste.jpg" };
                
                foreach (var category in addedCategories)
                {
                    for (int i = 1; i <= 10; i++)
                    {
                        await _productService.CreateProductAsync(new CreateProductDto
                        {
                            ProductName = $"Premium {category.CategoryName} - {i}",
                            ProductPrice = 500 + (i * 100),
                            ProductImageUrl = $"/images/featureproductimages/{productImages[i % productImages.Length]}",
                            ProductDescription = $"{category.CategoryName} için özel üretildi.",
                            CategoryId = category.CategoryID
                        });
                    }
                }
                resultMsg.AppendLine(" - Ürünler başarıyla eklendi.");

                return Content(resultMsg.ToString() + "\n--- İŞLEM KUSURSUZ TAMAMLANDI ---");
            }
            catch (Exception ex)
            {
                return Content(resultMsg.ToString() + $"\n!!! HATA !!!: {ex.Message}\nİç Hata: {ex.InnerException?.Message}");
            }
        }
    }
}
