using AutoMapper;
using Domain.Models;
using Domain.Repositories;
using Shared.Dtos;

namespace Application.AppScreenRoleUseCases
{

      public class GetAppScreenRolesByRoleUseCase
    {
        private readonly IAppScreenRoleRepository _repository;
        private readonly IMapper _mapper;
        public GetAppScreenRolesByRoleUseCase(IAppScreenRoleRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<List<AppScreenRoleDTO>> Execute(int roleId)
        {
            var permissions = await _repository.GetByRoleIdAsync(roleId);
            return _mapper.Map<List<AppScreenRoleDTO>>(permissions);
        }
    }
    
}