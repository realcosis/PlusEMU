using Plus.HabboHotel.GameClients;
using Plus.HabboHotel.Users.Badges;

namespace Plus.Communication.Packets.Outgoing.Users;

public class HabboUserBadgesComposer : IServerPacket
{
    private readonly int _userId;
    private readonly List<Badge> _equippedBadges;
    public uint MessageId => ServerPacketHeader.HabboUserBadgesComposer;

    public HabboUserBadgesComposer(int userId, List<Badge> equippedBadges)
    {
        _userId = userId;
        _equippedBadges = equippedBadges;
    }

    public void Compose(IOutgoingPacket packet)
    {
        var orderedBadges = _equippedBadges.OrderBy(b => b.Slot).ToList();

        packet.WriteInteger(_userId);
        packet.WriteInteger(orderedBadges.Count);

        foreach (var badge in orderedBadges)
        {
            packet.WriteInteger(badge.Slot);
            packet.WriteString(badge.Code);
        }
    }
}
