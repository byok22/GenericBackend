using Domain.Generics;

namespace Domain.Models
{
    public class Building : BasicFieldsModels
    {
        public int BuildingID { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;      
        public int SiteID { get; set; }
        public bool Available { get; set; } = true;
    }
}