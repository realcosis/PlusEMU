using Dapper;
using Plus.Database;

namespace Plus.HabboHotel.Users
{
    public class HabboStatsService : IHabboStatsService
    {
        private readonly IDatabase _database;

        public HabboStatsService(IDatabase database)
        {
            _database = database ?? throw new ArgumentNullException(nameof(database));
        }

        public async Task<HabboStats> LoadHabboStats(int userId)
        {
            using var connection = _database.Connection();

            var statRow = await connection.QueryFirstOrDefaultAsync<dynamic>(
                "SELECT RoomVisits, OnlineTime, Respect, RespectGiven, GiftsGiven, GiftsReceived, DailyRespectPoints, DailyPetRespectPoints, `AchievementScore` AS AchievementPoints, quest_id, quest_progress, groupid, respectsTimestamp, forum_posts FROM `user_statistics` WHERE `id` = @id LIMIT 1",
                new { id = userId });

            if (statRow == null)
            {
                await connection.ExecuteAsync(
                    "INSERT INTO `user_statistics` (`id`) VALUES (@id) ON DUPLICATE KEY UPDATE `id` = VALUES(`id`)",
                    new { id = userId });

                statRow = new
                {
                    RoomVisits = 0,
                    OnlineTime = 0.0,
                    Respect = 0,
                    RespectGiven = 0,
                    GiftsGiven = 0,
                    GiftsReceived = 0,
                    DailyRespectPoints = 0,
                    DailyPetRespectPoints = 0,
                    AchievementPoints = 0,
                    quest_id = 0,
                    quest_progress = 0,
                    groupid = 0,
                    respectsTimestamp = "",
                    forum_posts = 0
                };
            }

            return new HabboStats(
                (int)statRow.RoomVisits,
                (double)statRow.OnlineTime,
                (int)statRow.Respect,
                (int)statRow.RespectGiven,
                (int)statRow.GiftsGiven,
                (int)statRow.GiftsReceived,
                (int)statRow.DailyRespectPoints,
                (int)statRow.DailyPetRespectPoints,
                (int)statRow.AchievementPoints,
                (int)statRow.quest_id,
                (int)statRow.quest_progress,
                (int)statRow.groupid,
                (string)statRow.respectsTimestamp,
                (int)statRow.forum_posts);
        }
    }
}
