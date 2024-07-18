using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WebApi.Dtos.kullaniciDtos;
using WebApi.Repositories.KullaniciRepositories;

namespace WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]


    public class KullaniciControllers : ControllerBase
    {
        private readonly IKullaniciRepo _kullaniciRepo;

        public KullaniciControllers(IKullaniciRepo kullaniciRepo)
        {
            _kullaniciRepo = kullaniciRepo;
        }

        [HttpPost("/api/KullaniciControllers/KullaniciGiris")]
        public async Task<IActionResult> KullaniciGiris([FromBody] KullaniciGirisDto kullaniciGirisDto)
        {
            var (userId, message, statusCode) = await _kullaniciRepo.KullaniciGirisi(kullaniciGirisDto);

            if (statusCode == 200)
            {
                return Ok(new { userId, message });
            }
            else
            {
                return Ok(message);
            }
        }

        [HttpPost("/api/KullaniciControllers/KullaniciKayit")]

        public async Task<IActionResult> KullaniciKayit(KullaniciKayitDto kullaniciKayitDto)
        {
            var (userId, message, statusCode) = await _kullaniciRepo.KullaniciKayit(kullaniciKayitDto);
            if (statusCode == 200)
            {
                return Ok(new { userId, message });
            }
            else
            {
                return Ok(new { message });
            }
        }

        [HttpGet("/api/KullaniciControllers/KullaniciBilgileri/{id}")]

        public async Task<IActionResult> KullaniciBilgileri(int id)
        {
            var user =  await _kullaniciRepo.KullaniciBilgileri(id);
            return Ok(user);
        }

        [HttpPut("/api/KullaniciControllers/KullaniciGuncelle")]
        public async Task<IActionResult> KullaniciGuncelle(KullaniciBilgileriGuncelleDto kullaniciBilgileriGuncelleDto)
        {
            var result = await _kullaniciRepo.KullaniciBilgileriGuncelle(kullaniciBilgileriGuncelleDto); 
            return Ok(result);
        }

        [HttpPut("/api/KullaniciControllers/KodAl")]
        public async Task<IActionResult> Sifremiunuttum(SifremiUnuttumDto sifremiUnuttumDto)
        {
            var result =  await _kullaniciRepo.SifremiUnuttum(sifremiUnuttumDto);
            return Ok(result);
        }

        [HttpPut("/api/KullaniciControllers/SifreYenile")]
        public async Task<IActionResult> SifreYenile (SifreYenileDto sifreYenileDto)
        {
            var result = await _kullaniciRepo.SifreYenile(sifreYenileDto);
            return Ok(result);
        }

        [HttpPut("/api/KullaniciControllers/SifreDegistir")]
        public async Task<IActionResult> SifreDegistir(SifreYenileDto sifreYenileDto)
        {
            var result = await _kullaniciRepo.SifreDegistir(sifreYenileDto);
            return Ok(result);
        }
    }
}
