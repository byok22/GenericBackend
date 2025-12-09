using AutoMapper;
using Domain.Models;
using Domain.Repositories;
using Domain.Services;
using Shared.Dtos;
using Shared.Response;

namespace Application.BuildingUseCases
{
    public class CreateBuildingUseCase
    {
        private readonly IBuildingsRepository _repository;
        private readonly IMapper _mapper;
        private readonly ICurrentUserService _currentUserService;

        public CreateBuildingUseCase(IBuildingsRepository repository, IMapper mapper, ICurrentUserService currentUserService)
        {
            _repository = repository;
            _mapper = mapper;
            _currentUserService = currentUserService;
        }

        public async Task<GenericResponse> Execute(BuildingDto request)
        {
            var currentUser = await _currentUserService.GetCurrentUserAsync();
            var entity = _mapper.Map<Building>(request);
            entity.CreatedBy = currentUser != null ? currentUser.NTUser : "System";
            entity.UpdatedBy = currentUser != null ? currentUser.NTUser : "System";
            
            var response = await _repository.AddAsync(entity);

            if (response != null && response.BuildingID > 0)
            {
                return new GenericResponse { IsSuccessful = true, Message = "Building created", Id = response.BuildingID };
            }

            return new GenericResponse { IsSuccessful = false, Message = "Building not created" };
        }
    }
}
