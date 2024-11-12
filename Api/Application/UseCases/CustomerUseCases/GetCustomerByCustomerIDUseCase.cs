using AutoMapper;
using Domain.Repositories;
using Shared.Dtos;

namespace Application.CustomerUseCases
{
    public class GetCustomerByCustomerIDUseCase : CustomerGenericUseCase
    {
        public GetCustomerByCustomerIDUseCase(ICustomersRepository customersRepository, IMapper mapper) : base(customersRepository, mapper)
        {
        }

        public async Task<CustomerDto> Execute(string customerID)
        {
            var customer = await _repository.GetCustomerByCustomerIDAsync(customerID);
         

            return _mapper.Map<CustomerDto>(customer);
        }
    }
}