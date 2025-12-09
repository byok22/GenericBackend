using Domain.Models;

namespace Domain.Repositories
{
    public interface IUsersRepository: IGenericRepository<User>
    {
        Task<User> GetByUserName(string userName);
        Task<User> GetByNTUser(string ntUser);
        
    }
}