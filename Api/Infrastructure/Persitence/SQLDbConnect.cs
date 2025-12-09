using System.Data;
using System.Threading.Tasks;
using Microsoft.Data.SqlClient;
using Domain.DataBase;

namespace Infrastructure.Persitence
{
    public class SQLDbConnect : ISQLDbConnect
    {
        private readonly string _connectionString;

        public SQLDbConnect(string connectionString)
        {
            _connectionString = connectionString;
        }

        private SqlConnection CreateConnection()
        {
            return new SqlConnection(_connectionString);
        }

        public void ExecuteNonQuery(string query)
        {
            try
            {
                using (var conn = CreateConnection())
                {
                    conn.Open();
                    using (var cmd = new SqlCommand(query, conn))
                    {
                        cmd.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Error al ejecutar la consulta: {ex.Message}", ex);
            }
        }

        public async Task ExecuteNonQueryAsync(string query)
        {
            try
            {
                using (var conn = CreateConnection())
                {
                    await conn.OpenAsync().ConfigureAwait(false);
                    using (var cmd = new SqlCommand(query, conn))
                    {
                        await cmd.ExecuteNonQueryAsync().ConfigureAwait(false);
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Error al ejecutar la consulta async: {ex.Message}", ex);
            }
        }

        public DataTable GetData(string query)
        {
            try
            {
                using (var conn = CreateConnection())
                {
                    conn.Open();
                    using (var da = new SqlDataAdapter(query, conn))
                    {
                        var dt = new DataTable();
                        da.Fill(dt);
                        return dt;
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Error al obtener datos: {ex.Message}", ex);
            }
        }

        public async Task<DataTable> GetDataAsync(string query)
        {
            try
            {
                using (var conn = CreateConnection())
                {
                    await conn.OpenAsync().ConfigureAwait(false);
                    using (var cmd = new SqlCommand(query, conn))
                    using (var reader = await cmd.ExecuteReaderAsync().ConfigureAwait(false))
                    {
                        var dt = new DataTable();
                        dt.Load(reader);
                        return dt;
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Error al obtener datos async: {ex.Message}", ex);
            }
        }

        public DataTable GetDataSP(string spName, SqlParameter[] parameters = null)
        {
            try
            {
                using (var conn = CreateConnection())
                {
                    conn.Open();
                    using (var cmd = new SqlCommand(spName, conn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        if (parameters != null)
                            cmd.Parameters.AddRange(parameters);

                        using (var da = new SqlDataAdapter(cmd))
                        {
                            var dt = new DataTable();
                            da.Fill(dt);
                            return dt;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Error al obtener datos del SP: {ex.Message}", ex);
            }
        }

        public async Task<DataTable> GetDataSPAsync(string spName, SqlParameter[] parameters = null)
        {
            try
            {
                using (var conn = CreateConnection())
                {
                    await conn.OpenAsync().ConfigureAwait(false);
                    using (var cmd = new SqlCommand(spName, conn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        if (parameters != null)
                            cmd.Parameters.AddRange(parameters);

                        using (var reader = await cmd.ExecuteReaderAsync().ConfigureAwait(false))
                        {
                            var dt = new DataTable();
                            dt.Load(reader);
                            return dt;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Error al obtener datos del SP async: {ex.Message}", ex);
            }
        }

        public void SaveData(string query)
        {
            ExecuteNonQuery(query);
        }

        public async Task SaveDataAsync(string query)
        {
            await ExecuteNonQueryAsync(query);
        }

        public SqlConnection GetConnection()
        {
            throw new NotImplementedException();
        }

        public void CloseConnection()
        {
            throw new NotImplementedException();
        }
    }
}
