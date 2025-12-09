using AutoMapper;
using Shared.Dtos;
using Domain.Repositories;

namespace Application.UserUseCases
{
    public class GetUserByIdUseCase : UserGenericUseCase
    {
        public GetUserByIdUseCase(IUsersRepository usersRepository, IMapper mapper) : base(usersRepository, mapper)
        {
        }

        public async Task<UserDto> Execute(int id)
        {         
            var s = await _repository.GetByIdAsync(id);      
            return _mapper.Map<UserDto>(s);          
        }
    }
}
