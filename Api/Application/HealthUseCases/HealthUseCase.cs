using Domain.Repositories;
using Shared.Response;

namespace Application.HealthUseCases
{
    
    public class HealthUseCase
    {

         private readonly ICustomersRepository _repository;

        public HealthUseCase(ICustomersRepository repository)
        {
            _repository = repository;

        }
        public async Task<GenericResponse> Excecute()
        {
            try
            {

                var image = await _repository.Health();
                return new GenericResponse
                {
                    IsSuccessful = true,
                    Message = "Health"
                };

            }
            catch (Exception ex)
            {


                return new GenericResponse
                {
                    IsSuccessful = false,
                    Message = "Error Health " + ex.Message,
                };



            }

        }

    }
}