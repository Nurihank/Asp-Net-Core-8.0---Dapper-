namespace WebApi.Dtos.urunDtos
{
    public class UpdateUrunlerDto
    {
        public int UrunID { get; set; }
        public string UrunAdi { get; set; }
        public string UrunAciklamasi { get; set; }
        public double UrunFiyati { get; set; }
        public string UrunBarcode { get; set; }
    }
}
