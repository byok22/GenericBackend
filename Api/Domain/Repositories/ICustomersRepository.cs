
using Domain.Models;

namespace Domain.Repositories
{
    public interface ICustomersRepository: IGenericRepository<Customer>
    {
        Task<Customer> GetCustomerByCustomerIDAsync(string customerID);
        Task<Customer> GetCustomerByNameAsync(string customerName);        
    }
}