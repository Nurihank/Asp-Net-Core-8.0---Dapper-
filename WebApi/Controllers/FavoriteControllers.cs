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
        public async Task<IActionResult> FavEKle(FavoriEkleDto favoriEkleDto)
        {
            string FavoriEkle = await _favoriRepo.FavorilereEkle(favoriEkleDto);

            if(FavoriEkle == "0")
            {
                return NotFound("Öyle bir ürün bulunamadı");
            }
            else if(FavoriEkle == "1")
            {
                return Ok("Bu Ürün Zaten Favorilerde");
            }
            else if (FavoriEkle == "2")
            {
                return Ok("Ürün Favorilere Eklendi");
            }
        }
    }
}
