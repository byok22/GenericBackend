namespace Shared.Dtos
{
    public class AppScreenRoleDTO
    {
        public int PKScreenRoles { get; set; }
        public int FKScreen { get; set; }        
        public int FKRoles { get; set; }
    }
}