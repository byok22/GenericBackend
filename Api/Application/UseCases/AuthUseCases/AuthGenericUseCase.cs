using AutoMapper;
using Domain.Services;

namespace Application.UseCases.AuthUseCases
{
    public class AuthGenericUseCase
    {        
        public readonly ILdapService _ldapService;
        public readonly IMapper _mapper;

        public AuthGenericUseCase(ILdapService ldapService, IMapper mapper)
        {
            _ldapService = ldapService;
            _mapper = mapper;
        }
        
    }
}