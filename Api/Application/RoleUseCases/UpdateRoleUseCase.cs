using AutoMapper;
using Domain.Models;
using Shared.Dtos;
using Shared.Response;
using Domain.Repositories;

namespace Application.RoleUseCases
{
    public class UpdateRoleUseCase
    {
        private readonly IRoleRepository _repository;
        private readonly IMapper _mapper;
        public UpdateRoleUseCase(IRoleRepository rolerepository, IMapper mapper)
        {
            _repository = rolerepository;
            _mapper = mapper;
        }
        public async Task<GenericResponse> Execute(RoleDto objs)
        {                                       
            try
            {
                var role = _mapper.Map<Role>(objs);
                var result = await _repository.UpdateAsync(role);

                return new GenericResponse
                {
                    IsSuccessful = true,
                    Message = "Updated Role"
                };
            }
            catch (Exception ex)
            {
                return new GenericResponse
                {
                    IsSuccessful = false,
                    Message = "Error Updating Role " + ex.Message,
                };
            }
        }
    }
}
