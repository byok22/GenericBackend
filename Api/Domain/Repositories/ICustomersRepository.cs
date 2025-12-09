
using Domain.Models;
using Shared.Response;

namespace Domain.Repositories
{
    public interface ICustomersRepository : IGenericRepository<Customer>
    {
        Task<Customer> GetCustomerByExternalCustomerIDAsync(string customerID);
        Task<Customer> GetCustomerByNameAsync(string customerName);   
         Task<DBResponse> Health();     
    }
}