using Domain.Generics;

namespace Shared.Dtos
{
    public class BuildingDto
    {
        public int BuildingID { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;      
        public int SiteID { get; set; }
        public bool Available { get; set; } = true;       
    }
}