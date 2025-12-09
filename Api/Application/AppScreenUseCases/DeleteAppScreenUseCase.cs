using AutoMapper;
using Domain.Models;
using Domain.Repositories;
using Shared.Dtos;
using Shared.Response;

namespace Application.AppScreenUseCases
{
    public class DeleteAppScreenUseCase : AppScreenGenericUseCase
    {
        public DeleteAppScreenUseCase(IAppScreensRepository repository, IMapper mapper) : base(repository, mapper)
        {
        }
        public async Task<GenericResponse> Execute(AppScreenDto appScreendto)
        {
            var appScreen = _mapper.Map<AppScreen>(appScreendto);
            if (appScreen == null)
                throw new ArgumentNullException(nameof(appScreen));
            var result = await _repository.RemoveAsync(appScreen);
            return new GenericResponse
            {
                IsSuccessful = result.id > 0 ? true : false,
            };
        }
    }
}