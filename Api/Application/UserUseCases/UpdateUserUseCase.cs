using AutoMapper;
using Domain.Models;
using Shared.Dtos;
using Shared.Response;
using Domain.Repositories;
using Domain.Services;

namespace Application.UserUseCases
{
    public class UpdateUserUseCase : UserGenericUseCase
    {
        private readonly ICurrentUserService _currentUserService;
        public UpdateUserUseCase(IUsersRepository repository, IMapper mapper, ICurrentUserService currentUserService) : base(repository, mapper)
        {
            _currentUserService = currentUserService;
        }
        public async Task<GenericResponse> Execute(UserDto objs)
        {       
            var Userd =await  _currentUserService.GetCurrentUserAsync();
        
                                        
            try
            {
                var user = _mapper.Map<User>(objs);
                if(user == null || user.NTUser ==null || user.NTUser=="")
                {
                    return new GenericResponse
                    {
                        IsSuccessful = false,
                        Message = "User not found"
                    };
                }
                var oldUser = await _repository.GetByNTUser(user.NTUser, user.SiteId);
                var roleId = int.TryParse(user.Role ?? "0", out var rId) ? rId : 0;


                if(roleId == 0)
                {
                    return new GenericResponse
                    {
                        IsSuccessful = false,
                        Message = "Invalid Role"
                    };
                }
                user.UpdatedBy = Userd != null ? Userd.NTUser : "System";
                user.UpdatedAt = DateTime.Now;
                user.RoleId = roleId;
                user.SiteId = oldUser.SiteId; // Preserve existing SiteId

                
                var result = await _repository.UpdateAsync(user);

                return new GenericResponse
                {
                    IsSuccessful = true,
                    Message = "Update User"
                };
            }
            catch (Exception ex)
            {
                return new GenericResponse
                {
                    IsSuccessful = false,
                    Message = "Error Update User " + ex.Message,
                };
            }
        }
    }
}
