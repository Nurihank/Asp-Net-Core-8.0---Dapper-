using WebApi.Dtos.urun;
using WebApi.Dtos.urunDtos;

namespace WebApi.Repositories.UrunRepositories
{
    public interface IUrunRepo
    {
        Task<List<ResultUrunlerDto>> GetResultUrunlersAsync();
        Task<bool> CreateUrun(CreateUrunlerDto createUrunlerDto);
        Task<bool> DeleteUrunler(DeleteUrunDto deleteUrunDto);
        // Metodu async hale getirdik ve Task<bool> dönecek şekilde güncelledik
        Task<bool> UpdateUrunler(UpdateUrunlerDto updateUrunlerDto);
        Task<ResultUrunlerDto> GetUrunByBarCodeAsync(string UrunBarcode);
        Task<List<ResultUrunlerDto>> GetUrunByNameAsync(string UrunAdi);
    }
}
