using WebApi.Dtos.favoriteDtos;

namespace WebApi.Repositories.FavoriRepositories
{
    public interface IFavoriRepo
    {   
        Task<string> FavorilereEkle(FavoriEkleDto favoriEkleDto);
    }
}
