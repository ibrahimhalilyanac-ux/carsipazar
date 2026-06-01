using MultiShop.DtoLayer.CatalogDtos.ContactDtos;
using Newtonsoft.Json;

namespace MultiShop.WebUI.Services.CatalogServices.ContactServices
{
    public class ContactService:IContactService
    {
        private readonly HttpClient _httpClient;
        public ContactService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }
        public async Task CreateContactAsync(CreateContactDto createContactDto)
        {
            var response = await _httpClient.PostAsJsonAsync<CreateContactDto>("contacts", createContactDto);
            response.EnsureSuccessStatusCode();
        }
        public async Task DeleteContactAsync(string id)
        {
            var response = await _httpClient.DeleteAsync("contacts?id=" + id);
            response.EnsureSuccessStatusCode();
        }
        public async Task<GetByIdContactDto> GetByIdContactAsync(string id)
        {
            var responseMessage = await _httpClient.GetAsync("contacts/" + id);
            if (!responseMessage.IsSuccessStatusCode) return new GetByIdContactDto();
            var values = await responseMessage.Content.ReadFromJsonAsync<GetByIdContactDto>();
            return values ?? new GetByIdContactDto();
        }
        public async Task<List<ResultContactDto>> GetAllContactAsync()
        {
            var responseMessage = await _httpClient.GetAsync("contacts");
            if (!responseMessage.IsSuccessStatusCode) return new List<ResultContactDto>();
            var jsonData = await responseMessage.Content.ReadAsStringAsync();
            var values = JsonConvert.DeserializeObject<List<ResultContactDto>>(jsonData);
            return values ?? new List<ResultContactDto>();
        }
        public async Task UpdateContactAsync(UpdateContactDto updateContactDto)
        {
            var response = await _httpClient.PutAsJsonAsync<UpdateContactDto>("contacts", updateContactDto);
            response.EnsureSuccessStatusCode();
        }
    }
}
