namespace Domain.Models
{
    public class AppScreen
    {
        public int AppScreenID { get; set; }
        public int ParentAppScreenID { get; set; }
        public string ParentScreen { get; set; } = string.Empty;
        public string Screen { get; set; }= string.Empty;
        public string Url { get; set; }= string.Empty;
        public int SortOrder { get; set; }
        public string Icon { get; set; }= string.Empty;
        public int UserID { get; set; }
        public bool Available { get; set; } = false;
    }
}