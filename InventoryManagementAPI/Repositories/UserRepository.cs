using Dapper;
using InventoryManagementAPI.Models;
using InventoryManagementAPI.Repositories.Interfaces;
using InventoryManagementAPI.Helpers;

namespace InventoryManagementAPI.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly DatabaseHelper _databaseHelper;

        public UserRepository(DatabaseHelper databaseHelper)
        {
            _databaseHelper = databaseHelper;
        }

        public User? GetByEmail(string email)
        {
            using var connection = _databaseHelper.CreateConnection();
            const string query = @"
            SELECT 
                ID AS Id, 
                NAME AS Name, 
                EMAIL AS Email, 
                PASSWORD_HASH AS PasswordHash 
            FROM USERS 
            WHERE EMAIL = :Email";

            return connection.QueryFirstOrDefault<User>(query, new { Email = email });
        }



        public bool Create(User user)
        {
            using var connection = _databaseHelper.CreateConnection();
            const string query = @"
            INSERT INTO USERS (NAME, EMAIL, PASSWORD_HASH) 
            VALUES (:Name, :Email, :PasswordHash)";

            return connection.Execute(query, user) > 0;
        }
    }
}
