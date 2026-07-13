using EventReservation.Api.DTOs;
using EventReservation.Api.Services;
using EventReservation.Domain.Entities;
using EventReservation.Infrastructure.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EventReservation.Api.Controllers {
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase {
        private readonly ApplicationDbContext _context;
        private readonly IAuthService _authService;
        private readonly ITokenService _tokenService;

        public AuthController(
            ApplicationDbContext context,
            IAuthService authService,
            ITokenService tokenService) {
            _context = context;
            _authService = authService;
            _tokenService = tokenService;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register(RegisterRequest request) {
            var emailExists = await _context.Users
                .AnyAsync(u => u.Email == request.Email);

            if (emailExists) {
                return Conflict(new { message = "Este email já está cadastrado." });
            }

            var user = new User {
                Name = request.Name,
                Email = request.Email,
                Role = UserRole.Participant
            };

            user.PasswordHash = _authService.HashPassword(user, request.Password);

            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            return Created(string.Empty, new { message = "Usuário registrado com sucesso." });
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginRequest request) {
            var user = await _context.Users
                .FirstOrDefaultAsync(u => u.Email == request.Email);

            if (user is null) {
                return Unauthorized(new { message = "Credenciais inválidas." });
            }

            var passwordValid = _authService.VerifyPassword(user, user.PasswordHash, request.Password);

            if (!passwordValid) {
                return Unauthorized(new { message = "Credenciais inválidas." });
            }

            var token = _tokenService.GenerateToken(user);

            var response = new AuthResponse {
                Token = token,
                Email = user.Email,
                Name = user.Name,
                Role = user.Role.ToString()
            };

            return Ok(response);
        }
    }
}