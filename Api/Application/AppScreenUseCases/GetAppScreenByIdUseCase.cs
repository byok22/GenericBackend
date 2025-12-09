using AutoMapper;
using Domain.Repositories;
using Shared.Dtos;

namespace Application.AppScreenUseCases
{
    public class GetAppScreenByIdUseCase : AppScreenGenericUseCase
    {
        public GetAppScreenByIdUseCase(IAppScreensRepository appScreensRepository, IMapper mapper) : base(appScreensRepository, mapper)
        {
        }
        public async Task<AppScreenDto> Execute(int id)
        {
            var dtos = await _repository.GetByIdAsync(id);
            return _mapper.Map<AppScreenDto>(dtos);
        }
    }
}
