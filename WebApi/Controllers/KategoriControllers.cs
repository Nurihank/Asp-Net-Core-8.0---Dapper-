using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WebApi.Dtos.kategoriDtos;
using WebApi.Repositories.KategoriRepositories;

namespace WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class KategoriControllers : ControllerBase
    {
        public readonly IKategoriRepo _kategoriRepo;

        public KategoriControllers(IKategoriRepo kategoriRepo)
        {
            _kategoriRepo = kategoriRepo;
        }


        [HttpGet]
        public async Task<IActionResult> KategoriListesi()
        {
            var values = await _kategoriRepo.GetAllKategoriAsync();
            return Ok(values);
        }

        [HttpPost]
        public async Task<IActionResult> KategoriOluşturma([FromBody]CreateKategoriDto createKategoriDto)
        {
            var values =  await _kategoriRepo.CreateKategori(createKategoriDto);
            if(values)
            {
                return Ok("Kategori Başarıyla Eklendi");
            }
            else
            {
                return Ok("Böyle bir Kategori vardır");
            }

        }

        [HttpDelete]
        public async Task<IActionResult> KategoriSilme([FromBody]DeleteKategoriDto deleteKategoriDto)
        {
            var values = await _kategoriRepo.DeleteKategori(deleteKategoriDto);
            if(values)
            {
                return Ok("Başarıyla Silindi");
            }
            else
            {
                return Ok("Böyle bir Kategori bulunamadı");
            }
        }

        [HttpPut]
        public async Task<IActionResult> KategoriGüncelleme([FromBody] UpdateKategoriDto updateKategoriDto)
        {
            var values = await _kategoriRepo.UpdateKategori(updateKategoriDto);
            if (values)
            {
                return Ok("Başarıyla Güncellendi");
            }
            else
            {
                return Ok("Böyle bir Kategori bulunamadı");
            }
        }
    }
}
