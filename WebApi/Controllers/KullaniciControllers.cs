using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Org.BouncyCastle.Pqc.Crypto.Crystals.Dilithium;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using WebApi.Dtos.kullaniciDtos;
using WebApi.Repositories.KullaniciRepositories;
using static System.Net.Mime.MediaTypeNames;

namespace WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]


    public class KullaniciControllers : ControllerBase
    {
        private readonly IKullaniciRepo _kullaniciRepo;
        private readonly IConfiguration _configuration;


        public KullaniciControllers(IKullaniciRepo kullaniciRepo,IConfiguration configuration)
        {
            _kullaniciRepo = kullaniciRepo;
            _configuration = configuration;
        }


        [HttpPost("/api/KullaniciControllers/KullaniciGiris")]
        public async Task<IActionResult> KullaniciGiris([FromBody] KullaniciGirisDto kullaniciGirisDto)
        {
            var (userId, message, statusCode,image) = await _kullaniciRepo.KullaniciGirisi(kullaniciGirisDto);

            if (statusCode == 200)
            {
                var claims = new[]
                {
                    new Claim(JwtRegisteredClaimNames.Sub , _configuration["Jwt:Subject"]),
                    new Claim(JwtRegisteredClaimNames.Jti , Guid.NewGuid().ToString()),
                    new Claim("UserID",userId.ToString())
                };

                var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
                var signIn = new SigningCredentials(key,SecurityAlgorithms.HmacSha256);

                var accessToken = new JwtSecurityToken(
                        _configuration["Jwt:Issuer"],
                        _configuration["Jwt:Audience"],
                        claims,
                        expires:DateTime.UtcNow.Add(TimeSpan.FromSeconds(30)),
                        signingCredentials:signIn
                 );   
               string AccesTokenValue = new JwtSecurityTokenHandler().WriteToken(accessToken);

                var refreshToken = new JwtSecurityToken(
                        _configuration["Jwt:Issuer"],
                        _configuration["Jwt:Audience"],
                        claims,
                        expires: DateTime.UtcNow.Add(TimeSpan.FromHours(1)),
                        signingCredentials: signIn
                 );
                string RefreshTokenValue = new JwtSecurityTokenHandler().WriteToken(refreshToken);

                Console.WriteLine("AccesTokenValue = " + AccesTokenValue);
                Console.WriteLine("RefreshTokenValue = " + RefreshTokenValue);

                return Ok(new { userId, message,image, AccesTokenValue, RefreshTokenValue });
            }
            else
            {
                return Ok(message);
            }
        }

        [Authorize]
        [HttpPost("/api/KullaniciControllers/Token")]
        public async Task<IActionResult> RefreshToken()
        {
            var claims = new[]
                {
                    new Claim(JwtRegisteredClaimNames.Sub , _configuration["Jwt:Subject"]),
                    new Claim(JwtRegisteredClaimNames.Jti , Guid.NewGuid().ToString()),
                };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
            var signIn = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var accessToken = new JwtSecurityToken(
                    _configuration["Jwt:Issuer"],
                    _configuration["Jwt:Audience"],
                    claims,
                    expires: DateTime.UtcNow.Add(TimeSpan.FromSeconds(60)),
                    signingCredentials: signIn
             );
            string AccesTokenValue = new JwtSecurityTokenHandler().WriteToken(accessToken);

            Console.WriteLine(AccesTokenValue);
            return Ok(AccesTokenValue);
        }


        [HttpPost("/api/KullaniciControllers/KullaniciKayit")]
        public async Task<IActionResult> KullaniciKayit(KullaniciKayitDto kullaniciKayitDto)
        {
            var (userId, message, statusCode) = await _kullaniciRepo.KullaniciKayit(kullaniciKayitDto);
            if (statusCode == 200)
            {
                var claims = new[]
                {
                    new Claim(JwtRegisteredClaimNames.Sub , _configuration["Jwt:Subject"]),
                    new Claim(JwtRegisteredClaimNames.Jti , Guid.NewGuid().ToString()),
                    new Claim("UserID",userId.ToString())
                };

                var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
                var signIn = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

                var accessToken = new JwtSecurityToken(
                        _configuration["Jwt:Issuer"],
                        _configuration["Jwt:Audience"],
                        claims,
                        expires: DateTime.UtcNow.Add(TimeSpan.FromSeconds(30)),
                        signingCredentials: signIn
                 );
                string AccesTokenValue = new JwtSecurityTokenHandler().WriteToken(accessToken);

                var refreshToken = new JwtSecurityToken(
                        _configuration["Jwt:Issuer"],
                        _configuration["Jwt:Audience"],
                        claims,
                        expires: DateTime.UtcNow.Add(TimeSpan.FromHours(1)),
                        signingCredentials: signIn
                 );
                string RefreshTokenValue = new JwtSecurityTokenHandler().WriteToken(refreshToken);
                return Ok(new { userId, message, AccesTokenValue, RefreshTokenValue });
            }
            else
            {
                return Ok(new { message });
            }
        }

        [Authorize]
        [HttpGet("/api/KullaniciControllers/KullaniciBilgileri/{id}")]
        public async Task<IActionResult> KullaniciBilgileri(int id)
        {
            var user =  await _kullaniciRepo.KullaniciBilgileri(id);
            return Ok(user);
        }

        [HttpPut("/api/KullaniciControllers/KullaniciGuncelle")]
        public async Task<IActionResult> KullaniciGuncelle(KullaniciBilgileriGuncelleDto kullaniciBilgileriGuncelleDto)
        {
            var result = await _kullaniciRepo.KullaniciBilgileriGuncelle(kullaniciBilgileriGuncelleDto); 
            return Ok(result);
        }

        [HttpPut("/api/KullaniciControllers/KodAl")]
        public async Task<IActionResult> Sifremiunuttum(SifremiUnuttumDto sifremiUnuttumDto)
        {
            var result =  await _kullaniciRepo.SifremiUnuttum(sifremiUnuttumDto);
            return Ok(result);
        }

        [HttpPut("/api/KullaniciControllers/SifreYenile")]
        public async Task<IActionResult> SifreYenile (SifreYenileDto sifreYenileDto)
        {
            var result = await _kullaniciRepo.SifreYenile(sifreYenileDto);
            return Ok(result);
        }

        [HttpPut("/api/KullaniciControllers/SifreDegistir")]
        public async Task<IActionResult> SifreDegistir(SifreYenileDto sifreYenileDto)
        {
            var result = await _kullaniciRepo.SifreDegistir(sifreYenileDto);
            return Ok(result);
        }

        [HttpGet("/api/KullaniciControllers/ProfilResmi/{kullaniciID}")]

        public async Task<IActionResult> ProfilResmiGetir(int kullaniciID)
        {
            var result = await _kullaniciRepo.ProfilResmiGetir(kullaniciID);
            return Ok(result);
        }

        [HttpPut("/api/KullaniciControllers/ProfilResmiKaydet")]
        public async Task<IActionResult> ProfilResmiKaydet(ProfilResmiKaydetDto profilResmiKaydetDto)
        {
            var result = await _kullaniciRepo.ProfilResmiKaydet(profilResmiKaydetDto);
            return Ok(result);
        }
    }
}
