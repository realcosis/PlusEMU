using Plus.Communication.Packets.Outgoing.Navigator;
using Plus.HabboHotel.GameClients;
using Plus.HabboHotel.Rooms;

namespace Plus.Communication.Packets.Incoming.Navigator;

internal class GetGuestRoomEvent : IPacketEvent
{
    public void Parse(GameClient session, ClientPacket packet)
    {
        var roomId = packet.PopInt();
        if (!RoomFactory.TryGetData(roomId, out var data))
            return;
        var enter = packet.PopInt() == 1;
        var forward = packet.PopInt() == 1;
        session.SendPacket(new GetGuestRoomResultComposer(session, data, enter, forward));
    }
}