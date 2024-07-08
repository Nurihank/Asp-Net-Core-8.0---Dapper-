namespace WebApi.Dtos.urun
{
    public class CreateUrunlerDto
    {
        public string UrunAdi { get; set; }
        public string UrunAciklamasi { get; set; }
        public double UrunFiyati { get; set; }
        public int KategoriID { get; set; }
    }
}
