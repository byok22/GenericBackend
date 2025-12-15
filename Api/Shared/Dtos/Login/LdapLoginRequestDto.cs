namespace Shared.Dtos.Login
{
    public class LdapLoginRequestDto
    {
        public required string NtUser { get; set; }
        public required string Password { get; set; }

        public int SiteId { get; set; }
    }
}
    
