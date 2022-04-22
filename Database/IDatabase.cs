using System.Data;
using Plus.Database.Interfaces;

namespace Plus.Database;

public interface IDatabase
{
    bool IsConnected();
    IQueryAdapter GetQueryReactor();
    IDbConnection Connection();
}