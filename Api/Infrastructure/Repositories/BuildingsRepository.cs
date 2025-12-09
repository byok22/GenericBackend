using System.Linq.Expressions;
using Domain.Models;
using Domain.Repositories;
using Shared.Response;
using System.Data;
using Microsoft.Data.SqlClient;
using Domain.DataBase;

namespace Infrastructure.Repositories
{
    public class BuildingsRepository : IBuildingsRepository
    {
        private readonly ISQLDbConnect _dbConnect;
        private readonly ILogger<BuildingsRepository> _logger;

        public BuildingsRepository(ISQLDbConnect dbConnect, ILogger<BuildingsRepository> logger)
        {
            _dbConnect = dbConnect;
            _logger = logger;
        }

        public async Task<Building> AddAsync(Building entity)
        {
            try
            {
                SqlParameter[] parameters = {
                    new SqlParameter("@PKBuilding", entity.BuildingID),
                    new SqlParameter("@Name", entity.Name),
                    new SqlParameter("@Description", entity.Description ?? (object)DBNull.Value),
                    new SqlParameter("@FKSite", entity.SiteID),
                    new SqlParameter("@Available", entity.Available),
                    new SqlParameter("@CreatedBy", entity.CreatedBy ?? (object)DBNull.Value),
                    new SqlParameter("@UpdatedBy", entity.UpdatedBy ?? (object)DBNull.Value)
                };

                DataTable result = await _dbConnect.GetDataSPAsync("up_AddBuilding", parameters);

                return new Building
                {
                    BuildingID = result.Rows[0].Field<int>("PKBuilding"),
                    Name = result.Rows[0].Field<string>("Name") ?? string.Empty,
                    Description = result.Rows[0].Field<string>("Description"),
                    SiteID = result.Rows[0].Field<int?>("FKSite") ?? 0,
                    Available = result.Rows[0].Field<bool?>("Available") ?? true,
                    CreatedAt = result.Rows[0].Field<DateTime?>("CreatedAt") ?? DateTime.Now,
                    UpdatedAt = result.Rows[0].Field<DateTime?>("UpdatedAt") ?? DateTime.Now,
                    CreatedBy = result.Rows[0].Field<string>("CreatedBy"),
                    UpdatedBy = result.Rows[0].Field<string>("UpdatedBy")
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error adding building");
                throw;
            }
        }

        public async Task<IEnumerable<Building>> FindAsync(Expression<Func<Building, bool>> predicate)
        {
            try
            {
                throw new NotImplementedException();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error finding buildings");
                throw;
            }
        }

        public async Task<IEnumerable<Building>> GetAllAsync()
        {
            try
            {
                DataTable result = await _dbConnect.GetDataSPAsync("up_GetBuildings", null);
                List<Building> list = new List<Building>();

                foreach (DataRow row in result.Rows)
                {
                    list.Add(new Building
                    {
                        BuildingID = row.Field<int>("PKBuilding"),
                        Name = row.Field<string>("Name") ?? string.Empty,
                        Description = row.Field<string>("Description"),
                        SiteID = row.Field<int?>("FKSite") ?? 0,
                        Available = row.Field<bool?>("Available") ?? true,
                        CreatedAt = row.Field<DateTime?>("CreatedAt") ?? DateTime.Now,
                        UpdatedAt = row.Field<DateTime?>("UpdatedAt") ?? DateTime.Now,
                        CreatedBy = row.Field<string>("CreatedBy"),
                        UpdatedBy = row.Field<string>("UpdatedBy")
                    });
                }

                return list;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting all buildings");
                throw;
            }
        }

        public async Task<Building> GetByIdAsync(int id)
        {
            try
            {
                SqlParameter[] parameters = { new SqlParameter("@Id", id) };
                DataTable result = await _dbConnect.GetDataSPAsync("Up_GetBuildingById", parameters);

                if (result.Rows.Count == 0 || result.Rows[0].Field<int?>("PKBuilding") == null)
                    return null;

                DataRow row = result.Rows[0];

                return new Building
                {
                    BuildingID = row.Field<int>("PKBuilding"),
                    Name = row.Field<string>("Name") ?? string.Empty,
                    Description = row.Field<string>("Description"),
                    SiteID = row.Field<int?>("FKSite") ?? 0,
                    Available = row.Field<bool?>("Available") ?? true,
                    CreatedAt = row.Field<DateTime?>("CreatedAt") ?? DateTime.Now,
                    UpdatedAt = row.Field<DateTime?>("UpdatedAt") ?? DateTime.Now,
                    CreatedBy = row.Field<string>("CreatedBy"),
                    UpdatedBy = row.Field<string>("UpdatedBy")
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting building by id");
                throw;
            }
        }

        public async Task<DBResponse> UpdateAsync(Building entity)
        {
            try
            {
                SqlParameter[] parameters = {
                    new SqlParameter("@PKBuilding", entity.BuildingID),
                    new SqlParameter("@Name", entity.Name),
                    new SqlParameter("@Description", entity.Description ?? (object)DBNull.Value),
                    new SqlParameter("@FKSite", entity.SiteID),
                    new SqlParameter("@Available", entity.Available),
                    new SqlParameter("@UpdatedBy", entity.UpdatedBy ?? (object)DBNull.Value)
                };

                DataTable result = await _dbConnect.GetDataSPAsync("up_ChgBuilding", parameters);

                return new DBResponse
                {
                    id = result.Rows[0].Field<int>("PKBuilding"),
                    message = result.Rows[0].Field<string>("Message") ?? ""
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating building");
                throw;
            }
        }

        public async Task<DBResponse> RemoveAsync(Building entity)
        {
            try
            {
                SqlParameter[] parameters = { new SqlParameter("@PKBuilding", entity.BuildingID) };
                DataTable result = await _dbConnect.GetDataSPAsync("up_RmvBuilding", parameters);

                return new DBResponse
                {
                    id = result.Rows[0].Field<int>("PKBuilding"),
                    message = result.Rows[0].Field<string>("Message") ?? ""
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error removing building");
                throw;
            }
        }

        public async Task<IEnumerable<Building>> GetBySiteIdAsync(int siteId)
        {
            try
            {
                SqlParameter[] parameters = { new SqlParameter("@SiteId", siteId) };
                DataTable result = await _dbConnect.GetDataSPAsync("up_GetBuildingsBySite", parameters);
                List<Building> list = new List<Building>();

                foreach (DataRow row in result.Rows)
                {
                    list.Add(new Building
                    {
                        BuildingID = row.Field<int>("PKBuilding"),
                        Name = row.Field<string>("Name") ?? string.Empty,
                        Description = row.Field<string>("Description"),
                        SiteID = row.Field<int?>("FKSite") ?? 0,
                        Available = row.Field<bool?>("Available") ?? true,
                        CreatedAt = row.Field<DateTime?>("CreatedAt") ?? DateTime.Now,
                        UpdatedAt = row.Field<DateTime?>("UpdatedAt") ?? DateTime.Now,
                        CreatedBy = row.Field<string>("CreatedBy"),
                        UpdatedBy = row.Field<string>("UpdatedBy")
                    });
                }

                return list;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting buildings by site");
                throw;
            }
        }

        public Task<Building> GetByUuidAsync(string UuId)
        {
            throw new NotImplementedException();
        }
    }
}
