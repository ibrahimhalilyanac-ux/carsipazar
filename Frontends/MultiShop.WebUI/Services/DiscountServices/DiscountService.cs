using MultiShop.DtoLayer.DiscountDtos;

namespace MultiShop.WebUI.Services.DiscountServices
{
    public class DiscountService : IDiscountService
    {
        private readonly HttpClient _httpClient;
        public DiscountService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }
        public async Task<GetDiscountCodeDetailByCode> GetDiscountCode(string code)
        {
            try
            {
                var responseMessage = await _httpClient.GetAsync("discounts/GetCodeDetailByCodeAsync?code=" + code);
                if (!responseMessage.IsSuccessStatusCode) return new GetDiscountCodeDetailByCode();
                var values = await responseMessage.Content.ReadFromJsonAsync<GetDiscountCodeDetailByCode>();
                return values ?? new GetDiscountCodeDetailByCode();
            }
            catch
            {
                return new GetDiscountCodeDetailByCode();
            }
        }

        public async Task<int> GetDiscountCouponCountRate(string code)
        {
            var responseMessage = await _httpClient.GetAsync("discounts/GetDiscountCouponCountRate?code=" + code);
            if (!responseMessage.IsSuccessStatusCode) return 0;
            var values = await responseMessage.Content.ReadFromJsonAsync<int>();
            return values;
        }

        public async Task<List<GetDiscountCodeDetailByCode>> GetDiscountCouponsList()
        {
            var responseMessage = await _httpClient.GetAsync("discounts");
            if (!responseMessage.IsSuccessStatusCode) return new List<GetDiscountCodeDetailByCode>();
            var values = await responseMessage.Content.ReadFromJsonAsync<List<GetDiscountCodeDetailByCode>>();
            return values ?? new List<GetDiscountCodeDetailByCode>();
        }
    }
}
