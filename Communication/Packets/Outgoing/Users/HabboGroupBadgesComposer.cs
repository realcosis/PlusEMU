using Plus.HabboHotel.GameClients;
using Plus.HabboHotel.Groups;

namespace Plus.Communication.Packets.Outgoing.Users;

internal class HabboGroupBadgesComposer : IServerPacket
{
    private readonly Dictionary<int, string> _badges;

    public int MessageId => ServerPacketHeader.HabboGroupBadgesMessageComposer;

    public HabboGroupBadgesComposer(Dictionary<int, string> badges)
    {
        _badges = badges;
    }

    public HabboGroupBadgesComposer(Group group)
    {
        _badges = new() { { group.Id, group.Badge } };
    }

    public void Compose(IOutgoingPacket packet)
    {
        packet.WriteInteger(_badges.Count);
        foreach (var badge in _badges)
        {
            packet.WriteInteger(badge.Key);
            packet.WriteString(badge.Value);
        }
    }
}