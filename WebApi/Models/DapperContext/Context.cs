using Microsoft.Data.SqlClient;
using System.Data;

namespace WebApi.Models.DapperContext
{
    public class Context
    {
        public readonly IConfiguration _configuration;
        public readonly string _connectionString;

        
        //bu clası çağırınca bu constructor çalışcak ve bağlantıyı _connectionString'e getirdi
        public Context(IConfiguration configuration) 
        {
            _configuration = configuration;
            _connectionString = _configuration.GetConnectionString("connection");
        }
        //bağlantıyı oluşturduk
        public IDbConnection CreateConnection () => new SqlConnection(_connectionString);

    }
}
