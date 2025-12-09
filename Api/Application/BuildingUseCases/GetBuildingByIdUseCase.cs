using AutoMapper;
using Domain.Models;
using Domain.Repositories;
using Shared.Dtos;

namespace Application.BuildingUseCases
{
    public class GetBuildingByIdUseCase
    {
        private readonly IBuildingsRepository _repository;
        private readonly IMapper _mapper;

        public GetBuildingByIdUseCase(IBuildingsRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<BuildingDto?> Execute(int id)
        {
            var entity = await _repository.GetByIdAsync(id);
            if (entity == null) return null;
            return _mapper.Map<BuildingDto>(entity);
        }
    }
}
