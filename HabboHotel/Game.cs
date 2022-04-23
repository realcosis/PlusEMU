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

// Game services need to be implemented behind an interface.
// Dependency inject the required services in the IPacketEvent
// This class will be obsolete. Do not reference to Game().<Service> but inject it instead.
public class Game : IGame
{
    private readonly IPacketManager _packetManager;
    private readonly ILandingViewManager _landingViewManager; //TODO: Rename class
    private readonly IGameClientManager _clientManager;
    private readonly IModerationManager _moderationManager;
    private readonly IItemDataManager _itemDataManager;
    private readonly ICatalogManager _catalogManager;
    private readonly ITelevisionManager _televisionManager; //TODO: Initialize from the item manager.
    private readonly INavigatorManager _navigatorManager;
    private readonly IRoomManager _roomManager;
    private readonly IChatManager _chatManager;

    private AchievementManager _achievementManager;
    private BadgeManager _badgeManager;
    private BotManager _botManager;
    private CacheManager _cacheManager;
    private readonly int _cycleSleepTime = 25;
    private GameDataManager _gameDataManager;
    private ServerStatusUpdater _globalUpdater;
    private GroupManager _groupManager;
    private PermissionManager _permissionManager;
    private QuestManager _questManager;
    private RewardManager _rewardManager;
    private SubscriptionManager _subscriptionManager;
    private TalentTrackManager _talentTrackManager;
    private bool _cycleActive;

    private bool _cycleEnded;
    private Task _gameCycle;

    public Game(IPacketManager packetManager,
        ILandingViewManager landingViewManager,
        IGameClientManager gameClientManager,
        IModerationManager moderationManager,
        IItemDataManager itemDataManager,
        ICatalogManager catalogManager,
        ITelevisionManager televisionManager,
        INavigatorManager navigatorManager,
        IRoomManager roomManager,
        IChatManager chatManager)
    {
        _packetManager = packetManager;
        _landingViewManager = landingViewManager;
        _clientManager = gameClientManager;
        _moderationManager = moderationManager;
        _itemDataManager = itemDataManager;
        _catalogManager = catalogManager;
        _televisionManager = televisionManager;
        _navigatorManager = navigatorManager;
        _roomManager = roomManager;
        _chatManager = chatManager;
    }

    public Task Init()
    {
        _moderationManager.Init();
        _itemDataManager.Init();
        _catalogManager.Init(_itemDataManager);
        _televisionManager.Init();
        _navigatorManager.Init();
        _roomManager.LoadModels();
        _chatManager.Init();
        _groupManager = new GroupManager();
        _groupManager.Init();
        _questManager = new QuestManager();
        _questManager.Init();
        _achievementManager = new AchievementManager();
        _achievementManager.Init();
        _talentTrackManager = new TalentTrackManager();
        _talentTrackManager.Init();
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
        return Task.CompletedTask;
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

    public IPacketManager GetPacketManager() => _packetManager;

    public IGameClientManager GetClientManager() => _clientManager;

    public ICatalogManager GetCatalog() => _catalogManager;

    public INavigatorManager GetNavigator() => _navigatorManager;

    public IItemDataManager GetItemManager() => _itemDataManager;

    public IRoomManager GetRoomManager() => _roomManager;

    public AchievementManager GetAchievementManager() => _achievementManager;

    public TalentTrackManager GetTalentTrackManager() => _talentTrackManager;

    public IModerationManager GetModerationManager() => _moderationManager;

    public PermissionManager GetPermissionManager() => _permissionManager;

    public SubscriptionManager GetSubscriptionManager() => _subscriptionManager;

    public QuestManager GetQuestManager() => _questManager;

    public GroupManager GetGroupManager() => _groupManager;

    public ILandingViewManager GetLandingManager() => _landingViewManager;

    public ITelevisionManager GetTelevisionManager() => _televisionManager;

    public IChatManager GetChatManager() => _chatManager;

    public GameDataManager GetGameDataManager() => _gameDataManager;

    public BotManager GetBotManager() => _botManager;

    public CacheManager GetCacheManager() => _cacheManager;

    public RewardManager GetRewardManager() => _rewardManager;

    public BadgeManager GetBadgeManager() => _badgeManager;
}