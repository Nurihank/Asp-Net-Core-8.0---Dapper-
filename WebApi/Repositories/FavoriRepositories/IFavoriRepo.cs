using WebApi.Dtos.favoriteDtos;
using WebApi.Dtos.urun;

namespace WebApi.Repositories.FavoriRepositories
{
    public interface IFavoriRepo
    {   
        Task<bool> FavoriMiKontrol(int UrunID,int KullaniciID);
        Task<bool> FavorilereEkle(FavoriEkleDto favoriEkleDto);
        Task<bool> FavoriSil(FavoriSilDto favoriSilDto);
        Task<List<ResultUrunlerDto>> FavoriUrunleriGetir(int KullaniciID);
        Task<bool> FavoriSıfırla(FavoriSıfırlaDto favoriSıfırlaDto);
    }
}
