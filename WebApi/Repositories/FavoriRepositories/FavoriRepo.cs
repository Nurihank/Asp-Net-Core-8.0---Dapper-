using Dapper;
using WebApi.Dtos.favoriteDtos;
using WebApi.Models.DapperContext;

namespace WebApi.Repositories.FavoriRepositories
{
    public class FavoriRepo : IFavoriRepo
    {
        public readonly Context _context;

        public FavoriRepo(Context context) {  _context = context; }

        public async Task<string> FavorilereEkle(FavoriEkleDto favoriEkleDto)
        {
            var selectedQuery = "SELECT COUNT(1) FROM Urun WHERE UrunID = @UrunID";
            var sparameters = new DynamicParameters();
            sparameters.Add("@UrunID", favoriEkleDto.UrunID);

            var checkQuery = "SELECT UrunFavori FROM Urun Where UrunID = @UrunID";
            var cparameters = new DynamicParameters();
            cparameters.Add("@UrunID", favoriEkleDto.UrunID);
         

            var updateQuery = "UPDATE Urun (UrunFavori) VALUES(@UrunFavori) WHERE UrunID = @id";
            var parameters = new DynamicParameters();
            parameters.Add("@UrunID", favoriEkleDto.UrunID);
            parameters.Add("@UrunFavori", favoriEkleDto.UrunFavori);

            using (var connection = _context.CreateConnection())
            {
                var exist = await connection.ExecuteScalarAsync<int>(selectedQuery, sparameters);
                var check = await connection.ExecuteScalarAsync<string>(checkQuery, cparameters);
                Console.Write(check);
                if (exist == 0)
                {
                    return "0";
                }
                else if(check == "true")
                {
                    return "1";
                }
                else
                {
                    await connection.ExecuteAsync(updateQuery, parameters);
                    return "2";
                }
            }


            
        }
    }
}
