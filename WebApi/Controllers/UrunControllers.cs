using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WebApi.Dtos.urun;
using WebApi.Dtos.urunDtos;
using WebApi.Repositories.UrunRepositories;

namespace WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UrunControllers : ControllerBase
    {
        private readonly IUrunRepo _urunRepo;

        public UrunControllers(IUrunRepo urunRepo)
        {
            _urunRepo = urunRepo;
        }

        [HttpGet]
        public async Task<IActionResult> UrunlerListesi()
        {
            try
            {
                var values = await _urunRepo.GetResultUrunlersAsync();
                return Ok(values);
            }
            catch (Exception ex)
            {
                // Log the error (you can use any logging library, e.g., Serilog)
                Console.WriteLine($"Error: {ex.Message}");
                return StatusCode(500, "Internal server error");
            }
        }


        [HttpPost]

        public async Task<IActionResult> UrunleriOlustur(CreateUrunlerDto createUrunlerDto)
        {
            _urunRepo.CreateUrun(createUrunlerDto);
            return Ok("Başarıyla eklendi");
        }

        [HttpDelete]
        public async Task<IActionResult> UrunleriSil([FromBody] DeleteUrunDto deleteUrunDto)
        {
            bool isDeleted = await _urunRepo.DeleteUrunler(deleteUrunDto);

            if (isDeleted)
            {
                return Ok("Ürün başarıyla silindi");
            }
            else
            {
                return NotFound("Öyle bir ürün bulunamadı");
            }
        }

        [HttpPut]

        public async Task<IActionResult> UrunleriGuncelle(UpdateUrunlerDto updateUrunlerDto)
        {
            bool isGuncelle = await _urunRepo.UpdateUrunler(updateUrunlerDto);
            if (isGuncelle)
            {
                return Ok("Başarıyla Güncellendi");
            }
            else
            {
                return NotFound("Böyle bir ürün bulunamadı");
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetUrunById(int id)
        {
            var urun = await _urunRepo.GetUrunByIdAsync(id);

            if (urun == null)
            {
                return NotFound("Ürün bulunamadı");
            }

            return Ok(urun);
        }
    }
}
