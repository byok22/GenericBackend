using System.Security.Claims;
using Application.UserUseCases;
using Domain.Models;
using Domain.Services;
using AutoMapper;
using Microsoft.Extensions.Logging; // Añadir ILogger

namespace Application.Services
{
    public class CurrentUserService : ICurrentUserService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly GetUsersByWindowsIdUseCase _getUsersByWindowsIdUseCase;
        private readonly IMapper _mapper;
        private readonly ILogger<CurrentUserService> _logger; // Añadir Logger

        // --- INICIO DE LA CACHÉ POR REQUEST ---
        // Estas variables viven mientras dure la instancia "Scoped" (1 request)
        private User _cachedUser;
        private bool _isUserFetchAttempted = false;
        // --- FIN DE LA CACHÉ POR REQUEST ---

        public CurrentUserService(
            IHttpContextAccessor httpContextAccessor,
            GetUsersByWindowsIdUseCase getUsersByWindowsIdUseCase,
            IMapper mapper,
            ILogger<CurrentUserService> logger) // Inyectar Logger
        {
            _httpContextAccessor = httpContextAccessor;
            _getUsersByWindowsIdUseCase = getUsersByWindowsIdUseCase;
            _mapper = mapper;
            _logger = logger;
        }

        public string UserName => _httpContextAccessor.HttpContext?.User?.Identity?.Name ?? "Unknown";

        public string NTUser =>
            _httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "Unknown";

        public string Role =>
            _httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypes.Role)?.Value ?? "Unknown";

        public string SiteId =>
            _httpContextAccessor.HttpContext?.User?.FindFirst("SiteId")?.Value ?? "0";

        /// <summary>
        /// Obtiene el usuario actual completo. 
        /// La primera llamada en un request va a la BD.
        /// Las siguientes llamadas en el MISMO request devuelven la versión en caché.
        /// </summary>
        public async Task<User> GetCurrentUserAsync()
        {
            // 1. Si ya intentamos buscar al usuario en este request, devolvemos el resultado guardado
            //    (Incluso si el resultado fue 'null', no volvemos a buscar)
            if (_isUserFetchAttempted)
            {
                return _cachedUser;
            }

            var windowsId = NTUser;
            if (windowsId == "Unknown")
            {
                _isUserFetchAttempted = true; // Marcamos que intentamos
                _cachedUser = null;           // Guardamos el resultado nulo
                return null;
            }

            try
            {
                // 2. Si es la primera vez, vamos a la base de datos
                var userDto = await _getUsersByWindowsIdUseCase.Execute(windowsId, SiteId != "0" ? int.Parse(SiteId) : 0);
                _cachedUser = _mapper.Map<User>(userDto); // Guardamos el resultado
            }
            catch (Exception ex)
            {
                // Manejar error (ej. usuario no encontrado en BD, etc.)
                _logger.LogError(ex, "No se pudo obtener el usuario de la BD para WindowsID: {WindowsId}", windowsId);
                _cachedUser = null; // Guardamos el resultado nulo
            }

            // 3. Marcamos que ya hicimos el intento y devolvemos el resultado
            _isUserFetchAttempted = true;
            return _cachedUser;
        }
    }
}