
namespace Shared.Dtos
{
    public class CustomerDto
    {
        public int Id { get; set; }     
        //CustomerID UuId
        public string CustomerID { get; set; } = string.Empty;
        public string CustomerName { get; set; } = string.Empty;
        public string Division { get; set; } = string.Empty;
        public string? BuildingID { get; set; }       
        public bool Available { get; set; } = true;
        
    }
}