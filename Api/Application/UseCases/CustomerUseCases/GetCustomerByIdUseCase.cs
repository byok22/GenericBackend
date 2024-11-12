using AutoMapper;
using Shared.Dtos;
using Domain.Repositories;

namespace Application.CustomerUseCases
{
    public class GetCustomerByIdUseCase : CustomerGenericUseCase
    {
        public GetCustomerByIdUseCase(ICustomersRepository customersRepository, IMapper mapper) : base(customersRepository, mapper)
        {
        }

        public  async Task<CustomerDto> Execute(int id){         

            var s = await _repository.GetByIdAsync(id);      
            return _mapper.Map<CustomerDto>(s);          
           
        }
    }
}