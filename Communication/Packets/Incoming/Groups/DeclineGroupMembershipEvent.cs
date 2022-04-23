using Plus.Communication.Packets.Outgoing.Groups;
using Plus.HabboHotel.GameClients;
using Plus.HabboHotel.Groups;

namespace Plus.Communication.Packets.Incoming.Groups;

internal class DeclineGroupMembershipEvent : IPacketEvent
{
    private readonly IGroupManager _groupManager;

    public DeclineGroupMembershipEvent(IGroupManager groupManager)
    {
        _groupManager = groupManager;
    }

    public void Parse(GameClient session, ClientPacket packet)
    {
        var groupId = packet.PopInt();
        var userId = packet.PopInt();
        if (!_groupManager.TryGetGroup(groupId, out var group))
            return;
        if (session.GetHabbo().Id != group.CreatorId && !group.IsAdmin(session.GetHabbo().Id))
            return;
        if (!group.HasRequest(userId))
            return;
        group.HandleRequest(userId, false);
        session.SendPacket(new UnknownGroupComposer(group.Id, userId));
    }
}