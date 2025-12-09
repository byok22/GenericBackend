namespace Domain.Models
{
    public class AppScreenRole
    {
        public int PKScreenRoles { get; set; }
        public int FKScreen { get; set; }        
        public int FKRoles { get; set; }
    }
}