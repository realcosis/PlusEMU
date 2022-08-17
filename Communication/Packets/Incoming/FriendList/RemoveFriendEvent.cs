using Plus.HabboHotel.GameClients;

namespace Plus.Communication.Packets.Incoming.FriendList;

internal class RemoveFriendEvent : IPacketEvent
{
    public Task Parse(GameClient session, IIncomingPacket packet)
    {
        var amount = packet.ReadInt();
        if (amount > 100)
            amount = 100;
        else if (amount < 0)
            return Task.CompletedTask;
        for (var i = 0; i < amount; i++)
        {
            var id = packet.ReadInt();
            var friend = session.GetHabbo().GetMessenger().GetFriend(id);
            if (friend == null) continue;
            session.GetHabbo().GetMessenger().RemoveFriend(friend);
        }
        return Task.CompletedTask;
    }
}