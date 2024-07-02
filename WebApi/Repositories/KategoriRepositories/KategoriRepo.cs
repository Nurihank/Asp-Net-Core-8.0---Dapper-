using Dapper;
using WebApi.Dtos.kategoriDtos;
using WebApi.Models.DapperContext;

namespace WebApi.Repositories.KategoriRepositories
{
    public class KategoriRepo : IKategoriRepo
    {
        public readonly Context _context;

        public KategoriRepo(Context context) { _context = context; }

        public async Task<bool> CreateKategori(CreateKategoriDto createKategoriDto)
        {
            var selectQuery = "SELECT COUNT(1) FROM Kategori WHERE KategoriAdi = @adi";
            var createQuery = "INSERT INTO Kategori (KategoriAdi) values(@adi)";
            var parameters = new DynamicParameters();
            parameters.Add("@adi", createKategoriDto.KategoriAdi);

            using(var connection = _context.CreateConnection())
            {
                var exists = await connection.ExecuteScalarAsync<int>(selectQuery, parameters);
                if (exists == 1)
                {
                    return false;
                }
                else
                {
                    await connection.ExecuteAsync(createQuery, parameters);
                    return true;
                }
            }
        }

        public async Task<bool> DeleteKategori(DeleteKategoriDto deleteKategoriDto)
        {
            var selectQuery = "SELECT COUNT(1) FROM Kategori WHERE KategoriID = @id";
            var DeleteQuery = "DELETE FROM Kategori WHERE KategoriID = @id";

            var parameters = new DynamicParameters();
            parameters.Add("@id", deleteKategoriDto.KategoriID);

            using( var connection = _context.CreateConnection())
            {
                var exists = await connection.ExecuteScalarAsync<int>(selectQuery, parameters);
                if (exists == 1)
                {
                    await connection.ExecuteAsync(DeleteQuery, parameters);
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }

        public async Task<List<GetKategoriDto>> GetAllKategoriAsync()
        {
            string query = "SELECT * FROM Kategori";

            using (var connection = _context.CreateConnection())
            {
                var values =  await connection.QueryAsync<GetKategoriDto>(query);
                return values.ToList();
            }
        }

        public async Task<bool> UpdateKategori(UpdateKategoriDto updateKategoriDto)
        {
            var selectQuery = "SELECT COUNT(1) FROM Kategori WHERE KategoriID = @id";
            var DeleteQuery = "UPDATE FROM Kategori SET KategoriAdi = @adi WHERE KategoriID = @id";
            var parameters = new DynamicParameters();
            parameters.Add("adi", updateKategoriDto.KategoriAdi);
            parameters.Add("id", updateKategoriDto.KategoriID);

            using(var connection = _context.CreateConnection())
            {
                var exists = await connection.ExecuteScalarAsync<int>(selectQuery, parameters);
                if(exists == 1)
                {
                    await connection.ExecuteAsync(DeleteQuery, parameters);
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }
    }
}
