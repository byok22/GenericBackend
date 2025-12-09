using AutoMapper;
using Domain.Models;
using Domain.Repositories;
using Domain.Services;
using Shared.Dtos;
using Shared.Response;

namespace Application.SiteUseCases
{
    public class CreateSiteUseCase
    {
        private readonly ISitesRepository _repository;
        private readonly IMapper _mapper;
        private readonly ICurrentUserService _currentUserService;

        public CreateSiteUseCase(ISitesRepository repository, IMapper mapper, ICurrentUserService currentUserService)
        {
            _repository = repository;
            _mapper = mapper;
            _currentUserService = currentUserService;
        }

        public async Task<GenericResponse> Execute(SiteDto request)
        {
            var currentUser = await _currentUserService.GetCurrentUserAsync();
            var entity = _mapper.Map<Site>(request);
            entity.CreatedBy = currentUser != null ? currentUser.NTUser : "System";
            entity.UpdatedBy = currentUser != null ? currentUser.NTUser : "System";
            
            var response = await _repository.AddAsync(entity);

            if (response != null && response.SiteID > 0)
            {
                return new GenericResponse { IsSuccessful = true, Message = "Site created", Id = response.SiteID };
            }

            return new GenericResponse { IsSuccessful = false, Message = "Site not created" };
        }
    }
}
