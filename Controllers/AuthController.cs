using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StreamingAPI.Data;
using StreamingAPI.Helpers;
using StreamingAPI.Models;

namespace StreamingAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController(StreamingDbContext context, JwtHelper jwtHelper) : ControllerBase
    {
        private readonly StreamingDbContext _context = context;
        private readonly JwtHelper _jwtHelper = jwtHelper;

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterRequest request)
        {
            if (await _context.Usuarios.AnyAsync(u => u.Email == request.Email))
                return BadRequest(new { message = "O e-mail já está em uso." });

            var usuario = new Usuario
            {
                Nome = request.Nome,
                Email = request.Email,
                SenhaHash = BCrypt.Net.BCrypt.HashPassword(request.Senha),
            };

            await _context.Usuarios.AddAsync(usuario);
            await _context.SaveChangesAsync();

            return Ok(new { message = "Usuário registrado com sucesso." });
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest request)
        {
            var usuario = await _context.Usuarios.FirstOrDefaultAsync(u =>
                u.Email == request.Email
            );

            if (usuario == null || !BCrypt.Net.BCrypt.Verify(request.Senha, usuario.SenhaHash))
                return Unauthorized(new { message = "Credenciais inválidas." });

            var token = _jwtHelper.GenerateToken(usuario.Id, usuario.Email);
            return Ok(new { token });
        }

        [HttpGet("info")]
        public async Task<IActionResult> GetUserFromToken()
        {
            var authHeader = HttpContext.Request.Headers["Authorization"].ToString();

            // Verifica se o header de autorização existe e tem o prefixo "Bearer"
            if (string.IsNullOrEmpty(authHeader) || !authHeader.StartsWith("Bearer "))
                return Unauthorized(new { message = "Token ausente ou inválido." });

            // Extrai o token JWT do header
            var token = authHeader.Substring("Bearer ".Length).Trim();

            // Tenta validar e ler o token
            var jwtToken = _jwtHelper.ValidateToken(token);
            if (jwtToken == null)
                return Unauthorized(new { message = "Token inválido." });

            // Extrai o ID do usuário do token
            var userIdClaim = jwtToken.Claims.FirstOrDefault(c =>
                c.Type == System.Security.Claims.ClaimTypes.NameIdentifier
            );
            if (userIdClaim == null)
                return Unauthorized(new { message = "Token não contém informações de usuário." });

            var usuarioId = int.Parse(userIdClaim.Value);
            var usuario = await _context.Usuarios.FindAsync(usuarioId);

            if (usuario == null)
                return NotFound(new { message = "Usuário não encontrado." });

            return Ok(
                new
                {
                    usuario.Id,
                    usuario.Nome,
                    usuario.Email,
                }
            );
        }
    }

    public class RegisterRequest
    {
        public required string Nome { get; set; }
        public required string Email { get; set; }
        public required string Senha { get; set; }
    }

    public class LoginRequest
    {
        public required string Email { get; set; }
        public required string Senha { get; set; }
    }
}
