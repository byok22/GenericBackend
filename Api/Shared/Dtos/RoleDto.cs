namespace Shared.Dtos
{
    public class RoleDto 
    {
        public int PKRole { get; set;}
        public string RoleName { get; set; } = string.Empty;
        public bool Available {get;set;}             
    }
}