using AutoMapper;
using Domain.Models;
using Domain.Repositories;
using Shared.Dtos;

namespace Application.SiteUseCases
{
    public class GetAllSitesUseCase
    {
        private readonly ISitesRepository _repository;
        private readonly IMapper _mapper;

        public GetAllSitesUseCase(ISitesRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<List<SiteDto>> Execute()
        {
            var list = await _repository.GetAllAsync();
            return _mapper.Map<List<SiteDto>>(list);
        }
    }
}
