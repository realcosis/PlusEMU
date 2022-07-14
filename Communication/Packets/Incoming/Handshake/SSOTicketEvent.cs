using Plus.Communication.Attributes;
using Plus.Communication.Packets.Outgoing.BuildersClub;
using Plus.Communication.Packets.Outgoing.Handshake;
using Plus.Communication.Packets.Outgoing.Inventory.Achievements;
using Plus.Communication.Packets.Outgoing.Inventory.AvatarEffects;
using Plus.Communication.Packets.Outgoing.Moderation;
using Plus.Communication.Packets.Outgoing.Navigator;
using Plus.Communication.Packets.Outgoing.Notifications;
using Plus.Communication.Packets.Outgoing.Sound;
using Plus.HabboHotel.Badges;
using Plus.HabboHotel.GameClients;
using Plus.HabboHotel.Users.Authentication;
using Plus.HabboHotel.Users.Messenger.FriendBar;

namespace Plus.Communication.Packets.Incoming.Handshake;

[NoAuthenticationRequired]
public class SsoTicketEvent : IPacketEvent
{
    private readonly IAuthenticator _authenticate;
    private readonly IBadgeManager _badgeManager;

    public SsoTicketEvent(IAuthenticator authenticate, IBadgeManager badgeManager)
    {
        _authenticate = authenticate;
        _badgeManager = badgeManager;
    }

    public async Task Parse(GameClient session, IIncomingPacket packet)
    {
        var sso = packet.ReadString();
        var error = await _authenticate.AuthenticateUsingSSO(session, sso);
        if (error == null)
        {
            session.Send(new AuthenticationOkComposer());

            // TODO @80O: Move to individual incoming message handlers.
            session.Send(new AvatarEffectsComposer(session.GetHabbo().Effects().GetAllEffects));
            session.Send(new NavigatorSettingsComposer(session.GetHabbo().HomeRoom));
            session.Send(new FavouritesComposer(session.GetHabbo().FavoriteRooms));
            session.Send(new FigureSetIdsComposer(session.GetHabbo().GetClothing().GetClothingParts));
            session.Send(new UserRightsComposer(session.GetHabbo().Rank, session.GetHabbo().IsAmbassador));
            session.Send(new AvailabilityStatusComposer());
            session.Send(new AchievementScoreComposer(session.GetHabbo().GetStats().AchievementPoints));
            session.Send(new BuildersClubMembershipComposer());
            session.Send(new CfhTopicsInitComposer(PlusEnvironment.GetGame().GetModerationManager().UserActionPresets));
            session.Send(new BadgeDefinitionsComposer(PlusEnvironment.GetGame().GetAchievementManager().Achievements));
            session.Send(new SoundSettingsComposer(session.GetHabbo().ClientVolume, session.GetHabbo().ChatPreference, session.GetHabbo().AllowMessengerInvites,
                session.GetHabbo().FocusPreference,
                FriendBarStateUtility.GetInt(session.GetHabbo().FriendbarState)));
            //SendMessage(new TalentTrackLevelComposer());


            if (PlusEnvironment.GetGame().GetPermissionManager().TryGetGroup(session.GetHabbo().Rank, out var group))
            {
                if (!string.IsNullOrEmpty(group.Badge))
                {
                    if (!session.GetHabbo().Inventory.Badges.HasBadge(group.Badge))
                        await _badgeManager.GiveBadge(session.GetHabbo(), group.Badge);
                }
            }
            if (PlusEnvironment.GetGame().GetSubscriptionManager().TryGetSubscriptionData(session.GetHabbo().VipRank, out var subData))
            {
                if (!string.IsNullOrEmpty(subData.Badge))
                {
                    if (!session.GetHabbo().Inventory.Badges.HasBadge(subData.Badge))
                        await _badgeManager.GiveBadge(session.GetHabbo(), subData.Badge);
                }
            }
            if (!PlusEnvironment.GetGame().GetCacheManager().ContainsUser(session.GetHabbo().Id))
                PlusEnvironment.GetGame().GetCacheManager().GenerateUser(session.GetHabbo().Id);
            session.GetHabbo().Look = PlusEnvironment.GetFigureManager().ProcessFigure(session.GetHabbo().Look, session.GetHabbo().Gender, session.GetHabbo().GetClothing().GetClothingParts, true);
            session.GetHabbo().InitProcess();
            if (session.GetHabbo().GetPermissions().HasRight("mod_tickets"))
            {
                session.Send(new ModeratorInitComposer(
                    PlusEnvironment.GetGame().GetModerationManager().UserMessagePresets,
                    PlusEnvironment.GetGame().GetModerationManager().RoomMessagePresets,
                    PlusEnvironment.GetGame().GetModerationManager().GetTickets));
            }
            if (PlusEnvironment.GetSettingsManager().TryGetValue("user.login.message.enabled") == "1")
                session.Send(new MotdNotificationComposer(PlusEnvironment.GetLanguageManager().TryGetValue("user.login.message")));
            await PlusEnvironment.GetGame().GetRewardManager().CheckRewards(session);
        }
    }
}