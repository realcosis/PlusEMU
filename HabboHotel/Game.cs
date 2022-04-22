using System.Threading;
using System.Threading.Tasks;
using Plus.Communication.Packets;
using Plus.Core;
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

public class Game
{
    private readonly AchievementManager _achievementManager;
    private readonly BadgeManager _badgeManager;
    private readonly BotManager _botManager;
    private readonly CacheManager _cacheManager;
    private readonly CatalogManager _catalogManager;
    private readonly ChatManager _chatManager;
    private readonly GameClientManager _clientManager;
    private readonly int _cycleSleepTime = 25;
    private readonly GameDataManager _gameDataManager;
    private readonly ServerStatusUpdater _globalUpdater;
    private readonly GroupManager _groupManager;
    private readonly ItemDataManager _itemDataManager;
    private readonly LandingViewManager _landingViewManager; //TODO: Rename class
    private readonly ModerationManager _moderationManager;
    private readonly NavigatorManager _navigatorManager;
    private readonly PacketManager _packetManager;
    private readonly PermissionManager _permissionManager;
    private readonly QuestManager _questManager;
    private readonly RewardManager _rewardManager;
    private readonly RoomManager _roomManager;
    private readonly SubscriptionManager _subscriptionManager;
    private readonly TalentTrackManager _talentTrackManager;
    private readonly TelevisionManager _televisionManager; //TODO: Initialize from the item manager.
    private bool _cycleActive;

    private bool _cycleEnded;
    private Task _gameCycle;

    public Game()
    {
        _packetManager = new PacketManager();
        _clientManager = new GameClientManager();
        _moderationManager = new ModerationManager();
        _moderationManager.Init();
        _itemDataManager = new ItemDataManager();
        _itemDataManager.Init();
        _catalogManager = new CatalogManager();
        _catalogManager.Init(_itemDataManager);
        _televisionManager = new TelevisionManager();
        _televisionManager.Init();
        _navigatorManager = new NavigatorManager();
        _navigatorManager.Init();
        _roomManager = new RoomManager();
        _roomManager.LoadModels();
        _chatManager = new ChatManager();
        _groupManager = new GroupManager();
        _groupManager.Init();
        _questManager = new QuestManager();
        _questManager.Init();
        _achievementManager = new AchievementManager();
        _achievementManager.Init();
        _talentTrackManager = new TalentTrackManager();
        _talentTrackManager.Init();
        _landingViewManager = new LandingViewManager();
        _landingViewManager.Init();
        _gameDataManager = new GameDataManager();
        _gameDataManager.Init();
        _globalUpdater = new ServerStatusUpdater();
        _globalUpdater.Init();
        _botManager = new BotManager();
        _botManager.Init();
        _cacheManager = new CacheManager();
        _rewardManager = new RewardManager();
        _rewardManager.Init();
        _badgeManager = new BadgeManager();
        _badgeManager.Init();
        _permissionManager = new PermissionManager();
        _permissionManager.Init();
        _subscriptionManager = new SubscriptionManager();
        _subscriptionManager.Init();
    }

    public void StartGameLoop()
    {
        _gameCycle = new Task(GameCycle);
        _gameCycle.Start();
        _cycleActive = true;
    }

    private void GameCycle()
    {
        while (_cycleActive)
        {
            _cycleEnded = false;
            PlusEnvironment.GetGame().GetRoomManager().OnCycle();
            PlusEnvironment.GetGame().GetClientManager().OnCycle();
            _cycleEnded = true;
            Thread.Sleep(_cycleSleepTime);
        }
    }

    public void StopGameLoop()
    {
        _cycleActive = false;
        while (!_cycleEnded) Thread.Sleep(_cycleSleepTime);
    }

    public PacketManager GetPacketManager() => _packetManager;

    public GameClientManager GetClientManager() => _clientManager;

    public CatalogManager GetCatalog() => _catalogManager;

    public NavigatorManager GetNavigator() => _navigatorManager;

    public ItemDataManager GetItemManager() => _itemDataManager;

    public RoomManager GetRoomManager() => _roomManager;

    public AchievementManager GetAchievementManager() => _achievementManager;

    public TalentTrackManager GetTalentTrackManager() => _talentTrackManager;

    public ModerationManager GetModerationManager() => _moderationManager;

    public PermissionManager GetPermissionManager() => _permissionManager;

    public SubscriptionManager GetSubscriptionManager() => _subscriptionManager;

    public QuestManager GetQuestManager() => _questManager;

    public GroupManager GetGroupManager() => _groupManager;

    public LandingViewManager GetLandingManager() => _landingViewManager;

    public TelevisionManager GetTelevisionManager() => _televisionManager;

    public ChatManager GetChatManager() => _chatManager;

    public GameDataManager GetGameDataManager() => _gameDataManager;

    public BotManager GetBotManager() => _botManager;

    public CacheManager GetCacheManager() => _cacheManager;

    public RewardManager GetRewardManager() => _rewardManager;

    public BadgeManager GetBadgeManager() => _badgeManager;
}