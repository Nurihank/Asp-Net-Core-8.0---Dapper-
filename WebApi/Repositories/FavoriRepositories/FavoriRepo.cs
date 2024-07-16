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

            var checkQuery = "SELECT CASE WHEN UrunFavori = 'true' THEN 'true'  ELSE 'false'  END AS IsFavori FROM Urun WHERE UrunID = @UrunID";
            var cparameters = new DynamicParameters();
            cparameters.Add("@UrunID", favoriEkleDto.UrunID);

            var updateQuery = "UPDATE Urun Set UrunFavori = @UrunFavori WHERE UrunID = @UrunID ";
            var parameters = new DynamicParameters();   
            parameters.Add("@UrunID", favoriEkleDto.UrunID);
            parameters.Add("@UrunFavori", favoriEkleDto.UrunFavori);

            using (var connection = _context.CreateConnection())
            {
                var exist = await connection.ExecuteScalarAsync<int>(selectedQuery, sparameters);
                var check = await connection.ExecuteScalarAsync<string>(checkQuery, cparameters);

                Console.WriteLine(check);
                Console.WriteLine(check);

                if (exist == 0)
                {
                    return "0"; // Product does not exist
                }
                else if (check.Equals("true")) // Check against string "true"
                {
                    Console.WriteLine("burda");
                    var updateDeleteQuery = "UPDATE Urun Set UrunFavori = 0 WHERE UrunID = @UrunID ";
                    var DUparameters = new DynamicParameters();
                    DUparameters.Add("@UrunID", favoriEkleDto.UrunID);
                    Console.WriteLine(favoriEkleDto.UrunID);
                    try
                    {
                        await connection.ExecuteAsync(updateDeleteQuery, DUparameters);
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Error updating UrunFavori: {ex.Message}");
                        throw; // Optional: rethrow if you want to handle it upstream
                    } 
                    return "1"; // Already a favorite
                }
                else
                {
                    await connection.ExecuteAsync(updateQuery, parameters);
                    return "2"; // Successfully updated
                }
            }
        }

        public async Task<bool> FavoriMiKontrol(int UrunID, int KullaniciID)
        {
            string checkQuery = "SELECT COUNT(1) FROM FavoriUrunler WHERE UrunID = @UrunID AND KullaniciID = @KullaniciID";
            var parameters = new DynamicParameters();
            parameters.Add("@UrunID",UrunID);
            parameters.Add("@KullaniciID", KullaniciID);

            using(var connection = _context.CreateConnection())
            {
                var count = await connection.ExecuteScalarAsync<int>(checkQuery, parameters); 
                Console.WriteLine(count);
                if(count > 0)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }

        }

        public async Task<List<FavoriUrunleriGetirDto>> FavoriUrunleriGetir()
        {
            string query = "SELECT * FROM Urun WHERE UrunFavori = 1";

            using(var connection = _context.CreateConnection())
            {
                var result = await connection.QueryAsync<FavoriUrunleriGetirDto>(query);
                return result.ToList();
            }
        }
    }
}
