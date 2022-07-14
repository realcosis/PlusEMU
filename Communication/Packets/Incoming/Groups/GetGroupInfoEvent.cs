using Plus.Communication.Packets.Outgoing.Groups;
using Plus.HabboHotel.GameClients;
using Plus.HabboHotel.Groups;

namespace Plus.Communication.Packets.Incoming.Groups;

internal class GetGroupInfoEvent : IPacketEvent
{
    private readonly IGroupManager _groupManager;

    public GetGroupInfoEvent(IGroupManager groupManager)
    {
        _groupManager = groupManager;
    }

    public Task Parse(GameClient session, IIncomingPacket packet)
    {
        var groupId = packet.ReadInt();
        var newWindow = packet.ReadBool();
        if (!_groupManager.TryGetGroup(groupId, out var group))
            return Task.CompletedTask;
        session.Send(new GroupInfoComposer(group, session, newWindow));
        return Task.CompletedTask;
    }
}