using Domain.Models;

namespace Domain.Repositories
{
    public interface IUsersRepository: IGenericRepository<User>
    {
        Task<User> GetByUserName(string userName);
        Task<User> GetByNTUser(string ntUser, int siteId);

         Task<User> GetByIdAsync(int id, int siteId);
        // Nuevo método filtrado
        Task<IEnumerable<User>> GetAllBySiteAsync(int siteId);
        
    }
}