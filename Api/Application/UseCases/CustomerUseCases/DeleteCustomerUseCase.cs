using AutoMapper;
using Domain.Models;

using Domain.Repositories;
using Shared.Dtos;
using Shared.Response;

namespace Application.CustomerUseCases
{
    public class DeleteCustomerUseCase : CustomerGenericUseCase
    {
        public DeleteCustomerUseCase(ICustomersRepository repository, IMapper mapper) : base(repository, mapper)
        {
        }

        public async Task<GenericResponse> Execute(CustomerDto customerdto)
        {

            var customer = _mapper.Map<Customer>(customerdto);
            if (customer == null)
                throw new ArgumentNullException(nameof(customer));


           var result = await _repository.RemoveAsync(customer);
            return new GenericResponse
            {
                IsSuccessful = result.id>0?true:false,
                Message = result.message
            };
            
        }
    }
    
}