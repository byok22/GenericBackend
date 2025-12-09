using AutoMapper;
using Domain.Models;
using Shared.Dtos;
using Shared.Response;
using Domain.Repositories;

namespace Application.UserUseCases
{
    public class UpdateUserUseCase : UserGenericUseCase
    {
        public UpdateUserUseCase(IUsersRepository repository, IMapper mapper) : base(repository, mapper)
        {
        }
        public async Task<GenericResponse> Execute(UserDto objs)
        {                                       
            try
            {
                var user = _mapper.Map<User>(objs);
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
