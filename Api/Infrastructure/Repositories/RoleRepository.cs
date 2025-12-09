using System.Linq.Expressions;
using Domain.Models;
using Domain.Repositories;
using Shared.Response;
using System.Data;
using Microsoft.Data.SqlClient;
using Domain.DataBase;
using GraphQL.Federation.Types;

namespace Infrastructure.Repositories
{
    public class RoleRepository : IRoleRepository
    {
        private readonly ISQLDbConnect _dbConnect;
        private readonly ILogger<RoleRepository> _logger;
        private object roleList;

        public RoleRepository(ISQLDbConnect dbConnect, ILogger<RoleRepository> logger)
        {
            _dbConnect = dbConnect;
            _logger = logger;
        }

        public async Task<Role> AddAsync(Role entity)
        {
            try
            {
                SqlParameter[] parameters = {
                    new SqlParameter("@Role", entity.RoleName),
                    new SqlParameter("@Available", entity.Available),
                };

                DataTable result = await _dbConnect.GetDataSPAsync("up_InsertRole", parameters);

                return new Role
                {
                    //PKRole = result.Rows[0].Field<int>("PKRole"),
                    RoleName = result.Rows[0].Field<string>("RoleName"),
                    Available = result.Rows[0].Field<bool>("Available"),
                };

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error adding Role");
                throw ex;
            }
        }

        public Task<IEnumerable<Role>> FindAsync(Expression<Func<Role, bool>> predicate)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<Role>> GetAllAsync()
        {
            try
            {
                DataTable result = await _dbConnect.GetDataSPAsync("up_GetAllRoles", null);
                List<Role> roleList = new List<Role>();

                foreach (DataRow row in result.Rows)
                {
                    roleList.Add(new Role
                    {
                        //Model - Table Data Base
                        PKRole = row.Field<int>("PKRole"),
                        RoleName = row.Field<string>("Role") ?? string.Empty,
                        Available = row.Field<bool?>("Available") ?? false,

                    });
                }

                return (IEnumerable<Role>)roleList;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting all Role");
                throw;
            }
        }


        public async Task<Role> GetByIdAsync(int PKRole)
        {
            try
            {
                SqlParameter[] parameters = {
                  new SqlParameter("@PKRole", PKRole),
                };

                DataTable result = await _dbConnect.GetDataSPAsync("up_GetRoleById", parameters);

                if (result.Rows.Count == 0)
                {
                    return null;
                }

                DataRow row = result.Rows[0];

                return new Role
                {
                    PKRole = row.Field<int>("PKRole"),
                    RoleName = row.Field<string>("Role") ?? string.Empty,
                    //RoleName = row.Field<string>("RoleName") ?? string.Empty,
                    Available = row.Field<bool?>("Available") ?? false,


                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting Role by id");
                throw;
            }
        }
        public Task<Role> GetByUuidAsync(string UuId)
        {
            throw new NotImplementedException();
        }

        public async Task<DBResponse> RemoveAsync(Role entity)
        {
            try
            {
                SqlParameter[] parameters = {
                    //store procedure - declarados en la api
                    new SqlParameter("@PKRole", entity.PKRole),

                };

                DataTable result = await _dbConnect.GetDataSPAsync("up_DeleteRol", parameters);

                return new DBResponse
                {
                    id = result.Rows[0].Field<int>("PKRole"),
                    message = result.Rows[0].Field<string>("Message") ?? "",

                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error removing Role");
                throw;
            }
        }

        
        public async Task<DBResponse> UpdateAsync(Role entity)
        {
            try
            {
                SqlParameter[] parameters = {
                    //store procedure - declarados en la api
                    new SqlParameter("@PKRole", entity.PKRole),
                    new SqlParameter("@Role", entity.RoleName),                                  
                    new SqlParameter("@Available", entity.Available),

                };

                DataTable result = await _dbConnect.GetDataSPAsync("up_UpdateRole", parameters);

                return new DBResponse
                {
                    id = result.Rows[0].Field<int>("Id"),
                    message = result.Rows[0].Field<string>("Message") ?? ""
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating Role");
                throw;
            }
        }
      
        

        // public Task<Project> GetByUuidAsync(string UuId)
        // {
        //     throw new NotImplementedException();
        // }

        // public async Task<DBResponse> RemoveAsync(Project entity)
        // {
        //     try
        //     {
        //         SqlParameter[] parameters = {
        //             //store procedure - declarados en la api
        //             new SqlParameter("@PKProject", entity.PKProject),

        //         };

        //         DataTable result = await _dbConnect.GetDataSPAsync("up_DeleteProject", parameters);

        //         return new DBResponse
        //         {
        //             id = result.Rows[0].Field<int>("PKProject"),
        //             message = result.Rows[0].Field<string>("Message") ?? ""
        //         };
        //     }
        //     catch (Exception ex)
        //     {
        //         _logger.LogError(ex, "Error removing user");
        //         throw;
        //     }
        // }


    }
}