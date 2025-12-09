using Domain.Models;

namespace Domain.Repositories
{
    public interface IAppScreenRoleRepository
    {
        Task<AppScreenRole> AddAsync(AppScreenRole entity);
        Task<AppScreenRole> RemoveAsync(int id);
        Task<List<AppScreenRoleDetail>> GetByRoleIdAsync(int roleId);
                
        Task SyncPermissionsForRoleAsync(int roleId, IEnumerable<int> screenIds);
    }
}



    // Primero, actualiza la interfaz
