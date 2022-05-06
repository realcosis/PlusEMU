using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Plus.Communication.Packets.Outgoing.Users;
using Plus.HabboHotel.GameClients;
using Plus.HabboHotel.Groups;

namespace Plus.Communication.Packets.Incoming.Users;

internal class GetHabboGroupBadgesEvent : IPacketEvent
{
    private readonly IGroupManager _groupManager;

    public GetHabboGroupBadgesEvent(IGroupManager groupManager)
    {
        _groupManager = groupManager;
    }

    public async Task Parse(GameClient session, ClientPacket packet)
    {
        var room = session.GetHabbo().CurrentRoom;
        if (room == null)
            return;
        var badges = await _groupManager.GetHabboGroupBadges(session.GetHabbo());
        room.SendPacket(new HabboGroupBadgesComposer(badges));
        session.SendPacket(new HabboGroupBadgesComposer(badges));
        return;
    }
}