using Domain.Services;
using Domain.Models;
using System.Security.Claims;
using System.IdentityModel.Tokens.Jwt;
using System.Text;
using Microsoft.IdentityModel.Tokens;

namespace Infrastructure.Services
{
    public class TokenService : ITokenService
    {
        public async Task<string> GenerateToken(User user)
        {
            IConfigurationBuilder builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json");
            string secretKey = builder.Build().GetSection("secretKey").Value;

            var expiration = DateTime.Now.AddDays(1);

            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.UserName),
                new Claim(ClaimTypes.NameIdentifier, user.NTUser),
                new Claim(ClaimTypes.Role, user.Role),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
                // Add other claims as needed
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey)); // Replace with your secret key
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                claims: claims,
                notBefore: DateTime.Now,
                expires: expiration, // Adjust expiration as needed
                signingCredentials: creds);

              return  new JwtSecurityTokenHandler().WriteToken(token);
        }

        // Agrega esto a tu interfaz ITokenService
        // string GenerateRefreshToken(User user);

        // En tu implementación TokenService.cs
        public async Task<string> GenerateRefreshToken(User user)
        {
             IConfigurationBuilder builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json");
            string secretKey = builder.Build().GetSection("secretKey").Value;


            // Usamos la misma lógica que el token normal, pero con MÁS tiempo
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(secretKey); // Tu clave secreta

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.NameIdentifier, user.NTUser),
                    new Claim(ClaimTypes.Role, user.Role),
                    new Claim("type", "refresh") // Opcional: marca para diferenciarlo
                }),
                // IMPORTANTE: Este dura 7 días (o lo que decidas), el otro dura 15 min
                Expires = DateTime.UtcNow.AddDays(7), 
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }
    }
}