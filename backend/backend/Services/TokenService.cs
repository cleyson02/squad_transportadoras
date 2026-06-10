using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;

namespace backend.Services
{
    // Servico responsavel por gerar o token JWT depois do login.
    public class TokenService
    {
        private readonly IConfiguration _configuration;

        public TokenService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public (string token, DateTime expiraEm) GerarToken(IdentityUser usuario)
        {
            var chave = _configuration["Jwt:Key"]!;
            var emissor = _configuration["Jwt:Issuer"]!;
            var audiencia = _configuration["Jwt:Audience"]!;
            var horas = int.Parse(_configuration["Jwt:ExpiresHours"] ?? "8");

            var expiraEm = DateTime.UtcNow.AddHours(horas);

            var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Sub, usuario.Id),
                new Claim(ClaimTypes.Name, usuario.UserName ?? string.Empty),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };

            var credenciais = new SigningCredentials(
                new SymmetricSecurityKey(Encoding.UTF8.GetBytes(chave)),
                SecurityAlgorithms.HmacSha256);

            var jwt = new JwtSecurityToken(
                issuer: emissor,
                audience: audiencia,
                claims: claims,
                expires: expiraEm,
                signingCredentials: credenciais);

            var token = new JwtSecurityTokenHandler().WriteToken(jwt);
            return (token, expiraEm);
        }
    }
}
