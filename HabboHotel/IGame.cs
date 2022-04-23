using System.Threading.Tasks;
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
    IPacketManager GetPacketManager();
    IGameClientManager GetClientManager();
    CatalogManager GetCatalog();
    NavigatorManager GetNavigator();
    ItemDataManager GetItemManager();
    RoomManager GetRoomManager();
    AchievementManager GetAchievementManager();
    TalentTrackManager GetTalentTrackManager();
    ModerationManager GetModerationManager();
    PermissionManager GetPermissionManager();
    SubscriptionManager GetSubscriptionManager();
    QuestManager GetQuestManager();
    GroupManager GetGroupManager();
    ILandingViewManager GetLandingManager();
    TelevisionManager GetTelevisionManager();
    ChatManager GetChatManager();
    GameDataManager GetGameDataManager();
    BotManager GetBotManager();
    CacheManager GetCacheManager();
    RewardManager GetRewardManager();
    BadgeManager GetBadgeManager();
    Task Init();
}