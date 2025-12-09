using Application.CustomerUseCases;
using AutoMapper;
using Domain.Repositories;
using Domain.Services;
using Shared.Dtos;
using Shared.Response;

namespace Application.CustomerUseCases
{
    public class CreateCustomerUseCase : CustomerGenericUseCase
    {
        private readonly ICurrentUserService _currentUserService;
        public CreateCustomerUseCase(
            ICustomersRepository customersRepository, ICurrentUserService currentUserService, IMapper mapper) : base(customersRepository, mapper)
        {
            _currentUserService = currentUserService;
        }

        public async Task<GenericResponse> Execute(CustomerDto request)
        {

                //This is information from token
            var userNt = _currentUserService.NTUser;

       
            
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