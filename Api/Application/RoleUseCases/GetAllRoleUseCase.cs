using AutoMapper;
using Domain.Repositories;
using Shared.Dtos;

namespace Application.RoleUseCases
{
    public class GetAllRoleUseCase 
    {
        private readonly IRoleRepository _repository;
        private readonly IMapper _mapper;
        public GetAllRoleUseCase(IRoleRepository roleRepository, IMapper mapper)
        {
            _repository = roleRepository;
            _mapper = mapper;
        }

        public async Task<List<RoleDto>> Execute()
        {
            var dtos = await _repository.GetAllAsync();
            return _mapper.Map<List<RoleDto>>(dtos);
        }
    }
}