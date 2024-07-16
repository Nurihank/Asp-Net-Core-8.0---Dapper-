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

        [HttpPost]
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
    }
}
