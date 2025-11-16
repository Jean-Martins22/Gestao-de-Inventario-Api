using InventoryManagementAPI.Models;
using InventoryManagementAPI.Repositories.Interfaces;
using InventoryManagementAPI.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using InventoryManagementAPI.DTO;

namespace InventoryManagementAPI.Controllers
{
    [ApiController]
    [Route("api/auth")]
    public class AuthController : ControllerBase
    {
        private readonly IUserRepository _userRepository;
        private readonly IAuthService _authService;

        public AuthController(IUserRepository userRepository, IAuthService authService)
        {
            _userRepository = userRepository;
            _authService = authService;
        }

        [HttpPost]
        [Route("register")]
        public IActionResult Register([FromBody] RegisterRequest request)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(request.Email) || string.IsNullOrWhiteSpace(request.Password))
                    return BadRequest("Email e senha são obrigatórios");

                var existingUser = _userRepository.GetByEmail(request.Email);
                if (existingUser != null)
                    return Conflict("Já existe um usuário com este email");

                var hashedPassword = _authService.HashPassword(request.Password);
                var user = new User
                {
                    Name = request.Name,
                    Email = request.Email,
                    PasswordHash = hashedPassword
                };

                var created = _userRepository.Create(user);
                if (!created)
                    return BadRequest("Falha ao registrar o usuário");

                return Ok("Usuário registrado com sucesso");
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Erro interno: {ex.Message}");
            }
        }

        [HttpPost("login")]
        public IActionResult Login([FromBody] LoginRequest request)
        {
            try
            {
                var user = _userRepository.GetByEmail(request.Email);
                if (user == null)
                    return NotFound("Usuário não encontrado");

                if (string.IsNullOrWhiteSpace(user.PasswordHash))
                    return BadRequest("Senha inválida ou não configurada para o usuário");

                if (!_authService.VerifyPassword(request.Password, user.PasswordHash))
                    return Unauthorized("Email ou senha inválidos");

                var token = _authService.GenerateJwtToken(user);
                return Ok(new { Token = token });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Erro interno: {ex.Message}");
            }
        }
    }
}
