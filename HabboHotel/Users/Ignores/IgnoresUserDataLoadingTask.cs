using Dapper;
using Plus.Communication.Packets.Outgoing.Rooms.Action;
using Plus.Database;
using Plus.HabboHotel.GameClients;
using Plus.HabboHotel.Users.Authentication;
using Plus.HabboHotel.Users.UserData;
using System.Linq;
using System.Threading.Tasks;

namespace Plus.HabboHotel.Users.Ignores
{
    internal class IgnoresUserDataLoadingTask : IUserDataLoadingTask
    {
        private readonly IDatabase _database;

        public IgnoresUserDataLoadingTask(IDatabase database)
        {
            _database = database;
        }

        public async Task Load(Habbo habbo)
        {
            using var connection = _database.Connection();
            var ignoredUsers = (await connection.QueryAsync<int>("SELECT * FROM `user_ignores` WHERE `user_id` = @userId", new { userId = habbo.Id })).ToList();
            habbo.IgnoresComponent = new(ignoredUsers);
        }
    }

    internal class IgnoresEventSynchronizer : IAuthenticationTask
    {
        private readonly IDatabase _database;
        private readonly IGameClientManager _gameClientManager;

        public IgnoresEventSynchronizer(IDatabase database, IGameClientManager gameClientManager)
        {
            _database = database;
            _gameClientManager = gameClientManager;
        }

        public Task UserLoggedIn(Habbo habbo)
        {
            habbo.IgnoresComponent.UserIgnored += async (_, args) => await RegisterIgnore(habbo, args.UserId);
            habbo.IgnoresComponent.UserUnignored += async (_, args) => await UnregisterIgnore(habbo, args.UserId);
            return Task.CompletedTask;
        }

        public async Task RegisterIgnore(Habbo habbo, int targetId)
        {
            using var connection = _database.Connection();
            await connection.ExecuteAsync("INSERT INTO user_ignores (user_id, ignore_id) VALUES (@userId, @targetId)", new { userId = habbo.Id, targetId });
            var name = await _gameClientManager.GetNameById(targetId);
            habbo.GetClient().SendPacket(new IgnoreStatusComposer(1, name));
        }
        public async Task UnregisterIgnore(Habbo habbo, int targetId)
        {
            using var connection = _database.Connection();
            await connection.ExecuteAsync("DELETE FROM user_ignores WHERE user_id = @userId AND ignore_id = @targetId", new { userId = habbo.Id, targetId });
            var name = await _gameClientManager.GetNameById(targetId);
            habbo.GetClient().SendPacket(new IgnoreStatusComposer(3, name));
        }
    }
}
