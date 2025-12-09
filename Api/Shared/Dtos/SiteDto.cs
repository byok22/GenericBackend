namespace Shared.Dtos
{
    public class SiteDto
    {
        public int SiteID { get; set; }
        public string SiteName { get; set; } = string.Empty;
        public bool Available { get; set; } = true;
    }
}