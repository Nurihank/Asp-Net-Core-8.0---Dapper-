using WebApi.Dtos.filtreDto;

namespace WebApi.Repositories.FiltreRepositories
{
    public interface IFiltreRepo
    {
        Task<FiltreDto> GetFiltreListAsync(int number);
    }
}
