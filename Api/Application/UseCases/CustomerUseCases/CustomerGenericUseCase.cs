using AutoMapper;
using Domain.Repositories;

namespace Application.CustomerUseCases
{
    public class CustomerGenericUseCase
    {
        public readonly ICustomersRepository _repository;
        public readonly IMapper _mapper;

        public CustomerGenericUseCase(ICustomersRepository customersRepository, IMapper mapper)
        {
            _repository = customersRepository;
            _mapper = mapper;
        }
        
        
    }
}