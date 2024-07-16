using WebApi.Dtos.kullaniciDtos;

namespace WebApi.Repositories.KullaniciRepositories
{
    public interface IKullaniciRepo
    {
        Task<(string id, string message, int statusCode)> KullaniciGirisi(KullaniciGirisDto kullaniciGirisDto);
    }
}
