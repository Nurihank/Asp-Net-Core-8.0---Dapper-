using Dapper;
using WebApi.Dtos.kullaniciDtos;
using WebApi.Models.DapperContext;

namespace WebApi.Repositories.KullaniciRepositories
{
    public class KullaniciRepo : IKullaniciRepo
    {
        public readonly Context _context;
        public KullaniciRepo(Context context)
        {
            _context = context;
        }
        public async Task<(string id, string message, int statusCode)> KullaniciGirisi(KullaniciGirisDto kullaniciGirisDto)
        {
            string existQuery = "SELECT COUNT(1) FROM Kullanici WHERE KullaniciAdi = @KullaniciAdi";
            var existParameters = new DynamicParameters();
            existParameters.Add("@KullaniciAdi", kullaniciGirisDto.KullaniciAdi);

            string girisQuery = "SELECT Sifre FROM Kullanici WHERE KullaniciAdi = @KullaniciAdi";
            var girisParameters = new DynamicParameters();
            girisParameters.Add("@KullaniciAdi", kullaniciGirisDto.KullaniciAdi);

            using (var connection = _context.CreateConnection())
            {
                var count = await connection.ExecuteScalarAsync<int>(existQuery, existParameters);
                if (count > 0)
                {
                    var sifre = await connection.ExecuteScalarAsync<string>(girisQuery, girisParameters);

                    if (sifre == kullaniciGirisDto.Sifre)
                    {
                        string idQuery = "SELECT KullaniciID FROM Kullanici WHERE KullaniciAdi = @KullaniciAdi";
                        string id = await connection.ExecuteScalarAsync<string>(idQuery, existParameters);

                        return (id, "Başarıyla Giriş Yaptın", 200); // Success
                    }
                    else
                    {
                        return (null, "Şifre veya Kullanıcı Adı Hatalıdır", 400); // Bad Request
                    }
                }
                else
                {
                    return (null, "Böyle Bir Kullanıcı Adı Yoktur", 404); // Not Found
                }
            }
        }


    }
}
