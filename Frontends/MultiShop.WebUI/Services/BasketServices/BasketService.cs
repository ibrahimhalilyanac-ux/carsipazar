using MultiShop.DtoLayer.BasketDtos;

namespace MultiShop.WebUI.Services.BasketServices
{
    public class BasketService : IBasketService
    {
        private readonly HttpClient _httpClient;
        public BasketService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }
        public async Task AddBasketItem(BasketItemDto basketItemDto)
        {
            var values = await GetBasket();
            if (values == null)
            {
                values = new BasketTotalDto();
                values.BasketItems = new List<BasketItemDto>();
            }
            
            if (!values.BasketItems.Any(x => x.ProductId == basketItemDto.ProductId))
            {
                values.BasketItems.Add(basketItemDto);
            }
            else
            {
                var existingItem = values.BasketItems.FirstOrDefault(x => x.ProductId == basketItemDto.ProductId);
                if (existingItem != null)
                {
                    existingItem.Quantity += basketItemDto.Quantity;
                }
            }
            await SaveBasket(values);
        }

        public async Task DeleteBasket(string userId)
        {
            var response = await _httpClient.DeleteAsync("baskets");
            response.EnsureSuccessStatusCode();
        }

        public async Task<BasketTotalDto> GetBasket()
        {
            var responseMessage = await _httpClient.GetAsync("baskets");
            if (!responseMessage.IsSuccessStatusCode) return new BasketTotalDto { BasketItems = new List<BasketItemDto>() };
            var values = await responseMessage.Content.ReadFromJsonAsync<BasketTotalDto>();
            return values ?? new BasketTotalDto { BasketItems = new List<BasketItemDto>() };
        }

        public async Task<bool> RemoveBasketItem(string productId)
        {
            var values = await GetBasket();
            if (values == null || values.BasketItems == null) return false;
            var deletedItem = values.BasketItems.FirstOrDefault(x => x.ProductId == productId);
            if (deletedItem == null) return false;
            var result = values.BasketItems.Remove(deletedItem);
            await SaveBasket(values);
            return result;
        }

        public async Task SaveBasket(BasketTotalDto basketTotalDto)
        {
            var response = await _httpClient.PostAsJsonAsync<BasketTotalDto>("baskets", basketTotalDto);
            response.EnsureSuccessStatusCode();
        }
    }
}
