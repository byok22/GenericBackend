using Application.CustomerUseCases;
using Microsoft.AspNetCore.Mvc;
using Shared.Dtos;
using Shared.Response;
using System;

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

        public CustomersController(
            CreateCustomerUseCase createCustomerUseCase,
            GetCustomerByIdUseCase getCustomerById,
            GetAllCustomersUseCase getAllCustomers,
            UpdateCustomerUseCase updateCustomerUseCase,
            DeleteCustomerUseCase deleteCustomerUseCase)
        {
            _createCustomerUseCase = createCustomerUseCase;
            _getCustomerById = getCustomerById;
            _getAllCustomers = getAllCustomers;
            _updateCustomerUseCase = updateCustomerUseCase;
            _deleteCustomerUseCase = deleteCustomerUseCase;
        }

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
                // Log the exception (you can use a logging framework here)
                return StatusCode(500, new GenericResponse { IsSuccessful = false, Message = ex.Message });
            }
        }

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
                // Log the exception (you can use a logging framework here)
                return StatusCode(500, new GenericResponse { IsSuccessful = false, Message = ex.Message });
            }
        }

        [HttpGet("all")]
        public async Task<ActionResult<List<CustomerDto>>> GetAllCustomers()
        {
            try
            {
                var result = await _getAllCustomers.Execute();
                return Ok(result);
            }
            catch (Exception ex)
            {
                // Log the exception (you can use a logging framework here)
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
                // Log the exception (you can use a logging framework here)
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
                // Log the exception (you can use a logging framework here)
                return StatusCode(500, new GenericResponse { IsSuccessful = false, Message = ex.Message });
            }
        }
    }
}