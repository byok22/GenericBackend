using Domain.Generics;

namespace Domain.Models
{
    public class Customer: BasicFieldsModels
    {
        public int Id { get; set; }     
        //CustomerID UuId
        public string CustomerID { get; set; } = string.Empty;
        public string CustomerName { get; set; } = string.Empty;
        public string Division { get; set; } = string.Empty;
        public string BuildingID { get; set; }        
        public bool Available { get; set; } = true;
      
        
    }
}