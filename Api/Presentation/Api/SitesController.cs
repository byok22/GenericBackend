using Application.SiteUseCases;
using Microsoft.AspNetCore.Mvc;
using Shared.Dtos;
using Shared.Response;

namespace Presentation.Api
{
    [ApiController]
    [Route("api/[controller]")]
    public class SitesController : Controller
    {
        private readonly CreateSiteUseCase _create;
        private readonly EditSiteUseCase _edit;
        private readonly GetAllSitesUseCase _getAll;
        private readonly GetSiteByIdUseCase _getById;
        private readonly DeleteSiteUseCase _delete;

        public SitesController(CreateSiteUseCase create, EditSiteUseCase edit, GetAllSitesUseCase getAll, GetSiteByIdUseCase getById, DeleteSiteUseCase delete)
        {
            _create = create;
            _edit = edit;
            _getAll = getAll;
            _getById = getById;
            _delete = delete;
        }

        [HttpPost("create")]
        public async Task<ActionResult<GenericResponse>> Create(SiteDto dto)
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
        public async Task<ActionResult<GenericResponse>> Edit(SiteDto dto)
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
        public async Task<ActionResult<List<SiteDto>>> GetAll()
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
        public async Task<ActionResult<SiteDto>> GetById(int id)
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
        public async Task<ActionResult<GenericResponse>> Delete(SiteDto dto)
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
