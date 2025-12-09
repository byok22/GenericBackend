using Domain.Models;
using Shared.Dtos.Login;

namespace Domain.Services
{
    public interface ILdapService
    {
        public Task<User> GetUserByNtUser(string ntUser);        
        public Task<User> Authenticate(LdapLoginRequestDto ldapLoginRequestDto);        
    }
}