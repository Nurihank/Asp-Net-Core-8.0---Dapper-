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

        [HttpGet]
        public async Task<IActionResult> GetUrunList(int number)
        {
            Console.WriteLine(number);
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
    }
}
