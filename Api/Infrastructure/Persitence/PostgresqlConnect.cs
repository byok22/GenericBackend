using System.Data;
using Domain.DataBase;
using Npgsql;

namespace Infrastructure.Persitence
{
    public class PostgresqlConnect : IPostgresqlConnect
    {
        private readonly NpgsqlConnection _conn;

        public PostgresqlConnect(string connStr)
        {
            _conn = new NpgsqlConnection(connStr);
        }

        public void CloseConnection()
        {
            if (_conn.State == ConnectionState.Open)
            {
                _conn.Close();
            }
        }

        public NpgsqlConnection GetConnection()
        {
            if (_conn.State == ConnectionState.Closed)
            {
                _conn.Open();
            }
            return _conn;
        }

        public void ExecuteNonQuery(string query)
        {
            try
            {
                using var cmd = new NpgsqlCommand(query, GetConnection());
                cmd.ExecuteNonQuery();
                CloseConnection();
            }
            catch (Exception ex)
            {
                throw new Exception("Failed to execute non-query: " + ex.Message);
            }
        }

        public async Task ExecuteNonQueryAsync(string query)
        {
            try
            {
                using var cmd = new NpgsqlCommand(query, GetConnection());
                await cmd.ExecuteNonQueryAsync().ConfigureAwait(false);
                CloseConnection();
            }
            catch (Exception ex)
            {
                throw new Exception("Failed to execute non-query asynchronously: " + ex.Message);
            }
        }

        public DataTable GetData(string query)
        {
            try
            {
                DataTable dt = new DataTable();
                using var da = new NpgsqlDataAdapter(query, GetConnection());
                da.Fill(dt);
                CloseConnection();
                return dt;
            }
            catch (Exception ex)
            {
                throw new Exception("Failed to get data: " + ex.Message);
            }
        }

        public async Task<DataTable> GetDataAsync(string query)
        {
            try
            {
                DataTable dt = new DataTable();
                using var cmd = new NpgsqlCommand(query, GetConnection());
                using var reader = await cmd.ExecuteReaderAsync().ConfigureAwait(false);
                dt.Load(reader);
                CloseConnection();
                return dt;
            }
            catch (Exception ex)
            {
                throw new Exception("Failed to get data asynchronously: " + ex.Message);
            }
        }

        public DataTable GetDataSP(string spName, NpgsqlParameter[] param)
        {
            try
            {
                DataTable dt = new DataTable();
                using var cmd = new NpgsqlCommand(spName, GetConnection());
                cmd.CommandType = CommandType.StoredProcedure;
                if (param != null)
                    cmd.Parameters.AddRange(param);
                using var da = new NpgsqlDataAdapter(cmd);
                da.Fill(dt);
                CloseConnection();
                return dt;
            }
            catch (Exception ex)
            {
                throw new Exception("Failed to get data from stored procedure: " + ex.Message);
            }
        }

        public async Task<DataTable> GetDataSPAsync(string spName, NpgsqlParameter[] param)
        {
            try
            {
                DataTable dt = new DataTable();
                using var cmd = new NpgsqlCommand(spName, GetConnection());
                cmd.CommandType = CommandType.StoredProcedure;
                if (param != null)
                    cmd.Parameters.AddRange(param);
                using var reader = await cmd.ExecuteReaderAsync().ConfigureAwait(false);
                dt.Load(reader);
                CloseConnection();
                return dt;
            }
            catch (Exception ex)
            {
                throw new Exception("Failed to get data from stored procedure asynchronously: " + ex.Message);
            }
        }

        public void SaveData(string query)
        {
            try
            {
                using var cmd = new NpgsqlCommand(query, GetConnection());
                cmd.ExecuteNonQuery();
                CloseConnection();
            }
            catch (Exception ex)
            {
                throw new Exception("Failed to save data: " + ex.Message);
            }
        }

        public async Task SaveDataAsync(string query)
        {
            try
            {
                using var cmd = new NpgsqlCommand(query, GetConnection());
                await cmd.ExecuteNonQueryAsync().ConfigureAwait(false);
                CloseConnection();
            }
            catch (Exception ex)
            {
                throw new Exception("Failed to save data asynchronously: " + ex.Message);
            }
        }
    }
}