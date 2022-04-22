using System;
using System.Data;
using Plus.Database.Interfaces;

namespace Plus.Database;

public interface IDatabase
{
    bool IsConnected();
    [Obsolete]
    IQueryAdapter GetQueryReactor();
    IDbConnection Connection();
}