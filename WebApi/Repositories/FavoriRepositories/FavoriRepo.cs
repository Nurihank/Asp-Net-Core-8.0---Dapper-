using Dapper;
using WebApi.Dtos.favoriteDtos;
using WebApi.Dtos.urun;
using WebApi.Models.DapperContext;

namespace WebApi.Repositories.FavoriRepositories
{
    public class FavoriRepo : IFavoriRepo
    {
        public readonly Context _context;

        public  FavoriRepo(Context context) {  _context = context; }

        public async Task<bool> FavorilereEkle(FavoriEkleDto favoriEkleDto)
        {

            if (favoriEkleDto.UrunID == 0 && favoriEkleDto.KullaniciID == 0)
            {
                return false;
            }
            var query = "INSERT INTO FavoriUrunler (UrunID , KullaniciID) VALUES(@UrunID,@KullaniciID)";
            var parameters = new DynamicParameters();
            parameters.Add("@UrunID", favoriEkleDto.UrunID);
            parameters.Add("@KullaniciID", favoriEkleDto.KullaniciID);
 
            using (var connection = _context.CreateConnection())
            {
                    await connection.ExecuteAsync(query,parameters);
                    return true;
            
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

        public async Task<bool> FavoriSil(FavoriSilDto favoriSilDto)
        {
            var query = "DELETE FROM FavoriUrunler WHERE UrunID = @UrunID AND KullaniciID = @KullaniciID";
            var parameters = new DynamicParameters();
            parameters.Add("@UrunID", favoriSilDto.UrunID);
            parameters.Add("@KullaniciID", favoriSilDto.KullaniciID);

            using(var connections = _context.CreateConnection())
            {
                await connections.ExecuteAsync(query, parameters);
                return true;
            }
        }

        public async Task<bool> FavoriSıfırla(FavoriSıfırlaDto favoriSıfırlaDto)
        {
            var query = "DELETE FROM FavoriUrunler WHERE KullaniciID=@KullaniciID";
            var parameters = new DynamicParameters();
            parameters.Add("@KullaniciID",favoriSıfırlaDto.KullaniciID);
           
            using(var connection = _context.CreateConnection())
            {
                var rowsAffected = await connection.ExecuteAsync(query, parameters);
                Console.WriteLine(rowsAffected);
                return rowsAffected > 0;
            }
        }

        public async Task<List<ResultUrunlerDto>> FavoriUrunleriGetir(int KullaniciID)
        {
            var query = "SELECT UrunID FROM FavoriUrunler WHERE KullaniciID = @KullaniciID";
            var parameters = new DynamicParameters();
            parameters.Add("@KullaniciID", KullaniciID);

            using (var connection = _context.CreateConnection())
            {
                var urunIds = await connection.QueryAsync<int>(query, parameters);

                if (!urunIds.Any())
                {
                    return new List<ResultUrunlerDto>();
                }

                var inClause = string.Join(",", urunIds);
                var getQuery = $"SELECT * FROM Urun WHERE UrunID IN ({inClause})"; //FAVORİ ÜRÜNLERİ GETİRİYOR
                var values = await connection.QueryAsync<ResultUrunlerDto>(getQuery);
                return values.ToList();
            }
        }

    }
}
