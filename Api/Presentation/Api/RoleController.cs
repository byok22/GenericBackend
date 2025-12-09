using Microsoft.AspNetCore.Mvc;
using Application.UserUseCases;
using Shared.Dtos;
using Shared.Response;
using Microsoft.AspNetCore.Authorization;
using Domain.Models;
using Application.RoleUseCases;

namespace Presentation.Api
{
    [ApiController]
    [Route("api/[controller]")]

    public class RoleController : Controller
    {
        private readonly InsertRoleUseCase _insertRoleUseCase;
        private readonly UpdateRoleUseCase _updateRoleUseCase;
        private readonly DeleteRoleUseCase _deleteRoleUseCase;
        private readonly GetRoleByIdUseCase _getRoleByIdUseCase;
        private readonly GetAllRoleUseCase _getAllRole;
        private readonly ILogger<UsersController> _logger;



        public RoleController(
            InsertRoleUseCase insertRoleUseCase,
            UpdateRoleUseCase updateRoleUseCase,
             DeleteRoleUseCase deleteRoleUseCase,
            GetRoleByIdUseCase getRoleByIdUseCase,
            GetAllRoleUseCase getAllRole,
            ILogger<UsersController> logger

            )
        {
            _insertRoleUseCase = insertRoleUseCase;
            _updateRoleUseCase = updateRoleUseCase;
            _deleteRoleUseCase = deleteRoleUseCase;
            _getRoleByIdUseCase = getRoleByIdUseCase;
            _getAllRole = getAllRole;
            _logger = logger;

        }

        [HttpPost("create")]
        public async Task<ActionResult<GenericResponse>> CreateRole(RoleDto role)
        {
            try
            {
                var result = await _insertRoleUseCase.Execute(role);
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating Role");
                return StatusCode(500, new GenericResponse { IsSuccessful = false, Message = ex.Message });
            }
        }

        [HttpPut("update")]
        public async Task<ActionResult<GenericResponse>> UpdateRole(RoleDto role)
        {

            var result = await _updateRoleUseCase.Execute(role);
            return Ok(result);

        }

        [HttpDelete("delete")]
        public async Task<ActionResult<GenericResponse>> DeleteRole(RoleDto role)
        {

            try
            {
                var result = await _deleteRoleUseCase.Execute(role);
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting Role");
                return StatusCode(500, new GenericResponse { IsSuccessful = false, Message = ex.Message });
            }

        }

        [HttpGet("{id}")]
        public async Task<ActionResult<RoleDto>> GetRoletById(int id)
        {
            try
            {
                var result = await _getRoleByIdUseCase.Execute(id);
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting Role by id");
                return StatusCode(500, new GenericResponse { IsSuccessful = false, Message = ex.Message });
            }
        }
        
        [HttpGet("all")]
        public async Task<ActionResult<List<RoleDto>>> GetAllRole()
        {
            try
            {
                var result = await _getAllRole.Execute();
                return Ok(result);

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting all Role");
                return StatusCode(500, new GenericResponse { IsSuccessful = false, Message = ex.Message });
            }
        }

    }


}