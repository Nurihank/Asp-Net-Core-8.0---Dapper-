using Microsoft.AspNetCore.Http;
using WebApi.Dtos.kullaniciDtos;

namespace WebApi.Repositories.KullaniciRepositories
{
    public interface IKullaniciRepo
    {
        Task<(string id, string message, int statusCode,string image)> KullaniciGirisi(KullaniciGirisDto kullaniciGirisDto);
        Task<(int id,string message, int statusCode)> KullaniciKayit(KullaniciKayitDto kullaniciKayitDto);
        Task<KullaniciBilgileriDto> KullaniciBilgileri(int id);
        Task<bool> KullaniciBilgileriGuncelle(KullaniciBilgileriGuncelleDto kullaniciBilgileriGuncelleDto);
        Task<int> SifremiUnuttum(SifremiUnuttumDto sifremiUnuttumDto);
        Task<bool> SifreYenile(SifreYenileDto sifreYenileDto);
        Task<bool> SifreDegistir(SifreYenileDto sifreYenileDto);
        Task<string> ProfilResmiGetir(int KullaniciID);
        Task<bool> ProfilResmiKaydet(ProfilResmiKaydetDto profilResmiKaydetDto);
    }
}
