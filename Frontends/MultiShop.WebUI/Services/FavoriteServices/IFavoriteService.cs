using MultiShop.WebUI.Models;

namespace MultiShop.WebUI.Services.FavoriteServices
{
    public interface IFavoriteService
    {
        List<FavoriteItemModel> GetFavorites();
        void AddFavorite(FavoriteItemModel item);
        void RemoveFavorite(string productId);
        bool IsFavorite(string productId);
        int GetFavoriteCount();
    }
}
