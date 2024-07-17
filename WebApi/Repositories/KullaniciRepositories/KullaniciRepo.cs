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

        public async Task<(int id, string message, int statusCode)> KullaniciKayit(KullaniciKayitDto kullaniciKayitDto)
        {
            var existKullaniciAdiQuery = "SELECT COUNT(1) FROM Kullanici WHERE KullaniciAdi = @KullaniciAdi";
            var kaParameters = new DynamicParameters();
            kaParameters.Add("@KullaniciAdi", kullaniciKayitDto.KullaniciAdi);
 
            var existEpostaQuery = "SELECT COUNT(1) FROM Kullanici WHERE Eposta = @Eposta";
            var epParameters = new DynamicParameters();
            epParameters.Add("@Eposta", kullaniciKayitDto.Eposta);

            var existTelNoQuery = "SELECT COUNT(1) FROM Kullanici WHERE TelefonNo = @TelefonNo";
            var tnParameters = new DynamicParameters();
            tnParameters.Add("@TelefonNo", kullaniciKayitDto.TelNo);

            var insertQuery = "INSERT INTO Kullanici (KullaniciAdi, Eposta, Sifre, TelefonNo, Cinsiyet, Yas) VALUES (@KullaniciAdi, @Eposta, @Sifre, @TelefonNo, @Cinsiyet, @Yas)";
            var parameters = new DynamicParameters();
            parameters.Add("@KullaniciAdi", kullaniciKayitDto.KullaniciAdi);
            parameters.Add("@Eposta", kullaniciKayitDto.Eposta);
            parameters.Add("@Sifre", kullaniciKayitDto.Sifre);
            parameters.Add("@TelefonNo", kullaniciKayitDto.TelNo);
            parameters.Add("@Cinsiyet", kullaniciKayitDto.Cinsiyet);
            parameters.Add("@Yas", kullaniciKayitDto.Yas);

            using (var connection = _context.CreateConnection())
            {
                var countka = await connection.ExecuteScalarAsync<int>(existKullaniciAdiQuery, kaParameters);
                if (countka == 0)

                {
                
                    var countep = await connection.ExecuteScalarAsync<int>(existEpostaQuery, epParameters);
                    if (countep == 0)
                    {
                        
                        var counttn = await connection.ExecuteScalarAsync<int>(existTelNoQuery,tnParameters);
                        if (counttn == 0)
                        {
                          
                            await connection.ExecuteAsync(insertQuery, parameters);  //kullaniciyi ekledik

                            var getUserQuery = "SELECT KullaniciID FROM Kullanici WHERE KullaniciAdi = @KullaniciAdi";
                            int userID = await connection.ExecuteScalarAsync<int>(getUserQuery, kaParameters);
                            return (userID, "okey",200);
                        }
                        else
                        {
                            return (0, "Böyle Bir Telefon No Vardir", 0);
                        }
                    }
                    else
                    {
                        return (0, "Böyle Bir Eposta Vardir",0);
                    }
                }
                else
                {
                    return (0, "Böyle Bir Kullanici Adi Vardir", 0);
                }
            }
        }
    }
}
