using Domain.Models.Generics;

namespace Domain.Services
{
    public interface IDropDownsService
    {
        Task<List<DropDown>> GetStatus();
    }
}