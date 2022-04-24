using Plus.Communication.Packets.Outgoing.Moderation;
using Plus.HabboHotel.GameClients;
using Plus.HabboHotel.Rooms;

namespace Plus.Communication.Packets.Incoming.Moderation;

internal class GetModeratorRoomInfoEvent : IPacketEvent
{
    private readonly IRoomManager _roomManager;

    public GetModeratorRoomInfoEvent(IRoomManager roomManager)
    {
        _roomManager = roomManager;
    }

    public void Parse(GameClient session, ClientPacket packet)
    {
        if (!session.GetHabbo().GetPermissions().HasRight("mod_tool"))
            return;
        var roomId = packet.PopInt();
        if (!RoomFactory.TryGetData(roomId, out var data))
            return;
        if (!_roomManager.TryGetRoom(roomId, out var room))
            return;
        session.SendPacket(new ModeratorRoomInfoComposer(data, room.GetRoomUserManager().GetRoomUserByHabbo(data.OwnerName) != null));
    }
}