namespace Shared.Dtos.Login
{
    public class LdapLoginResponseDto
    {
        public bool IsAuthenticated { get; set; }
        public UserDto? User { get; set; }
        public string Token { get; set; } // Si estás generando un token JWT, por ejemplo
        public string Message { get; set; }
        public string? ErrorType { get; set; }
    }
}