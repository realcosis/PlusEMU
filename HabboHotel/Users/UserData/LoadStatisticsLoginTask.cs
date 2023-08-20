using Dapper;
using Plus.Core;
using Plus.Database;
using Plus.HabboHotel.Groups;

namespace Plus.HabboHotel.Users.UserData;

internal class LoadStatisticsLoginTask : IUserDataLoadingTask
{
    private readonly IDatabase _database;

    public LoadStatisticsLoginTask(IDatabase database) => _database = database ?? throw new ArgumentNullException(nameof(database));

    public async Task Load(Habbo habbo)
    {
        if (habbo == null) throw new ArgumentNullException(nameof(habbo));

        using var connection = _database.Connection();

        var statRow = await connection.QueryFirstOrDefaultAsync<dynamic>(
            "SELECT RoomVisits, OnlineTime, Respect, RespectGiven, GiftsGiven, GiftsReceived, DailyRespectPoints, DailyPetRespectPoints, `AchievementScore` AS AchievementPoints, quest_id, quest_progress, groupid, respectsTimestamp, forum_posts FROM `user_statistics` WHERE `id` = @id LIMIT 1",
            new { id = habbo.Id });

        if (statRow == null)
        {
            await connection.ExecuteAsync(
                "INSERT INTO `user_statistics` (`id`) VALUES (@id) ON DUPLICATE KEY UPDATE `id` = VALUES(`id`)",
                new { id = habbo.Id });

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

        try
        {
            var stats = new HabboStats(
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

            if (statRow.respectsTimestamp != DateTime.Today.ToString("MM/dd"))
            {
                stats.RespectsTimestamp = DateTime.Today.ToString("MM/dd");
                var dailyRespects = 10;
                stats.DailyRespectPoints = dailyRespects;
                stats.DailyPetRespectPoints = dailyRespects;

                await connection.ExecuteAsync(
                    "UPDATE `user_statistics` SET `dailyRespectPoints` = @dailyRespects, `dailyPetRespectPoints` = @dailyRespects, `respectsTimestamp` = @respectsTimestamp WHERE `id` = @user_id",
                    new { dailyRespects, respectsTimestamp = DateTime.Today.ToString("MM/dd"), user_id = habbo.Id });
            }

            if (!PlusEnvironment.Game.GroupManager.TryGetGroup(stats.FavouriteGroupId, out Group g))
                stats.FavouriteGroupId = 0;

            habbo.HabboStats = stats;
        }
        catch (Exception e)
        {
            ExceptionLogger.LogException(e);
        }
    }
}
