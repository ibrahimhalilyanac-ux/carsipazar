using MultiShop.WebUI.Models;
using MultiShop.WebUI.Services.Interfaces;
using System.Text.Json;

namespace MultiShop.WebUI.Services.FavoriteServices
{
    /// <summary>
    /// Favorileri kullanıcıya özgü cookie'de saklar.
    /// Cookie adı = "Favorites_{UserId}" — her kullanıcı kendi favorilerine sahip olur.
    /// </summary>
    public class FavoriteService : IFavoriteService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly ILoginService _loginService;

        public FavoriteService(IHttpContextAccessor httpContextAccessor, ILoginService loginService)
        {
            _httpContextAccessor = httpContextAccessor;
            _loginService = loginService;
        }

        private string CookieKey
        {
            get
            {
                var userId = _loginService.GetUserId;
                if (string.IsNullOrEmpty(userId))
                {
                    return "Favorites_guest";
                }
                return $"Favorites_{userId}";
            }
        }

        public List<FavoriteItemModel> GetFavorites()
        {
            var context = _httpContextAccessor.HttpContext;
            var raw = context.Request.Cookies[CookieKey];
            if (string.IsNullOrEmpty(raw))
                return new List<FavoriteItemModel>();

            try
            {
                return JsonSerializer.Deserialize<List<FavoriteItemModel>>(raw)
                       ?? new List<FavoriteItemModel>();
            }
            catch
            {
                return new List<FavoriteItemModel>();
            }
        }

        public void AddFavorite(FavoriteItemModel item)
        {
            var favorites = GetFavorites();
            if (!favorites.Any(f => f.ProductId == item.ProductId))
            {
                favorites.Add(item);
                SaveFavorites(favorites);
            }
        }

        public void RemoveFavorite(string productId)
        {
            var favorites = GetFavorites();
            var toRemove = favorites.FirstOrDefault(f => f.ProductId == productId);
            if (toRemove != null)
            {
                favorites.Remove(toRemove);
                SaveFavorites(favorites);
            }
        }

        public bool IsFavorite(string productId)
        {
            return GetFavorites().Any(f => f.ProductId == productId);
        }

        public int GetFavoriteCount()
        {
            return GetFavorites().Count;
        }

        private void SaveFavorites(List<FavoriteItemModel> favorites)
        {
            var context = _httpContextAccessor.HttpContext;
            var json = JsonSerializer.Serialize(favorites);
            context.Response.Cookies.Append(CookieKey, json, new CookieOptions
            {
                Expires = DateTimeOffset.UtcNow.AddDays(30),
                HttpOnly = true,
                SameSite = SameSiteMode.Lax
            });
        }
    }
}
