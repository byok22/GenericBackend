using Application.AppScreenUseCases;
using AutoMapper;
using Domain.Repositories;
using Shared.Dtos;
using Shared.Response;

namespace Application.AppScreenUseCases
{
    public class CreateAppScreenUseCase : AppScreenGenericUseCase
    {
        public CreateAppScreenUseCase(
            IAppScreensRepository AppScreensRepository, IMapper mapper) : base(AppScreensRepository, mapper)
        {
        }
        public async Task<GenericResponse> Execute(AppScreenDto request)
        {

            var AppScreen = _mapper.Map<Domain.Models.AppScreen>(request);
            var response = await _repository.AddAsync(AppScreen);
            if (response.AppScreenID > 0)
            {
                return new GenericResponse
                {
                    Message = "AppScreen created successfully",
                    IsSuccessful = true,
                    Id = response.AppScreenID
                };
            }
            else
            {
                return new GenericResponse
                {
                    Message = "AppScreen not created",
                    IsSuccessful = false,
                    Id = response.AppScreenID
                };
            }
        }
    }
}