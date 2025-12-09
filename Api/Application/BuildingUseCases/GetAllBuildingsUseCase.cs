using AutoMapper;
using Domain.Models;
using Domain.Repositories;
using Shared.Dtos;

namespace Application.BuildingUseCases
{
    public class GetAllBuildingsUseCase
    {
        private readonly IBuildingsRepository _repository;
        private readonly IMapper _mapper;

        public GetAllBuildingsUseCase(IBuildingsRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<List<BuildingDto>> Execute()
        {
            var list = await _repository.GetAllAsync();
            return _mapper.Map<List<BuildingDto>>(list);
        }
    }
}
