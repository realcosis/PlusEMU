using Plus.Communication.Packets.Outgoing.Groups;
using Plus.HabboHotel.GameClients;

namespace Plus.Communication.Packets.Incoming.Groups;

internal class GetGroupInfoEvent : IPacketEvent
{
    public void Parse(GameClient session, ClientPacket packet)
    {
        var groupId = packet.PopInt();
        var newWindow = packet.PopBoolean();
        if (!PlusEnvironment.GetGame().GetGroupManager().TryGetGroup(groupId, out var group))
            return;
        session.SendPacket(new GroupInfoComposer(group, session, newWindow));
    }
}