using System.Data;

namespace Domain.DataBase
{
    public interface IConnectionDB<DBType, DBParameters> where DBType : class
    {
        DBType GetConnection();
        void CloseConnection();
        DataTable GetData(string query);
        void SaveData(string query);
        DataTable GetDataSP(string spName, DBParameters[] param);
        void ExecuteNonQuery(string query);
        Task<DataTable> GetDataAsync(string query);
        Task SaveDataAsync(string query);
        Task<DataTable> GetDataSPAsync(string spName, DBParameters[] param);
        Task ExecuteNonQueryAsync(string query);        
    }
}