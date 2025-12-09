using AutoMapper;
using Domain.Repositories;
using Shared.Dtos;

namespace Application.UserUseCases
{
    public class GetUserByUserIDUseCase : UserGenericUseCase
    {
        public GetUserByUserIDUseCase(IUsersRepository usersRepository, IMapper mapper) : base(usersRepository, mapper)
        {
        }

        public async Task<UserDto> Execute(string userID)
        {
            var user = await _repository.GetByUuidAsync(userID);
         
            return _mapper.Map<UserDto>(user);
        }
    }
}
