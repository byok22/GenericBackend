using AutoMapper;
using Domain.Repositories;
using Domain.Services;
using Shared.Dtos;

namespace Application.AppScreenUseCases
{
    public class GetAppScreensUseCase : AppScreenGenericUseCase
    {
        private readonly ICurrentUserService _currentUserService;     
        
        public GetAppScreensUseCase(IAppScreensRepository appScreensRepository, ICurrentUserService currentUserService , IMapper mapper) : base(appScreensRepository, mapper)
        {
            _currentUserService = currentUserService;
        }
        public async Task<List<AppScreenDto>> Execute()
        {
            var Userd = await _currentUserService.GetCurrentUserAsync();

            var dtos = await _repository.GetAppScreensByNtUser(Userd.NTUser);
            return _mapper.Map<List<AppScreenDto>>(dtos);
        }
    }
}
