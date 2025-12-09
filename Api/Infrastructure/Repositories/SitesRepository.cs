using System.Linq.Expressions;
using Domain.Models;
using Domain.Repositories;
using Shared.Response;
using System.Data;
using Microsoft.Data.SqlClient;
using Domain.DataBase;

namespace Infrastructure.Repositories
{
    public class SitesRepository : ISitesRepository
    {
        private readonly ISQLDbConnect _dbConnect;
        private readonly ILogger<SitesRepository> _logger;

        public SitesRepository(ISQLDbConnect dbConnect, ILogger<SitesRepository> logger)
        {
            _dbConnect = dbConnect;
            _logger = logger;
        }

        public async Task<Site> AddAsync(Site entity)
        {
            try
            {
                SqlParameter[] parameters = {
                    new SqlParameter("@PKSite", entity.SiteID),
                    new SqlParameter("@SiteName", entity.SiteName),
                    new SqlParameter("@Available", entity.Available),
                    new SqlParameter("@CreatedBy", entity.CreatedBy ?? (object)DBNull.Value),
                    new SqlParameter("@UpdatedBy", entity.UpdatedBy ?? (object)DBNull.Value)
                };

                DataTable result = await _dbConnect.GetDataSPAsync("up_AddSite", parameters);

                return new Site
                {
                    SiteID = result.Rows[0].Field<int>("PKSite"),
                    SiteName = result.Rows[0].Field<string>("SiteName") ?? string.Empty,
                    Available = result.Rows[0].Field<bool?>("Available") ?? true,
                    CreatedAt = result.Rows[0].Field<DateTime?>("CreatedAt") ?? DateTime.Now,
                    UpdatedAt = result.Rows[0].Field<DateTime?>("UpdatedAt") ?? DateTime.Now,
                    CreatedBy = result.Rows[0].Field<string>("CreatedBy"),
                    UpdatedBy = result.Rows[0].Field<string>("UpdatedBy")
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error adding site");
                throw;
            }
        }

        public async Task<IEnumerable<Site>> FindAsync(Expression<Func<Site, bool>> predicate)
        {
            try
            {
                throw new NotImplementedException();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error finding sites");
                throw;
            }
        }

        public async Task<IEnumerable<Site>> GetAllAsync()
        {
            try
            {
                DataTable result = await _dbConnect.GetDataSPAsync("up_GetSites", null);
                List<Site> list = new List<Site>();

                foreach (DataRow row in result.Rows)
                {
                    list.Add(new Site
                    {
                        SiteID = row.Field<int>("PKSite"),
                        SiteName = row.Field<string>("SiteName") ?? string.Empty,
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
                _logger.LogError(ex, "Error getting all sites");
                throw;
            }
        }

        public async Task<Site> GetByIdAsync(int id)
        {
            try
            {
                SqlParameter[] parameters = { new SqlParameter("@Id", id) };
                DataTable result = await _dbConnect.GetDataSPAsync("Up_GetSiteById", parameters);

                if (result.Rows.Count == 0 || result.Rows[0].Field<int?>("PKSite") == null)
                    return null;

                DataRow row = result.Rows[0];

                return new Site
                {
                    SiteID = row.Field<int>("PKSite"),
                    SiteName = row.Field<string>("SiteName") ?? string.Empty,
                    Available = row.Field<bool?>("Available") ?? true,
                    CreatedAt = row.Field<DateTime?>("CreatedAt") ?? DateTime.Now,
                    UpdatedAt = row.Field<DateTime?>("UpdatedAt") ?? DateTime.Now,
                    CreatedBy = row.Field<string>("CreatedBy"),
                    UpdatedBy = row.Field<string>("UpdatedBy")
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting site by id");
                throw;
            }
        }

        public async Task<DBResponse> UpdateAsync(Site entity)
        {
            try
            {
                SqlParameter[] parameters = {
                    new SqlParameter("@PKSite", entity.SiteID),
                    new SqlParameter("@SiteName", entity.SiteName),
                    new SqlParameter("@Available", entity.Available),
                    new SqlParameter("@UpdatedBy", entity.UpdatedBy ?? (object)DBNull.Value)
                };

                DataTable result = await _dbConnect.GetDataSPAsync("up_ChgSite", parameters);

                return new DBResponse
                {
                    id = result.Rows[0].Field<int>("PKSite"),
                    message = result.Rows[0].Field<string>("Message") ?? ""
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating site");
                throw;
            }
        }

        public async Task<DBResponse> RemoveAsync(Site entity)
        {
            try
            {
                SqlParameter[] parameters = { new SqlParameter("@PKSite", entity.SiteID) };
                DataTable result = await _dbConnect.GetDataSPAsync("up_RmvSite", parameters);

                return new DBResponse
                {
                    id = result.Rows[0].Field<int>("PKSite"),
                    message = result.Rows[0].Field<string>("Message") ?? ""
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error removing site");
                throw;
            }
        }

        public Task<Site> GetByUuidAsync(string UuId)
        {
            throw new NotImplementedException();
        }
    }
}
