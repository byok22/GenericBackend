using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Api.Domain.Models.DataBase;
using Microsoft.Data.SqlClient;

namespace Api.Infrastructure.Persitence
{
    public class SQLDbConnect : IConnectionDB<SqlConnection, SqlParameter>
    {
        private SqlConnection _conn;
        
        public SQLDbConnect(SqlConnection conn)
        {
            _conn = conn;
        }
        public void CloseConnection()
        {
            if (_conn.State == ConnectionState.Open)
            {
                _conn.Close();
            }
        }
        public SqlConnection GetConnection()
        {
               if (_conn.State == ConnectionState.Closed)
                {
                    _conn.Open();
                }
                 return _conn;

        }

        public void ExecuteNonQuery(string query)
        {
            SqlCommand cmd = new SqlCommand(query, GetConnection());
            cmd.ExecuteNonQuery();
            CloseConnection();
        }

        public async Task ExecuteNonQueryAsync(string query)
        {
            SqlCommand cmd = new SqlCommand(query, GetConnection());
            await cmd.ExecuteNonQueryAsync();
            CloseConnection();
        }

       

        public DataTable GetData(string query)
        {
             DataTable dt = new DataTable();
            SqlDataAdapter da = new SqlDataAdapter(query, GetConnection());
            da.Fill(dt);
            CloseConnection();
            return dt;
        }

        public  async Task<DataTable> GetDataAsync(string query)
        {
              DataTable dt = new DataTable();
                SqlCommand cmd = new SqlCommand(query, GetConnection());
                using (SqlDataReader reader = await cmd.ExecuteReaderAsync())
                {
                    dt.Load(reader);
                }
            CloseConnection();
                return dt;
        }

        public DataTable GetDataSP(string spName, SqlParameter[] param)
        {
             DataTable dt = new DataTable();
            SqlCommand cmd = new SqlCommand(spName, GetConnection());
            cmd.CommandType = CommandType.StoredProcedure;
            if (param != null)
                cmd.Parameters.AddRange(param);
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            da.Fill(dt);
           CloseConnection();
            return dt;
        }

        public async Task<DataTable> GetDataSPAsync(string spName, SqlParameter[] param)
        {
             DataTable dt = new DataTable();
                SqlCommand cmd = new SqlCommand(spName, GetConnection());
                cmd.CommandType = CommandType.StoredProcedure;
                if (param != null)
                    cmd.Parameters.AddRange(param);
                using (SqlDataReader reader = await cmd.ExecuteReaderAsync())
                {
                    dt.Load(reader);
                }
               CloseConnection();
                return dt;
        }

        public void SaveData(string query)
        {
             SqlCommand cmd = new SqlCommand(query, GetConnection());
            cmd.ExecuteNonQuery();
           CloseConnection();
        }

        public async Task SaveDataAsync(string query)
        {
            SqlCommand cmd = new SqlCommand(query, GetConnection());
            await cmd.ExecuteNonQueryAsync();
            CloseConnection();
           
        }
    }
}