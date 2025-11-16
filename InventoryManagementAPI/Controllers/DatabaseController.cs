using InventoryManagementAPI.Helpers;
using Microsoft.AspNetCore.Mvc;

namespace InventoryManagementAPI.Controllers
{

    [ApiController]
    [Route("api/[controller]")]
    public class DatabaseController : ControllerBase
    {
        private readonly DatabaseHelper _dbHelper;

        public DatabaseController(DatabaseHelper dbHelper)
        {
            _dbHelper = dbHelper;
        }

        [HttpGet]
        [Route("test-connection")]
        public IActionResult TestConnection()
        {
            try
            {
                using var connection = _dbHelper.CreateConnection();
                connection.Open();
                return Ok("Conexão com o banco de dados foi bem-sucedida!");
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Erro ao conectar: {ex.Message}");
            }
        }
    }
}
