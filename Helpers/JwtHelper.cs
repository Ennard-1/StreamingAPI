using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace StreamingAPI.Helpers
{
    public class JwtHelper
    {
        private readonly string _secretKey;
        private readonly string _issuer;
        private readonly string _audience;

        public JwtHelper(IConfiguration configuration)
        {
            var jwtSettings = configuration.GetSection("Jwt");
            _secretKey = jwtSettings["Key"];
            _issuer = jwtSettings["Issuer"];
            _audience = jwtSettings["Audience"];
        }

        public string GenerateToken(int userId, string userEmail)
        {
            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, userId.ToString()),
                new Claim(ClaimTypes.Email, userEmail),
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_secretKey));
            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: _issuer,
                audience: _audience,
                claims: claims,
                expires: DateTime.UtcNow.AddHours(2),
                signingCredentials: credentials
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        public ClaimsPrincipal? ValidateToken(string token)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.UTF8.GetBytes(_secretKey);

            try
            {
                var principal = tokenHandler.ValidateToken(
                    token,
                    new TokenValidationParameters
                    {
                        ValidateIssuer = true,
                        ValidateAudience = true,
                        ValidIssuer = _issuer,
                        ValidAudience = _audience,
                        IssuerSigningKey = new SymmetricSecurityKey(key),
                        ValidateLifetime = true,
                    },
                    out _
                );

                return principal;
            }
            catch
            {
                return null; // Token inválido ou expirado
            }
        }

        public int? GetUsuarioIdFromToken(HttpRequest request)
        {
            var authorizationHeader = request.Headers["Authorization"].ToString();
            if (
                string.IsNullOrEmpty(authorizationHeader)
                || !authorizationHeader.StartsWith("Bearer ")
            )
            {
                return null;
            }

            var token = authorizationHeader["Bearer ".Length..];
            var handler = new JwtSecurityTokenHandler();
            var jwtToken = handler.ReadJwtToken(token);

            var userIdClaim = jwtToken.Claims.FirstOrDefault(c =>
                c.Type == ClaimTypes.NameIdentifier
            );
            return userIdClaim != null ? int.Parse(userIdClaim.Value) : null;
        }
    }
}
