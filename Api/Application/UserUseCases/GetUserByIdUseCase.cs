using AutoMapper;
using Shared.Dtos;
using Domain.Repositories;
using Domain.Services;

namespace Application.UserUseCases
{
    public class GetUserByIdUseCase : UserGenericUseCase
    {
        private readonly ICurrentUserService _currentUserService; // Servicio para obtener datos del token/sesión
         
        public GetUserByIdUseCase(IUsersRepository usersRepository, IMapper mapper, ICurrentUserService currentUserService) : base(usersRepository, mapper)
        {
            _currentUserService = currentUserService;
        }

        public async Task<UserDto> Execute(int id)
        {         
              //  1. Averiguar en qué sitio está el usuario logueado (Admin)
            var currentUser = await _currentUserService.GetCurrentUserAsync();
            
            if (currentUser == null || currentUser.SiteId == 0)
            {
                throw new UnauthorizedAccessException("User context or Site ID not found.");
            }



            var s = await _repository.GetByIdAsync(id, currentUser.SiteId); // Filtrar por SiteId  
            return _mapper.Map<UserDto>(s);          
        }
    }
}
