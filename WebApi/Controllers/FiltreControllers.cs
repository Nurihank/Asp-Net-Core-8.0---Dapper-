using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WebApi.Repositories.FiltreRepositories;

namespace WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FiltreControllers : ControllerBase
    {
        public readonly IFiltreRepo _filtreRepo;

        public FiltreControllers(IFiltreRepo filtreRepo)
        {
            _filtreRepo = filtreRepo;
        }

        [HttpGet("{number}")]
        public async Task<IActionResult> GetFiltreliUrunList(int number)
        {
            var values = await _filtreRepo.GetFiltreListAsync(number); 
            if(values == null)
            {
                return NotFound();
            }
            else
            {
                return Ok(values);
            }
        }

        [HttpGet("/api/FiltreControllers/FiyatAraligi/{enDüsük}/{enYüksek}")]
         public async Task<IActionResult> GetFiyatAraligiUrunList(int enDüsük, int enYüksek)
        {
            var values = await _filtreRepo.GetFiyatAraligiListAsync(enDüsük, enYüksek);
            if (values == null)
            {
                return NotFound();
            }
            else
            {
                return Ok(values);
            }
        }
    }
}
