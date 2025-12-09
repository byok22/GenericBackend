using AutoMapper;
using Domain.Models;
using Domain.Repositories;
using Shared.Dtos;
using Shared.Response;

namespace Application.BuildingUseCases
{
    public class DeleteBuildingUseCase
    {
        private readonly IBuildingsRepository _repository;
        private readonly IMapper _mapper;

        public DeleteBuildingUseCase(IBuildingsRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<GenericResponse> Execute(BuildingDto request)
        {
            var entity = _mapper.Map<Building>(request);
            var response = await _repository.RemoveAsync(entity);
            return new GenericResponse { IsSuccessful = true, Message = response.message, Id = response.id };
        }
    }
}
