using Plus.HabboHotel.Users.Messenger;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Dapper;
using Plus.Database;

namespace Plus.HabboHotel.Friends
{
    internal class FriendsService : IFriendsService
    {
        private readonly IDatabase _database;

        public FriendsService(IDatabase database)
        {
            _database = database;
        }

        public async Task<List<MessengerBuddy>> GetBuddiesForUser(int userId)
        {
            using var connection = _database.Connection();
            var query = "SELECT users.id,users.username,users.motto,users.look,users.last_online,users.hide_inroom = '1' ,users.hide_online = '1' FROM users JOIN messenger_friendships ON users.id = messenger_friendships.user_one_id  WHERE messenger_friendships.user_two_id = @userId UNION ALL SELECT users.id,users.username,users.motto,users.look,users.last_online,users.hide_inroom,users.hide_online FROM users JOIN messenger_friendships ON users.id = messenger_friendships.user_two_id WHERE messenger_friendships.user_one_id = @userId";
            return (await connection.QueryAsync<MessengerBuddy>(query, new { userId })).ToList();
        }

        public async Task<List<MessengerRequest>> GetRequestsForUser(int userId)
        {
            using var connection = _database.Connection();
            return (await connection.QueryAsync<MessengerRequest>("SELECT messenger_requests.from_id,messenger_requests.to_id,users.username FROM users JOIN messenger_requests ON users.id = messenger_requests.from_id WHERE messenger_requests.to_id = @userId",
                new
                {
                    userId
                })).ToList();
        }
    }
}
