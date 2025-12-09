using Domain.Models;

namespace Domain.Repositories
{
    public interface IAppScreensRepository : IGenericRepository<AppScreen>
    {
        Task<List<AppScreen>> GetAppScreensByNtUser(string? nTUser);
    }
}
