namespace Shared.Dtos
{
    public class SideNavItemDto 
    {
        public string Name { get; set;}
        public string Icon { get; set; } = string.Empty;
        public string Href {get;set;}             
        public  List<SideNavItemDto> Childrens { get; set;}
         public bool Expanded {get;set;} 
         public bool External {get;set;}
    }
}