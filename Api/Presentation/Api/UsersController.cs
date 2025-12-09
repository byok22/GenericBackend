using Microsoft.AspNetCore.Mvc;
using Application.UserUseCases;
using Shared.Dtos;
using Shared.Response;
using Microsoft.AspNetCore.Authorization;

namespace Presentation.Api
{
    [ApiController]
    [Route("api/[controller]")]
    //[Authorize] // All users must be authorized to access the endpoints
    public class UsersController : Controller
    {
        private readonly CreateUserUseCase _createUserUseCase;
        private readonly GetUserByIdUseCase _getUserById;
        private readonly GetAllUsersUseCase _getAllUsers;
        private readonly UpdateUserUseCase _updateUserUseCase;
        private readonly DeleteUserUseCase _deleteUserUseCase;
        private readonly GetUserByUserIDUseCase _getUserByUserID;
        private readonly GetUserByUserNameUseCase _getUserByUserName;
        private readonly GetUserByNTUserUseCase _getUserByNTUser;
        private readonly ILogger<UsersController> _logger;

        public UsersController(
            CreateUserUseCase createUserUseCase,
            GetUserByIdUseCase getUserById,
            GetAllUsersUseCase getAllUsers,
            UpdateUserUseCase updateUserUseCase,
            DeleteUserUseCase deleteUserUseCase,
            GetUserByUserIDUseCase getUserByUserID,
            GetUserByUserNameUseCase getUserByUserName,
            GetUserByNTUserUseCase getUserByNTUserUseCase,
            ILogger<UsersController> logger
            )
        {
            _createUserUseCase = createUserUseCase;
            _getUserById = getUserById;
            _getAllUsers = getAllUsers;
            _updateUserUseCase = updateUserUseCase;
            _deleteUserUseCase = deleteUserUseCase;
            _getUserByUserID = getUserByUserID;
            _getUserByUserName = getUserByUserName;
            _getUserByNTUser = getUserByNTUserUseCase;
            _logger = logger;
        }

        //[Authorize(Roles = "Admin,Developer")]
        [HttpPost("create")]
        public async Task<ActionResult<GenericResponse>> CreateUser(UserDto user)
        {
            try
            {
                var result = await _createUserUseCase.Execute(user);
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating user");
                return StatusCode(500, new GenericResponse { IsSuccessful = false, Message = ex.Message });
            }
        }

        //[Authorize(Roles = "Admin,Developer")]
        [HttpGet("{id}")]
        public async Task<ActionResult<UserDto>> GetUserById(int id)
        {
            try
            {
                var result = await _getUserById.Execute(id);
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting user by id");
                return StatusCode(500, new GenericResponse { IsSuccessful = false, Message = ex.Message });
            }
        }

        //[Authorize(Roles = "Admin,Developer")]
        [HttpGet("all")]
        public async Task<ActionResult<List<UserDto>>> GetAllUsers()
        {
            try
            {
                var result = await _getAllUsers.Execute();
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting all users");
                return StatusCode(500, new GenericResponse { IsSuccessful = false, Message = ex.Message });
            }
        }

        //[Authorize(Roles = "Admin,Developer")]
        [HttpPut("update")]
        public async Task<ActionResult<GenericResponse>> UpdateUser(UserDto user)
        {
            try
            {
                var result = await _updateUserUseCase.Execute(user);
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating user");
                return StatusCode(500, new GenericResponse { IsSuccessful = false, Message = ex.Message });
            }
        }

        //[Authorize(Roles = "Admin,Developer")]
        [HttpDelete("delete")]
        public async Task<ActionResult<GenericResponse>> DeleteUser(UserDto user)
        {
            try
            {
                var result = await _deleteUserUseCase.Execute(user);
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting user");
                return StatusCode(500, new GenericResponse { IsSuccessful = false, Message = ex.Message });
            }
        }

        //[Authorize(Roles = "Admin,Developer")]
        [HttpGet("by-ntuser/{ntUser}")]
        public async Task<ActionResult<UserDto>> GetUserByNTUser(string ntUser)
        {
            try
            {
                var result = await _getUserByNTUser.Execute(ntUser);
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting user by ntUser");
                return StatusCode(500, new GenericResponse { IsSuccessful = false, Message = ex.Message });
            }
        }

        //[Authorize(Roles = "Admin,Developer")]
        [HttpGet("by-userid/{userId}")]
        public async Task<ActionResult<UserDto>> GetUserByUserID(string userId)
        {
            try
            {
                var result = await _getUserByUserID.Execute(userId);
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting user by userId");
                return StatusCode(500, new GenericResponse { IsSuccessful = false, Message = ex.Message });
            }
        }

        //[Authorize(Roles = "Admin,Developer")]
        [HttpGet("by-username/{userName}")]
        public async Task<ActionResult<UserDto>> GetUserByUserName(string userName)
        {
            try
            {
                var result = await _getUserByUserName.Execute(userName);
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting user by userName");
                return StatusCode(500, new GenericResponse { IsSuccessful = false, Message = ex.Message });
            }
        }
    }
}