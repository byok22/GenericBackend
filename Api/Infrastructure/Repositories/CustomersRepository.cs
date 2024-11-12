using System.Linq.Expressions;
using Domain.Models;
using Shared.Response;
using Domain.Repositories;
using System.Data;
using Microsoft.Data.SqlClient;
using Domain.DataBase;
using Microsoft.Extensions.Logging;

namespace Infrastructure.Repositories
{
    public class CustomersRepository : ICustomersRepository
    {
        private readonly ISQLDbConnect _dbConnect;
        private readonly ILogger<CustomersRepository> _logger;

        public CustomersRepository(ISQLDbConnect dbConnect, ILogger<CustomersRepository> logger)
        {
            _dbConnect = dbConnect;
            _logger = logger;
        }

        public async Task<Customer> AddAsync(Customer entity)
        {
            try
            {
                SqlParameter[] parameters = {
                    new SqlParameter("@CustomerName", entity.CustomerName),
                    new SqlParameter("@CustomerID", entity.CustomerID == null || entity.CustomerID == string.Empty || entity.CustomerID == "new" || entity.CustomerID == "''" || entity.CustomerID == "00000000-0000-0000-0000-000000000000" ? Guid.NewGuid() : Guid.Parse(entity.CustomerID)),
                    new SqlParameter("@Division", entity.Division),
                    new SqlParameter("@BuildingID", entity.BuildingID == null? (object)DBNull.Value: Guid.Parse(entity.BuildingID)),  
                    new SqlParameter("@Available", entity.Available),
                    new SqlParameter("@UpdatedBy", entity.UpdatedBy ?? (object)DBNull.Value),
                    new SqlParameter("@CreatedBy", entity.CreatedBy ?? (object)DBNull.Value)
                };

                DataTable result = await _dbConnect.GetDataSPAsync("AddCustomer", parameters);

                return new Customer
                {
                    Id = result.Rows[0].Field<int>("Id"),
                    CustomerID = result.Rows[0].Field<Guid>("CustomerID").ToString(),
                    CustomerName = result.Rows[0].Field<string>("CustomerName"),
                    Division = result.Rows[0].Field<string>("Division"),
                    BuildingID = result.Rows[0].Field<Guid>("BuildingID").ToString(),  
                    Available = result.Rows[0].Field<bool>("Available"),
                    CreatedAt = result.Rows[0].Field<DateTime>("CreatedAt"),
                    UpdatedAt = result.Rows[0].Field<DateTime>("UpdatedAt"),
                    UpdatedBy = result.Rows[0].Field<string>("UpdatedBy"),
                    CreatedBy = result.Rows[0].Field<string>("CreatedBy")
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error adding customer");
                throw;
            }
        }

        public async Task<IEnumerable<Customer>> FindAsync(Expression<Func<Customer, bool>> predicate)
        {
            try
            {
                // Implementación futura para filtros avanzados
                throw new NotImplementedException();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error finding customers");
                throw;
            }
        }

        public async Task<IEnumerable<Customer>> GetAllAsync()
        {
            try
            {
                DataTable result = await _dbConnect.GetDataSPAsync("GetAllCustomers", null);
                List<Customer> customersList = new List<Customer>();

                foreach (DataRow row in result.Rows)
                {
                    customersList.Add(new Customer
                    {
                        Id = row.Field<int?>("Id") ?? 0,
                        CustomerID = row.Field<Guid?>("CustomerID")?.ToString() ?? Guid.Empty.ToString(),
                        CustomerName = row.Field<string>("CustomerName") ?? string.Empty,
                        Division = row.Field<string>("Division") ?? string.Empty,
                        BuildingID = row.Field<Guid?>("BuildingID")?.ToString() ?? Guid.Empty.ToString(),
                        Available = row.Field<bool?>("Available") ?? false,
                        CreatedAt = row.Field<DateTime?>("CreatedAt") ?? DateTime.MinValue,
                        UpdatedAt = row.Field<DateTime?>("UpdatedAt") ?? DateTime.MinValue,
                        UpdatedBy = row.Field<string>("UpdatedBy") ?? string.Empty,
                        CreatedBy = row.Field<string>("CreatedBy") ?? string.Empty
                    });
                }

                return customersList;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting all customers");
                throw;
            }
        }

        public async Task<Customer> GetByIdAsync(int id)
        {
            try
            {
                SqlParameter[] parameters = {
                    new SqlParameter("@Id", id)
                };

                DataTable result = await _dbConnect.GetDataSPAsync("GetCustomerById", parameters);

                if (result.Rows.Count == 0 || result.Rows[0].Field<int?>("Id") == null)
                {
                    return null;
                }

                DataRow row = result.Rows[0];

                return new Customer
                {
                    Id = row.Field<int>("Id"),
                    CustomerID = row.Field<Guid>("CustomerID").ToString(),
                    CustomerName = row.Field<string>("CustomerName"),
                    Division = row.Field<string>("Division"),
                    BuildingID = row.Field<Guid>("BuildingID").ToString(),
                    Available = row.Field<bool>("Available"),
                    CreatedAt = row.Field<DateTime>("CreatedAt"),
                    UpdatedAt = row.Field<DateTime>("UpdatedAt"),
                    UpdatedBy = row.Field<string>("UpdatedBy"),
                    CreatedBy = row.Field<string>("CreatedBy")
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting customer by id");
                throw;
            }
        }

        public async Task<DBResponse> UpdateAsync(Customer entity)
        {
            try
            {
                SqlParameter[] parameters = {
                    new SqlParameter("@Id", entity.Id),
                    new SqlParameter("@CustomerName", entity.CustomerName),
                    new SqlParameter("@Division", entity.Division),
                    new SqlParameter("@BuildingID", entity.BuildingID),              
                    new SqlParameter("@Available", entity.Available),
                    new SqlParameter("@UpdatedBy", entity.UpdatedBy ?? (object)DBNull.Value),
                    new SqlParameter("@CreatedBy", entity.CreatedBy ?? (object)DBNull.Value)
                };

                DataTable result = await _dbConnect.GetDataSPAsync("UpdateCustomer", parameters);

                return new DBResponse
                {
                    id = result.Rows[0].Field<int>("Id"),
                    message = result.Rows[0].Field<string>("Message") ?? ""
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating customer");
                throw;
            }
        }

        public async Task<DBResponse> RemoveAsync(Customer entity)
        {
            try
            {
                SqlParameter[] parameters = {
                    new SqlParameter("@Id", entity.Id)
                };

                DataTable result = await _dbConnect.GetDataSPAsync("RemoveCustomer", parameters);

                return new DBResponse
                {
                    id = result.Rows[0].Field<int>("Id"),
                    message = result.Rows[0].Field<string>("Message") ?? ""
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error removing customer");
                throw;
            }
        }

        public async Task<Customer> GetCustomerByCustomerIDAsync(string customerID)
        {
            try
            {
                SqlParameter[] parameters = {
                    new SqlParameter("@CustomerID", SqlDbType.UniqueIdentifier) { Value = Guid.Parse(customerID) }
                };

                DataTable result = await _dbConnect.GetDataSPAsync("GetCustomerByUUID", parameters);

                if (result.Rows.Count == 0 || result.Rows[0].Field<int?>("Id") == null)
                {
                    return new Customer
                    {
                        Id = 0,
                        CustomerID = string.Empty,
                        CustomerName = string.Empty,
                        Division = string.Empty,
                        BuildingID = Guid.Empty.ToString(),            
                        Available = false,
                        CreatedAt = DateTime.MinValue,
                        UpdatedAt = DateTime.MinValue,
                        UpdatedBy = string.Empty,
                        CreatedBy = string.Empty
                    };
                }

                DataRow row = result.Rows[0];

                return new Customer
                {
                    Id = row.Field<int>("Id"),
                    CustomerID = row.Field<Guid>("CustomerID").ToString(),
                    CustomerName = row.Field<string>("CustomerName"),
                    Division = row.Field<string>("Division"),
                    BuildingID = row.Field<Guid>("BuildingID").ToString(),
                    Available = row.Field<bool>("Available"),
                    CreatedAt = row.Field<DateTime>("CreatedAt"),
                    UpdatedAt = row.Field<DateTime>("UpdatedAt"),
                    UpdatedBy = row.Field<string>("UpdatedBy"),
                    CreatedBy = row.Field<string>("CreatedBy")
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting customer by customer ID");
                throw;
            }
        }

        public async Task<Customer> GetByUuidAsync(string UuId)
        {
            try
            {
                SqlParameter[] sqlParameters = new SqlParameter[1];
                sqlParameters[0] = new SqlParameter("@CustomerID", SqlDbType.UniqueIdentifier) { Value = Guid.Parse(UuId) };
                DataTable result = await _dbConnect.GetDataSPAsync("GetCustomerByUUID", sqlParameters);

                if (result.Rows.Count == 0 || result.Rows[0].Field<int?>("Id") == null)
                {
                    return null;
                }

                DataRow row = result.Rows[0];

                return new Customer
                {
                    Id = row.Field<int>("Id"),
                    CustomerID = row.Field<Guid>("CustomerID").ToString(),
                    CustomerName = row.Field<string>("CustomerName") ?? string.Empty,
                    Division = row.Field<string>("Division") ?? string.Empty,
                    BuildingID = row.Field<Guid>("BuildingID").ToString(), 
                    Available = row.Field<bool>("Available"),
                    CreatedAt = row.Field<DateTime>("CreatedAt"),
                    UpdatedAt = row.Field<DateTime>("UpdatedAt"),
                    UpdatedBy = row.Field<string>("UpdatedBy"),
                    CreatedBy = row.Field<string>("CreatedBy")
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting customer by UUID");
                throw;
            }
        }

        public async Task<Customer> GetCustomerByNameAsync(string customerName)
        {
            try
            {
                SqlParameter[] parameters = {
                    new SqlParameter("@CustomerName", customerName),               
                };

                DataTable result = await _dbConnect.GetDataSPAsync("GetCustomerByName", parameters);

                if (result.Rows.Count == 0 || result.Rows[0].Field<int?>("Id") == null)
                {
                    return null;
                }

                DataRow row = result.Rows[0];

                return new Customer
                {
                    Id = row.Field<int>("Id"),
                    CustomerID = row.Field<Guid>("CustomerID").ToString(),
                    CustomerName = row.Field<string>("CustomerName"),
                    Division = row.Field<string>("Division"),
                    BuildingID = row.Field<Guid>("BuildingID").ToString(),            
                    Available = row.Field<bool>("Available"),
                    CreatedAt = row.Field<DateTime>("CreatedAt"),
                    UpdatedAt = row.Field<DateTime>("UpdatedAt"),
                    UpdatedBy = row.Field<string>("UpdatedBy"),
                    CreatedBy = row.Field<string>("CreatedBy")
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting customer by name");
                throw;
            }
        }
    }
}