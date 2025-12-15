using AutoMapper;
using Domain.Repositories;
using Domain.Services;
using Shared.Dtos;
using Shared.Response;

namespace Application.UserUseCases
{
    public class CreateUserUseCase : UserGenericUseCase
    {
        private readonly ICurrentUserService _currentUserService;
        public CreateUserUseCase(
            IUsersRepository usersRepository, IMapper mapper, ICurrentUserService currentUserService) : base(usersRepository, mapper)
        {
            _currentUserService = currentUserService;
        }

        public async Task<GenericResponse> Execute(UserDto request)
        {
            var Userd =await  _currentUserService.GetCurrentUserAsync();
            var User = _mapper.Map<Domain.Models.User>(request);
            User.CreatedBy =Userd != null? Userd.NTUser:"System";
            var user = _mapper.Map<Domain.Models.User>(request);

            var roleId = int.TryParse(user.Role ?? "0", out var rId) ? rId : 0;
            if (roleId == 0)
            {
                return new GenericResponse
                {
                    IsSuccessful = false,
                    Message = "Invalid Role"
                };
            }
            user.RoleId = roleId;
            user.CreatedBy = Userd != null ? Userd.NTUser : "System";
            user.SiteId = Userd != null ? Userd.SiteId : 0;
            var response = await _repository.AddAsync(user);
            if(response.Id>0)
            {
                return new GenericResponse
                {
                    Message = "User created successfully",
                    IsSuccessful = true,
                    Id = response.Id
                };
            }else{
                return new GenericResponse
                {
                    Message = "User not created",
                    IsSuccessful = false,
                     Id = response.Id
                };
            }          
        }


    }
}