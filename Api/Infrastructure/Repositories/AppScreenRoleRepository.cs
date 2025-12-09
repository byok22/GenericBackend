using System.Data;
using Microsoft.Data.SqlClient;
using Domain.Models;
using Domain.Repositories;
using Domain.DataBase;
using System.Linq.Expressions;
using Microsoft.Extensions.Logging;

namespace Infrastructure.Repositories
{
    // Modelo para devolver datos con JOIN
   


    public class AppScreenRoleRepository : IAppScreenRoleRepository
    {
        private readonly ISQLDbConnect _dbConnect;
        private readonly ILogger<AppScreenRoleRepository> _logger; // Corregido el tipo del Logger

        public AppScreenRoleRepository(ISQLDbConnect dbConnect, ILogger<AppScreenRoleRepository> logger)
        {
            _dbConnect = dbConnect;
            _logger = logger;
        }

        public Task<AppScreenRole> AddAsync(AppScreenRole entity)
        {
            throw new NotImplementedException();
        }

        public async Task<List<AppScreenRoleDetail>> GetByRoleIdAsync(int roleId)
        {
            try
            {
                var parameters = new[] { new SqlParameter("@FKRoles", roleId) };
                var result = await _dbConnect.GetDataSPAsync("up_GetAppScreenRolesByRoleID", parameters);

                return (from DataRow row in result.Rows
                        select new AppScreenRoleDetail
                        {
                            PKScreenRoles = row.Field<int>("PKScreenRoles"),
                            FKScreen = row.Field<int>("FKScreen"),
                            FKRoles = row.Field<int>("FKRoles"),
                            ScreenName = row.Field<string>("ScreenName") ?? string.Empty,
                            ScreenPath = row.Field<string>("ScreenPath") ?? string.Empty,
                        }).ToList();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting AppScreenRoles for role ID {RoleId}", roleId);
                throw;
            }
        }

        public Task<AppScreenRole> RemoveAsync(int id)
        {
            throw new NotImplementedException();
        }

        public async Task SyncPermissionsForRoleAsync(int roleId, IEnumerable<int> screenIds)
        {
            try
            {
                // Crear una DataTable en memoria para pasarla como Table-Valued Parameter
                var dt = new DataTable();
                dt.Columns.Add("ID", typeof(int));
                foreach (var id in screenIds)
                {
                    dt.Rows.Add(id);
                }

                var parameters = new[] {
                    new SqlParameter("@FKRoles", roleId),
                    new SqlParameter("@ScreenIDs", dt) { SqlDbType = SqlDbType.Structured, TypeName = "dbo.IdList" }
                };

                await _dbConnect.GetDataSPAsync("up_SyncAppScreenRoles", parameters);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error syncing permissions for role ID {RoleId}", roleId);
                throw;
            }
        }
    }
}