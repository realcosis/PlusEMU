using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Data;
using Plus.Communication.Packets.Outgoing.Handshake;
using Plus.Communication.Packets.Outgoing.Inventory.Purse;
using Plus.Communication.Packets.Outgoing.Navigator;
using Plus.Communication.Packets.Outgoing.Rooms.Engine;
using Plus.Communication.Packets.Outgoing.Rooms.Session;
using Plus.Core;
using Plus.HabboHotel.Achievements;
using Plus.HabboHotel.GameClients;
using Plus.HabboHotel.Groups;
using Plus.HabboHotel.Rooms;
using Plus.HabboHotel.Rooms.Chat.Commands;
using Plus.HabboHotel.Subscriptions;
using Plus.HabboHotel.Users.Badges;
using Plus.HabboHotel.Users.Clothing;
using Plus.HabboHotel.Users.Effects;
using Plus.HabboHotel.Users.Ignores;
using Plus.HabboHotel.Users.Inventory;
using Plus.HabboHotel.Users.Messenger;
using Plus.HabboHotel.Users.Messenger.FriendBar;
using Plus.HabboHotel.Users.Navigator.SavedSearches;
using Plus.HabboHotel.Users.Permissions;
using Plus.HabboHotel.Users.Process;
using Plus.HabboHotel.Users.Relationships;
using Plus.Utilities;

using Dapper;

namespace Plus.HabboHotel.Users;

public class Habbo
{
    private HabboStats _habboStats;

    //Room related

    private readonly DateTime _timeCached;
    //Abilitys triggered by generic events.
    private BadgeComponent _badgeComponent;

    private GameClient _client;
    private ClothingComponent _clothing;

    //Player saving.
    private bool _disconnected;

    //Fastfood

    //Counters
    private EffectsComponent _fx;

    private bool _habboSaved;

    //Advertising reporting system.

    //Generic player values.
    private IgnoresComponent _ignores;
    private InventoryComponent _inventoryComponent;

    //Anti-script placeholders.
    private HabboMessenger _messenger;

    private SearchesComponent _navigatorSearches;
    private PermissionComponent _permissions;

    //Just random fun stuff.
    private ProcessComponent _process;

    //Values generated within the game.
    public ConcurrentDictionary<string, UserAchievement> Achievements = new();
    public ArrayList FavoriteRooms = new();
    public Dictionary<int, int> Quests = new();

    public List<int> RatedRooms = new();
    public Dictionary<int, Relationship> Relationships = new();

    //public Habbo(int id, string username, int rank, string motto, string look, string gender, int credits, int activityPoints, int homeRoom,
    //    bool hasFriendRequestsDisabled, int lastOnline, bool appearOffline, bool hideInRoom, double createDate, int diamonds,
    //    string machineId, string clientVolume, bool chatPreference, bool focusPreference, bool petsMuted, bool botsMuted, bool advertisingReportBlocked, double lastNameChange,
    //    int gotwPoints, bool ignoreInvites, double timeMuted, double tradingLock, bool allowGifts, int friendBarState, bool disableForcedEffects, bool allowMimic, int vipRank)
    //{
    //    Id = id;
    //    Username = username;
    //    Rank = rank;
    //    Motto = motto;
    //    Look = look;
    //    Gender = gender.ToLower();
    //    FootballLook = PlusEnvironment.GetFigureManager().FilterFigure(look.ToLower());
    //    FootballGender = gender.ToLower();
    //    Credits = credits;
    //    Duckets = activityPoints;
    //    Diamonds = diamonds;
    //    GotwPoints = gotwPoints;
    //    HomeRoom = homeRoom;
    //    LastOnline = lastOnline;
    //    AccountCreated = createDate;
    //    ClientVolume = new List<int>();
    //    foreach (var str in clientVolume.Split(','))
    //    {
    //        var val = 0;
    //        if (int.TryParse(str, out val))
    //            ClientVolume.Add(int.Parse(str));
    //        else
    //            ClientVolume.Add(100);
    //    }
    //    LastNameChange = lastNameChange;
    //    MachineId = machineId;
    //    ChatPreference = chatPreference;
    //    FocusPreference = focusPreference;
    //    IsExpert = IsExpert;
    //    AppearOffline = appearOffline;
    //    AllowTradingRequests = true; //TODO
    //    AllowUserFollowing = true; //TODO
    //    AllowFriendRequests = hasFriendRequestsDisabled; //TODO
    //    AllowMessengerInvites = ignoreInvites;
    //    AllowPetSpeech = petsMuted;
    //    AllowBotSpeech = botsMuted;
    //    AllowPublicRoomStatus = hideInRoom;
    //    AllowConsoleMessages = true;
    //    AllowGifts = allowGifts;
    //    AllowMimic = allowMimic;
    //    ReceiveWhispers = true;
    //    IgnorePublicWhispers = false;
    //    PlayingFastFood = false;
    //    FriendbarState = FriendBarStateUtility.GetEnum(friendBarState);
    //    ChristmasDay = ChristmasDay;
    //    WantsToRideHorse = 0;
    //    TimeAfk = 0;
    //    DisableForcedEffects = disableForcedEffects;
    //    VipRank = vipRank;
    //    _disconnected = false;
    //    _habboSaved = false;
    //    ChangingName = false;
    //    FloodTime = 0;
    //    FriendCount = 0;
    //    TimeMuted = timeMuted;
    //    _timeCached = DateTime.Now;
    //    TradingLockExpiry = tradingLock;

    //    // TODO: 80O: Convert to IUserDataLoadingTask
    //    if (TradingLockExpiry > 0 && UnixTimestamp.GetNow() > TradingLockExpiry)
    //    {
    //        TradingLockExpiry = 0;
    //        using var dbClient = PlusEnvironment.GetDatabaseManager().GetQueryReactor();
    //        dbClient.RunQuery("UPDATE `user_info` SET `trading_locked` = '0' WHERE `user_id` = '" + id + "' LIMIT 1");
    //    }
    //    BannedPhraseCount = 0;
    //    SessionStart = UnixTimestamp.GetNow();
    //    MessengerSpamCount = 0;
    //    MessengerSpamTime = 0;
    //    CreditsUpdateTick = Convert.ToInt32(PlusEnvironment.GetSettingsManager().TryGetValue("user.currency_scheduler.tick"));
    //    TentId = 0;
    //    HopperId = 0;
    //    IsHopping = false;
    //    TeleporterId = 0;
    //    IsTeleporting = false;
    //    TeleportingRoomId = 0;
    //    RoomAuthOk = false;
    //    CurrentRoomId = 0;
    //    HasSpoken = false;
    //    LastAdvertiseReport = 0;
    //    AdvertisingReported = false;
    //    AdvertisingReportedBlocked = advertisingReportBlocked;
    //    WiredInteraction = false;
    //    QuestLastCompleted = 0;
    //    InventoryAlert = false;
    //    IgnoreBobbaFilter = false;
    //    WiredTeleporting = false;
    //    CustomBubbleId = 0;
    //    OnHelperDuty = false;
    //    FastfoodScore = 0;
    //    PetId = 0;
    //    TempInt = 0;
    //    LastGiftPurchaseTime = DateTime.Now;
    //    LastMottoUpdateTime = DateTime.Now;
    //    LastClothingUpdateTime = DateTime.Now;
    //    LastForumMessageUpdateTime = DateTime.Now;
    //    GiftPurchasingWarnings = 0;
    //    MottoUpdateWarnings = 0;
    //    ClothingUpdateWarnings = 0;
    //    SessionGiftBlocked = false;
    //    SessionMottoBlocked = false;
    //    SessionClothingBlocked = false;
    //    FavoriteRooms = new ArrayList();
    //    Achievements = new ConcurrentDictionary<string, UserAchievement>();
    //    Relationships = new Dictionary<int, Relationship>();
    //    RatedRooms = new List<int>();

    //    InitPermissions();
    //    DataRow statRow = null;
    //    
    //}

    public int Id { get; set; }

    public string Username { get; set; } = string.Empty;

    public int Rank { get; set; }

    public string Motto { get; set; } = string.Empty;

    public string Look { get; set; } = string.Empty;

    public string Gender { get; set; } = string.Empty;

    public string FootballLook { get; set; } = string.Empty;

    public string FootballGender { get; set; } = string.Empty;

    public int Credits { get; set; }

    public int Duckets { get; set; }

    public int Diamonds { get; set; }

    public int GotwPoints { get; set; }

    public int HomeRoom { get; set; }

    public double LastOnline { get; set; }

    public double AccountCreated { get; set; }

    public List<int> ClientVolume { get; set; } = new() { 0, 0, 0};

    public double LastNameChange { get; set; }

    public string MachineId { get; set; }

    public bool ChatPreference { get; set; }

    public bool FocusPreference { get; set; }

    public bool IsExpert { get; set; }

    public bool AppearOffline { get; set; }

    public int VipRank { get; set; }

    public int TempInt { get; set; }

    public bool AllowTradingRequests { get; set; }

    public bool AllowUserFollowing { get; set; }

    public bool AllowFriendRequests { get; set; }

    public bool AllowMessengerInvites { get; set; }

    public bool AllowPetSpeech { get; set; }

    public bool AllowBotSpeech { get; set; }

    public bool AllowPublicRoomStatus { get; set; }

    public bool AllowConsoleMessages { get; set; }

    public bool AllowGifts { get; set; }

    public bool AllowMimic { get; set; }

    public bool ReceiveWhispers { get; set; }

    public bool IgnorePublicWhispers { get; set; }

    public bool PlayingFastFood { get; set; }

    public FriendBarState FriendbarState { get; set; }

    public int ChristmasDay { get; set; }

    public int WantsToRideHorse { get; set; }

    public int TimeAfk { get; set; }

    public bool DisableForcedEffects { get; set; }

    public bool ChangingName { get; set; }

    public int FriendCount { get; set; }

    public double FloodTime { get; set; }

    public int BannedPhraseCount { get; set; }

    public bool RoomAuthOk { get; set; }

    public int CurrentRoomId { get; set; }

    public int QuestLastCompleted { get; set; }

    public int MessengerSpamCount { get; set; }

    public double MessengerSpamTime { get; set; }

    public double TimeMuted { get; set; }

    public double TradingLockExpiry { get; set; }

    public double SessionStart { get; set; }

    public int TentId { get; set; }

    public int HopperId { get; set; }

    public bool IsHopping { get; set; }

    public int TeleporterId { get; set; }

    public bool IsTeleporting { get; set; }

    public int TeleportingRoomId { get; set; }

    public bool HasSpoken { get; set; }

    public double LastAdvertiseReport { get; set; }

    public bool AdvertisingReported { get; set; }

    public bool AdvertisingReportedBlocked { get; set; }

    public bool WiredInteraction { get; set; }

    public bool InventoryAlert { get; set; }

    public bool IgnoreBobbaFilter { get; set; }

    public bool WiredTeleporting { get; set; }

    public int CustomBubbleId { get; set; }

    public bool OnHelperDuty { get; set; }

    public int FastfoodScore { get; set; }

    public int PetId { get; set; }

    public int CreditsUpdateTick { get; set; }

    public IChatCommand ChatCommand { get; set; }

    public DateTime LastGiftPurchaseTime { get; set; }

    public DateTime LastMottoUpdateTime { get; set; }

    public DateTime LastClothingUpdateTime { get; set; }

    public DateTime LastForumMessageUpdateTime { get; set; }

    public int GiftPurchasingWarnings { get; set; }

    public int MottoUpdateWarnings { get; set; }

    public int ClothingUpdateWarnings { get; set; }

    public bool SessionGiftBlocked { get; set; }

    public bool SessionMottoBlocked { get; set; }

    public bool SessionClothingBlocked { get; set; }

    public bool InRoom => CurrentRoomId >= 1 && CurrentRoom != null;

    public Room? CurrentRoom
    {
        // TODO: Cache Room instead of fetching it from RoomManager
        get
        {
            if (CurrentRoomId <= 0)
                return null;
            if (PlusEnvironment.GetGame().GetRoomManager().TryGetRoom(CurrentRoomId, out var room))
                return room;
            return null;
        }
    }

    public string GetQueryString
    {
        get
        {
            _habboSaved = true;
            return "UPDATE `users` SET `online` = '0', `last_online` = '" + UnixTimestamp.GetNow() + "', `activity_points` = '" + Duckets + "', `credits` = '" + Credits +
                   "', `vip_points` = '" + Diamonds + "', `home_room` = '" + HomeRoom + "', `gotw_points` = '" + GotwPoints + "', `time_muted` = '" + TimeMuted + "',`friend_bar_state` = '" +
                   FriendBarStateUtility.GetInt(FriendbarState) + "' WHERE id = '" + Id + "' LIMIT 1;UPDATE `user_stats` SET `roomvisits` = '" + _habboStats.RoomVisits + "', `onlineTime` = '" +
                   (UnixTimestamp.GetNow() - SessionStart + _habboStats.OnlineTime) + "', `respect` = '" + _habboStats.Respect + "', `respectGiven` = '" + _habboStats.RespectGiven +
                   "', `giftsGiven` = '" + _habboStats.GiftsGiven + "', `giftsReceived` = '" + _habboStats.GiftsReceived + "', `dailyRespectPoints` = '" + _habboStats.DailyRespectPoints +
                   "', `dailyPetRespectPoints` = '" + _habboStats.DailyPetRespectPoints + "', `AchievementScore` = '" + _habboStats.AchievementPoints + "', `quest_id` = '" + _habboStats.QuestId +
                   "', `quest_progress` = '" + _habboStats.QuestProgress + "', `groupid` = '" + _habboStats.FavouriteGroupId + "',`forum_posts` = '" + _habboStats.ForumPosts + "' WHERE `id` = '" +
                   Id + "' LIMIT 1;";
        }
    }

    public HabboStats GetStats() => _habboStats;

    public bool CacheExpired()
    {
        var span = DateTime.Now - _timeCached;
        return span.TotalMinutes >= 30;
    }

    public bool InitProcess()
    {
        _process = new ProcessComponent();
        return _process.Init(this);
    }

    public bool InitSearches()
    {
        _navigatorSearches = new SearchesComponent();
        return _navigatorSearches.Init(this);
    }

    public bool InitFx()
    {
        _fx = new EffectsComponent();
        return _fx.Init(this);
    }

    public bool InitClothing()
    {
        _clothing = new ClothingComponent();
        return _clothing.Init(this);
    }

    public bool InitIgnores()
    {
        _ignores = new IgnoresComponent();
        return _ignores.Init(this);
    }

    public void Init(GameClient client)
    {
        // Move each of these loading tasks to their own IUserDataLoadingTask implementation.
        //foreach (var id in data.FavouritedRooms) FavoriteRooms.Add(id);
        _client = client;
        _badgeComponent = new BadgeComponent(this);
        _inventoryComponent = new InventoryComponent(Id, client);
        //Quests = data.Quests;
        _messenger = new(Id);
        _messenger.Init(new Dictionary<int, MessengerBuddy>(), new Dictionary<int, MessengerRequest>());
        _disconnected = false;
        InitSearches();
        InitFx();
        InitClothing();
        InitIgnores();
        _permissions = new PermissionComponent();
        _permissions.Init(this);
    }


    public PermissionComponent GetPermissions() => _permissions;

    public IgnoresComponent GetIgnores() => _ignores;

    public void OnDisconnect()
    {
        if (_disconnected)
            return;
        try
        {
            if (_process != null)
                _process.Dispose();
        }
        catch { }
        _disconnected = true;
        PlusEnvironment.GetGame().GetClientManager().UnregisterClient(Id, Username);
        if (!_habboSaved)
        {
            _habboSaved = true;
            using var dbClient = PlusEnvironment.GetDatabaseManager().GetQueryReactor();
            dbClient.RunQuery("UPDATE `users` SET `online` = '0', `last_online` = '" + (int)UnixTimestamp.GetNow() + "', `activity_points` = '" + Duckets + "', `credits` = '" + Credits +
                              "', `vip_points` = '" + Diamonds + "', `home_room` = '" + HomeRoom + "', `gotw_points` = '" + GotwPoints + "', `time_muted` = '" + TimeMuted +
                              "',`friend_bar_state` = '" + FriendBarStateUtility.GetInt(FriendbarState) + "' WHERE id = '" + Id + "' LIMIT 1;UPDATE `user_stats` SET `roomvisits` = '" +
                              _habboStats.RoomVisits + "', `onlineTime` = '" + (int)(UnixTimestamp.GetNow() - SessionStart + _habboStats.OnlineTime) + "', `respect` = '" + _habboStats.Respect +
                              "', `respectGiven` = '" + _habboStats.RespectGiven + "', `giftsGiven` = '" + _habboStats.GiftsGiven + "', `giftsReceived` = '" + _habboStats.GiftsReceived +
                              "', `dailyRespectPoints` = '" + _habboStats.DailyRespectPoints + "', `dailyPetRespectPoints` = '" + _habboStats.DailyPetRespectPoints + "', `AchievementScore` = '" +
                              _habboStats.AchievementPoints + "', `quest_id` = '" + _habboStats.QuestId + "', `quest_progress` = '" + _habboStats.QuestProgress + "', `groupid` = '" +
                              _habboStats.FavouriteGroupId + "',`forum_posts` = '" + _habboStats.ForumPosts + "' WHERE `id` = '" + Id + "' LIMIT 1;");
            if (GetPermissions().HasRight("mod_tickets"))
                dbClient.RunQuery("UPDATE `moderation_tickets` SET `status` = 'open', `moderator_id` = '0' WHERE `status` ='picked' AND `moderator_id` = '" + Id + "'");
        }
        Dispose();
        _client = null;
    }

    public void Dispose()
    {
        if (_inventoryComponent != null)
            _inventoryComponent.SetIdleState();
        if (InRoom && CurrentRoom != null)
            CurrentRoom.GetRoomUserManager().RemoveUserFromRoom(_client, false);
        if (_messenger != null)
        {
            _messenger.AppearOffline = true;
            _messenger.Destroy();
        }
        if (_fx != null)
            _fx.Dispose();
        if (_clothing != null)
            _clothing.Dispose();
        if (_permissions != null)
            _permissions.Dispose();
        if (_ignores != null)
            _ignores.Dispose();
    }

    public void CheckCreditsTimer()
    {
        try
        {
            CreditsUpdateTick--;
            if (CreditsUpdateTick <= 0)
            {
                var creditUpdate = Convert.ToInt32(PlusEnvironment.GetSettingsManager().TryGetValue("user.currency_scheduler.credit_reward"));
                var ducketUpdate = Convert.ToInt32(PlusEnvironment.GetSettingsManager().TryGetValue("user.currency_scheduler.ducket_reward"));
                SubscriptionData subData = null;
                if (PlusEnvironment.GetGame().GetSubscriptionManager().TryGetSubscriptionData(VipRank, out subData))
                {
                    creditUpdate += subData.Credits;
                    ducketUpdate += subData.Duckets;
                }
                Credits += creditUpdate;
                Duckets += ducketUpdate;
                _client.SendPacket(new CreditBalanceComposer(Credits));
                _client.SendPacket(new HabboActivityPointNotificationComposer(Duckets, ducketUpdate));
                CreditsUpdateTick = Convert.ToInt32(PlusEnvironment.GetSettingsManager().TryGetValue("user.currency_scheduler.tick"));
            }
        }
        catch { }
    }

    public GameClient GetClient()
    {
        if (_client != null)
            return _client;
        return PlusEnvironment.GetGame().GetClientManager().GetClientByUserId(Id);
    }

    public HabboMessenger GetMessenger() => _messenger;

    public BadgeComponent GetBadgeComponent() => _badgeComponent;

    public InventoryComponent GetInventoryComponent() => _inventoryComponent;

    public SearchesComponent GetNavigatorSearches() => _navigatorSearches;

    public EffectsComponent Effects() => _fx;

    public ClothingComponent GetClothing() => _clothing;

    public int GetQuestProgress(int p)
    {
        var progress = 0;
        Quests.TryGetValue(p, out progress);
        return progress;
    }

    public UserAchievement GetAchievementData(string p)
    {
        UserAchievement achievement = null;
        Achievements.TryGetValue(p, out achievement);
        return achievement;
    }

    public void ChangeName(string username)
    {
        LastNameChange = UnixTimestamp.GetNow();
        Username = username;
        SaveKey("username", username);
        SaveKey("last_change", LastNameChange.ToString());
    }

    public void SaveKey(string key, string value)
    {
        using var dbClient = PlusEnvironment.GetDatabaseManager().GetQueryReactor();
        dbClient.SetQuery("UPDATE `users` SET " + key + " = @value WHERE `id` = '" + Id + "' LIMIT 1;");
        dbClient.AddParameter("value", value);
        dbClient.RunQuery();
    }

    public void PrepareRoom(int id, string password)
    {
        if (GetClient() == null || GetClient().GetHabbo() == null)
            return;
        if (GetClient().GetHabbo().InRoom)
        {
            Room oldRoom = null;
            if (!PlusEnvironment.GetGame().GetRoomManager().TryGetRoom(GetClient().GetHabbo().CurrentRoomId, out oldRoom))
                return;
            if (oldRoom.GetRoomUserManager() != null)
                oldRoom.GetRoomUserManager().RemoveUserFromRoom(GetClient(), false);
        }
        if (GetClient().GetHabbo().IsTeleporting && GetClient().GetHabbo().TeleportingRoomId != id)
        {
            GetClient().SendPacket(new CloseConnectionComposer());
            return;
        }
        Room room = null;
        if (!PlusEnvironment.GetGame().GetRoomManager().TryLoadRoom(id, out room))
        {
            GetClient().SendPacket(new CloseConnectionComposer());
            return;
        }
        if (room.IsCrashed)
        {
            GetClient().SendNotification("This room has crashed! :(");
            GetClient().SendPacket(new CloseConnectionComposer());
            return;
        }
        GetClient().GetHabbo().CurrentRoomId = room.RoomId;
        if (room.GetRoomUserManager().UserCount >= room.UsersMax && !GetClient().GetHabbo().GetPermissions().HasRight("room_enter_full") && GetClient().GetHabbo().Id != room.OwnerId)
        {
            GetClient().SendPacket(new CantConnectComposer(1));
            GetClient().SendPacket(new CloseConnectionComposer());
            return;
        }
        if (!GetPermissions().HasRight("room_ban_override") && room.GetBans().IsBanned(Id))
        {
            RoomAuthOk = false;
            GetClient().GetHabbo().RoomAuthOk = false;
            GetClient().SendPacket(new CantConnectComposer(4));
            GetClient().SendPacket(new CloseConnectionComposer());
            return;
        }
        GetClient().SendPacket(new OpenConnectionComposer());
        if (!room.CheckRights(GetClient(), true, true) && !GetClient().GetHabbo().IsTeleporting && !GetClient().GetHabbo().IsHopping)
        {
            if (room.Access == RoomAccess.Doorbell && !GetClient().GetHabbo().GetPermissions().HasRight("room_enter_locked"))
            {
                if (room.UserCount > 0)
                {
                    GetClient().SendPacket(new DoorbellComposer(""));
                    room.SendPacket(new DoorbellComposer(GetClient().GetHabbo().Username), true);
                    return;
                }
                GetClient().SendPacket(new FlatAccessDeniedComposer(""));
                GetClient().SendPacket(new CloseConnectionComposer());
                return;
            }
            if (room.Access == RoomAccess.Password && !GetClient().GetHabbo().GetPermissions().HasRight("room_enter_locked"))
            {
                if (password.ToLower() != room.Password.ToLower() || string.IsNullOrWhiteSpace(password))
                {
                    GetClient().SendPacket(new GenericErrorComposer(-100002));
                    GetClient().SendPacket(new CloseConnectionComposer());
                    return;
                }
            }
        }
        if (!EnterRoom(room))
            GetClient().SendPacket(new CloseConnectionComposer());
    }

    public bool EnterRoom(Room room)
    {
        if (room == null)
            GetClient().SendPacket(new CloseConnectionComposer());
        GetClient().SendPacket(new RoomReadyComposer(room.RoomId, room.ModelName));
        if (room.Wallpaper != "0.0")
            GetClient().SendPacket(new RoomPropertyComposer("wallpaper", room.Wallpaper));
        if (room.Floor != "0.0")
            GetClient().SendPacket(new RoomPropertyComposer("floor", room.Floor));
        GetClient().SendPacket(new RoomPropertyComposer("landscape", room.Landscape));
        GetClient().SendPacket(new RoomRatingComposer(room.Score, !(GetClient().GetHabbo().RatedRooms.Contains(room.RoomId) || room.OwnerId == GetClient().GetHabbo().Id)));


        using (var dbClient = PlusEnvironment.GetDatabaseManager().Connection())
        {
            dbClient.Execute("INSERT INTO user_roomvisits (user_id,room_id,entry_timestamp,exit_timestamp) VALUES (@userId, @roomId, @entryTimestamp, @exitTimestamp)",
                new
                {
                    userId = GetClient().GetHabbo().Id,
                    roomId = GetClient().GetHabbo().CurrentRoomId,
                    entryTimestamp = UnixTimestamp.GetNow(),
                    exitTimestamp = 0,
                });
        }

        if (room.OwnerId != Id)
        {
            GetClient().GetHabbo().GetStats().RoomVisits += 1;
            PlusEnvironment.GetGame().GetAchievementManager().ProgressAchievement(GetClient(), "ACH_RoomEntry", 1);
        }
        return true;
    }

    public void SetStats(HabboStats stats)
    {
        _habboStats = stats;
    }
}