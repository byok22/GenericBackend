using AutoMapper;
using Domain.Repositories;
using Shared.Dtos;

namespace Application.UserUseCases
{
    public class GetAllUsersUseCase : UserGenericUseCase
    {
        public GetAllUsersUseCase(IUsersRepository usersRepository, IMapper mapper) : base(usersRepository, mapper)
        {
        }

        public async Task<List<UserDto>> Execute()
        {
            var dtos = await _repository.GetAllAsync();
            return _mapper.Map<List<UserDto>>(dtos);
        }
    }
}