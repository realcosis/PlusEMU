using Plus.HabboHotel.GameClients;

namespace Plus.Communication.Packets.Incoming.FriendList;

internal class DeclineFriendEvent : IPacketEvent
{
    public Task Parse(GameClient session, IIncomingPacket packet)
    {
        var declineAll = packet.ReadBool();
        packet.ReadInt(); //amount
        if (!declineAll)
        {
            var requestId = packet.ReadInt();
            session.GetHabbo().GetMessenger().DeclineFriendRequest(requestId);
        }
        else
        {
            foreach (var request in session.GetHabbo().GetMessenger().Requests.Values)
                session.GetHabbo().GetMessenger().DeclineFriendRequest(request.FromId);
        }
        return Task.CompletedTask;
    }
}