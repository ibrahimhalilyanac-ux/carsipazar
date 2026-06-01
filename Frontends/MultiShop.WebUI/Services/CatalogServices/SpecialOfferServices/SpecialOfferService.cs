using MultiShop.DtoLayer.CatalogDtos.SpecialOfferDtos;
using Newtonsoft.Json;

namespace MultiShop.WebUI.Services.CatalogServices.SpecialOfferServices
{
    public class SpecialOfferService: ISpecialOfferService
    {
        private readonly HttpClient _httpClient;
        public SpecialOfferService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }
        public async Task CreateSpecialOfferAsync(CreateSpecialOfferDto createSpecialOfferDto)
        {
            var response = await _httpClient.PostAsJsonAsync<CreateSpecialOfferDto>("specialoffers", createSpecialOfferDto);
            response.EnsureSuccessStatusCode();
        }
        public async Task DeleteSpecialOfferAsync(string id)
        {
            var response = await _httpClient.DeleteAsync("specialoffers?id=" + id);
            response.EnsureSuccessStatusCode();
        }
        public async Task<UpdateSpecialOfferDto> GetByIdSpecialOfferAsync(string id)
        {
            var responseMessage = await _httpClient.GetAsync("specialoffers/" + id);
            if (!responseMessage.IsSuccessStatusCode) return new UpdateSpecialOfferDto();
            var values = await responseMessage.Content.ReadFromJsonAsync<UpdateSpecialOfferDto>();
            return values ?? new UpdateSpecialOfferDto();
        }
        public async Task<List<ResultSpecialOfferDto>> GetAllSpecialOfferAsync()
        {
            var responseMessage = await _httpClient.GetAsync("specialoffers");
            if (!responseMessage.IsSuccessStatusCode) return new List<ResultSpecialOfferDto>();
            var jsonData = await responseMessage.Content.ReadAsStringAsync();
            var values = JsonConvert.DeserializeObject<List<ResultSpecialOfferDto>>(jsonData);
            return values ?? new List<ResultSpecialOfferDto>();
        }
        public async Task UpdateSpecialOfferAsync(UpdateSpecialOfferDto updateSpecialOfferDto)
        {
            var response = await _httpClient.PutAsJsonAsync<UpdateSpecialOfferDto>("specialoffers", updateSpecialOfferDto);
            response.EnsureSuccessStatusCode();
        }

    }
}
