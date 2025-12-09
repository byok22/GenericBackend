using Application.BuildingUseCases;
using Microsoft.AspNetCore.Mvc;
using Shared.Dtos;
using Shared.Response;

namespace Presentation.Api
{
    [ApiController]
    [Route("api/[controller]")]
    public class BuildingsController : Controller
    {
        private readonly CreateBuildingUseCase _create;
        private readonly EditBuildingUseCase _edit;
        private readonly GetAllBuildingsUseCase _getAll;
        private readonly GetBuildingByIdUseCase _getById;
        private readonly DeleteBuildingUseCase _delete;

        public BuildingsController(CreateBuildingUseCase create, EditBuildingUseCase edit, GetAllBuildingsUseCase getAll, GetBuildingByIdUseCase getById, DeleteBuildingUseCase delete)
        {
            _create = create;
            _edit = edit;
            _getAll = getAll;
            _getById = getById;
            _delete = delete;
        }

        [HttpPost("create")]
        public async Task<ActionResult<GenericResponse>> Create(BuildingDto dto)
        {
            try
            {
                var res = await _create.Execute(dto);
                return Ok(res);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new GenericResponse { IsSuccessful = false, Message = ex.Message });
            }
        }

        [HttpPost("edit")]
        public async Task<ActionResult<GenericResponse>> Edit(BuildingDto dto)
        {
            try
            {
                var res = await _edit.Execute(dto);
                return Ok(res);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new GenericResponse { IsSuccessful = false, Message = ex.Message });
            }
        }

        [HttpGet("all")]
        public async Task<ActionResult<List<BuildingDto>>> GetAll()
        {
            try
            {
                var res = await _getAll.Execute();
                return Ok(res);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new GenericResponse { IsSuccessful = false, Message = ex.Message });
            }
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<BuildingDto>> GetById(int id)
        {
            try
            {
                var res = await _getById.Execute(id);
                return Ok(res);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new GenericResponse { IsSuccessful = false, Message = ex.Message });
            }
        }

        [HttpDelete("delete")]
        public async Task<ActionResult<GenericResponse>> Delete(BuildingDto dto)
        {
            try
            {
                var res = await _delete.Execute(dto);
                return Ok(res);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new GenericResponse { IsSuccessful = false, Message = ex.Message });
            }
        }
    }
}
