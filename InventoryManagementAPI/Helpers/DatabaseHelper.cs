using Oracle.ManagedDataAccess.Client;
using System.Data;

namespace InventoryManagementAPI.Helpers
{
    public class DatabaseHelper
    {
        private readonly string _connectionString;

        public DatabaseHelper(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("OracleDb");
        }

        public IDbConnection CreateConnection()
        {
            return new OracleConnection(_connectionString);
        }
    }
}
