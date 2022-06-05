using Dapper;
using Plus.Database;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Plus.HabboHotel.Users.UserData;

public class UserDataFactory : IUserDataFactory
{
    private readonly IDatabase _database;
    private readonly IEnumerable<IUserDataLoadingTask> _userDataLoadingTasks;

    public UserDataFactory(IDatabase database, IEnumerable<IUserDataLoadingTask> userDataLoadingTasks)
    {
        _database = database;
        _userDataLoadingTasks = userDataLoadingTasks;
    }

    public async Task<Habbo?> Create(int userId)
    {
        var habbo = await LoadHabboInfo(userId);

        foreach (var task in _userDataLoadingTasks)
            await task.Load(habbo);

        return habbo;
    }

    public async Task<string> GetUsernameForHabboById(int userId)
    {
        using var connection = _database.Connection();
        return await connection.ExecuteScalarAsync<string>("SELECT username FROM users WHERE id = @userId", new { userId });
    }

    private async Task<Habbo> LoadHabboInfo(int userId)
    {
        using var connection = _database.Connection();
        // TODO: 80O: Load `volume` via IUserDataLoadingTask
        var habbo = await connection.QuerySingleAsync<Habbo>(
            "SELECT `id`,`username`,`rank`,`motto`,`look`,`gender`,`last_online`,`credits`,`activity_points` as Duckets,`home_room`,`block_newfriends` = '1' as AllowFriendRequests,`hide_online` = '1' as AppearOffline,`hide_inroom` = '1' as AllowPublicRoomStatus,`vip`,`account_created`,`vip_points` as Diamonds,`chat_preference` = '1' as `chat_preference`,`focus_preference` = '1' as `focus_preference`, `pets_muted` = '1' as AllowPetSpeech,`bots_muted` = '1' as AllowBotSpeech,`advertising_report_blocked` = '1' as advertising_report_blocked,`last_change` as LastNameChange,`gotw_points`,`ignore_invites` = '1' as AllowMessengerInvites,`time_muted`,`allow_gifts` = '1' as `allow_gifts`,`friend_bar_state`,`disable_forced_effects` = '1' as `disable_forced_effects`,`allow_mimic` = '1' as `allow_mimic`,`rank_vip` as VipRank, `is_ambassador`, `bubble_id` as CustomBubbleId FROM `users` WHERE `id` = @userId LIMIT 1",
            new { userId });
        return habbo;
    }
}