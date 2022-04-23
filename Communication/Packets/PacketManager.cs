using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using NLog;
using Plus.Communication.Attributes;
using Plus.Communication.Packets.Incoming;
using Plus.Communication.Packets.Incoming.Avatar;
using Plus.Communication.Packets.Incoming.Catalog;
using Plus.Communication.Packets.Incoming.GameCenter;
using Plus.Communication.Packets.Incoming.Groups;
using Plus.Communication.Packets.Incoming.Handshake;
using Plus.Communication.Packets.Incoming.Help;
using Plus.Communication.Packets.Incoming.Inventory.Achievements;
using Plus.Communication.Packets.Incoming.Inventory.AvatarEffects;
using Plus.Communication.Packets.Incoming.Inventory.Badges;
using Plus.Communication.Packets.Incoming.Inventory.Bots;
using Plus.Communication.Packets.Incoming.Inventory.Furni;
using Plus.Communication.Packets.Incoming.Inventory.Pets;
using Plus.Communication.Packets.Incoming.Inventory.Purse;
using Plus.Communication.Packets.Incoming.Inventory.Trading;
using Plus.Communication.Packets.Incoming.LandingView;
using Plus.Communication.Packets.Incoming.Marketplace;
using Plus.Communication.Packets.Incoming.Messenger;
using Plus.Communication.Packets.Incoming.Misc;
using Plus.Communication.Packets.Incoming.Moderation;
using Plus.Communication.Packets.Incoming.Navigator;
using Plus.Communication.Packets.Incoming.Quests;
using Plus.Communication.Packets.Incoming.Rooms.Action;
using Plus.Communication.Packets.Incoming.Rooms.AI.Bots;
using Plus.Communication.Packets.Incoming.Rooms.AI.Pets;
using Plus.Communication.Packets.Incoming.Rooms.AI.Pets.Horse;
using Plus.Communication.Packets.Incoming.Rooms.Avatar;
using Plus.Communication.Packets.Incoming.Rooms.Chat;
using Plus.Communication.Packets.Incoming.Rooms.Connection;
using Plus.Communication.Packets.Incoming.Rooms.Engine;
using Plus.Communication.Packets.Incoming.Rooms.FloorPlan;
using Plus.Communication.Packets.Incoming.Rooms.Furni;
using Plus.Communication.Packets.Incoming.Rooms.Furni.LoveLocks;
using Plus.Communication.Packets.Incoming.Rooms.Furni.Moodlight;
using Plus.Communication.Packets.Incoming.Rooms.Furni.RentableSpaces;
using Plus.Communication.Packets.Incoming.Rooms.Furni.Stickys;
using Plus.Communication.Packets.Incoming.Rooms.Furni.Wired;
using Plus.Communication.Packets.Incoming.Rooms.Furni.YouTubeTelevisions;
using Plus.Communication.Packets.Incoming.Rooms.Settings;
using Plus.Communication.Packets.Incoming.Sound;
using Plus.Communication.Packets.Incoming.Talents;
using Plus.Communication.Packets.Incoming.Users;
using Plus.HabboHotel.GameClients;

namespace Plus.Communication.Packets;

public sealed class PacketManager : IPacketManager
{
    private static readonly ILogger Log = LogManager.GetLogger("Plus.Communication.Packets");

    /// <summary>
    ///     The task factory which is used for running Asynchronous tasks, in this case we use it to execute packets.
    /// </summary>
    private readonly TaskFactory _eventDispatcher;

    /// <summary>
    ///     Testing the Task code
    /// </summary>
    private readonly bool _ignoreTasks = true;

    private readonly Dictionary<int, Type> _headerToPacketMapping;
    private readonly Dictionary<int, IPacketEvent> _incomingPackets = new();

    /// <summary>
    ///     The maximum time a task can run for before it is considered dead
    ///     (can be used for debugging any locking issues with certain areas of code)
    /// </summary>
    private readonly int _maximumRunTimeInSec = 300; // 5 minutes
    private readonly Dictionary<int, string> _packetNames;

    /// <summary>
    ///     Currently running tasks to keep track of what the current load is
    /// </summary>
    private readonly ConcurrentDictionary<int, Task> _runningTasks;

    /// <summary>
    ///     Should the handler throw errors or log and continue.
    /// </summary>
    private readonly bool _throwUserErrors = false;

    public PacketManager(IEnumerable<IPacketEvent> incomingPackets)
    {
        _headerToPacketMapping = new Dictionary<int, Type>();
        _eventDispatcher = new TaskFactory(TaskCreationOptions.PreferFairness, TaskContinuationOptions.None);
        _runningTasks = new ConcurrentDictionary<int, Task>();
        _packetNames = new Dictionary<int, string>();

        RegisterHandshake();
        RegisterLandingView();
        RegisterCatalog();
        RegisterMarketplace();
        RegisterNavigator();
        RegisterNewNavigator();
        RegisterRoomAction();
        RegisterQuests();
        RegisterRoomConnection();
        RegisterRoomChat();
        RegisterRoomEngine();
        RegisterFurni();
        RegisterUsers();
        RegisterSound();
        RegisterMisc();
        RegisterInventory();
        RegisterTalents();
        RegisterPurse();
        RegisterRoomAvatar();
        RegisterAvatar();
        RegisterMessenger();
        RegisterGroups();
        RegisterRoomSettings();
        RegisterPets();
        RegisterBots();
        RegisterHelp();
        FloorPlanEditor();
        RegisterModeration();
        RegisterGameCenter();
        RegisterNames();

        foreach (var packet in incomingPackets)
        {
            var header = _headerToPacketMapping.FirstOrDefault(m => m.Value == packet.GetType()).Key;
            if (header == default) continue;
            _incomingPackets.Add(header, packet);
        }
    }

    public void TryExecutePacket(GameClient session, ClientPacket packet)
    {
        if (!_incomingPackets.TryGetValue(packet.Id, out var pak))
        {
            if (Debugger.IsAttached)
                Log.Debug("Unhandled Packet: " + packet);
            return;
        }
        
        if (Debugger.IsAttached)
        {
            if (_packetNames.ContainsKey(packet.Id))
                Log.Debug("Handled Packet: [" + packet.Id + "] " + _packetNames[packet.Id]);
            else
                Log.Debug("Handled Packet: [" + packet.Id + "] UnnamedPacketEvent");
        }

        var needAuthentication = pak.GetType().GetCustomAttribute<NoAuth>() is null;
        if (needAuthentication && session.GetHabbo() == null) // null-forgiving return
        {
            Log.Debug($"Session {session.ConnectionId} tried execute packet {packet.Id} but didn't handshake yet.");
            return;
        }
        
        if (!_ignoreTasks)
            ExecutePacketAsync(session, packet, pak);
        else
            pak.Parse(session, packet);
    }

    private void ExecutePacketAsync(GameClient session, ClientPacket packet, IPacketEvent pak)
    {
        var cancelSource = new CancellationTokenSource();
        var token = cancelSource.Token;
        var t = _eventDispatcher.StartNew(() =>
        {
            pak.Parse(session, packet);
            token.ThrowIfCancellationRequested();
        }, token);
        _runningTasks.TryAdd(t.Id, t);
        try
        {
            if (!t.Wait(_maximumRunTimeInSec * 1000, token)) cancelSource.Cancel();
        }
        catch (AggregateException ex)
        {
            foreach (var e in ex.Flatten().InnerExceptions)
            {
                if (_throwUserErrors)
                    throw e;
                else
                {
                    //log.Fatal("Unhandled Error: " + e.Message + " - " + e.StackTrace);
                    session.Disconnect();
                }
            }
        }
        catch (OperationCanceledException)
        {
            session.Disconnect();
        }
        finally
        {
            _runningTasks.TryRemove(t.Id, out var _);
            cancelSource.Dispose();

            //log.Debug("Event took " + (DateTime.Now - Start).Milliseconds + "ms to complete.");
        }
    }

    public void WaitForAllToComplete()
    {
        foreach (var t in _runningTasks.Values.ToList()) t.Wait();
    }

    public void UnregisterAll()
    {
        _headerToPacketMapping.Clear();
    }

    private void RegisterHandshake()
    {
        _headerToPacketMapping.Add(ClientPacketHeader.GetClientVersionMessageEvent, typeof(GetClientVersionEvent));
        _headerToPacketMapping.Add(ClientPacketHeader.InitCryptoMessageEvent, typeof(InitCryptoEvent));
        _headerToPacketMapping.Add(ClientPacketHeader.GenerateSecretKeyMessageEvent, typeof(GenerateSecretKeyEvent));
        _headerToPacketMapping.Add(ClientPacketHeader.UniqueIdMessageEvent, typeof(UniqueIdEvent));
        _headerToPacketMapping.Add(ClientPacketHeader.SsoTicketMessageEvent, typeof(SsoTicketEvent));
        _headerToPacketMapping.Add(ClientPacketHeader.InfoRetrieveMessageEvent, typeof(InfoRetrieveEvent));
        _headerToPacketMapping.Add(ClientPacketHeader.PingMessageEvent, typeof(PingEvent));
    }

    private void RegisterLandingView()
    {
        _headerToPacketMapping.Add(ClientPacketHeader.RefreshCampaignMessageEvent, typeof(RefreshCampaignEvent));
        _headerToPacketMapping.Add(ClientPacketHeader.GetPromoArticlesMessageEvent, typeof(GetPromoArticlesEvent));
    }

    private void RegisterCatalog()
    {
        _headerToPacketMapping.Add(ClientPacketHeader.GetCatalogModeMessageEvent, typeof(GetCatalogModeEvent));
        _headerToPacketMapping.Add(ClientPacketHeader.GetCatalogIndexMessageEvent, typeof(GetCatalogIndexEvent));
        _headerToPacketMapping.Add(ClientPacketHeader.GetCatalogPageMessageEvent, typeof(GetCatalogPageEvent));
        _headerToPacketMapping.Add(ClientPacketHeader.GetCatalogOfferMessageEvent, typeof(GetCatalogOfferEvent));
        _headerToPacketMapping.Add(ClientPacketHeader.PurchaseFromCatalogMessageEvent, typeof(PurchaseFromCatalogEvent));
        _headerToPacketMapping.Add(ClientPacketHeader.PurchaseFromCatalogAsGiftMessageEvent, typeof(PurchaseFromCatalogAsGiftEvent));
        _headerToPacketMapping.Add(ClientPacketHeader.PurchaseRoomPromotionMessageEvent, typeof(PurchaseRoomPromotionEvent));
        _headerToPacketMapping.Add(ClientPacketHeader.GetGiftWrappingConfigurationMessageEvent, typeof(GetGiftWrappingConfigurationEvent));
        _headerToPacketMapping.Add(ClientPacketHeader.GetMarketplaceConfigurationMessageEvent, typeof(GetMarketplaceConfigurationEvent));
        _headerToPacketMapping.Add(ClientPacketHeader.GetRecyclerRewardsMessageEvent, typeof(GetRecyclerRewardsEvent));
        _headerToPacketMapping.Add(ClientPacketHeader.CheckPetNameMessageEvent, typeof(CheckPetNameEvent));
        _headerToPacketMapping.Add(ClientPacketHeader.RedeemVoucherMessageEvent, typeof(RedeemVoucherEvent));
        _headerToPacketMapping.Add(ClientPacketHeader.GetSellablePetBreedsMessageEvent, typeof(GetSellablePetBreedsEvent));
        _headerToPacketMapping.Add(ClientPacketHeader.GetPromotableRoomsMessageEvent, typeof(GetPromotableRoomsEvent));
        _headerToPacketMapping.Add(ClientPacketHeader.GetCatalogRoomPromotionMessageEvent, typeof(GetCatalogRoomPromotionEvent));
        _headerToPacketMapping.Add(ClientPacketHeader.GetGroupFurniConfigMessageEvent, typeof(GetGroupFurniConfigEvent));
        _headerToPacketMapping.Add(ClientPacketHeader.CheckGnomeNameMessageEvent, typeof(CheckGnomeNameEvent));
        _headerToPacketMapping.Add(ClientPacketHeader.GetClubGiftsMessageEvent, typeof(GetClubGiftsEvent));
    }

    private void RegisterMarketplace()
    {
        _headerToPacketMapping.Add(ClientPacketHeader.GetOffersMessageEvent, typeof(GetOffersEvent));
        _headerToPacketMapping.Add(ClientPacketHeader.GetOwnOffersMessageEvent, typeof(GetOwnOffersEvent));
        _headerToPacketMapping.Add(ClientPacketHeader.GetMarketplaceCanMakeOfferMessageEvent, typeof(GetMarketplaceCanMakeOfferEvent));
        _headerToPacketMapping.Add(ClientPacketHeader.GetMarketplaceItemStatsMessageEvent, typeof(GetMarketplaceItemStatsEvent));
        _headerToPacketMapping.Add(ClientPacketHeader.MakeOfferMessageEvent, typeof(MakeOfferEvent));
        _headerToPacketMapping.Add(ClientPacketHeader.CancelOfferMessageEvent, typeof(CancelOfferEvent));
        _headerToPacketMapping.Add(ClientPacketHeader.BuyOfferMessageEvent, typeof(BuyOfferEvent));
        _headerToPacketMapping.Add(ClientPacketHeader.RedeemOfferCreditsMessageEvent, typeof(RedeemOfferCreditsEvent));
    }

    private void RegisterNavigator()
    {
        _headerToPacketMapping.Add(ClientPacketHeader.AddFavouriteRoomMessageEvent, typeof(AddFavouriteRoomEvent));
        _headerToPacketMapping.Add(ClientPacketHeader.GetUserFlatCatsMessageEvent, typeof(GetUserFlatCatsEvent));
        _headerToPacketMapping.Add(ClientPacketHeader.DeleteFavouriteRoomMessageEvent, typeof(RemoveFavouriteRoomEvent));
        _headerToPacketMapping.Add(ClientPacketHeader.GoToHotelViewMessageEvent, typeof(GoToHotelViewEvent));
        _headerToPacketMapping.Add(ClientPacketHeader.UpdateNavigatorSettingsMessageEvent, typeof(UpdateNavigatorSettingsEvent));
        _headerToPacketMapping.Add(ClientPacketHeader.CanCreateRoomMessageEvent, typeof(CanCreateRoomEvent));
        _headerToPacketMapping.Add(ClientPacketHeader.CreateFlatMessageEvent, typeof(CreateFlatEvent));
        _headerToPacketMapping.Add(ClientPacketHeader.GetGuestRoomMessageEvent, typeof(GetGuestRoomEvent));
        _headerToPacketMapping.Add(ClientPacketHeader.EditRoomPromotionMessageEvent, typeof(EditRoomEventEvent));
        _headerToPacketMapping.Add(ClientPacketHeader.GetEventCategoriesMessageEvent, typeof(GetNavigatorFlatsEvent));
    }

    public void RegisterNewNavigator()
    {
        _headerToPacketMapping.Add(ClientPacketHeader.InitializeNewNavigatorMessageEvent, typeof(InitializeNewNavigatorEvent));
        _headerToPacketMapping.Add(ClientPacketHeader.NavigatorSearchMessageEvent, typeof(NavigatorSearchEvent));
        _headerToPacketMapping.Add(ClientPacketHeader.FindRandomFriendingRoomMessageEvent, typeof(FindRandomFriendingRoomEvent));
    }

    private void RegisterQuests()
    {
        _headerToPacketMapping.Add(ClientPacketHeader.GetQuestListMessageEvent, typeof(GetQuestListEvent));
        _headerToPacketMapping.Add(ClientPacketHeader.StartQuestMessageEvent, typeof(StartQuestEvent));
        _headerToPacketMapping.Add(ClientPacketHeader.CancelQuestMessageEvent, typeof(CancelQuestEvent));
        _headerToPacketMapping.Add(ClientPacketHeader.GetCurrentQuestMessageEvent, typeof(GetCurrentQuestEvent));
        _headerToPacketMapping.Add(ClientPacketHeader.GetDailyQuestMessageEvent, typeof(GetDailyQuestEvent));
        //this._incomingPackets.Add(ClientPacketHeader.GetCommunityGoalHallOfFameMessageEvent, typeof(GetCommunityGoalHallOfFameEvent));
    }

    private void RegisterHelp()
    {
        _headerToPacketMapping.Add(ClientPacketHeader.OnBullyClickMessageEvent, typeof(OnBullyClickEvent));
        _headerToPacketMapping.Add(ClientPacketHeader.SendBullyReportMessageEvent, typeof(SendBullyReportEvent));
        _headerToPacketMapping.Add(ClientPacketHeader.SubmitBullyReportMessageEvent, typeof(SubmitBullyReportEvent));
        _headerToPacketMapping.Add(ClientPacketHeader.GetSanctionStatusMessageEvent, typeof(GetSanctionStatusEvent));
    }

    private void RegisterRoomAction()
    {
        _headerToPacketMapping.Add(ClientPacketHeader.LetUserInMessageEvent, typeof(LetUserInEvent));
        _headerToPacketMapping.Add(ClientPacketHeader.BanUserMessageEvent, typeof(BanUserEvent));
        _headerToPacketMapping.Add(ClientPacketHeader.KickUserMessageEvent, typeof(KickUserEvent));
        _headerToPacketMapping.Add(ClientPacketHeader.AssignRightsMessageEvent, typeof(AssignRightsEvent));
        _headerToPacketMapping.Add(ClientPacketHeader.RemoveRightsMessageEvent, typeof(RemoveRightsEvent));
        _headerToPacketMapping.Add(ClientPacketHeader.RemoveAllRightsMessageEvent, typeof(RemoveAllRightsEvent));
        _headerToPacketMapping.Add(ClientPacketHeader.MuteUserMessageEvent, typeof(MuteUserEvent));
        _headerToPacketMapping.Add(ClientPacketHeader.GiveHandItemMessageEvent, typeof(GiveHandItemEvent));
        _headerToPacketMapping.Add(ClientPacketHeader.RemoveMyRightsMessageEvent, typeof(RemoveMyRightsEvent));
    }

    private void RegisterAvatar()
    {
        _headerToPacketMapping.Add(ClientPacketHeader.GetWardrobeMessageEvent, typeof(GetWardrobeEvent));
        _headerToPacketMapping.Add(ClientPacketHeader.SaveWardrobeOutfitMessageEvent, typeof(SaveWardrobeOutfitEvent));
    }

    private void RegisterRoomAvatar()
    {
        _headerToPacketMapping.Add(ClientPacketHeader.ActionMessageEvent, typeof(ActionEvent));
        _headerToPacketMapping.Add(ClientPacketHeader.ApplySignMessageEvent, typeof(ApplySignEvent));
        _headerToPacketMapping.Add(ClientPacketHeader.DanceMessageEvent, typeof(DanceEvent));
        _headerToPacketMapping.Add(ClientPacketHeader.SitMessageEvent, typeof(SitEvent));
        _headerToPacketMapping.Add(ClientPacketHeader.ChangeMottoMessageEvent, typeof(ChangeMottoEvent));
        _headerToPacketMapping.Add(ClientPacketHeader.LookToMessageEvent, typeof(LookToEvent));
        _headerToPacketMapping.Add(ClientPacketHeader.DropHandItemMessageEvent, typeof(DropHandItemEvent));
        _headerToPacketMapping.Add(ClientPacketHeader.GiveRoomScoreMessageEvent, typeof(GiveRoomScoreEvent));
        _headerToPacketMapping.Add(ClientPacketHeader.IgnoreUserMessageEvent, typeof(IgnoreUserEvent));
        _headerToPacketMapping.Add(ClientPacketHeader.UnIgnoreUserMessageEvent, typeof(UnIgnoreUserEvent));
    }

    private void RegisterRoomConnection()
    {
        _headerToPacketMapping.Add(ClientPacketHeader.OpenFlatConnectionMessageEvent, typeof(OpenFlatConnectionEvent));
        _headerToPacketMapping.Add(ClientPacketHeader.GoToFlatMessageEvent, typeof(GoToFlatEvent));
    }

    private void RegisterRoomChat()
    {
        _headerToPacketMapping.Add(ClientPacketHeader.ChatMessageEvent, typeof(ChatEvent));
        _headerToPacketMapping.Add(ClientPacketHeader.ShoutMessageEvent, typeof(ShoutEvent));
        _headerToPacketMapping.Add(ClientPacketHeader.WhisperMessageEvent, typeof(WhisperEvent));
        _headerToPacketMapping.Add(ClientPacketHeader.StartTypingMessageEvent, typeof(StartTypingEvent));
        _headerToPacketMapping.Add(ClientPacketHeader.CancelTypingMessageEvent, typeof(CancelTypingEvent));
    }

    private void RegisterRoomEngine()
    {
        _headerToPacketMapping.Add(ClientPacketHeader.GetRoomEntryDataMessageEvent, typeof(GetRoomEntryDataEvent));
        _headerToPacketMapping.Add(ClientPacketHeader.GetFurnitureAliasesMessageEvent, typeof(GetFurnitureAliasesEvent));
        _headerToPacketMapping.Add(ClientPacketHeader.MoveAvatarMessageEvent, typeof(MoveAvatarEvent));
        _headerToPacketMapping.Add(ClientPacketHeader.MoveObjectMessageEvent, typeof(MoveObjectEvent));
        _headerToPacketMapping.Add(ClientPacketHeader.PickupObjectMessageEvent, typeof(PickupObjectEvent));
        _headerToPacketMapping.Add(ClientPacketHeader.MoveWallItemMessageEvent, typeof(MoveWallItemEvent));
        _headerToPacketMapping.Add(ClientPacketHeader.ApplyDecorationMessageEvent, typeof(ApplyDecorationEvent));
        _headerToPacketMapping.Add(ClientPacketHeader.PlaceObjectMessageEvent, typeof(PlaceObjectEvent));
        _headerToPacketMapping.Add(ClientPacketHeader.UseFurnitureMessageEvent, typeof(UseFurnitureEvent));
        _headerToPacketMapping.Add(ClientPacketHeader.UseWallItemMessageEvent, typeof(UseWallItemEvent));
    }

    private void RegisterInventory()
    {
        _headerToPacketMapping.Add(ClientPacketHeader.InitTradeMessageEvent, typeof(InitTradeEvent));
        _headerToPacketMapping.Add(ClientPacketHeader.TradingOfferItemMessageEvent, typeof(TradingOfferItemEvent));
        _headerToPacketMapping.Add(ClientPacketHeader.TradingOfferItemsMessageEvent, typeof(TradingOfferItemsEvent));
        _headerToPacketMapping.Add(ClientPacketHeader.TradingRemoveItemMessageEvent, typeof(TradingRemoveItemEvent));
        _headerToPacketMapping.Add(ClientPacketHeader.TradingAcceptMessageEvent, typeof(TradingAcceptEvent));
        _headerToPacketMapping.Add(ClientPacketHeader.TradingCancelMessageEvent, typeof(TradingCancelEvent));
        _headerToPacketMapping.Add(ClientPacketHeader.TradingConfirmMessageEvent, typeof(TradingConfirmEvent));
        _headerToPacketMapping.Add(ClientPacketHeader.TradingModifyMessageEvent, typeof(TradingModifyEvent));
        _headerToPacketMapping.Add(ClientPacketHeader.TradingCancelConfirmMessageEvent, typeof(TradingCancelConfirmEvent));
        _headerToPacketMapping.Add(ClientPacketHeader.RequestFurniInventoryMessageEvent, typeof(RequestFurniInventoryEvent));
        _headerToPacketMapping.Add(ClientPacketHeader.GetBadgesMessageEvent, typeof(GetBadgesEvent));
        _headerToPacketMapping.Add(ClientPacketHeader.GetAchievementsMessageEvent, typeof(GetAchievementsEvent));
        _headerToPacketMapping.Add(ClientPacketHeader.SetActivatedBadgesMessageEvent, typeof(SetActivatedBadgesEvent));
        _headerToPacketMapping.Add(ClientPacketHeader.GetBotInventoryMessageEvent, typeof(GetBotInventoryEvent));
        _headerToPacketMapping.Add(ClientPacketHeader.GetPetInventoryMessageEvent, typeof(GetPetInventoryEvent));
        _headerToPacketMapping.Add(ClientPacketHeader.AvatarEffectActivatedMessageEvent, typeof(AvatarEffectActivatedEvent));
        _headerToPacketMapping.Add(ClientPacketHeader.AvatarEffectSelectedMessageEvent, typeof(AvatarEffectSelectedEvent));
    }

    private void RegisterTalents()
    {
        _headerToPacketMapping.Add(ClientPacketHeader.GetTalentTrackMessageEvent, typeof(GetTalentTrackEvent));
    }

    private void RegisterPurse()
    {
        _headerToPacketMapping.Add(ClientPacketHeader.GetCreditsInfoMessageEvent, typeof(GetCreditsInfoEvent));
        _headerToPacketMapping.Add(ClientPacketHeader.GetHabboClubWindowMessageEvent, typeof(GetHabboClubWindowEvent));
    }

    private void RegisterUsers()
    {
        _headerToPacketMapping.Add(ClientPacketHeader.ScrGetUserInfoMessageEvent, typeof(ScrGetUserInfoEvent));
        _headerToPacketMapping.Add(ClientPacketHeader.SetChatPreferenceMessageEvent, typeof(SetChatPreferenceEvent));
        _headerToPacketMapping.Add(ClientPacketHeader.SetUserFocusPreferenceEvent, typeof(SetUserFocusPreferenceEvent));
        _headerToPacketMapping.Add(ClientPacketHeader.SetMessengerInviteStatusMessageEvent, typeof(SetMessengerInviteStatusEvent));
        _headerToPacketMapping.Add(ClientPacketHeader.RespectUserMessageEvent, typeof(RespectUserEvent));
        _headerToPacketMapping.Add(ClientPacketHeader.UpdateFigureDataMessageEvent, typeof(UpdateFigureDataEvent));
        _headerToPacketMapping.Add(ClientPacketHeader.OpenPlayerProfileMessageEvent, typeof(OpenPlayerProfileEvent));
        _headerToPacketMapping.Add(ClientPacketHeader.GetSelectedBadgesMessageEvent, typeof(GetSelectedBadgesEvent));
        _headerToPacketMapping.Add(ClientPacketHeader.GetRelationshipsMessageEvent, typeof(GetRelationshipsEvent));
        _headerToPacketMapping.Add(ClientPacketHeader.SetRelationshipMessageEvent, typeof(SetRelationshipEvent));
        _headerToPacketMapping.Add(ClientPacketHeader.CheckValidNameMessageEvent, typeof(CheckValidNameEvent));
        _headerToPacketMapping.Add(ClientPacketHeader.ChangeNameMessageEvent, typeof(ChangeNameEvent));
        _headerToPacketMapping.Add(ClientPacketHeader.GetHabboGroupBadgesMessageEvent, typeof(GetHabboGroupBadgesEvent));
        _headerToPacketMapping.Add(ClientPacketHeader.GetUserTagsMessageEvent, typeof(GetUserTagsEvent));
        _headerToPacketMapping.Add(ClientPacketHeader.GetIgnoredUsersMessageEvent, typeof(GetIgnoredUsersEvent));
    }

    private void RegisterSound()
    {
        _headerToPacketMapping.Add(ClientPacketHeader.SetSoundSettingsMessageEvent, typeof(SetSoundSettingsEvent));
        _headerToPacketMapping.Add(ClientPacketHeader.GetSongInfoMessageEvent, typeof(GetSongInfoEvent));
    }

    private void RegisterMisc()
    {
        _headerToPacketMapping.Add(ClientPacketHeader.EventTrackerMessageEvent, typeof(EventTrackerEvent));
        _headerToPacketMapping.Add(ClientPacketHeader.ClientVariablesMessageEvent, typeof(ClientVariablesEvent));
        _headerToPacketMapping.Add(ClientPacketHeader.DisconnectionMessageEvent, typeof(DisconnectEvent));
        _headerToPacketMapping.Add(ClientPacketHeader.LatencyTestMessageEvent, typeof(LatencyTestEvent));
        _headerToPacketMapping.Add(ClientPacketHeader.MemoryPerformanceMessageEvent, typeof(MemoryPerformanceEvent));
        _headerToPacketMapping.Add(ClientPacketHeader.SetFriendBarStateMessageEvent, typeof(SetFriendBarStateEvent));
    }


    private void RegisterMessenger()
    {
        _headerToPacketMapping.Add(ClientPacketHeader.MessengerInitMessageEvent, typeof(MessengerInitEvent));
        _headerToPacketMapping.Add(ClientPacketHeader.GetBuddyRequestsMessageEvent, typeof(GetBuddyRequestsEvent));
        _headerToPacketMapping.Add(ClientPacketHeader.FollowFriendMessageEvent, typeof(FollowFriendEvent));
        _headerToPacketMapping.Add(ClientPacketHeader.FindNewFriendsMessageEvent, typeof(FindNewFriendsEvent));
        _headerToPacketMapping.Add(ClientPacketHeader.FriendListUpdateMessageEvent, typeof(FriendListUpdateEvent));
        _headerToPacketMapping.Add(ClientPacketHeader.RemoveBuddyMessageEvent, typeof(RemoveBuddyEvent));
        _headerToPacketMapping.Add(ClientPacketHeader.RequestBuddyMessageEvent, typeof(RequestBuddyEvent));
        _headerToPacketMapping.Add(ClientPacketHeader.SendMsgMessageEvent, typeof(SendMsgEvent));
        _headerToPacketMapping.Add(ClientPacketHeader.SendRoomInviteMessageEvent, typeof(SendRoomInviteEvent));
        _headerToPacketMapping.Add(ClientPacketHeader.HabboSearchMessageEvent, typeof(HabboSearchEvent));
        _headerToPacketMapping.Add(ClientPacketHeader.AcceptBuddyMessageEvent, typeof(AcceptBuddyEvent));
        _headerToPacketMapping.Add(ClientPacketHeader.DeclineBuddyMessageEvent, typeof(DeclineBuddyEvent));
    }

    private void RegisterGroups()
    {
        _headerToPacketMapping.Add(ClientPacketHeader.JoinGroupMessageEvent, typeof(JoinGroupEvent));
        _headerToPacketMapping.Add(ClientPacketHeader.RemoveGroupFavouriteMessageEvent, typeof(RemoveGroupFavouriteEvent));
        _headerToPacketMapping.Add(ClientPacketHeader.SetGroupFavouriteMessageEvent, typeof(SetGroupFavouriteEvent));
        _headerToPacketMapping.Add(ClientPacketHeader.GetGroupInfoMessageEvent, typeof(GetGroupInfoEvent));
        _headerToPacketMapping.Add(ClientPacketHeader.GetGroupMembersMessageEvent, typeof(GetGroupMembersEvent));
        _headerToPacketMapping.Add(ClientPacketHeader.GetGroupCreationWindowMessageEvent, typeof(GetGroupCreationWindowEvent));
        _headerToPacketMapping.Add(ClientPacketHeader.GetBadgeEditorPartsMessageEvent, typeof(GetBadgeEditorPartsEvent));
        _headerToPacketMapping.Add(ClientPacketHeader.PurchaseGroupMessageEvent, typeof(PurchaseGroupEvent));
        _headerToPacketMapping.Add(ClientPacketHeader.UpdateGroupIdentityMessageEvent, typeof(UpdateGroupIdentityEvent));
        _headerToPacketMapping.Add(ClientPacketHeader.UpdateGroupBadgeMessageEvent, typeof(UpdateGroupBadgeEvent));
        _headerToPacketMapping.Add(ClientPacketHeader.UpdateGroupColoursMessageEvent, typeof(UpdateGroupColoursEvent));
        _headerToPacketMapping.Add(ClientPacketHeader.UpdateGroupSettingsMessageEvent, typeof(UpdateGroupSettingsEvent));
        _headerToPacketMapping.Add(ClientPacketHeader.ManageGroupMessageEvent, typeof(ManageGroupEvent));
        _headerToPacketMapping.Add(ClientPacketHeader.GiveAdminRightsMessageEvent, typeof(GiveAdminRightsEvent));
        _headerToPacketMapping.Add(ClientPacketHeader.TakeAdminRightsMessageEvent, typeof(TakeAdminRightsEvent));
        _headerToPacketMapping.Add(ClientPacketHeader.RemoveGroupMemberMessageEvent, typeof(RemoveGroupMemberEvent));
        _headerToPacketMapping.Add(ClientPacketHeader.AcceptGroupMembershipMessageEvent, typeof(AcceptGroupMembershipEvent));
        _headerToPacketMapping.Add(ClientPacketHeader.DeclineGroupMembershipMessageEvent, typeof(DeclineGroupMembershipEvent));
        _headerToPacketMapping.Add(ClientPacketHeader.DeleteGroupMessageEvent, typeof(DeleteGroupEvent));
    }

    private void RegisterRoomSettings()
    {
        _headerToPacketMapping.Add(ClientPacketHeader.GetRoomSettingsMessageEvent, typeof(GetRoomSettingsEvent));
        _headerToPacketMapping.Add(ClientPacketHeader.SaveRoomSettingsMessageEvent, typeof(SaveRoomSettingsEvent));
        _headerToPacketMapping.Add(ClientPacketHeader.DeleteRoomMessageEvent, typeof(DeleteRoomEvent));
        _headerToPacketMapping.Add(ClientPacketHeader.ToggleMuteToolMessageEvent, typeof(ToggleMuteToolEvent));
        _headerToPacketMapping.Add(ClientPacketHeader.GetRoomFilterListMessageEvent, typeof(GetRoomFilterListEvent));
        _headerToPacketMapping.Add(ClientPacketHeader.ModifyRoomFilterListMessageEvent, typeof(ModifyRoomFilterListEvent));
        _headerToPacketMapping.Add(ClientPacketHeader.GetRoomRightsMessageEvent, typeof(GetRoomRightsEvent));
        _headerToPacketMapping.Add(ClientPacketHeader.GetRoomBannedUsersMessageEvent, typeof(GetRoomBannedUsersEvent));
        _headerToPacketMapping.Add(ClientPacketHeader.UnbanUserFromRoomMessageEvent, typeof(UnbanUserFromRoomEvent));
        _headerToPacketMapping.Add(ClientPacketHeader.SaveEnforcedCategorySettingsMessageEvent, typeof(SaveEnforcedCategorySettingsEvent));
    }

    private void RegisterPets()
    {
        _headerToPacketMapping.Add(ClientPacketHeader.RespectPetMessageEvent, typeof(RespectPetEvent));
        _headerToPacketMapping.Add(ClientPacketHeader.GetPetInformationMessageEvent, typeof(GetPetInformationEvent));
        _headerToPacketMapping.Add(ClientPacketHeader.PickUpPetMessageEvent, typeof(PickUpPetEvent));
        _headerToPacketMapping.Add(ClientPacketHeader.PlacePetMessageEvent, typeof(PlacePetEvent));
        _headerToPacketMapping.Add(ClientPacketHeader.RideHorseMessageEvent, typeof(RideHorseEvent));
        _headerToPacketMapping.Add(ClientPacketHeader.ApplyHorseEffectMessageEvent, typeof(ApplyHorseEffectEvent));
        _headerToPacketMapping.Add(ClientPacketHeader.RemoveSaddleFromHorseMessageEvent, typeof(RemoveSaddleFromHorseEvent));
        _headerToPacketMapping.Add(ClientPacketHeader.ModifyWhoCanRideHorseMessageEvent, typeof(ModifyWhoCanRideHorseEvent));
        _headerToPacketMapping.Add(ClientPacketHeader.GetPetTrainingPanelMessageEvent, typeof(GetPetTrainingPanelEvent));
    }

    private void RegisterBots()
    {
        _headerToPacketMapping.Add(ClientPacketHeader.PlaceBotMessageEvent, typeof(PlaceBotEvent));
        _headerToPacketMapping.Add(ClientPacketHeader.PickUpBotMessageEvent, typeof(PickUpBotEvent));
        _headerToPacketMapping.Add(ClientPacketHeader.OpenBotActionMessageEvent, typeof(OpenBotActionEvent));
        _headerToPacketMapping.Add(ClientPacketHeader.SaveBotActionMessageEvent, typeof(SaveBotActionEvent));
    }

    private void RegisterFurni()
    {
        _headerToPacketMapping.Add(ClientPacketHeader.UpdateMagicTileMessageEvent, typeof(UpdateMagicTileEvent));
        _headerToPacketMapping.Add(ClientPacketHeader.GetYouTubeTelevisionMessageEvent, typeof(GetYouTubeTelevisionEvent));
        _headerToPacketMapping.Add(ClientPacketHeader.GetRentableSpaceMessageEvent, typeof(GetRentableSpaceEvent));
        _headerToPacketMapping.Add(ClientPacketHeader.ToggleYouTubeVideoMessageEvent, typeof(ToggleYouTubeVideoEvent));
        _headerToPacketMapping.Add(ClientPacketHeader.YouTubeVideoInformationMessageEvent, typeof(YouTubeVideoInformationEvent));
        _headerToPacketMapping.Add(ClientPacketHeader.YouTubeGetNextVideo, typeof(YouTubeGetNextVideo));
        _headerToPacketMapping.Add(ClientPacketHeader.SaveWiredTriggeRconfigMessageEvent, typeof(SaveWiredConfigEvent));
        _headerToPacketMapping.Add(ClientPacketHeader.SaveWiredEffectConfigMessageEvent, typeof(SaveWiredConfigEvent));
        _headerToPacketMapping.Add(ClientPacketHeader.SaveWiredConditionConfigMessageEvent, typeof(SaveWiredConfigEvent));
        _headerToPacketMapping.Add(ClientPacketHeader.SaveBrandingItemMessageEvent, typeof(SaveBrandingItemEvent));
        _headerToPacketMapping.Add(ClientPacketHeader.SetTonerMessageEvent, typeof(SetTonerEvent));
        _headerToPacketMapping.Add(ClientPacketHeader.DiceOffMessageEvent, typeof(DiceOffEvent));
        _headerToPacketMapping.Add(ClientPacketHeader.ThrowDiceMessageEvent, typeof(ThrowDiceEvent));
        _headerToPacketMapping.Add(ClientPacketHeader.SetMannequinNameMessageEvent, typeof(SetMannequinNameEvent));
        _headerToPacketMapping.Add(ClientPacketHeader.SetMannequinFigureMessageEvent, typeof(SetMannequinFigureEvent));
        _headerToPacketMapping.Add(ClientPacketHeader.CreditFurniRedeemMessageEvent, typeof(CreditFurniRedeemEvent));
        _headerToPacketMapping.Add(ClientPacketHeader.GetStickyNoteMessageEvent, typeof(GetStickyNoteEvent));
        _headerToPacketMapping.Add(ClientPacketHeader.AddStickyNoteMessageEvent, typeof(AddStickyNoteEvent));
        _headerToPacketMapping.Add(ClientPacketHeader.UpdateStickyNoteMessageEvent, typeof(UpdateStickyNoteEvent));
        _headerToPacketMapping.Add(ClientPacketHeader.DeleteStickyNoteMessageEvent, typeof(DeleteStickyNoteEvent));
        _headerToPacketMapping.Add(ClientPacketHeader.GetMoodlightConfigMessageEvent, typeof(GetMoodlightConfigEvent));
        _headerToPacketMapping.Add(ClientPacketHeader.MoodlightUpdateMessageEvent, typeof(MoodlightUpdateEvent));
        _headerToPacketMapping.Add(ClientPacketHeader.ToggleMoodlightMessageEvent, typeof(ToggleMoodlightEvent));
        _headerToPacketMapping.Add(ClientPacketHeader.UseOneWayGateMessageEvent, typeof(UseFurnitureEvent));
        _headerToPacketMapping.Add(ClientPacketHeader.UseHabboWheelMessageEvent, typeof(UseFurnitureEvent));
        _headerToPacketMapping.Add(ClientPacketHeader.OpenGiftMessageEvent, typeof(OpenGiftEvent));
        _headerToPacketMapping.Add(ClientPacketHeader.GetGroupFurniSettingsMessageEvent, typeof(GetGroupFurniSettingsEvent));
        _headerToPacketMapping.Add(ClientPacketHeader.UseSellableClothingMessageEvent, typeof(UseSellableClothingEvent));
        _headerToPacketMapping.Add(ClientPacketHeader.ConfirmLoveLockMessageEvent, typeof(ConfirmLoveLockEvent));
    }

    private void FloorPlanEditor()
    {
        _headerToPacketMapping.Add(ClientPacketHeader.SaveFloorPlanModelMessageEvent, typeof(SaveFloorPlanModelEvent));
        _headerToPacketMapping.Add(ClientPacketHeader.InitializeFloorPlanSessionMessageEvent, typeof(InitializeFloorPlanSessionEvent));
        _headerToPacketMapping.Add(ClientPacketHeader.FloorPlanEditorRoomPropertiesMessageEvent, typeof(FloorPlanEditorRoomPropertiesEvent));
    }

    private void RegisterModeration()
    {
        _headerToPacketMapping.Add(ClientPacketHeader.OpenHelpToolMessageEvent, typeof(OpenHelpToolEvent));
        _headerToPacketMapping.Add(ClientPacketHeader.GetModeratorRoomInfoMessageEvent, typeof(GetModeratorRoomInfoEvent));
        _headerToPacketMapping.Add(ClientPacketHeader.GetModeratorUserInfoMessageEvent, typeof(GetModeratorUserInfoEvent));
        _headerToPacketMapping.Add(ClientPacketHeader.GetModeratorUserRoomVisitsMessageEvent, typeof(GetModeratorUserRoomVisitsEvent));
        _headerToPacketMapping.Add(ClientPacketHeader.ModerateRoomMessageEvent, typeof(ModerateRoomEvent));
        _headerToPacketMapping.Add(ClientPacketHeader.ModeratorActionMessageEvent, typeof(ModeratorActionEvent));
        _headerToPacketMapping.Add(ClientPacketHeader.SubmitNewTicketMessageEvent, typeof(SubmitNewTicketEvent));
        _headerToPacketMapping.Add(ClientPacketHeader.GetModeratorRoomChatlogMessageEvent, typeof(GetModeratorRoomChatlogEvent));
        _headerToPacketMapping.Add(ClientPacketHeader.GetModeratorUserChatlogMessageEvent, typeof(GetModeratorUserChatlogEvent));
        _headerToPacketMapping.Add(ClientPacketHeader.GetModeratorTicketChatlogsMessageEvent, typeof(GetModeratorTicketChatlogsEvent));
        _headerToPacketMapping.Add(ClientPacketHeader.PickTicketMessageEvent, typeof(PickTicketEvent));
        _headerToPacketMapping.Add(ClientPacketHeader.ReleaseTicketMessageEvent, typeof(ReleaseTicketEvent));
        _headerToPacketMapping.Add(ClientPacketHeader.CloseTicketMesageEvent, typeof(CloseTicketEvent));
        _headerToPacketMapping.Add(ClientPacketHeader.ModerationMuteMessageEvent, typeof(ModerationMuteEvent));
        _headerToPacketMapping.Add(ClientPacketHeader.ModerationKickMessageEvent, typeof(ModerationKickEvent));
        _headerToPacketMapping.Add(ClientPacketHeader.ModerationBanMessageEvent, typeof(ModerationBanEvent));
        _headerToPacketMapping.Add(ClientPacketHeader.ModerationMsgMessageEvent, typeof(ModerationMsgEvent));
        _headerToPacketMapping.Add(ClientPacketHeader.ModerationCautionMessageEvent, typeof(ModerationCautionEvent));
        _headerToPacketMapping.Add(ClientPacketHeader.ModerationTradeLockMessageEvent, typeof(ModerationTradeLockEvent));
        _headerToPacketMapping.Add(ClientPacketHeader.CallForHelpPendingCallsDeletedMessageEvent, typeof(CallForHelpPendingCallsDeletedEvent));
        _headerToPacketMapping.Add(ClientPacketHeader.CloseIssueDefaultActionEvent, typeof(CloseIssueDefaultActionEvent));
    }

    public void RegisterGameCenter()
    {
        _headerToPacketMapping.Add(ClientPacketHeader.GetGameListingMessageEvent, typeof(GetGameListingEvent));
        _headerToPacketMapping.Add(ClientPacketHeader.InitializeGameCenterMessageEvent, typeof(InitializeGameCenterEvent));
        _headerToPacketMapping.Add(ClientPacketHeader.GetPlayableGamesMessageEvent, typeof(GetPlayableGamesEvent));
        _headerToPacketMapping.Add(ClientPacketHeader.JoinPlayerQueueMessageEvent, typeof(JoinPlayerQueueEvent));
        _headerToPacketMapping.Add(ClientPacketHeader.Game2GetWeeklyLeaderboardMessageEvent, typeof(Game2GetWeeklyLeaderboardEvent));
    }

    public void RegisterNames()
    {
        _packetNames.Add(ClientPacketHeader.GetClientVersionMessageEvent, "GetClientVersionEvent");
        _packetNames.Add(ClientPacketHeader.InitCryptoMessageEvent, "InitCryptoEvent");
        _packetNames.Add(ClientPacketHeader.GenerateSecretKeyMessageEvent, "GenerateSecretKeyEvent");
        _packetNames.Add(ClientPacketHeader.UniqueIdMessageEvent, "UniqueIDEvent");
        _packetNames.Add(ClientPacketHeader.SsoTicketMessageEvent, "SSOTicketEvent");
        _packetNames.Add(ClientPacketHeader.InfoRetrieveMessageEvent, "InfoRetrieveEvent");
        _packetNames.Add(ClientPacketHeader.PingMessageEvent, "PingEvent");
        _packetNames.Add(ClientPacketHeader.RefreshCampaignMessageEvent, "RefreshCampaignEvent");
        _packetNames.Add(ClientPacketHeader.GetPromoArticlesMessageEvent, "RefreshPromoEvent");
        _packetNames.Add(ClientPacketHeader.GetCatalogModeMessageEvent, "GetCatalogModeEvent");
        _packetNames.Add(ClientPacketHeader.GetCatalogIndexMessageEvent, "GetCatalogIndexEvent");
        _packetNames.Add(ClientPacketHeader.GetCatalogPageMessageEvent, "GetCatalogPageEvent");
        _packetNames.Add(ClientPacketHeader.GetCatalogOfferMessageEvent, "GetCatalogOfferEvent");
        _packetNames.Add(ClientPacketHeader.PurchaseFromCatalogMessageEvent, "PurchaseFromCatalogEvent");
        _packetNames.Add(ClientPacketHeader.PurchaseFromCatalogAsGiftMessageEvent, "PurchaseFromCatalogAsGiftEvent");
        _packetNames.Add(ClientPacketHeader.PurchaseRoomPromotionMessageEvent, "PurchaseRoomPromotionEvent");
        _packetNames.Add(ClientPacketHeader.GetGiftWrappingConfigurationMessageEvent, "GetGiftWrappingConfigurationEvent");
        _packetNames.Add(ClientPacketHeader.GetMarketplaceConfigurationMessageEvent, "GetMarketplaceConfigurationEvent");
        _packetNames.Add(ClientPacketHeader.GetRecyclerRewardsMessageEvent, "GetRecyclerRewardsEvent");
        _packetNames.Add(ClientPacketHeader.CheckPetNameMessageEvent, "CheckPetNameEvent");
        _packetNames.Add(ClientPacketHeader.RedeemVoucherMessageEvent, "RedeemVoucherEvent");
        _packetNames.Add(ClientPacketHeader.GetSellablePetBreedsMessageEvent, "GetSellablePetBreedsEvent");
        _packetNames.Add(ClientPacketHeader.GetPromotableRoomsMessageEvent, "GetPromotableRoomsEvent");
        _packetNames.Add(ClientPacketHeader.GetCatalogRoomPromotionMessageEvent, "GetCatalogRoomPromotionEvent");
        _packetNames.Add(ClientPacketHeader.GetGroupFurniConfigMessageEvent, "GetGroupFurniConfigEvent");
        _packetNames.Add(ClientPacketHeader.CheckGnomeNameMessageEvent, "CheckGnomeNameEvent");
        _packetNames.Add(ClientPacketHeader.GetOffersMessageEvent, "GetOffersEvent");
        _packetNames.Add(ClientPacketHeader.GetOwnOffersMessageEvent, "GetOwnOffersEvent");
        _packetNames.Add(ClientPacketHeader.GetMarketplaceCanMakeOfferMessageEvent, "GetMarketplaceCanMakeOfferEvent");
        _packetNames.Add(ClientPacketHeader.GetMarketplaceItemStatsMessageEvent, "GetMarketplaceItemStatsEvent");
        _packetNames.Add(ClientPacketHeader.MakeOfferMessageEvent, "MakeOfferEvent");
        _packetNames.Add(ClientPacketHeader.CancelOfferMessageEvent, "CancelOfferEvent");
        _packetNames.Add(ClientPacketHeader.BuyOfferMessageEvent, "BuyOfferEvent");
        _packetNames.Add(ClientPacketHeader.RedeemOfferCreditsMessageEvent, "RedeemOfferCreditsEvent");
        _packetNames.Add(ClientPacketHeader.AddFavouriteRoomMessageEvent, "AddFavouriteRoomEvent");
        _packetNames.Add(ClientPacketHeader.GetUserFlatCatsMessageEvent, "GetUserFlatCatsEvent");
        _packetNames.Add(ClientPacketHeader.DeleteFavouriteRoomMessageEvent, "RemoveFavouriteRoomEvent");
        _packetNames.Add(ClientPacketHeader.GoToHotelViewMessageEvent, "GoToHotelViewEvent");
        _packetNames.Add(ClientPacketHeader.UpdateNavigatorSettingsMessageEvent, "UpdateNavigatorSettingsEvent");
        _packetNames.Add(ClientPacketHeader.CanCreateRoomMessageEvent, "CanCreateRoomEvent");
        _packetNames.Add(ClientPacketHeader.CreateFlatMessageEvent, "CreateFlatEvent");
        _packetNames.Add(ClientPacketHeader.GetGuestRoomMessageEvent, "GetGuestRoomEvent");
        _packetNames.Add(ClientPacketHeader.EditRoomPromotionMessageEvent, "EditRoomEventEvent");
        _packetNames.Add(ClientPacketHeader.GetEventCategoriesMessageEvent, "GetNavigatorFlatsEvent");
        _packetNames.Add(ClientPacketHeader.InitializeNewNavigatorMessageEvent, "InitializeNewNavigatorEvent");
        _packetNames.Add(ClientPacketHeader.NavigatorSearchMessageEvent, "NewNavigatorSearchEvent");
        _packetNames.Add(ClientPacketHeader.FindRandomFriendingRoomMessageEvent, "FindRandomFriendingRoomEvent");
        _packetNames.Add(ClientPacketHeader.GetQuestListMessageEvent, "GetQuestListEvent");
        _packetNames.Add(ClientPacketHeader.StartQuestMessageEvent, "StartQuestEvent");
        _packetNames.Add(ClientPacketHeader.CancelQuestMessageEvent, "CancelQuestEvent");
        _packetNames.Add(ClientPacketHeader.GetCurrentQuestMessageEvent, "GetCurrentQuestEvent");
        _packetNames.Add(ClientPacketHeader.OnBullyClickMessageEvent, "OnBullyClickEvent");
        _packetNames.Add(ClientPacketHeader.SendBullyReportMessageEvent, "SendBullyReportEvent");
        _packetNames.Add(ClientPacketHeader.SubmitBullyReportMessageEvent, "SubmitBullyReportEvent");
        _packetNames.Add(ClientPacketHeader.LetUserInMessageEvent, "LetUserInEvent");
        _packetNames.Add(ClientPacketHeader.BanUserMessageEvent, "BanUserEvent");
        _packetNames.Add(ClientPacketHeader.KickUserMessageEvent, "KickUserEvent");
        _packetNames.Add(ClientPacketHeader.AssignRightsMessageEvent, "AssignRightsEvent");
        _packetNames.Add(ClientPacketHeader.RemoveRightsMessageEvent, "RemoveRightsEvent");
        _packetNames.Add(ClientPacketHeader.RemoveAllRightsMessageEvent, "RemoveAllRightsEvent");
        _packetNames.Add(ClientPacketHeader.MuteUserMessageEvent, "MuteUserEvent");
        _packetNames.Add(ClientPacketHeader.GiveHandItemMessageEvent, "GiveHandItemEvent");
        _packetNames.Add(ClientPacketHeader.GetWardrobeMessageEvent, "GetWardrobeEvent");
        _packetNames.Add(ClientPacketHeader.SaveWardrobeOutfitMessageEvent, "SaveWardrobeOutfitEvent");
        _packetNames.Add(ClientPacketHeader.ActionMessageEvent, "ActionEvent");
        _packetNames.Add(ClientPacketHeader.ApplySignMessageEvent, "ApplySignEvent");
        _packetNames.Add(ClientPacketHeader.DanceMessageEvent, "DanceEvent");
        _packetNames.Add(ClientPacketHeader.SitMessageEvent, "SitEvent");
        _packetNames.Add(ClientPacketHeader.ChangeMottoMessageEvent, "ChangeMottoEvent");
        _packetNames.Add(ClientPacketHeader.LookToMessageEvent, "LookToEvent");
        _packetNames.Add(ClientPacketHeader.DropHandItemMessageEvent, "DropHandItemEvent");
        _packetNames.Add(ClientPacketHeader.GiveRoomScoreMessageEvent, "GiveRoomScoreEvent");
        _packetNames.Add(ClientPacketHeader.IgnoreUserMessageEvent, "IgnoreUserEvent");
        _packetNames.Add(ClientPacketHeader.UnIgnoreUserMessageEvent, "UnIgnoreUserEvent");
        _packetNames.Add(ClientPacketHeader.OpenFlatConnectionMessageEvent, "OpenFlatConnectionEvent");
        _packetNames.Add(ClientPacketHeader.GoToFlatMessageEvent, "GoToFlatEvent");
        _packetNames.Add(ClientPacketHeader.ChatMessageEvent, "ChatEvent");
        _packetNames.Add(ClientPacketHeader.ShoutMessageEvent, "ShoutEvent");
        _packetNames.Add(ClientPacketHeader.WhisperMessageEvent, "WhisperEvent");
        _packetNames.Add(ClientPacketHeader.StartTypingMessageEvent, "StartTypingEvent");
        _packetNames.Add(ClientPacketHeader.CancelTypingMessageEvent, "CancelTypingEvent");
        _packetNames.Add(ClientPacketHeader.GetRoomEntryDataMessageEvent, "GetRoomEntryDataEvent");
        _packetNames.Add(ClientPacketHeader.GetFurnitureAliasesMessageEvent, "GetFurnitureAliasesEvent");
        _packetNames.Add(ClientPacketHeader.MoveAvatarMessageEvent, "MoveAvatarEvent");
        _packetNames.Add(ClientPacketHeader.MoveObjectMessageEvent, "MoveObjectEvent");
        _packetNames.Add(ClientPacketHeader.PickupObjectMessageEvent, "PickupObjectEvent");
        _packetNames.Add(ClientPacketHeader.MoveWallItemMessageEvent, "MoveWallItemEvent");
        _packetNames.Add(ClientPacketHeader.ApplyDecorationMessageEvent, "ApplyDecorationEvent");
        _packetNames.Add(ClientPacketHeader.PlaceObjectMessageEvent, "PlaceObjectEvent");
        _packetNames.Add(ClientPacketHeader.UseFurnitureMessageEvent, "UseFurnitureEvent");
        _packetNames.Add(ClientPacketHeader.UseWallItemMessageEvent, "UseWallItemEvent");
        _packetNames.Add(ClientPacketHeader.InitTradeMessageEvent, "InitTradeEvent");
        _packetNames.Add(ClientPacketHeader.TradingOfferItemMessageEvent, "TradingOfferItemEvent");
        _packetNames.Add(ClientPacketHeader.TradingRemoveItemMessageEvent, "TradingRemoveItemEvent");
        _packetNames.Add(ClientPacketHeader.TradingAcceptMessageEvent, "TradingAcceptEvent");
        _packetNames.Add(ClientPacketHeader.TradingCancelMessageEvent, "TradingCancelEvent");
        _packetNames.Add(ClientPacketHeader.TradingConfirmMessageEvent, "TradingConfirmEvent");
        _packetNames.Add(ClientPacketHeader.TradingModifyMessageEvent, "TradingModifyEvent");
        _packetNames.Add(ClientPacketHeader.TradingCancelConfirmMessageEvent, "TradingCancelConfirmEvent");
        _packetNames.Add(ClientPacketHeader.RequestFurniInventoryMessageEvent, "RequestFurniInventoryEvent");
        _packetNames.Add(ClientPacketHeader.GetBadgesMessageEvent, "GetBadgesEvent");
        _packetNames.Add(ClientPacketHeader.GetAchievementsMessageEvent, "GetAchievementsEvent");
        _packetNames.Add(ClientPacketHeader.SetActivatedBadgesMessageEvent, "SetActivatedBadgesEvent");
        _packetNames.Add(ClientPacketHeader.GetBotInventoryMessageEvent, "GetBotInventoryEvent");
        _packetNames.Add(ClientPacketHeader.GetPetInventoryMessageEvent, "GetPetInventoryEvent");
        _packetNames.Add(ClientPacketHeader.AvatarEffectActivatedMessageEvent, "AvatarEffectActivatedEvent");
        _packetNames.Add(ClientPacketHeader.AvatarEffectSelectedMessageEvent, "AvatarEffectSelectedEvent");
        _packetNames.Add(ClientPacketHeader.GetTalentTrackMessageEvent, "GetTalentTrackEvent");
        _packetNames.Add(ClientPacketHeader.GetCreditsInfoMessageEvent, "GetCreditsInfoEvent");
        _packetNames.Add(ClientPacketHeader.GetHabboClubWindowMessageEvent, "GetHabboClubWindowEvent");
        _packetNames.Add(ClientPacketHeader.ScrGetUserInfoMessageEvent, "ScrGetUserInfoEvent");
        _packetNames.Add(ClientPacketHeader.SetChatPreferenceMessageEvent, "SetChatPreferenceEvent");
        _packetNames.Add(ClientPacketHeader.SetUserFocusPreferenceEvent, "SetUserFocusPreferenceEvent");
        _packetNames.Add(ClientPacketHeader.SetMessengerInviteStatusMessageEvent, "SetMessengerInviteStatusEvent");
        _packetNames.Add(ClientPacketHeader.RespectUserMessageEvent, "RespectUserEvent");
        _packetNames.Add(ClientPacketHeader.UpdateFigureDataMessageEvent, "UpdateFigureDataEvent");
        _packetNames.Add(ClientPacketHeader.OpenPlayerProfileMessageEvent, "OpenPlayerProfileEvent");
        _packetNames.Add(ClientPacketHeader.GetSelectedBadgesMessageEvent, "GetSelectedBadgesEvent");
        _packetNames.Add(ClientPacketHeader.GetRelationshipsMessageEvent, "GetRelationshipsEvent");
        _packetNames.Add(ClientPacketHeader.SetRelationshipMessageEvent, "SetRelationshipEvent");
        _packetNames.Add(ClientPacketHeader.CheckValidNameMessageEvent, "CheckValidNameEvent");
        _packetNames.Add(ClientPacketHeader.ChangeNameMessageEvent, "ChangeNameEvent");
        _packetNames.Add(ClientPacketHeader.GetHabboGroupBadgesMessageEvent, "GetHabboGroupBadgesEvent");
        _packetNames.Add(ClientPacketHeader.GetUserTagsMessageEvent, "GetUserTagsEvent");
        _packetNames.Add(ClientPacketHeader.SetSoundSettingsMessageEvent, "SetSoundSettingsEvent");
        _packetNames.Add(ClientPacketHeader.GetSongInfoMessageEvent, "GetSongInfoEvent");
        _packetNames.Add(ClientPacketHeader.EventTrackerMessageEvent, "EventTrackerEvent");
        _packetNames.Add(ClientPacketHeader.ClientVariablesMessageEvent, "ClientVariablesEvent");
        _packetNames.Add(ClientPacketHeader.DisconnectionMessageEvent, "DisconnectEvent");
        _packetNames.Add(ClientPacketHeader.LatencyTestMessageEvent, "LatencyTestEvent");
        _packetNames.Add(ClientPacketHeader.MemoryPerformanceMessageEvent, "MemoryPerformanceEvent");
        _packetNames.Add(ClientPacketHeader.SetFriendBarStateMessageEvent, "SetFriendBarStateEvent");
        _packetNames.Add(ClientPacketHeader.MessengerInitMessageEvent, "MessengerInitEvent");
        _packetNames.Add(ClientPacketHeader.GetBuddyRequestsMessageEvent, "GetBuddyRequestsEvent");
        _packetNames.Add(ClientPacketHeader.FollowFriendMessageEvent, "FollowFriendEvent");
        _packetNames.Add(ClientPacketHeader.FindNewFriendsMessageEvent, "FindNewFriendsEvent");
        _packetNames.Add(ClientPacketHeader.FriendListUpdateMessageEvent, "FriendListUpdateEvent");
        _packetNames.Add(ClientPacketHeader.RemoveBuddyMessageEvent, "RemoveBuddyEvent");
        _packetNames.Add(ClientPacketHeader.RequestBuddyMessageEvent, "RequestBuddyEvent");
        _packetNames.Add(ClientPacketHeader.SendMsgMessageEvent, "SendMsgEvent");
        _packetNames.Add(ClientPacketHeader.SendRoomInviteMessageEvent, "SendRoomInviteEvent");
        _packetNames.Add(ClientPacketHeader.HabboSearchMessageEvent, "HabboSearchEvent");
        _packetNames.Add(ClientPacketHeader.AcceptBuddyMessageEvent, "AcceptBuddyEvent");
        _packetNames.Add(ClientPacketHeader.DeclineBuddyMessageEvent, "DeclineBuddyEvent");
        _packetNames.Add(ClientPacketHeader.JoinGroupMessageEvent, "JoinGroupEvent");
        _packetNames.Add(ClientPacketHeader.RemoveGroupFavouriteMessageEvent, "RemoveGroupFavouriteEvent");
        _packetNames.Add(ClientPacketHeader.SetGroupFavouriteMessageEvent, "SetGroupFavouriteEvent");
        _packetNames.Add(ClientPacketHeader.GetGroupInfoMessageEvent, "GetGroupInfoEvent");
        _packetNames.Add(ClientPacketHeader.GetGroupMembersMessageEvent, "GetGroupMembersEvent");
        _packetNames.Add(ClientPacketHeader.GetGroupCreationWindowMessageEvent, "GetGroupCreationWindowEvent");
        _packetNames.Add(ClientPacketHeader.GetBadgeEditorPartsMessageEvent, "GetBadgeEditorPartsEvent");
        _packetNames.Add(ClientPacketHeader.PurchaseGroupMessageEvent, "PurchaseGroupEvent");
        _packetNames.Add(ClientPacketHeader.UpdateGroupIdentityMessageEvent, "UpdateGroupIdentityEvent");
        _packetNames.Add(ClientPacketHeader.UpdateGroupBadgeMessageEvent, "UpdateGroupBadgeEvent");
        _packetNames.Add(ClientPacketHeader.UpdateGroupColoursMessageEvent, "UpdateGroupColoursEvent");
        _packetNames.Add(ClientPacketHeader.UpdateGroupSettingsMessageEvent, "UpdateGroupSettingsEvent");
        _packetNames.Add(ClientPacketHeader.ManageGroupMessageEvent, "ManageGroupEvent");
        _packetNames.Add(ClientPacketHeader.GiveAdminRightsMessageEvent, "GiveAdminRightsEvent");
        _packetNames.Add(ClientPacketHeader.TakeAdminRightsMessageEvent, "TakeAdminRightsEvent");
        _packetNames.Add(ClientPacketHeader.RemoveGroupMemberMessageEvent, "RemoveGroupMemberEvent");
        _packetNames.Add(ClientPacketHeader.AcceptGroupMembershipMessageEvent, "AcceptGroupMembershipEvent");
        _packetNames.Add(ClientPacketHeader.DeclineGroupMembershipMessageEvent, "DeclineGroupMembershipEvent");
        _packetNames.Add(ClientPacketHeader.DeleteGroupMessageEvent, "DeleteGroupEvent");
        _packetNames.Add(ClientPacketHeader.GetForumsListDataMessageEvent, "GetForumsListDataEvent");
        _packetNames.Add(ClientPacketHeader.GetForumStatsMessageEvent, "GetForumStatsEvent");
        _packetNames.Add(ClientPacketHeader.GetThreadsListDataMessageEvent, "GetThreadsListDataEvent");
        _packetNames.Add(ClientPacketHeader.GetThreadDataMessageEvent, "GetThreadDataEvent");
        _packetNames.Add(ClientPacketHeader.GetRoomSettingsMessageEvent, "GetRoomSettingsEvent");
        _packetNames.Add(ClientPacketHeader.SaveRoomSettingsMessageEvent, "SaveRoomSettingsEvent");
        _packetNames.Add(ClientPacketHeader.DeleteRoomMessageEvent, "DeleteRoomEvent");
        _packetNames.Add(ClientPacketHeader.ToggleMuteToolMessageEvent, "ToggleMuteToolEvent");
        _packetNames.Add(ClientPacketHeader.GetRoomFilterListMessageEvent, "GetRoomFilterListEvent");
        _packetNames.Add(ClientPacketHeader.ModifyRoomFilterListMessageEvent, "ModifyRoomFilterListEvent");
        _packetNames.Add(ClientPacketHeader.GetRoomRightsMessageEvent, "GetRoomRightsEvent");
        _packetNames.Add(ClientPacketHeader.GetRoomBannedUsersMessageEvent, "GetRoomBannedUsersEvent");
        _packetNames.Add(ClientPacketHeader.UnbanUserFromRoomMessageEvent, "UnbanUserFromRoomEvent");
        _packetNames.Add(ClientPacketHeader.SaveEnforcedCategorySettingsMessageEvent, "SaveEnforcedCategorySettingsEvent");
        _packetNames.Add(ClientPacketHeader.RespectPetMessageEvent, "RespectPetEvent");
        _packetNames.Add(ClientPacketHeader.GetPetInformationMessageEvent, "GetPetInformationEvent");
        _packetNames.Add(ClientPacketHeader.PickUpPetMessageEvent, "PickUpPetEvent");
        _packetNames.Add(ClientPacketHeader.PlacePetMessageEvent, "PlacePetEvent");
        _packetNames.Add(ClientPacketHeader.RideHorseMessageEvent, "RideHorseEvent");
        _packetNames.Add(ClientPacketHeader.ApplyHorseEffectMessageEvent, "ApplyHorseEffectEvent");
        _packetNames.Add(ClientPacketHeader.RemoveSaddleFromHorseMessageEvent, "RemoveSaddleFromHorseEvent");
        _packetNames.Add(ClientPacketHeader.ModifyWhoCanRideHorseMessageEvent, "ModifyWhoCanRideHorseEvent");
        _packetNames.Add(ClientPacketHeader.GetPetTrainingPanelMessageEvent, "GetPetTrainingPanelEvent");
        _packetNames.Add(ClientPacketHeader.PlaceBotMessageEvent, "PlaceBotEvent");
        _packetNames.Add(ClientPacketHeader.PickUpBotMessageEvent, "PickUpBotEvent");
        _packetNames.Add(ClientPacketHeader.OpenBotActionMessageEvent, "OpenBotActionEvent");
        _packetNames.Add(ClientPacketHeader.SaveBotActionMessageEvent, "SaveBotActionEvent");
        _packetNames.Add(ClientPacketHeader.UpdateMagicTileMessageEvent, "UpdateMagicTileEvent");
        _packetNames.Add(ClientPacketHeader.GetYouTubeTelevisionMessageEvent, "GetYouTubeTelevisionEvent");
        _packetNames.Add(ClientPacketHeader.GetRentableSpaceMessageEvent, "GetRentableSpaceEvent");
        _packetNames.Add(ClientPacketHeader.ToggleYouTubeVideoMessageEvent, "ToggleYouTubeVideoEvent");
        _packetNames.Add(ClientPacketHeader.YouTubeVideoInformationMessageEvent, "YouTubeVideoInformationEvent");
        _packetNames.Add(ClientPacketHeader.YouTubeGetNextVideo, "YouTubeGetNextVideo");
        _packetNames.Add(ClientPacketHeader.SaveWiredTriggeRconfigMessageEvent, "SaveWiredConfigEvent");
        _packetNames.Add(ClientPacketHeader.SaveWiredEffectConfigMessageEvent, "SaveWiredConfigEvent");
        _packetNames.Add(ClientPacketHeader.SaveWiredConditionConfigMessageEvent, "SaveWiredConfigEvent");
        _packetNames.Add(ClientPacketHeader.SaveBrandingItemMessageEvent, "SaveBrandingItemEvent");
        _packetNames.Add(ClientPacketHeader.SetTonerMessageEvent, "SetTonerEvent");
        _packetNames.Add(ClientPacketHeader.DiceOffMessageEvent, "DiceOffEvent");
        _packetNames.Add(ClientPacketHeader.ThrowDiceMessageEvent, "ThrowDiceEvent");
        _packetNames.Add(ClientPacketHeader.SetMannequinNameMessageEvent, "SetMannequinNameEvent");
        _packetNames.Add(ClientPacketHeader.SetMannequinFigureMessageEvent, "SetMannequinFigureEvent");
        _packetNames.Add(ClientPacketHeader.CreditFurniRedeemMessageEvent, "CreditFurniRedeemEvent");
        _packetNames.Add(ClientPacketHeader.GetStickyNoteMessageEvent, "GetStickyNoteEvent");
        _packetNames.Add(ClientPacketHeader.AddStickyNoteMessageEvent, "AddStickyNoteEvent");
        _packetNames.Add(ClientPacketHeader.UpdateStickyNoteMessageEvent, "UpdateStickyNoteEvent");
        _packetNames.Add(ClientPacketHeader.DeleteStickyNoteMessageEvent, "DeleteStickyNoteEvent");
        _packetNames.Add(ClientPacketHeader.GetMoodlightConfigMessageEvent, "GetMoodlightConfigEvent");
        _packetNames.Add(ClientPacketHeader.MoodlightUpdateMessageEvent, "MoodlightUpdateEvent");
        _packetNames.Add(ClientPacketHeader.ToggleMoodlightMessageEvent, "ToggleMoodlightEvent");
        _packetNames.Add(ClientPacketHeader.UseOneWayGateMessageEvent, "UseFurnitureEvent");
        _packetNames.Add(ClientPacketHeader.UseHabboWheelMessageEvent, "UseFurnitureEvent");
        _packetNames.Add(ClientPacketHeader.OpenGiftMessageEvent, "OpenGiftEvent");
        _packetNames.Add(ClientPacketHeader.GetGroupFurniSettingsMessageEvent, "GetGroupFurniSettingsEvent");
        _packetNames.Add(ClientPacketHeader.UseSellableClothingMessageEvent, "UseSellableClothingEvent");
        _packetNames.Add(ClientPacketHeader.ConfirmLoveLockMessageEvent, "ConfirmLoveLockEvent");
        _packetNames.Add(ClientPacketHeader.SaveFloorPlanModelMessageEvent, "SaveFloorPlanModelEvent");
        _packetNames.Add(ClientPacketHeader.InitializeFloorPlanSessionMessageEvent, "InitializeFloorPlanSessionEvent");
        _packetNames.Add(ClientPacketHeader.FloorPlanEditorRoomPropertiesMessageEvent, "FloorPlanEditorRoomPropertiesEvent");
        _packetNames.Add(ClientPacketHeader.OpenHelpToolMessageEvent, "OpenHelpToolEvent");
        _packetNames.Add(ClientPacketHeader.GetModeratorRoomInfoMessageEvent, "GetModeratorRoomInfoEvent");
        _packetNames.Add(ClientPacketHeader.GetModeratorUserInfoMessageEvent, "GetModeratorUserInfoEvent");
        _packetNames.Add(ClientPacketHeader.GetModeratorUserRoomVisitsMessageEvent, "GetModeratorUserRoomVisitsEvent");
        _packetNames.Add(ClientPacketHeader.ModerateRoomMessageEvent, "ModerateRoomEvent");
        _packetNames.Add(ClientPacketHeader.ModeratorActionMessageEvent, "ModeratorActionEvent");
        _packetNames.Add(ClientPacketHeader.SubmitNewTicketMessageEvent, "SubmitNewTicketEvent");
        _packetNames.Add(ClientPacketHeader.GetModeratorRoomChatlogMessageEvent, "GetModeratorRoomChatlogEvent");
        _packetNames.Add(ClientPacketHeader.GetModeratorUserChatlogMessageEvent, "GetModeratorUserChatlogEvent");
        _packetNames.Add(ClientPacketHeader.GetModeratorTicketChatlogsMessageEvent, "GetModeratorTicketChatlogsEvent");
        _packetNames.Add(ClientPacketHeader.PickTicketMessageEvent, "PickTicketEvent");
        _packetNames.Add(ClientPacketHeader.ReleaseTicketMessageEvent, "ReleaseTicketEvent");
        _packetNames.Add(ClientPacketHeader.CloseTicketMesageEvent, "CloseTicketEvent");
        _packetNames.Add(ClientPacketHeader.ModerationMuteMessageEvent, "ModerationMuteEvent");
        _packetNames.Add(ClientPacketHeader.ModerationKickMessageEvent, "ModerationKickEvent");
        _packetNames.Add(ClientPacketHeader.ModerationBanMessageEvent, "ModerationBanEvent");
        _packetNames.Add(ClientPacketHeader.ModerationMsgMessageEvent, "ModerationMsgEvent");
        _packetNames.Add(ClientPacketHeader.ModerationCautionMessageEvent, "ModerationCautionEvent");
        _packetNames.Add(ClientPacketHeader.ModerationTradeLockMessageEvent, "ModerationTradeLockEvent");
        _packetNames.Add(ClientPacketHeader.GetGameListingMessageEvent, "GetGameListingEvent");
        _packetNames.Add(ClientPacketHeader.InitializeGameCenterMessageEvent, "InitializeGameCenterEvent");
        _packetNames.Add(ClientPacketHeader.GetPlayableGamesMessageEvent, "GetPlayableGamesEvent");
        _packetNames.Add(ClientPacketHeader.JoinPlayerQueueMessageEvent, "JoinPlayerQueueEvent");
        _packetNames.Add(ClientPacketHeader.Game2GetWeeklyLeaderboardMessageEvent, "Game2GetWeeklyLeaderboardEvent");
    }
}