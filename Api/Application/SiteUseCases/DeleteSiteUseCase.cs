using AutoMapper;
using Domain.Models;
using Domain.Repositories;
using Shared.Dtos;
using Shared.Response;

namespace Application.SiteUseCases
{
    public class DeleteSiteUseCase
    {
        private readonly ISitesRepository _repository;
        private readonly IMapper _mapper;

        public DeleteSiteUseCase(ISitesRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<GenericResponse> Execute(SiteDto request)
        {
            var entity = _mapper.Map<Site>(request);
            var response = await _repository.RemoveAsync(entity);
            return new GenericResponse { IsSuccessful = true, Message = response.message, Id = response.id };
        }
    }
}
