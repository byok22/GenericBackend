using Domain.Generics;

namespace Domain.Models
{
    public class Site : BasicFieldsModels
    {
        public int SiteID { get; set; }     
        public string SiteName { get; set; } = string.Empty;
        public bool Available { get; set; } = true;
    }
}