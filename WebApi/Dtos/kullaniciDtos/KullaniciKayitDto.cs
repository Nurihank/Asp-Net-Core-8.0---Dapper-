using System.Numerics;

namespace WebApi.Dtos.kullaniciDtos
{
    public class KullaniciKayitDto
    { 
        public string KullaniciAdi { get; set; }
        public string Sifre { get; set; }
        public string TelefonNo { get; set; }
        public string Eposta { get; set; }
        public bool Cinsiyet { get; set; }
        public int Yas { get; set; }


    }
}
