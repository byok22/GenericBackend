using Application.AppScreenUseCases;
using Domain.Models.Generics;
using Microsoft.AspNetCore.Mvc;
using Shared.Dtos;
using Shared.Response;

namespace Presentation.Api
{
    [ApiController]
    [Route("api/[controller]")]
    public class AppScreensController : Controller
    {
        private readonly CreateAppScreenUseCase _createAppScreenUseCase;
        private readonly EditAppScreenUseCase _editAppScreenUseCase;
        private readonly GetAllAppScreensUseCase _getAllAppScreens;
        private readonly DeleteAppScreenUseCase _deleteAppScreenUseCase;

        private readonly GetAppScreenByIdUseCase _getAppScreenByIdUseCase;

        private readonly GetAppScreensUseCase _getAppScreensUseCase;

        
        public AppScreensController(
            CreateAppScreenUseCase createAppScreenUseCase,
            EditAppScreenUseCase editAppScreenUseCase,
            DeleteAppScreenUseCase deleteAppScreenUseCase,
            GetAppScreensUseCase getAppScreensUseCase,
            GetAllAppScreensUseCase getAllAppScreens, GetAppScreenByIdUseCase getAppScreenByIdUseCase)

        {
            _createAppScreenUseCase = createAppScreenUseCase;
            _editAppScreenUseCase = editAppScreenUseCase;
            _getAllAppScreens = getAllAppScreens;
            _deleteAppScreenUseCase = deleteAppScreenUseCase;
            _getAppScreenByIdUseCase = getAppScreenByIdUseCase;
            _getAppScreensUseCase = getAppScreensUseCase;

        }

        [HttpPost("create")]
        public async Task<ActionResult<GenericResponse>> CreateAppScreen(AppScreenDto appScreen)
        {
            try
            {
                var result = await _createAppScreenUseCase.Execute(appScreen);
                return Ok(result);
            }

            catch (Exception ex)
            {
                return StatusCode(500, new GenericResponse { IsSuccessful = false, Message = ex.Message });
            }
        }

        [HttpPost("edit")]
        public async Task<ActionResult<GenericResponse>> EditAppScreen(AppScreenDto appScreen)
        {
            try
            {
                var result = await _editAppScreenUseCase.Execute(appScreen);
                return Ok(result);
            }

            catch (Exception ex)
            {
                return StatusCode(500, new GenericResponse { IsSuccessful = false, Message = ex.Message });
            }
        }

        //_getAppScreensUseCase

        // Este es tu método de controlador, ahora completo:

        [HttpGet("get-by-ntuser")]
        public async Task<ActionResult<List<SideNavItemDto>>> GetAppScreensByTokenUser()
        {
            try
            {
                // 1. Obtener la lista plana de pantallas desde el caso de uso
                var flatList = await _getAppScreensUseCase.Execute();
                
                // 2. Convertir la lista plana en una jerarquía (árbol)
                var menuSide = BuildMenuTree(flatList); 
                
                // 3. Devolver la lista jerárquica
                return Ok(menuSide);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new GenericResponse { IsSuccessful = false, Message = ex.Message });
            }
        }


        [HttpGet("all")]
        public async Task<ActionResult<List<AppScreenDto>>> GetAllAppScreens([FromQuery] int available)
        {
            try
            {
                var result = await _getAllAppScreens.Execute();
                if(available == 1)
                    return Ok(result.Where(x => x.Available == true).ToList());
                    
                return Ok(result.Where(x => x.Available == false).ToList());
            }
            catch (Exception ex)
            {
                return StatusCode(500, new GenericResponse { IsSuccessful = false, Message = ex.Message });
            }
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<List<AppScreenDto>>> GetByIdAppScreens(int id)
        {
            try
            {
                var result = await _getAppScreenByIdUseCase.Execute(id);
                return Ok(result);
            }

            catch (Exception ex)
            {
                return StatusCode(500, new GenericResponse { IsSuccessful = false, Message = ex.Message });
            }
        }

        [HttpGet("all-dropdown")]
        public async Task<ActionResult<List<DropDown>>> GetAllAppDropDownScreens()
        {
            try
            {
                var result = await _getAllAppScreens.Execute();
                var listDropDown = new List<DropDown>();
                 listDropDown = result.Select(x => new DropDown{ Id= x.AppScreenID.ToString(), Text =x.Screen}).ToList();
                return Ok(result);
            }

            catch (Exception ex)
            {
                return StatusCode(500, new GenericResponse { IsSuccessful = false, Message = ex.Message });
            }
        }

        [HttpDelete("delete")]
        public async Task<ActionResult<GenericResponse>> DeleteAppScreen(AppScreenDto appScreen)
        {
            try
            {
                var result = await _deleteAppScreenUseCase.Execute(appScreen);
                return Ok(result);
            }

            catch (Exception ex)
            {
                return StatusCode(500, new GenericResponse { IsSuccessful = false, Message = ex.Message });
            }
        }


        /// <summary>
        /// Convierte una lista plana de AppScreenDto en una lista jerárquica de SideNavItemDto.
        /// </summary>
        /// <param name="flatList">La lista de DTOs traída de la base de datos.</param>
        /// <returns>Una lista de SideNavItemDto de nivel raíz con sus hijos anidados.</returns>
        private List<SideNavItemDto> BuildMenuTree(List<AppScreenDto> flatList)
        {
            // 1. Ordenar por SortOrder para asegurar el orden correcto al construir
            var sortedList = flatList.OrderBy(x => x.SortOrder).ToList();
            
            // 2. Usar un diccionario (lookup) para acceso O(1) a cada nodo
            var lookup = new Dictionary<int, SideNavItemDto>();

            // 3. Mapear cada AppScreenDto a un SideNavItemDto
            foreach (var dto in sortedList)
            {
                lookup[dto.AppScreenID] = new SideNavItemDto
                {
                    Name = dto.Screen,
                    Icon = dto.Icon,
                    Href = dto.Url,
                    Childrens = new List<SideNavItemDto>(), // Inicializar lista de hijos
                    Expanded = false, // Puedes cambiar esto según tu lógica
                    External = false  // Puedes cambiar esto si determinas que es un enlace externo
                };
            }

            // 4. Construir la jerarquía
            var tree = new List<SideNavItemDto>();
            foreach (var dto in sortedList)
            {
                var node = lookup[dto.AppScreenID];

                // Asumimos que '0' o un ID no existente es un ítem raíz.
                // (Tu DTO usa 'int' y no 'int?', así que '0' es la raíz más probable)
                if (dto.ParentAppScreenID == 0)
                {
                    tree.Add(node);
                }
                else if (lookup.TryGetValue(dto.ParentAppScreenID, out SideNavItemDto parentNode))
                {
                    // Encontramos al padre, agregamos este nodo como su hijo
                    parentNode.Childrens.Add(node);
                }
                // else: Es un nodo "huérfano" (su padre no existe en la lista), no se agregará al árbol.
            }

            return tree;
        }
            }
}
