using MultiShop.DtoLayer.CatalogDtos.CategoryDtos;
using Newtonsoft.Json;

namespace MultiShop.WebUI.Services.CatalogServices.CategoryServices
{
    public class CategoryService : ICategoryService
    {
        private readonly HttpClient _httpClient;
        public CategoryService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }
        public async Task CreateCategoryAsync(CreateCategoryDto createCategoryDto)
        {
            var response = await _httpClient.PostAsJsonAsync<CreateCategoryDto>("categories", createCategoryDto);
            response.EnsureSuccessStatusCode();
        }
        public async Task DeleteCategoryAsync(string id)
        {
            var response = await _httpClient.DeleteAsync("categories?id=" + id);
            response.EnsureSuccessStatusCode();
        }
        public async Task<UpdateCategoryDto> GetByIdCategoryAsync(string id)
        {
            var responseMessage = await _httpClient.GetAsync("categories/" + id);
            if (!responseMessage.IsSuccessStatusCode) return new UpdateCategoryDto();
            var values = await responseMessage.Content.ReadFromJsonAsync<UpdateCategoryDto>();
            return values ?? new UpdateCategoryDto();
        }
        public async Task<List<ResultCategoryDto>> GetAllCategoryAsync()
        {
            var responseMessage = await _httpClient.GetAsync("categories");
            if (!responseMessage.IsSuccessStatusCode) return new List<ResultCategoryDto>();
            var jsonData = await responseMessage.Content.ReadAsStringAsync();
            var values = JsonConvert.DeserializeObject<List<ResultCategoryDto>>(jsonData);
            return values ?? new List<ResultCategoryDto>();
        }
        public async Task UpdateCategoryAsync(UpdateCategoryDto updateCategoryDto)
        {
            var response = await _httpClient.PutAsJsonAsync<UpdateCategoryDto>("categories", updateCategoryDto);
            response.EnsureSuccessStatusCode();
        }
    }
}
