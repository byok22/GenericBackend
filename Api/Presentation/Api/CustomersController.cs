using Application.CustomerUseCases;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Shared.Dtos;
using Shared.Response;
using Microsoft.AspNetCore.OutputCaching;

namespace Presentation.Api
{
    [ApiController]
    [Route("api/[controller]")]
    public class CustomersController : Controller
    {
        private readonly CreateCustomerUseCase _createCustomerUseCase;
        private readonly GetCustomerByIdUseCase _getCustomerById;
        private readonly GetAllCustomersUseCase _getAllCustomers;
        private readonly UpdateCustomerUseCase _updateCustomerUseCase;
        private readonly DeleteCustomerUseCase _deleteCustomerUseCase;
        private readonly ILogger<CustomersController> _logger;

        public CustomersController(
            CreateCustomerUseCase createCustomerUseCase,
            GetCustomerByIdUseCase getCustomerById,
            GetAllCustomersUseCase getAllCustomers,
            UpdateCustomerUseCase updateCustomerUseCase,
            DeleteCustomerUseCase deleteCustomerUseCase,
            ILogger<CustomersController> logger
            )
        {
            _createCustomerUseCase = createCustomerUseCase;
            _getCustomerById = getCustomerById;
            _getAllCustomers = getAllCustomers;
            _updateCustomerUseCase = updateCustomerUseCase;
            _deleteCustomerUseCase = deleteCustomerUseCase;
            _logger = logger;
        }
        [Authorize(Roles = "Admin,")]  // This is an example of how to use the Authorize attribute
        [HttpPost("create")]
        public async Task<ActionResult<GenericResponse>> CreateCustomer(CustomerDto customer)
        {
            try
            {
                var result = await _createCustomerUseCase.Execute(customer);
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating customer");
                return StatusCode(500, new GenericResponse { IsSuccessful = false, Message = ex.Message });
            }
        }

        [Authorize] // This is an example of how to use the Authorize attribute with roles
        [HttpGet("{id}")]
        public async Task<ActionResult<CustomerDto>> GetCustomerById(int id)
        {
            try
            {
                var result = await _getCustomerById.Execute(id);
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting customer by id");
                return StatusCode(500, new GenericResponse { IsSuccessful = false, Message = ex.Message });
            }
        }

        [HttpGet("all")]
         [OutputCache(Duration = 60)] // Cache por 60 segundos
        public async Task<ActionResult<List<CustomerDto>>> GetAllCustomers()
        {
            try
            {
                var result = await _getAllCustomers.Execute();
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting all customers");
                return StatusCode(500, new GenericResponse { IsSuccessful = false, Message = ex.Message });
            }
        }

        [HttpPut("update")]
        public async Task<ActionResult<GenericResponse>> UpdateCustomer(CustomerDto customer)
        {
            try
            {
                var result = await _updateCustomerUseCase.Execute(customer);
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating customer");
                return StatusCode(500, new GenericResponse { IsSuccessful = false, Message = ex.Message });
            }
        }

        [HttpDelete("delete")]
        public async Task<ActionResult<GenericResponse>> DeleteCustomer(CustomerDto customer)
        {
            try
            {
                var result = await _deleteCustomerUseCase.Execute(customer);
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting customer");
                return StatusCode(500, new GenericResponse { IsSuccessful = false, Message = ex.Message });
            }
        }
    }
}