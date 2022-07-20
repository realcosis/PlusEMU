using Plus.HabboHotel.GameClients;
using Plus.HabboHotel.Users.Badges;

namespace Plus.Communication.Packets.Outgoing.Inventory.Badges;

public class BadgesComposer : IServerPacket
{
    private readonly GameClient _session;
    public uint MessageId => ServerPacketHeader.BadgesComposer;

    public BadgesComposer(GameClient session)
    {
        _session = session;
        // TODO @80O: Pass badges instead of whole session object.
    }

    public void Compose(IOutgoingPacket packet)
    {
        var equippedBadges = new List<Badge>();
        var badges = _session.GetHabbo().Inventory.Badges.Badges;
        packet.WriteInteger(badges.Count);
        foreach (var (_, badge) in badges)
        {
            packet.WriteInteger(1);
            packet.WriteString(badge.Code);
            if (badge.Slot > 0)
                equippedBadges.Add(badge);
        }
        packet.WriteInteger(equippedBadges.Count);
        foreach (var badge in equippedBadges)
        {
            packet.WriteInteger(badge.Slot);
            packet.WriteString(badge.Code);
        }
    }
}