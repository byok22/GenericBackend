namespace Domain.Models.Generics
{
    public class GenericUpdate
    {
        public int Id { get; set; }
        public DateTime UpdatedAt { get; set; }
        public User? User{ get; set; }
        public string? Message { get; set; }
        public string? Changes { get; set; }= string.Empty;
    }
}