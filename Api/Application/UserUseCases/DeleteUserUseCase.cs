using AutoMapper;
using Domain.Models;
using Domain.Repositories;
using Shared.Dtos;
using Shared.Response;

namespace Application.UserUseCases
{
    public class DeleteUserUseCase : UserGenericUseCase
    {
        public DeleteUserUseCase(IUsersRepository repository, IMapper mapper) : base(repository, mapper)
        {
        }

        public async Task<GenericResponse> Execute(UserDto userDto)
        {
            var user = _mapper.Map<User>(userDto);
            if (user == null)
                throw new ArgumentNullException(nameof(user));

            var result = await _repository.RemoveAsync(user);
            return new GenericResponse
            {
                IsSuccessful = result.id > 0 ? true : false,
                Message = result.message
            };
        }
    }
}
