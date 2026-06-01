using MultiShop.DtoLayer.CatalogDtos.FeatureSliderDtos;
using Newtonsoft.Json;

namespace MultiShop.WebUI.Services.CatalogServices.FeatureSliderServices
{
    public class FeatureSliderService : IFeatureSliderService
    {
        private readonly HttpClient _httpClient;
        public FeatureSliderService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }
        public async Task CreateFeatureSliderAsync(CreateFeatureSliderDto createFeatureSliderDto)
        {
            var response = await _httpClient.PostAsJsonAsync<CreateFeatureSliderDto>("featuresliders", createFeatureSliderDto);
            response.EnsureSuccessStatusCode();
        }
        public async Task DeleteFeatureSliderAsync(string id)
        {
            var response = await _httpClient.DeleteAsync("featuresliders?id=" + id);
            response.EnsureSuccessStatusCode();
        }
        public async Task<UpdateFeatureSliderDto> GetByIdFeatureSliderAsync(string id)
        {
            var responseMessage = await _httpClient.GetAsync("featuresliders/" + id);
            if (!responseMessage.IsSuccessStatusCode) return new UpdateFeatureSliderDto();
            var values = await responseMessage.Content.ReadFromJsonAsync<UpdateFeatureSliderDto>();
            return values ?? new UpdateFeatureSliderDto();
        }
        public async Task<List<ResultFeatureSliderDto>> GetAllFeatureSliderAsync()
        {
            var responseMessage = await _httpClient.GetAsync("featuresliders");
            if (!responseMessage.IsSuccessStatusCode) return new List<ResultFeatureSliderDto>();
            var jsonData = await responseMessage.Content.ReadAsStringAsync();
            var values = JsonConvert.DeserializeObject<List<ResultFeatureSliderDto>>(jsonData);
            return values ?? new List<ResultFeatureSliderDto>();
        }
        public async Task UpdateFeatureSliderAsync(UpdateFeatureSliderDto updateFeatureSliderDto)
        {
            var response = await _httpClient.PutAsJsonAsync<UpdateFeatureSliderDto>("featuresliders", updateFeatureSliderDto);
            response.EnsureSuccessStatusCode();
        }

        public Task FeatureSliderChageStatusToTrue(string id)
        {
            throw new NotImplementedException();
        }

        public Task FeatureSliderChageStatusToFalse(string id)
        {
            throw new NotImplementedException();
        }
    }
}
