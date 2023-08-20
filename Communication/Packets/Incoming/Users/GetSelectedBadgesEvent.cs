using Plus.Communication.Packets.Outgoing.Users;
using Plus.HabboHotel.Badges;
using Plus.HabboHotel.GameClients;

namespace Plus.Communication.Packets.Incoming.Users;

internal class GetSelectedBadgesEvent : IPacketEvent
{
    private readonly BadgeManager _badgeManager;

    public GetSelectedBadgesEvent(BadgeManager badgeManager) => _badgeManager = badgeManager;

    public async Task Parse(GameClient session, IIncomingPacket packet)
    {
        var userId = packet.ReadInt();
        var equippedBadges = await _badgeManager.GetEquippedBadgesForUserAsync(userId);
        session.Send(new HabboUserBadgesComposer(userId, equippedBadges));
    }
}
