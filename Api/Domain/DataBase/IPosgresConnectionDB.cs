using Npgsql;

namespace Domain.DataBase
{
    public interface IPosgresConnectionDB: IConnectionDB<NpgsqlConnection, NpgsqlParameter>
    {
        
    }
}