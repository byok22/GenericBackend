using Api.Shared.Filters;
using Microsoft.AspNetCore.Authorization;
using Application.UseCases.AuthUseCases;

using Microsoft.AspNetCore.Mvc;
using Shared.Dtos.Login;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Domain.Services;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace Presentation.Api
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : Controller
    {
        private readonly LoginUseCase _loginUseCase;
        private readonly ILogger<AuthController> _logger;
         private readonly ITokenService _tokenService;

        


        public AuthController(
            LoginUseCase loginUseCase,
            ITokenService tokenService,
            ILogger<AuthController> logger
            )
        {
            _loginUseCase = loginUseCase;
             _tokenService = tokenService;
            _logger = logger;
        }

         /// <summary>
        /// Authenticate user via LDAP and return JWT token.
        /// </summary>

        [HttpPost("login")]
        [ServiceFilter(typeof(LoginExceptionFilter))] // Apply the exception filter to this action
        public async Task<ActionResult<LdapLoginResponseDto>> Login(LdapLoginRequestDto request)
        {
        
                var result = await _loginUseCase.Execute(request);
                return Ok(result);
          
        }

        /// <summary>
        /// Validates if current JWT token is still valid.
        /// </summary>
        [HttpGet("check-token")]
        [Authorize]
        public IActionResult CheckToken()
        {
            try
            {
                var authHeader = Request.Headers["Authorization"].ToString();
                if (string.IsNullOrEmpty(authHeader) || !authHeader.StartsWith("Bearer "))
                    return Unauthorized(new { message = "Missing or invalid token" });

                var token = authHeader.Substring("Bearer ".Length);
                var jwtHandler = new JwtSecurityTokenHandler();

                if (!jwtHandler.CanReadToken(token))
                    return BadRequest(new { message = "Invalid token format" });

                var jwtToken = jwtHandler.ReadJwtToken(token);
                var exp = jwtToken.ValidTo.ToLocalTime();

                if (exp < DateTime.Now)
                    return Unauthorized(new { message = "Token expired" });

                return Ok(new { message = "Token is valid", expiresAt = exp });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error validating token");
                return BadRequest(new { message = "Token validation failed" });
            }
        }
        [HttpPost("refresh")]
        public async Task<IActionResult> RefreshToken([FromBody] RefreshTokenRequest request)
        {
            try
            {
                IConfigurationBuilder builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json");
                string secretKey = builder.Build().GetSection("secretKey").Value;

                if (string.IsNullOrEmpty(request.RefreshToken))
                    return BadRequest(new { message = "Missing refresh token" });

                var tokenHandler = new JwtSecurityTokenHandler();
                
                // 1. CONFIGURACIÓN DE VALIDACIÓN
                // Necesitas recuperar tu SecretKey del appsettings.json (la misma con la que firmas los tokens)
                // Asumo que lo inyectas o lo tienes en una variable, aquí lo simulo:
                var key = Encoding.ASCII.GetBytes(secretKey); 

                var validationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateIssuer = false, // O true si usas Issuer
                    ValidateAudience = false, // O true si usas Audience
                    // IMPORTANTE: Para el refresh token, normalmente queremos que NO haya expirado aún,
                    // o si permites refrescar un token expirado, pon ValidateLifetime = false (pero es arriesgado).
                    // Lo normal es: El Refresh token dura 7 días, el Access token 15 min.
                    ValidateLifetime = true, 
                    ClockSkew = TimeSpan.Zero
                };

                // 2. VALIDAR EL TOKEN (Esto lanza excepción si es falso o manipulado)
                SecurityToken validatedToken;
                var principal = tokenHandler.ValidateToken(request.RefreshToken, validationParameters, out validatedToken);

                // 3. VALIDACIÓN DE SEGURIDAD EXTRA (Algoritmo)
                var jwtToken = validatedToken as JwtSecurityToken;
                if (jwtToken == null || !jwtToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256, StringComparison.InvariantCultureIgnoreCase))
                {
                    throw new SecurityTokenException("Invalid token algorithm");
                }

                // 4. OBTENER DATOS DEL USUARIO DEL TOKEN YA VALIDADO
                // Usamos 'principal' que contiene los Claims seguros
                var windowsId = principal.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;
                var role = principal.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Role)?.Value ?? "User";

                if (string.IsNullOrEmpty(windowsId))
                    return Unauthorized(new { message = "Invalid token claims" });

                // 5. GENERAR NUEVO ACCESS TOKEN
                // Nota: Aquí decides si también rotas el RefreshToken (generas uno nuevo) o devuelves el mismo.
                // Por seguridad, es mejor generar uno nuevo también.
                
                var newUserModel = new Domain.Models.User
                {
                    NTUser = windowsId,
                    Role = role
                };

                var newAccessToken = await _tokenService.GenerateToken(newUserModel);
                
                // Opcional: Generar nuevo refresh token si quieres rotación (recomendado)
                // var newRefreshToken = ... generar otro token con más duración ...

                return Ok(new
                {
                    token = newAccessToken,
                    refreshToken = request.RefreshToken, // O el nuevo si decidiste rotarlo
                    message = "Token refreshed successfully"
                });
            }
            catch (SecurityTokenExpiredException)
            {
                // Si el Refresh Token también expiró (pasaron los 7 días, por ejemplo)
                return Unauthorized(new { message = "Refresh token has expired, please login again" });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error refreshing token");
                return Unauthorized(new { message = "Invalid refresh token" });
            }
        }        /// <summary>
        /// Logout user (optional endpoint)
        /// </summary>
        [HttpPost("logout")]
        [Authorize]
        public IActionResult Logout()
        {
            // En JWT no se puede invalidar el token desde backend sin un store.
            // Pero puedes registrar el logout o limpiar en frontend.
            return Ok(new { message = "User logged out successfully" });
        }
    }

        /// <summary>
        /// DTO para refresh token
        /// </summary>
        public class RefreshTokenRequest
        {
            public string RefreshToken { get; set; } = string.Empty;
        }
        
    
}