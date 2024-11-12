namespace Domain.Models
{
    public class User
    {
        public int Id { get; set;}
        public string UserName { get; set; }= string.Empty;
        public string EmployeeAccount { get; set; }= string.Empty;
        public string? Email{get; set; }             
    }
}