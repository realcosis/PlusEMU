using Plus.Communication.Packets;
using Plus.HabboHotel.Achievements;
using Plus.HabboHotel.Badges;
using Plus.HabboHotel.Bots;
using Plus.HabboHotel.Cache;
using Plus.HabboHotel.Catalog;
using Plus.HabboHotel.GameClients;
using Plus.HabboHotel.Games;
using Plus.HabboHotel.Groups;
using Plus.HabboHotel.Items;
using Plus.HabboHotel.Items.Televisions;
using Plus.HabboHotel.LandingView;
using Plus.HabboHotel.Moderation;
using Plus.HabboHotel.Navigator;
using Plus.HabboHotel.Permissions;
using Plus.HabboHotel.Quests;
using Plus.HabboHotel.Rewards;
using Plus.HabboHotel.Rooms;
using Plus.HabboHotel.Rooms.Chat;
using Plus.HabboHotel.Subscriptions;
using Plus.HabboHotel.Talents;

namespace Plus.HabboHotel;

public interface IGame
{
    void StartGameLoop();
    void StopGameLoop();
    [Obsolete("Use dependency injection instead.")]
    IPacketManager GetPacketManager();
    [Obsolete("Use dependency injection instead.")]
    IGameClientManager GetClientManager();
    [Obsolete("Use dependency injection instead.")]
    ICatalogManager GetCatalog();
    [Obsolete("Use dependency injection instead.")]
    INavigatorManager GetNavigator();
    [Obsolete("Use dependency injection instead.")]
    IItemDataManager GetItemManager();
    [Obsolete("Use dependency injection instead.")]
    IRoomManager GetRoomManager();
    [Obsolete("Use dependency injection instead.")]
    IAchievementManager GetAchievementManager();
    [Obsolete("Use dependency injection instead.")]
    ITalentTrackManager GetTalentTrackManager();
    [Obsolete("Use dependency injection instead.")]
    IModerationManager GetModerationManager();
    [Obsolete("Use dependency injection instead.")]
    IPermissionManager GetPermissionManager();
    [Obsolete("Use dependency injection instead.")]
    ISubscriptionManager GetSubscriptionManager();
    [Obsolete("Use dependency injection instead.")]
    IQuestManager GetQuestManager();
    [Obsolete("Use dependency injection instead.")]
    IGroupManager GetGroupManager();
    [Obsolete("Use dependency injection instead.")]
    ILandingViewManager GetLandingManager();
    [Obsolete("Use dependency injection instead.")]
    ITelevisionManager GetTelevisionManager();
    [Obsolete("Use dependency injection instead.")]
    IChatManager GetChatManager();
    [Obsolete("Use dependency injection instead.")]
    IGameDataManager GetGameDataManager();
    [Obsolete("Use dependency injection instead.")]
    IBotManager GetBotManager();
    [Obsolete("Use dependency injection instead.")]
    ICacheManager GetCacheManager();
    [Obsolete("Use dependency injection instead.")]
    IRewardManager GetRewardManager();
    [Obsolete("Use dependency injection instead.")]
    IBadgeManager GetBadgeManager();
    Task Init();
}