using Microsoft.Data.SqlClient;

namespace Domain.DataBase
{
    public interface IAppConnectionDB: IConnectionDB<SqlConnection, SqlParameter>
    {
        
    }
}