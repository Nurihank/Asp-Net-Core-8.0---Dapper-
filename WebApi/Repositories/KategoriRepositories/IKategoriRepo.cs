using WebApi.Dtos.kategoriDtos;

namespace WebApi.Repositories.KategoriRepositories
{
    public interface IKategoriRepo
    {
        Task<List<GetKategoriDto>> GetAllKategoriAsync();
        Task<bool> CreateKategori(CreateKategoriDto createKategoriDto);
        Task<bool> DeleteKategori(DeleteKategoriDto deleteKategoriDto);
        Task<bool> UpdateKategori(UpdateKategoriDto updateKategoriDto);
    }
}
