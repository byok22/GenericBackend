using AutoMapper;
using Domain.Repositories;
using Shared.Dtos;

namespace  Application.CustomerUseCases
{
    public class GetAllCustomersUseCase: CustomerGenericUseCase
    {
        public GetAllCustomersUseCase(ICustomersRepository customersRepository, IMapper mapper) : base(customersRepository, mapper)
        {
        }

        public  async Task<List<CustomerDto>> Execute(){           
            var dtos = await _repository.GetAllAsync();
            return _mapper.Map<List<CustomerDto>>(dtos);                                  

        }
    }
}