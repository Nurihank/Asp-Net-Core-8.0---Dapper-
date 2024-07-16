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
                Console.WriteLine($"User ID: {userId}, Message: {message}");
                return Ok(new { userId, message });
            }
            else
            {
                Console.WriteLine($"Error: {message}");
                return Ok(message);
            }
        }

        [HttpPost("/api/KullaniciControllers/KullaniciKayit")]

        public async Task<IActionResult> KullaniciKayit(KullaniciKayitDto kullaniciKayitDto)
        {
            var (userId , message, statusCode) = await _kullaniciRepo.KullaniciKayit(kullaniciKayitDto);

            if(statusCode == 200)
            {
                return Ok(new{ userId , message });
            }
            else
            {
                return Ok(message);
            }
        }
    }
}
