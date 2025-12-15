using System.Linq.Expressions;
using Domain.Models;
using Domain.Repositories;
using Shared.Response;
using System.Data;
using Microsoft.Data.SqlClient;
using Domain.DataBase;

namespace Infrastructure.Repositories
{
    public class UsersRepository : IUsersRepository
    {
        private readonly ISQLDbConnect _dbConnect;

        private readonly ILogger<UsersRepository> _logger;

        public UsersRepository(ISQLDbConnect dbConnect, ILogger<UsersRepository> logger)
        {
            _dbConnect = dbConnect;
            _logger = logger;
        }

        public async Task<User> AddAsync(User entity)
        {
            try
            {
                SqlParameter[] parameters = {
                    new SqlParameter("@NTUser", entity.NTUser ?? ""),
                    new SqlParameter("@UserName", entity.UserName),                    
                    new SqlParameter("@Email", entity.Email ?? ""),
                    new SqlParameter("@RoleId", entity.RoleId),
                    new SqlParameter("@SiteId", entity.SiteId),
                    new SqlParameter("@Available", entity.Available),
                    new SqlParameter("@CreatedBy", entity.CreatedBy??"")

                };

                DataTable result = await _dbConnect.GetDataSPAsync("up_AddUser", parameters);

                return new User
                {
                    Id = result.Rows[0].Field<int>("Id"),
                    UserName = result.Rows[0].Field<string>("UserName") ?? string.Empty,
                    NTUser = result.Rows[0].Field<string>("NTAccount"),                   
                    Email = result.Rows[0].Field<string>("Email"),
                   
                    Role = result.Rows[0].Field<string>("Role")?? string.Empty,
                    SiteId = result.Rows[0].Field<int>("SiteId"),
                     RoleId = result.Rows[0].Field<int>("RoleId"),
                    Available = result.Rows[0].Field<bool>("Available"),
                    
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error adding user");
                throw;
            }
        }

        public async Task<IEnumerable<User>> FindAsync(Expression<Func<User, bool>> predicate)
        {
            try
            {
                // Implementación futura para filtros avanzados
                throw new NotImplementedException();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error finding users");
                throw;
            }
        }

        public async Task<IEnumerable<User>> GetAllAsync()
        {
            try
            {
                DataTable result = await _dbConnect.GetDataSPAsync("up_GetAllUsers", null);
                List<User> usersList = new List<User>();

                foreach (DataRow row in result.Rows)
                {
                    usersList.Add(new User
                    {
                        Id = row.Field<int?>("Id") ?? 0,
                        UserName = row.Field<string>("UserName") ?? string.Empty,
                        NTUser = row.Field<string>("NTAccount"),
                        
                        Email = row.Field<string>("Email"),
                        Role = row.Field<string>("Role") ?? string.Empty,
                         SiteId = result.Rows[0].Field<int>("SiteId"),
                     RoleId = result.Rows[0].Field<int>("RoleId"),
                        Available = row.Field<bool?>("Available") ?? false,
                       
                    });
                }

                return usersList;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting all users");
                throw;
            }
        }

        public async Task<User> GetByIdAsync(int id, int siteId)
        {
            try
            {
                SqlParameter[] parameters = {
                    new SqlParameter("@Id", id),
                    new SqlParameter("@SiteId", siteId)
                };

                DataTable result = await _dbConnect.GetDataSPAsync("up_GetUserById", parameters);

                if (result.Rows.Count == 0 || result.Rows[0].Field<int?>("Id") == null)
                {
                    return null;
                }

                DataRow row = result.Rows[0];

                return new User
                {
                    Id = row.Field<int?>("Id") ?? 0,
                    UserName = row.Field<string>("UserName") ?? string.Empty,
                    NTUser = row.Field<string>("NTUser"),
                    
                    Email = row.Field<string>("Email"),
                    Role = row.Field<string>("Role") ?? string.Empty,
                     SiteId = result.Rows[0].Field<int>("SiteId"),
                     RoleId = result.Rows[0].Field<int>("RoleId"),
                    Available = row.Field<bool?>("Available") ?? false,
                    
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting user by id");
                throw;
            }
        }

        public async Task<DBResponse> UpdateAsync(User entity)
        {
            try
            {
                SqlParameter[] parameters = {
                    new SqlParameter("@Id", entity.Id),
                    new SqlParameter("@NTUser", entity.NTUser ?? (object)DBNull.Value),
                    new SqlParameter("@UserName", entity.UserName),
                    new SqlParameter("@Email", entity.Email ?? (object)DBNull.Value),                    
                    new SqlParameter("@RoleId", entity.RoleId),
                    new SqlParameter("@SiteId", entity.SiteId),
                    new SqlParameter("@Available", entity.Available),
                    
                };

                DataTable result = await _dbConnect.GetDataSPAsync("up_UpdateUser", parameters);

                return new DBResponse
                {
                    id = result.Rows[0].Field<int>("Id"),
                    message = result.Rows[0].Field<string>("Message") ?? ""
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating user");
                throw;
            }
        }

        public async Task<DBResponse> RemoveAsync(User entity)
        {
            try
            {
                SqlParameter[] parameters = {
                    new SqlParameter("@PKUser", entity.Id)
                };

                DataTable result = await _dbConnect.GetDataSPAsync("up_DeleteUser", parameters);

                return new DBResponse
                {
                    id = result.Rows[0].Field<int>("Id"),
                    message = result.Rows[0].Field<string>("Message") ?? ""
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error removing user");
                throw;
            }
        }

        public async Task<User> GetByUserName(string userName)
        {
            try
            {
                SqlParameter[] parameters = {
                    new SqlParameter("@UserName", userName)
                };

                DataTable result = await _dbConnect.GetDataSPAsync("up_GetUserByUserName", parameters);

                if (result.Rows.Count == 0 || result.Rows[0].Field<int?>("Id") == null)
                {
                    return null;
                }

                DataRow row = result.Rows[0];

                return new User
                {
                    Id = row.Field<int>("Id"),
                    UserName = row.Field<string>("UserName") ?? string.Empty,
                    NTUser = row.Field<string>("NTUser"),
                    
                    Email = row.Field<string>("Email"),
                    Role = row.Field<string>("Role") ?? string.Empty,
                     SiteId = result.Rows[0].Field<int>("SiteId"),
                     RoleId = result.Rows[0].Field<int>("RoleId"),
                    Available = row.Field<bool>("Available"),
                   
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting user by userName");
                throw;
            }
        }

        public async Task<User> GetByUuidAsync(string UuId)
        {
            try
            {
                SqlParameter[] sqlParameters = new SqlParameter[1];
                sqlParameters[0] = new SqlParameter("@UserID", SqlDbType.UniqueIdentifier) { Value = Guid.Parse(UuId) };
                DataTable result = await _dbConnect.GetDataSPAsync("up_GetUserByUUID", sqlParameters);

                if (result.Rows.Count == 0 || result.Rows[0].Field<int?>("Id") == null)
                {
                    return null;
                }

                DataRow row = result.Rows[0];

                return new User
                {
                    Id = row.Field<int>("Id"),
                    UserName = row.Field<string>("UserName") ?? string.Empty,
                    NTUser = row.Field<string>("NTUser"),                   
                    Email = row.Field<string>("Email"),
                    Role = row.Field<string>("Role") ?? string.Empty,
                     SiteId = result.Rows[0].Field<int>("SiteId"),
                     RoleId = result.Rows[0].Field<int>("RoleId"),
                    Available = row.Field<bool>("Available"),
                    
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting user by UUID");
                throw;
            }
        }

        public async Task<IEnumerable<User>> GetAllBySiteAsync(int siteId)
        {
            try
            {
                // Enviamos el SiteId al SP
                SqlParameter[] parameters = { new SqlParameter("@SiteId", siteId) };
                
                // Llamamos al nuevo SP filtrado
                DataTable result = await _dbConnect.GetDataSPAsync("up_GetUsersBySite", parameters);
                
                List<User> usersList = new List<User>();

                foreach (DataRow row in result.Rows)
                {
                    usersList.Add(new User
                    {
                        Id = row.Field<int?>("Id") ?? 0,
                        UserName = row.Field<string>("UserName") ?? string.Empty,
                        NTUser = row.Field<string>("NTAccount"),
                        
                        Email = row.Field<string>("Email"),
                        Role = row.Field<string>("Role") ?? string.Empty,
                         SiteId = result.Rows[0].Field<int>("SiteId"),
                     RoleId = result.Rows[0].Field<int>("RoleId"),
                        Available = row.Field<bool?>("Available") ?? false,
                       
                    });
                }

                return usersList;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting all users");
                throw;
            }
        }


        public async Task<User> GetByNTUser(string ntUser, int siteId)
        {
            try
            {
            SqlParameter[] parameters = {
                new SqlParameter("@NTUser", ntUser),
                new SqlParameter("@SiteId", siteId)
            };

            DataTable result = await _dbConnect.GetDataSPAsync("up_GetUserByNTUser", parameters);

            if (result.Rows.Count == 0 || result.Rows[0].Field<int?>("Id") == null)
            {
                return null;
            }

            DataRow row = result.Rows[0];

            return new User
            {
                Id = row.Field<int>("Id"),
                UserName = row.Field<string>("UserName") ?? string.Empty,
                NTUser = row.Field<string>("NTUser"),
                
                Email = row.Field<string>("Email"),
                Role = row.Field<string>("Role") ?? string.Empty,
                 SiteId = result.Rows[0].Field<int>("SiteId"),
                     RoleId = result.Rows[0].Field<int>("RoleId"),
                Available = row.Field<bool>("Available"),
                
            };
            }
            catch (Exception ex)
            {
            _logger.LogError(ex, "Error getting user by NTUser");
            throw;
            }
        }

        public Task<User> GetByIdAsync(int id)
        {
            throw new NotImplementedException();
        }
    }
}