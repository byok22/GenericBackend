using AutoMapper;
using Domain.Repositories;
using Domain.Services;
using Shared.Dtos;

namespace Application.UserUseCases
{
    public class GetUserByNTUserUseCase : UserGenericUseCase
    {
        private readonly ICurrentUserService _currentUserService; // Servicio para obtener datos del token/

        public GetUserByNTUserUseCase(IUsersRepository usersRepository, IMapper mapper,  ICurrentUserService currentUserService) : base(usersRepository, mapper)
        {
            _currentUserService = currentUserService;
        }

        public async Task<UserDto> Execute(string userName)
        {   
            var currentUser = await _currentUserService.GetCurrentUserAsync();
            
            if (currentUser == null || currentUser.SiteId == 0)
            {
                throw new UnauthorizedAccessException("User context or Site ID not found.");
            }

            var s = await _repository.GetByNTUser(userName, currentUser.SiteId);      
            return _mapper.Map<UserDto>(s);          
        }
    }
}