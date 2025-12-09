using Domain.Generics;

namespace Domain.Models
{
    public class Role
    {
        public int PKRole { get; set;}
        public string RoleName { get; set; } = string.Empty;
        public bool Available {get;set;}             
    }
}