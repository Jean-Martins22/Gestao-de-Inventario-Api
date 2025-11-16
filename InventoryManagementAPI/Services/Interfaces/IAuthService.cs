using InventoryManagementAPI.Models;

namespace InventoryManagementAPI.Services.Interfaces
{
    public interface IAuthService
    {
        string HashPassword(string password);
        bool VerifyPassword(string password, string hash);
        string GenerateJwtToken(User user);
    }
}
