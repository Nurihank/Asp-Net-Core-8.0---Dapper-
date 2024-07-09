namespace WebApi.Dtos.urun
{
    public class ResultUrunlerDto
    {
        public int UrunID { get; set; }
        public string UrunAdi{ get; set; }
        public string UrunAciklamasi { get; set; }
        public string KategoriAdi { get; set; }
        public double UrunFiyati { get; set; }
        public string UrunBarcode { get; set; }
    }
}
