using AutoMapper;
using Domain.Models;
using Domain.Repositories;
using Shared.Dtos;

namespace Application.SiteUseCases
{
    public class GetSiteByIdUseCase
    {
        private readonly ISitesRepository _repository;
        private readonly IMapper _mapper;

        public GetSiteByIdUseCase(ISitesRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<SiteDto?> Execute(int id)
        {
            var entity = await _repository.GetByIdAsync(id);
            if (entity == null) return null;
            return _mapper.Map<SiteDto>(entity);
        }
    }
}
