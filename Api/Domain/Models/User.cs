using Domain.Generics;

namespace Domain.Models
{
    public class User: BasicFieldsModels
    {
        public int Id { get; set;}
        public string UserName { get; set; }= string.Empty;
        public string? NTUser { get; set; }      
        public string? Email{get; set; }
        public string Role {get;set;}  = string.Empty;
        public int RoleId {get;set;}
        public int SiteId {get;set;}        
        public bool Available {get;set;}             
    }
}