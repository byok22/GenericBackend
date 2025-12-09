using AutoMapper;
using Shared.Dtos;
using Domain.Repositories;

namespace Application.RoleUseCases
{
    public class GetRoleByIdUseCase 
    {
        private readonly IRoleRepository _repository;
        private readonly IMapper _mapper;
        public GetRoleByIdUseCase(IRoleRepository roleRepository, IMapper mapper)
        {
            _repository = roleRepository;
            _mapper = mapper;
        }

        public async Task<RoleDto> Execute(int id)
        {         
            var s = await _repository.GetByIdAsync(id);      
            return _mapper.Map<RoleDto>(s);          
        }
    }
}
