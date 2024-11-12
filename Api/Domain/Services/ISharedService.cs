using Domain.Models.Generics;

namespace Domain.Service.Shared
{
    public interface ISharedService
    {
        Task<List<DropDown>> GetStatuses();
    }
}