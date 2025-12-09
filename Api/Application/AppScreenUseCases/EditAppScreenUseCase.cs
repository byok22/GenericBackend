using AutoMapper;
using Domain.Repositories;
using Shared.Dtos;
using Shared.Response;

namespace Application.AppScreenUseCases
{
    public class EditAppScreenUseCase : AppScreenGenericUseCase
    {
        public EditAppScreenUseCase(
            IAppScreensRepository AppScreensRepository, IMapper mapper) : base(AppScreensRepository, mapper)
        {
        }
        public async Task<GenericResponse> Execute(AppScreenDto request)
        {

            var AppScreen = _mapper.Map<Domain.Models.AppScreen>(request);
            var response = await _repository.UpdateAsync(AppScreen);
            if (response.id > 0)
            {
                return new GenericResponse
                {
                    Message = "AppScreen Edited successfully",
                    IsSuccessful = true,
                     Id = response.id
                };
            }
            else
            {
                return new GenericResponse
                {
                    Message = "AppScreen not edited",
                    IsSuccessful = false,
                    Id = response.id
                };
            }
        }
    }
}