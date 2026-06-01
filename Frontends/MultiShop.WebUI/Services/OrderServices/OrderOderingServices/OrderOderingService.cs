using MultiShop.DtoLayer.CommentDtos;
using MultiShop.DtoLayer.OrderDtos.OrderOrderingDtos;
using Newtonsoft.Json;

namespace MultiShop.WebUI.Services.OrderServices.OrderOderingServices
{
    public class OrderOderingService : IOrderOderingService
    {
        private readonly HttpClient _httpClient;
        public OrderOderingService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }
        public async Task<List<ResultOrderingByUserIdDto>> GetOrderingByUserId(string id)
        {
            var responseMessage = await _httpClient.GetAsync($"orderings/GetOrderingByUserId/{id}");
            if (!responseMessage.IsSuccessStatusCode) return new List<ResultOrderingByUserIdDto>();
            var jsonData = await responseMessage.Content.ReadAsStringAsync();
            var values = JsonConvert.DeserializeObject<List<ResultOrderingByUserIdDto>>(jsonData);
            return values ?? new List<ResultOrderingByUserIdDto>();
        }
        public async Task<List<ResultOrderDetailDto>> GetOrderDetailsByOrderingId(int orderingId)
        {
            var responseMessage = await _httpClient.GetAsync("OrderDetails");
            if (!responseMessage.IsSuccessStatusCode) return new List<ResultOrderDetailDto>();
            var jsonData = await responseMessage.Content.ReadAsStringAsync();
            var values = JsonConvert.DeserializeObject<List<ResultOrderDetailDto>>(jsonData);
            return values?.Where(x => x.OrderingId == orderingId).ToList() ?? new List<ResultOrderDetailDto>();
        }
    }
}
