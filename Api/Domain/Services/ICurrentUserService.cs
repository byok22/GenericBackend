using Domain.Models;

namespace Domain.Services
{

    public interface ICurrentUserService
    {
        string UserName { get; }
        string NTUser { get; }
        string Role { get; }
        public Task<User> GetCurrentUserAsync();
    }

}
