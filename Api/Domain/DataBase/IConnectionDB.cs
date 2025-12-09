using System.Data;
using System.Threading.Tasks;

namespace Domain.DataBase
{
    /// <summary>
    /// Interface for database connection operations.
    /// </summary>
    /// <typeparam name="DBType">The type of the database connection.</typeparam>
    /// <typeparam name="DBParameters">The type of the database parameters.</typeparam>
    public interface IConnectionDB<DBType, DBParameters> where DBType : class
    {
        /// <summary>
        /// Gets the database connection.
        /// </summary>
        /// <returns>The database connection.</returns>
        DBType GetConnection();

        /// <summary>
        /// Closes the database connection.
        /// </summary>
        void CloseConnection();

        /// <summary>
        /// Executes a query and returns the result as a DataTable.
        /// </summary>
        /// <param name="query">The query to execute.</param>
        /// <returns>A DataTable containing the result of the query.</returns>
        DataTable GetData(string query);

        /// <summary>
        /// Executes a query to save data.
        /// </summary>
        /// <param name="query">The query to execute.</param>
        void SaveData(string query);

        /// <summary>
        /// Executes a stored procedure and returns the result as a DataTable.
        /// </summary>
        /// <param name="spName">The name of the stored procedure.</param>
        /// <param name="param">The parameters for the stored procedure.</param>
        /// <returns>A DataTable containing the result of the stored procedure.</returns>
        DataTable GetDataSP(string spName, DBParameters[] param);

        /// <summary>
        /// Executes a non-query command.
        /// </summary>
        /// <param name="query">The query to execute.</param>
        void ExecuteNonQuery(string query);

        /// <summary>
        /// Asynchronously executes a query and returns the result as a DataTable.
        /// </summary>
        /// <param name="query">The query to execute.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains a DataTable with the result of the query.</returns>
        Task<DataTable> GetDataAsync(string query);

        /// <summary>
        /// Asynchronously executes a query to save data.
        /// </summary>
        /// <param name="query">The query to execute.</param>
        /// <returns>A task that represents the asynchronous operation.</returns>
        Task SaveDataAsync(string query);

        /// <summary>
        /// Asynchronously executes a stored procedure and returns the result as a DataTable.
        /// </summary>
        /// <param name="spName">The name of the stored procedure.</param>
        /// <param name="param">The parameters for the stored procedure.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains a DataTable with the result of the stored procedure.</returns>
        Task<DataTable> GetDataSPAsync(string spName, DBParameters[] param);

        /// <summary>
        /// Asynchronously executes a non-query command.
        /// </summary>
        /// <param name="query">The query to execute.</param>
        /// <returns>A task that represents the asynchronous operation.</returns>
        Task ExecuteNonQueryAsync(string query);
    }
}