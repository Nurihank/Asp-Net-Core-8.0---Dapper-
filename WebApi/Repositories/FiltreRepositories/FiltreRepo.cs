using Dapper;
using WebApi.Dtos.filtreDto;
using WebApi.Models.DapperContext;

namespace WebApi.Repositories.FiltreRepositories
{
    public class FiltreRepo : IFiltreRepo
    {
        public readonly Context _context;

        public FiltreRepo(Context context)
        {
            _context = context;
        }

        public async Task<List<FiltreDto>> GetFiltreListAsync(int number)
        {
            if (number == 1)
            {
                var query = "SELECT * FROM Urun ORDER BY UrunAdi ASC";

                using (var connection = _context.CreateConnection())
                {
                    var urun = await connection.QueryAsync<FiltreDto>(query);
                    return urun.ToList();
                }
            }
            else if (number == 2)
            {
                var query = "SELECT * FROM Urun ORDER BY UrunFiyati ASC";
                using (var connection = _context.CreateConnection())
                {
                    var urun = await connection.QueryAsync<FiltreDto>(query);
                        return urun.ToList();
                }
            }
            else if (number == 3)
            {
                var query = "SELECT * FROM Urun ORDER BY UrunFiyati DESC";
                using (var connection = _context.CreateConnection())
                {
                    var urun = await connection.QueryAsync<FiltreDto>(query);
                    return urun.ToList();
                }
            }
            else
            {
                return null;
            }
        }
    }
}
