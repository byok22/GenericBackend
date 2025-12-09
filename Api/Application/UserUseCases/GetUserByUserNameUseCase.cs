using AutoMapper;
using Domain.Repositories;
using Shared.Dtos;

namespace Application.UserUseCases
{
    public class GetUserByUserNameUseCase : UserGenericUseCase
    {
        public GetUserByUserNameUseCase(IUsersRepository usersRepository, IMapper mapper) : base(usersRepository, mapper)
        {
        }

        public async Task<UserDto> Execute(string userName)
        {         
            var s = await _repository.GetByUserName(userName);      
            return _mapper.Map<UserDto>(s);          
        }
    }
}