using Plus.HabboHotel.GameClients;
using Plus.HabboHotel.Users;

namespace Plus.Communication.Packets.Outgoing.Users;

public class HabboUserBadgesComposer : IServerPacket
{
    private readonly Habbo _habbo;
    public uint MessageId => ServerPacketHeader.HabboUserBadgesComposer;

    public HabboUserBadgesComposer(Habbo habbo)
    {
        _habbo = habbo;
    }

    public void Compose(IOutgoingPacket packet)
    {
        packet.WriteInteger(_habbo.Id);
        var badges = _habbo.Inventory.Badges.EquippedBadges;
        packet.WriteInteger(badges.Count());
        foreach (var badge in badges)
        {
            packet.WriteInteger(badge.Slot);
            packet.WriteString(badge.Code);
        }
    }
}