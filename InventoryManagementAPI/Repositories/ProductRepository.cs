using InventoryManagementAPI.Helpers;
using InventoryManagementAPI.Models;
using InventoryManagementAPI.Repositories.Interfaces;
using Oracle.ManagedDataAccess.Client;

namespace InventoryManagementAPI.Repositories
{
    public class ProductRepository : IProductRepository
    {
        private readonly DatabaseHelper _dbHelper;

        public ProductRepository(DatabaseHelper dbHelper)
        {
            _dbHelper = dbHelper;
        }

        public (IEnumerable<Product> Products, int TotalCount) GetFilteredProductsWithCount(string? name, int page, int pageSize)
        {
            var products = new List<Product>();
            var offset = (page - 1) * pageSize;

            const string query = @"
            SELECT * 
            FROM (
                SELECT p.*, ROWNUM rnum
                FROM Products p
                WHERE (:name IS NULL OR UPPER(p.Name) LIKE UPPER(:name))
                  AND ROWNUM <= :maxRow
            )
            WHERE rnum > :minRow";

            const string countQuery = @"
            SELECT COUNT(*) AS TotalCount
            FROM Products p
            WHERE (:name IS NULL OR UPPER(p.Name) LIKE UPPER(:name))";

            using var connection = _dbHelper.CreateConnection();
            connection.Open();

            // Obter contagem total de produtos
            using (var countCommand = new OracleCommand(countQuery, connection as OracleConnection))
            {
                countCommand.Parameters.Add(new OracleParameter(":name", string.IsNullOrEmpty(name) ? DBNull.Value : $"%{name}%"));
                var totalCount = Convert.ToInt32(countCommand.ExecuteScalar());

                // Obter lista de produtos paginados
                using (var command = new OracleCommand(query, connection as OracleConnection))
                {
                    command.BindByName = true;
                    command.Parameters.Add(new OracleParameter(":name", string.IsNullOrEmpty(name) ? DBNull.Value : $"%{name}%"));
                    command.Parameters.Add(new OracleParameter(":maxRow", offset + pageSize));
                    command.Parameters.Add(new OracleParameter(":minRow", offset));

                    using var reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                        products.Add(new Product
                        {
                            Id = Convert.ToInt32(reader["ID"]),
                            Name = reader["NAME"].ToString(),
                            Description = reader["DESCRIPTION"]?.ToString(),
                            Price = Convert.ToDecimal(reader["PRICE"]),
                            Quantity = Convert.ToInt32(reader["QUANTITY"])
                        });
                    }
                }

                return (products, totalCount);
            }
        }

        public bool AddProduct(Product product)
        {
            const string query = @"
                INSERT INTO Products (NAME, DESCRIPTION, PRICE, QUANTITY)
                VALUES (:name, :description, :price, :quantity)";

            using var connection = _dbHelper.CreateConnection();
            using var command = new OracleCommand(query, connection as OracleConnection);

            command.Parameters.Add("name", OracleDbType.Varchar2).Value = product.Name;
            command.Parameters.Add("description", OracleDbType.Varchar2).Value = product.Description;
            command.Parameters.Add("price", OracleDbType.Decimal).Value = product.Price;
            command.Parameters.Add("quantity", OracleDbType.Int32).Value = product.Quantity;

            connection.Open();
            return command.ExecuteNonQuery() > 0;
        }

        public bool UpdateProduct(Product product)
        {
            const string query = @"
                UPDATE Products
                SET NAME = :name, DESCRIPTION = :description, PRICE = :price,
                    QUANTITY = :quantity
                WHERE ID = :id";

            using var connection = _dbHelper.CreateConnection();
            using var command = new OracleCommand(query, connection as OracleConnection);

            command.Parameters.Add("name", OracleDbType.Varchar2).Value = product.Name;
            command.Parameters.Add("description", OracleDbType.Varchar2).Value = product.Description;
            command.Parameters.Add("price", OracleDbType.Decimal).Value = product.Price;
            command.Parameters.Add("quantity", OracleDbType.Int32).Value = product.Quantity;
            command.Parameters.Add("id", OracleDbType.Int32).Value = product.Id;

            connection.Open();
            return command.ExecuteNonQuery() > 0;
        }

        public bool DeleteProduct(int id)
        {
            const string query = "DELETE FROM Products WHERE ID = :id";

            using var connection = _dbHelper.CreateConnection();
            using var command = new OracleCommand(query, connection as OracleConnection);

            command.Parameters.Add("id", OracleDbType.Int32).Value = id;

            connection.Open();
            return command.ExecuteNonQuery() > 0;
        }
    }
}
