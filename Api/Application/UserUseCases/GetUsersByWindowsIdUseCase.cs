using AutoMapper;
using Domain.Repositories;
using Shared.Dtos;

namespace Application.UserUseCases
{
    public class GetUsersByWindowsIdUseCase : UserGenericUseCase
    {      

        public GetUsersByWindowsIdUseCase(
            IUsersRepository usersRepository,
            IMapper mapper
         ) : base(usersRepository, mapper)
        {
          
        }

       public async Task<UserDto> Execute(string windowsID)
            {
                var user = await _repository.GetByNTUser(windowsID);
                if (user == null) return null;

                var dto = _mapper.Map<UserDto>(user);

              ;
                return dto;
            }

    }
}
