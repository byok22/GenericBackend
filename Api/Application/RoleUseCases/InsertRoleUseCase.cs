using AutoMapper;
using Domain.Repositories;
using Shared.Dtos;
using Shared.Response;

namespace Application.RoleUseCases
{
    public class InsertRoleUseCase 
    {   
        private readonly IRoleRepository _repository;
        private readonly IMapper _mapper;
        public InsertRoleUseCase(IRoleRepository roleRepository, IMapper mapper)
        {
            _repository = roleRepository;
            _mapper = mapper;

        }

        public async Task<GenericResponse> Execute(RoleDto request)
        {    
            var role = _mapper.Map<Domain.Models.Role>(request);
             var response = await _repository.AddAsync(role);
            if(response.PKRole>0)
            {
                return new GenericResponse
                {
                    Message = "Role created successfully",
                    IsSuccessful = true,
                    Id = response.PKRole
                };
            }else{
                return new GenericResponse
                {
                    Message = "Role not created",
                    IsSuccessful = false,
                    Id = response.PKRole
                };
            }     
        }


    }
}