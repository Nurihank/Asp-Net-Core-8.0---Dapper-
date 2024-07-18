using Dapper;
using WebApi.Dtos.kullaniciDtos;
using WebApi.Models.DapperContext;
using System.Net.Mail;
using System.Net;
using System.Text;
using static Org.BouncyCastle.Crypto.Engines.SM2Engine;

namespace WebApi.Repositories.KullaniciRepositories
{
    public class KullaniciRepo : IKullaniciRepo
    {
        public readonly Context _context;
        public KullaniciRepo(Context context)
        {
            _context = context;
        }

        public async Task<KullaniciBilgileriDto> KullaniciBilgileri(int id)
        {
            var query = "SELECT * FROM Kullanici WHERE KullaniciID = @KullaniciID";
            var parameters = new DynamicParameters();
            parameters.Add("@KullaniciID", id);

            using (var connection = _context.CreateConnection())
            {
                var user = await connection.QuerySingleOrDefaultAsync<KullaniciBilgileriDto>(query, parameters);
                return user;
            }
        }

        public async Task<bool> KullaniciBilgileriGuncelle(KullaniciBilgileriGuncelleDto kullaniciBilgileriGuncelleDto)
        {
            var updateQuery = "UPDATE Kullanici SET Yas = @Yas, TelefonNo = @telno, Cinsiyet = @cinsiyet WHERE KullaniciID = @kid";
            var parameters = new DynamicParameters();
            parameters.Add("@Yas", kullaniciBilgileriGuncelleDto.Yas);
            parameters.Add("@telno", kullaniciBilgileriGuncelleDto.TelefonNo);
            parameters.Add("@cinsiyet", kullaniciBilgileriGuncelleDto.Cinsiyet);
            parameters.Add("@kid", kullaniciBilgileriGuncelleDto.KullaniciID);

            Console.WriteLine(kullaniciBilgileriGuncelleDto.Yas);
            Console.WriteLine(kullaniciBilgileriGuncelleDto.TelefonNo);
            Console.WriteLine(kullaniciBilgileriGuncelleDto.Cinsiyet);
            Console.WriteLine(kullaniciBilgileriGuncelleDto.KullaniciID);
            using (var connection = _context.CreateConnection())
            {
                var affectedRows = await connection.ExecuteAsync(updateQuery, parameters);
                Console.WriteLine(affectedRows);
                if(affectedRows > 0)
                {
                    return true;
                }
                else
                {
                    return false;
                }
                
            }
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
            tnParameters.Add("@TelefonNo", kullaniciKayitDto.TelefonNo);

            var insertQuery = "INSERT INTO Kullanici (KullaniciAdi, Eposta, Sifre, TelefonNo, Cinsiyet, Yas) VALUES (@KullaniciAdi, @Eposta, @Sifre, @TelefonNo, @Cinsiyet, @Yas)";
            var parameters = new DynamicParameters();
            parameters.Add("@KullaniciAdi", kullaniciKayitDto.KullaniciAdi);
            parameters.Add("@Eposta", kullaniciKayitDto.Eposta);
            parameters.Add("@Sifre", kullaniciKayitDto.Sifre);
            parameters.Add("@TelefonNo", kullaniciKayitDto.TelefonNo);
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

        public async Task<bool> SifreDegistir(SifreYenileDto sifreYenileDto)
        {
            var query = "UPDATE Kullanici SET Sifre = @Sifre WHERE KullaniciAdi = @KullaniciAdi";
            var parameters = new DynamicParameters();
            parameters.Add("@Sifre", sifreYenileDto.Sifre);
            parameters.Add("@KullaniciAdi", sifreYenileDto.KullaniciAdi);

            using(var connection = _context.CreateConnection())
            {
                await connection.ExecuteScalarAsync<bool>(query, parameters);
                return true;
            }
        }

        public async Task<int> SifremiUnuttum(SifremiUnuttumDto sifremiUnuttumDto)
        {
            var query = "SELECT Eposta FROM Kullanici WHERE KullaniciAdi = @KullaniciAdi AND Eposta = @Eposta";
            var parameters = new DynamicParameters();
            parameters.Add("@KullaniciAdi", sifremiUnuttumDto.KullaniciAdi);
            parameters.Add("@Eposta", sifremiUnuttumDto.Eposta);
           

            using (var connection = _context.CreateConnection())
            {
                var result = await connection.ExecuteScalarAsync<string>(query,parameters);
                Console.WriteLine(result);
                if(result == null)
                {
                    return 0;
                    
                }
                else
                {

                    var random = new Random();
                    int code = random.Next(100000, 999999);
                    var subject = "Sifremi Unuttum Kodu";
                    var message = $"İşte girmeniz gereken kod = {code}";
                    var client = new SmtpClient("smtp.gmail.com", 587)
                    {
                        EnableSsl = true,
                        Credentials = new NetworkCredential("kavalcinurihan01@gmail.com", "avlf fwny yfbe efiz")
                    };

                    var mailMessage = new MailMessage
                    {
                        From = new MailAddress("kavalcinurihan01@gmail.com"),
                        Subject = subject,
                        Body = message,
                        IsBodyHtml = true
                    };

                    mailMessage.To.Add(sifremiUnuttumDto.Eposta);

                    try
                    {
                        await client.SendMailAsync(mailMessage);
                        var codeUpdateQuery = $"UPDATE Kullanici SET SifremiUnuttumKod = {code} WHERE KullaniciAdi = @KullaniciAdi";
                        var uparameters = new DynamicParameters();
                        uparameters.Add("@KullaniciAdi", sifremiUnuttumDto.KullaniciAdi);
                        await connection.ExecuteScalarAsync<string>(codeUpdateQuery, uparameters);
                        return code;
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Mail gönderilirken hata: {ex.Message}");
                        return 0;
                    }
                }
                
            }



        }

        public async Task<bool> SifreYenile(SifreYenileDto sifreYenileDto)
        {
            var query = "UPDATE Kullanici SET Sifre = @Sifre WHERE KullaniciAdi = @KullaniciAdi";
            var parameters = new DynamicParameters();
            parameters.Add("@Sifre", sifreYenileDto.Sifre);
            parameters.Add("@KullaniciAdi", sifreYenileDto.KullaniciAdi);

            using(var connection = _context.CreateConnection())
            {
                await connection.ExecuteScalarAsync<bool>(query, parameters);
                return true;
            }

        }
    }
}
