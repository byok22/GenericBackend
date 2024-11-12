using Application.CustomerUseCases;
using AutoMapper;
using Domain.Repositories;
using Shared.Dtos;
using Shared.Response;

namespace Application.CustomerUseCases
{
    public class CreateCustomerUseCase : CustomerGenericUseCase
    {
        public CreateCustomerUseCase(
            ICustomersRepository customersRepository, IMapper mapper) : base(customersRepository, mapper)
        {
        }

        public async Task<GenericResponse> Execute(CustomerDto request)
        {
            
            var customer = _mapper.Map<Domain.Models.Customer>(request);
            var response = await _repository.AddAsync(customer);
            if(response.Id>0)
            {
                return new GenericResponse
                {
                    Message = "Customer created successfully",
                    IsSuccessful = true,
                    Id = response.Id
                };
            }else{
                return new GenericResponse
                {
                    Message = "Customer not created",
                    IsSuccessful = false,
                     Id = response.Id
                };
            }          
        }


    }
}