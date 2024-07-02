using Dapper;
using System.Reflection.Metadata;
using WebApi.Dtos.urun;
using WebApi.Dtos.urunDtos;
using WebApi.Models.DapperContext;

namespace WebApi.Repositories.UrunRepositories
{
    public class UrunRepo : IUrunRepo //oluşturduğumuz interface'i aşağıda çağırcaz
    {
        public readonly Context _context;

        public UrunRepo(Context context) //Urunrepo clasını çağırınca direkt bu constructor çalışcak
        {
            _context = context;
        }

        public async void CreateUrun(CreateUrunlerDto createUrunlerDto)
        {
            string query = "INSERT INTO Urun (UrunAdi,UrunAciklamasi,KategoriID) values(@adi,@aciklama,@kategoriID)";
            var parameters = new DynamicParameters();
            parameters.Add("@adi",createUrunlerDto.UrunAdi);
            parameters.Add("@aciklama", createUrunlerDto.UrunAciklamasi);
            parameters.Add("@kategoriID", createUrunlerDto.KategoriID);
            

            using (var connection = _context.CreateConnection())
            {
                await connection.ExecuteAsync(query,parameters);
            }
        }

        

        public async Task<List<ResultUrunlerDto>> GetResultUrunlersAsync()
        {
            string query = "SELECT UrunAdi , UrunAciklamasi, KategoriAdi FROM " +
                "Urun INNER JOIN Kategori ON Urun.KategoriID = Kategori.KategoriID";
            using (var connection = _context.CreateConnection()) 
            {
                var values = await connection.QueryAsync<ResultUrunlerDto>(query);
                return values.ToList();
            }
             


        }
        public async Task<bool> DeleteUrunler(DeleteUrunDto deleteUrunDto)
        {
            string selectQuery = "SELECT COUNT(1) FROM Urun WHERE UrunID = @id";
            string deleteQuery = "DELETE FROM Urun WHERE UrunID = @id";
            var parameters = new DynamicParameters();
            parameters.Add("@id", deleteUrunDto.Id);
            Console.WriteLine("+" + parameters.ToString());
            using (var connection = _context.CreateConnection())
            {
                var exists = await connection.ExecuteScalarAsync<int>(selectQuery, parameters);
                if (exists == 0)
                {
                    return false; // Ürün bulunamadı
                }

                await connection.ExecuteAsync(deleteQuery, parameters);
                return true; // Ürün başarıyla silindi
            }
        }
        public async Task<bool> UpdateUrunler(UpdateUrunlerDto updateUrunlerDto)
        {
            string selectedQuery = "SELECT COUNT(1) FROM Urun WHERE UrunID = @UrunID";
            string updateQuery = "UPDATE Urun Set UrunAdi = @UrunAdi , UrunAciklamasi=@UrunAciklamasi , KategoriID=@KategoriID WHERE UrunID = @UrunID";
            
            var Sparameters = new DynamicParameters();
            Sparameters.Add("@UrunID", updateUrunlerDto.UrunID);

            var Uparameters = new DynamicParameters();
            Uparameters.Add("@UrunID", updateUrunlerDto.UrunID);
            Uparameters.Add("@UrunAdi", updateUrunlerDto.UrunAdi);
            Uparameters.Add("@UrunAciklamasi", updateUrunlerDto.UrunAciklamasi);
            Uparameters.Add("@KategoriID", updateUrunlerDto.KategoriID);

            using (var connection = _context.CreateConnection())
            {
                var exists = await connection.ExecuteScalarAsync<int>(selectedQuery, Sparameters);
                if(exists == 0)
                {
                    return false;
                }
                else
                {
                    await connection.ExecuteAsync(updateQuery, Uparameters);
                    return true;
                }             
            }

        }

        public async Task<ResultUrunlerDto> GetUrunByIdAsync(int id)
        {
            string query = "SELECT UrunAdi , UrunAciklamasi , KategoriAdi FROM Urun INNER JOIN Kategori " +
                "ON Urun.KategoriID = Kategori.KategoriID WHERE UrunID = @id";
            var parameters = new DynamicParameters();
            parameters.Add("@id", id);

            using (var connection = _context.CreateConnection())
            {
                var urun = await connection.QueryFirstOrDefaultAsync<ResultUrunlerDto>(query, parameters);
                return urun;
            }
        }
    }
}
