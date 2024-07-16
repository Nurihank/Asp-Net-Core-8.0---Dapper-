using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WebApi.Dtos.favoriteDtos;
using WebApi.Repositories.FavoriRepositories;

namespace WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FavoriteControllers : ControllerBase
    {
        private readonly IFavoriRepo _favoriRepo;
             
        public FavoriteControllers(IFavoriRepo favoriRepo)
        {
            _favoriRepo = favoriRepo;
        }        

        [HttpPut]
        public async Task<IActionResult> FavoriEKle(FavoriEkleDto favoriEkleDto)
        {
            string FavoriEkle = await _favoriRepo.FavorilereEkle(favoriEkleDto);
            Console.WriteLine(FavoriEkle);
            if(FavoriEkle == "0")
            {
                return NotFound("Öyle bir ürün bulunamadı");
            }
            else if(FavoriEkle == "1")
            {
                return Ok("Ürün favorilerden kaldırıldı");
            }
            else if (FavoriEkle == "2")
            {
                return Ok("Ürün Favorilere Eklendi");
            }
            else
            {
                return BadRequest("Hiç");
            }
        }

        [HttpGet]
        public async Task<IActionResult> FavorileriGetir()
        {
            var result = await _favoriRepo.FavoriUrunleriGetir();
            return Ok(result);
        }

        [HttpGet("/api/FavoriteControllers/FavoriMi")]

        public async Task<IActionResult> FavoriMi(int UrunID, int KullaniciID)
        {
            var result = await _favoriRepo.FavoriMiKontrol(UrunID,KullaniciID);
            if (result)
            {
                return Ok(true);
            }
            else
            {
                return Ok(false);
            }
        }
    }
}
