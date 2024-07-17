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

        [HttpPut("/api/FavoriteControllers/FavoriEkle")]
       public async Task<IActionResult> FavoriEkle(FavoriEkleDto favoriEkleDto)
        {
            var result = await _favoriRepo.FavorilereEkle(favoriEkleDto);
            if(result == true)
            {
                return Ok("Favoriye Eklendi");
            }
            else
            {
                return Ok("Başarısız Oldu");
            }
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

        [HttpDelete("/api/FavoriteControllers/FavoriSil")]
        public async Task<IActionResult> FavoriSil(FavoriSilDto favoriSilDto)
        {
            var result =  await _favoriRepo.FavoriSil(favoriSilDto);
            return Ok(result);
        }

        [HttpGet("/api/FavoriteControllers/FavoriUrunler")]
        public async Task<IActionResult> FavoriUrunler(int kullaniciId)
        {
            var result = await _favoriRepo.FavoriUrunleriGetir(kullaniciId);
            return Ok(result);
        }

    }
}
