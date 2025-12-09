using AutoMapper;
using Domain.Repositories;
using Shared.Dtos;

namespace Application.UserUseCases
{
    public class GetUserByNTUserUseCase : UserGenericUseCase
    {
        public GetUserByNTUserUseCase(IUsersRepository usersRepository, IMapper mapper) : base(usersRepository, mapper)
        {
        }

        public async Task<UserDto> Execute(string userName)
        {         
            var s = await _repository.GetByNTUser(userName);      
            return _mapper.Map<UserDto>(s);          
        }
    }
}