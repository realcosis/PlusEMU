using System.Threading.Tasks;
using Dapper;
using Plus.Database;
using Plus.Utilities;

namespace Plus.HabboHotel.Ambassadors
{

    public class AmbassadorsManager : IAmbassadorsManager
    {
        private readonly IDatabase _database;

        public AmbassadorsManager(IDatabase database)
        {
            _database = database;
        }

        public async Task AddLogs(int userid, string target, string type)
        {
            using var connection = _database.Connection();
            await connection.ExecuteAsync("INSERT INTO `ambassador_logs` (`user_id`,`target`,`sanctions_type`,`timestamp`) VALUES (@user_id,@target_name,@sanctions_type,@timestamp)",
                new { user_id = userid, target_name = target, sanctions_type = type, timestamp = UnixTimestamp.GetNow() });
        }
    }
}
