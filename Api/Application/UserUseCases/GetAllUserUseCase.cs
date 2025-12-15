using AutoMapper;
using Domain.Repositories;
using Domain.Services; // Necesitas esto para saber QUIÉN pregunta
using Shared.Dtos;

namespace Application.UserUseCases
{
    public class GetAllUsersUseCase : UserGenericUseCase
    {
        private readonly ICurrentUserService _currentUserService; // Servicio para obtener datos del token/sesión

        public GetAllUsersUseCase(
            IUsersRepository usersRepository, 
            IMapper mapper, 
            ICurrentUserService currentUserService) : base(usersRepository, mapper)
        {
            _currentUserService = currentUserService;
        }

        public async Task<List<UserDto>> Execute()
        {
          //  1. Averiguar en qué sitio está el usuario logueado (Admin)
            var currentUser = await _currentUserService.GetCurrentUserAsync();
            
            if (currentUser == null || currentUser.SiteId == 0)
            {
                throw new UnauthorizedAccessException("User context or Site ID not found.");
            }

            //2. Pedir al repo SOLO los usuarios de ese sitio
            var dtos = await _repository.GetAllBySiteAsync(currentUser.SiteId);
            
            return _mapper.Map<List<UserDto>>(dtos);
        }
    }
}