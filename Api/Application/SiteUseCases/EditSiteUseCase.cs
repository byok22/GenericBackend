using AutoMapper;
using Domain.Models;
using Domain.Repositories;
using Domain.Services;
using Shared.Dtos;
using Shared.Response;

namespace Application.SiteUseCases
{
    public class EditSiteUseCase
    {
        private readonly ISitesRepository _repository;
        private readonly IMapper _mapper;
        private readonly ICurrentUserService _currentUserService;

        public EditSiteUseCase(ISitesRepository repository, IMapper mapper, ICurrentUserService currentUserService)
        {
            _repository = repository;
            _mapper = mapper;
            _currentUserService = currentUserService;
        }

        public async Task<GenericResponse> Execute(SiteDto request)
        {
            var currentUser = await _currentUserService.GetCurrentUserAsync();
            var entity = _mapper.Map<Site>(request);
            entity.UpdatedBy = currentUser != null ? currentUser.NTUser : "System";
            
            var response = await _repository.UpdateAsync(entity);
            return new GenericResponse { IsSuccessful = true, Message = response.message, Id = response.id };
        }
    }
}
