// --- Presentation/Api/PermissionsController.cs ---
using Application.AppScreenRoleUseCases;
using Microsoft.AspNetCore.Mvc;
using Shared.Dtos;
using Shared.Response;

namespace Presentation.Api
{
    [ApiController]
    [Route("api/permissions")]
    public class PermissionsController : ControllerBase
    {
        private readonly GetAppScreenRolesByRoleUseCase _getByRole;
        private readonly SyncPermissionsForRoleUseCase _sync;

        public PermissionsController(GetAppScreenRolesByRoleUseCase getByRole, SyncPermissionsForRoleUseCase sync)
        {
            _getByRole = getByRole;
            _sync = sync;
        }

        /// <summary>
        /// Obtiene todas las pantallas asignadas a un rol específico.
        /// </summary>
        [HttpGet("by-role/{roleId}")]
        public async Task<ActionResult<IEnumerable<AppScreenRoleDTO>>> GetByRole(int roleId)
        {
            var result = await _getByRole.Execute(roleId);
            return Ok(result);
        }

        /// <summary>
        /// Sincroniza la lista de pantallas para un rol.
        /// Reemplaza los permisos existentes con la lista proporcionada.
        /// </summary>
        [HttpPost("sync")]
        public async Task<ActionResult<GenericResponse>> SyncPermissions([FromBody] SyncPermissionsDto dto)
        {
            if (dto == null || dto.ScreenIds == null)
            {
                return BadRequest(new GenericResponse { IsSuccessful = false, Message = "Invalid payload" });
            }
            var response = await _sync.Execute(dto);
            return Ok(response);
        }
    }
}