using AutoMapper;
using Domain.Models;
using Domain.Repositories;
using Infrastructure.Repositories;
using Shared.Dtos;
using Shared.Response;

namespace Application.RoleUseCases
{
    public class DeleteRoleUseCase
    {
        private readonly IRoleRepository _repository;
        private readonly IMapper _mapper;

        public DeleteRoleUseCase(IRoleRepository roleRepository, IMapper mapper)
        {
            _repository = roleRepository;
            _mapper = mapper;
        }

        public async Task<GenericResponse> Execute(RoleDto RoleDto)
        {
            var role = _mapper.Map<Domain.Models.Role>(RoleDto);
            if (role == null)
                throw new ArgumentNullException(nameof(role));

            var result = await _repository.RemoveAsync(role);
            return new GenericResponse
            {
                IsSuccessful = result.id > 0 ? true : false,
                Message = result.message
            };
        }
    }
}
