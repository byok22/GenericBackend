using AutoMapper;
using Domain.Repositories;
using Shared.Dtos;

namespace Application.AppScreenUseCases
{
    public class GetAllAppScreensUseCase : AppScreenGenericUseCase
    {
        public GetAllAppScreensUseCase(IAppScreensRepository appScreensRepository, IMapper mapper) : base(appScreensRepository, mapper)
        {
        }
        public async Task<List<AppScreenDto>> Execute()
        {
            var dtos = await _repository.GetAllAsync();
            return _mapper.Map<List<AppScreenDto>>(dtos);
        }
    }
}
