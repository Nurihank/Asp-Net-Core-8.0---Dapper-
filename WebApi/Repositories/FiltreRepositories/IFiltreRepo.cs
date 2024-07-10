using WebApi.Dtos.filtreDto;

namespace WebApi.Repositories.FiltreRepositories
{
    public interface IFiltreRepo
    {
        Task<List<FiltreDto>> GetFiltreListAsync(int number);
        Task<List<FiltreDto>> GetFiyatAraligiListAsync(int enDüsük,int enYüksek);
    }
}
