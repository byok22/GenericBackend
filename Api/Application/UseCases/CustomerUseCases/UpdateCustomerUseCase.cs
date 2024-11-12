using AutoMapper;
using Domain.Models;
using Shared.Dtos;
using Shared.Response;
using Domain.Repositories;


namespace Application.CustomerUseCases
{
    public class UpdateCustomerUseCase : CustomerGenericUseCase
    {
        public UpdateCustomerUseCase(ICustomersRepository repository, IMapper mapper) : base(repository, mapper)
        {
        }
         public  async Task<GenericResponse> Execute(CustomerDto objs)
        {                                       

            try{
                var customer = _mapper.Map<Customer>(objs);
                var result = await _repository.UpdateAsync(customer);

                    return new GenericResponse{
                    IsSuccessful =true,
                    Message = "Update Customer"
                };

            }catch(Exception ex){

                 return  new GenericResponse{
                 IsSuccessful =false,
                 Message = "Error Update Customer "+ex.Message,
                };

            }
            


           

            

            


            //enviarlo al Repository


            //Send Notification


        
        }
    }
}