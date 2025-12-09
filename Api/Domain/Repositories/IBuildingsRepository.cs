using Domain.Models;

namespace Domain.Repositories
{
    public interface IBuildingsRepository : IGenericRepository<Building>
    {
        Task<IEnumerable<Building>> GetBySiteIdAsync(int siteId);
    }
}
