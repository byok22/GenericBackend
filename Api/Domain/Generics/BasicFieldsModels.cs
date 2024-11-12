
namespace Domain.Generics
{
    public class BasicFieldsModels
    {
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public DateTime UpdatedAt { get; set; } = DateTime.Now;

        public string? UpdatedBy {get;set;}
        public string? CreatedBy { get; set; }

        
    }
}