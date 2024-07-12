using Dapper;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using System.Reflection.Metadata;
using WebApi.Dtos.urun;
using WebApi.Dtos.urunDtos;
using WebApi.Models.DapperContext;
using static System.Runtime.InteropServices.JavaScript.JSType;

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
            string query = "INSERT INTO Urun (UrunAdi,UrunAciklamasi,KategoriID,UrunFiyati,UrunBarcode) values(@adi,@aciklama,@kategoriID,@UrunFiyati,@UrunBarcode)";
            var parameters = new DynamicParameters();
            parameters.Add("@adi",createUrunlerDto.UrunAdi);
            parameters.Add("@aciklama", createUrunlerDto.UrunAciklamasi);
            parameters.Add("@kategoriID", createUrunlerDto.KategoriID);
            parameters.Add("@UrunFiyati",createUrunlerDto.UrunFiyati);
            parameters.Add("@UrunBarcode", createUrunlerDto.UrunBarcode);

            using (var connection = _context.CreateConnection())
            {
                await connection.ExecuteAsync(query,parameters);
            }
        }

        

        public async Task<List<ResultUrunlerDto>> GetResultUrunlersAsync()
        {
            string query = "SELECT * FROM " +
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
            string updateQuery = "UPDATE Urun Set UrunAdi = @UrunAdi , UrunAciklamasi=@UrunAciklamasi UrunFiyati = @UrunFiyati, KategoriID=@KategoriID WHERE UrunID = @UrunID";
            
            var Sparameters = new DynamicParameters();
            Sparameters.Add("@UrunID", updateUrunlerDto.UrunID);

            var Uparameters = new DynamicParameters();
            Uparameters.Add("@UrunID", updateUrunlerDto.UrunID);
            Uparameters.Add("@UrunFiyati", updateUrunlerDto.UrunFiyati);
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

        public async Task<ResultUrunlerDto> GetUrunByBarCodeAsync(string UrunBarcode)
        {
            string query = "SELECT UrunID , UrunAdi , UrunAciklamasi ,UrunFiyati, UrunFavorite  ,KategoriAdi,UrunBarcode FROM Urun INNER JOIN Kategori " +
                "ON Urun.KategoriID = Kategori.KategoriID WHERE UrunBarcode = @UrunBarcode";
            var parameters = new DynamicParameters();
            parameters.Add("@UrunBarcode", UrunBarcode);

            using (var connection = _context.CreateConnection())
            {
                var urun = await connection.QueryFirstOrDefaultAsync<ResultUrunlerDto>(query, parameters);
                return urun;
            }
        }

        public async Task<List<ResultUrunlerDto>> GetUrunByNameAsync(string UrunAdi)
        {
            string query = "SELECT * FROM Urun WHERE UrunAdi LIKE @UrunAdi";
            var parameters = new DynamicParameters();
            parameters.Add("@UrunAdi", "%" + UrunAdi + "%");


            using (var connection = _context.CreateConnection())
            {
                var urun = await connection.QueryAsync<ResultUrunlerDto>(query, parameters);
                return urun.ToList();
            }
        }
    }
}
