using InventoryManagementAPI.Models;

namespace InventoryManagementAPI.Repositories.Interfaces
{
    public interface IUserRepository
    {
        User? GetByEmail(string email);
        bool Create(User user);
    }
}
