using System.Linq.Expressions;
using Domain.Models;
using Shared.Response;
using Domain.Repositories;
using System.Data;
using Microsoft.Data.SqlClient;
using Domain.DataBase;

namespace Infrastructure.Repositories
{
    public class AppScreensRepository : IAppScreensRepository
    {
        private readonly ISQLDbConnect _dbConnect;
        private readonly ILogger<AppScreensRepository> _logger;
        public AppScreensRepository(ISQLDbConnect dbConnect, ILogger<AppScreensRepository> logger)
        {
            _dbConnect = dbConnect;
            _logger = logger;
        }
        public async Task<AppScreen> AddAsync(AppScreen entity)
        {
            try
            {
                SqlParameter[] parameters = {
                    new SqlParameter("@PKAppScreen", entity.@AppScreenID),
                    new SqlParameter("@FKParentAppScreen", entity.@ParentAppScreenID),
                    new SqlParameter("@Screen", entity.Screen),
                    new SqlParameter("@Url", entity.Url),
                    new SqlParameter("@Sortorder", entity.@SortOrder),
                    new SqlParameter("@Icon", entity.Icon),
                    new SqlParameter("@FKUser", entity.@UserID),
                    new SqlParameter("@Available", entity.Available)

                };
            
                DataTable result = await _dbConnect.GetDataSPAsync("up_AddAppScreen", parameters);
            
                return new AppScreen
                {
                    AppScreenID = result.Rows[0].Field<int>("PKAppScreen"),
                    ParentAppScreenID = result.Rows[0].Field<int?>("FKParentAppScreen") ?? 0,
                    Screen = result.Rows[0].Field<string>("Screen") ?? string.Empty,
                    Url = result.Rows[0].Field<string>("Url") ?? string.Empty,
                    SortOrder = result.Rows[0].Field<int?>("Sortorder") ?? 0,
                    Icon = result.Rows[0].Field<string>("Icon") ?? string.Empty,
                    UserID = result.Rows[0].Field<int?>("FKUser") ?? 0,
                };
            }
            
            catch (Exception ex)
            {
                _logger.LogError(ex, "error al agregar AppScreen");
                throw;
            }
        }
        public async Task<IEnumerable<AppScreen>> FindAsync(Expression<Func<AppScreen, bool>> predicate)
        {
            try
            {
                throw new NotImplementedException();
            }
            
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error finding AppScreens");
                throw;
            }
        }
        public async Task<IEnumerable<AppScreen>> GetAllAsync()
        {
            try
            {
                DataTable result = await _dbConnect.GetDataSPAsync("up_GetAppScreens", null);
                List<AppScreen> appScreensList = new List<AppScreen>();
             
                foreach (DataRow row in result.Rows)
                {
                    appScreensList.Add(new AppScreen
                    {
                        AppScreenID = row.Field<int>("PKAppScreen"),
                        ParentAppScreenID = row.Field<int?>("FKParentAppScreen") ?? 0,
                        ParentScreen =   row.Field<string>("ParentScreen") ?? string.Empty,
                        Screen = row.Field<string>("Screen") ?? string.Empty,
                        Url = row.Field<string>("Url") ?? string.Empty,
                        SortOrder = row.Field<int?>("SortOrder") ?? 0,
                        Icon = row.Field<string>("Icon") ?? string.Empty,
                        UserID = row.Field<int?>("FKUser") ?? 0,
                        Available = row.Field<bool>("Available"),
                       
                    });
                }
                return appScreensList;
            }
            
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener todas las AppScreens");
                throw;
            }
        }

       

        public async  Task<AppScreen> GetByIdAsync(int id)
        {
             try
            {

                 SqlParameter[] parameters = {
                    new SqlParameter("@Id", id)
                };

                DataTable result = await _dbConnect.GetDataSPAsync("Up_GetScreenById", parameters);


           
                AppScreen appScreens = new AppScreen();

                if (result.Rows.Count == 0 || result.Rows[0].Field<int?>("PKAppScreen") == null)
                {
                    return null;
                }
                
                 DataRow row = result.Rows[0];

                return new AppScreen
                {
                    AppScreenID = row.Field<int>("PKAppScreen"),
                        ParentAppScreenID = row.Field<int?>("FKParentAppScreen") ?? 0,
                        Screen = row.Field<string>("Screen") ?? string.Empty,
                        Url = row.Field<string>("Url") ?? string.Empty,
                        SortOrder = row.Field<int?>("SortOrder") ?? 0,
                        Icon = row.Field<string>("Icon") ?? string.Empty,
                        UserID = row.Field<int?>("FKUser") ?? 0,
                         Available = row.Field<bool>("Available"),
                };
             
             
               
            }
            
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener todas las AppScreens");
                throw;
            }

            
        }

        public Task<AppScreen> GetByUuidAsync(string UuId)
        {
            throw new NotImplementedException();
        }
         
         public Task<AppScreen> GetAppScreenByNTUser(string UuId)
        {
            throw new NotImplementedException();
        }

       public async  Task<DBResponse> UpdateAsync(AppScreen entity)
        {
               try
            {
                SqlParameter[] parameters = {
                    new SqlParameter("@PKAppScreen", entity.@AppScreenID),
                    new SqlParameter("@FKParentAppScreen", entity.@ParentAppScreenID),
                    new SqlParameter("@Screen", entity.Screen),
                    new SqlParameter("@Url", entity.Url),
                    new SqlParameter("@Sortorder", entity.@SortOrder),
                    new SqlParameter("@Icon", entity.Icon),
                    new SqlParameter("@FKUser", entity.@UserID),
                    new SqlParameter("@Available", entity.Available),
                };
            
                DataTable result = await _dbConnect.GetDataSPAsync("up_ChgAppScreen", parameters);
            
                return new DBResponse
                {
                    id = result.Rows[0].Field<int>("PKAppScreen"),
                    message = "Updated Succesfully",
                };
            }
            
            catch (Exception ex)
            {
                _logger.LogError(ex, "error al editar AppScreen");
                throw;
            }
        }

        public async Task<DBResponse> RemoveAsync(AppScreen entity)
        {
             try
            {
                SqlParameter[] parameters = {
                    new SqlParameter("@PKAppScreen", entity.AppScreenID)
                };
            
                DataTable result = await _dbConnect.GetDataSPAsync("up_RmvAppScreen", parameters);
            
                 return new DBResponse
                {
                    id = entity.AppScreenID,
                    message = "Deleted Succesfully",
                };
            }

            catch (Exception ex)
            {
                _logger.LogError(ex, "Error removing AppScreen");
                throw;
            }
        }

        public async Task<List<AppScreen>> GetAppScreensByNtUser(string? nTUser, int siteId)
        {
           try
            {
                   SqlParameter[] parameters = {
                    new SqlParameter("@ntUser", nTUser),
                    new SqlParameter("@SiteId", siteId)
                };
            
            
                DataTable result = await _dbConnect.GetDataSPAsync("Up_GetAppScreenByNtUser", parameters);
                List<AppScreen> appScreensList = new List<AppScreen>();
             
                foreach (DataRow row in result.Rows)
                {
                    appScreensList.Add(new AppScreen
                    {
                        AppScreenID = row.Field<int>("PKAppScreen"),
                        ParentAppScreenID = row.Field<int?>("FKParentAppScreen") ?? 0,
                        Screen = row.Field<string>("Screen") ?? string.Empty,
                        Url = row.Field<string>("Url") ?? string.Empty,
                        SortOrder = row.Field<int?>("SortOrder") ?? 0,
                        Icon = row.Field<string>("Icon") ?? string.Empty,
                        UserID = row.Field<int?>("FKUser") ?? 0,
                         Available = row.Field<bool>("Available"),
                    });
                }
                return appScreensList;
            }
            
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener todas las AppScreens");
                throw;
            }

            
        }
    }
}