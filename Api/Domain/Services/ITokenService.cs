using Domain.Models;

namespace Domain.Services
{
    public interface ITokenService
    {
        public Task<string> GenerateToken(User user);
        public Task<string> GenerateRefreshToken(User user);
    }
}