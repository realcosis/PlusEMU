using Plus.Communication.Packets.Outgoing.Rooms.Session;
using Plus.HabboHotel.GameClients;

namespace Plus.Communication.Packets.Incoming.Navigator;

internal class FindRandomFriendingRoomEvent : IPacketEvent
{
    public void Parse(GameClient session, ClientPacket packet)
    {
        var instance = PlusEnvironment.GetGame().GetRoomManager().TryGetRandomLoadedRoom();
        if (instance != null)
            session.SendPacket(new RoomForwardComposer(instance.Id));
    }
}