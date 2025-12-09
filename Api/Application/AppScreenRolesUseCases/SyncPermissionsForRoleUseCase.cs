using AutoMapper;
using Domain.Models;
using Domain.Repositories;
using Shared.Dtos;
using Shared.Response;

namespace Application.AppScreenRoleUseCases
{

    public class SyncPermissionsForRoleUseCase
    {
        private readonly IAppScreenRoleRepository _repository;
        public SyncPermissionsForRoleUseCase(IAppScreenRoleRepository repository)
        {
            _repository = repository;
        }

        public async Task<GenericResponse> Execute(SyncPermissionsDto dto)
        {
            await _repository.SyncPermissionsForRoleAsync(dto.RoleId, dto.ScreenIds);
            return new GenericResponse { IsSuccessful = true, Message = "Permissions synchronized successfully." };
        }
    }
    
}