using AutoMapper;
using Domain.Repositories;

namespace Application.UserUseCases
{
    public class UserGenericUseCase
    {
        public readonly IUsersRepository _repository;
        public readonly IMapper _mapper;

        public UserGenericUseCase(IUsersRepository usersRepository, IMapper mapper)
        {

            _repository = usersRepository;
            _mapper = mapper;
        }
        
        
       
        
        
    }
}