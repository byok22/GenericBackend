namespace Shared.Dtos
{
    public class UserDto
    {
        public int Id { get; set;}
        public string UserName { get; set; }= string.Empty;
        public string? NTUser { get; set; }
        public string EmployeeNumber { get; set; }= string.Empty;
        public string? Email{get; set; }
        public string Role {get;set;}  = string.Empty;
        public bool Available {get;set;}
    }
}