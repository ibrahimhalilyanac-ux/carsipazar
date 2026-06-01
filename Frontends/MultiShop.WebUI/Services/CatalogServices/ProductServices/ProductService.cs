using MultiShop.DtoLayer.CatalogDtos.ProductDtos;
using Newtonsoft.Json;

namespace MultiShop.WebUI.Services.CatalogServices.ProductServices
{
    public class ProductService : IProductService
    {
        private readonly HttpClient _httpClient;
        public ProductService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }
        public async Task CreateProductAsync(CreateProductDto createProductDto)
        {
            var response = await _httpClient.PostAsJsonAsync<CreateProductDto>("products", createProductDto);
            response.EnsureSuccessStatusCode();
        }
        public async Task DeleteProductAsync(string id)
        {
            var response = await _httpClient.DeleteAsync("products?id=" + id);
            response.EnsureSuccessStatusCode();
        }
        public async Task<UpdateProductDto> GetByIdProductAsync(string id)
        {
            var responseMessage = await _httpClient.GetAsync("products/" + id);
            if (!responseMessage.IsSuccessStatusCode) return new UpdateProductDto();
            var values = await responseMessage.Content.ReadFromJsonAsync<UpdateProductDto>();
            return values ?? new UpdateProductDto();
        }
        public async Task<List<ResultProductDto>> GetAllProductAsync()
        {
            var responseMessage = await _httpClient.GetAsync("products");
            if (!responseMessage.IsSuccessStatusCode) return new List<ResultProductDto>();
            var jsonData = await responseMessage.Content.ReadAsStringAsync();
            var values = JsonConvert.DeserializeObject<List<ResultProductDto>>(jsonData);
            return values ?? new List<ResultProductDto>();
        }
        public async Task UpdateProductAsync(UpdateProductDto updateProductDto)
        {
            var response = await _httpClient.PutAsJsonAsync<UpdateProductDto>("products", updateProductDto);
            response.EnsureSuccessStatusCode();
        }

        public async Task<List<ResultProductWithCategoryDto>> GetProductsWithCategoryAsync()
        {
            var responseMessage = await _httpClient.GetAsync("products/ProductListWithCategory");
            if (!responseMessage.IsSuccessStatusCode) return new List<ResultProductWithCategoryDto>();
            var jsonData = await responseMessage.Content.ReadAsStringAsync();
            var values = JsonConvert.DeserializeObject<List<ResultProductWithCategoryDto>>(jsonData);
            return values ?? new List<ResultProductWithCategoryDto>();
        }

        public async Task<List<ResultProductWithCategoryDto>> GetProductsWithCategoryByCatetegoryIdAsync(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                return await GetProductsWithCategoryAsync();
            }
            var responseMessage = await _httpClient.GetAsync($"products/ProductListWithCategoryByCategoryId/{id}");
            if (!responseMessage.IsSuccessStatusCode) return new List<ResultProductWithCategoryDto>();
            var jsonData = await responseMessage.Content.ReadAsStringAsync();
            var values = JsonConvert.DeserializeObject<List<ResultProductWithCategoryDto>>(jsonData);
            return values ?? new List<ResultProductWithCategoryDto>();
        }
    }
}