using Plus.Communication.Packets.Outgoing.Rooms.Engine;
using Plus.HabboHotel.GameClients;
using Plus.HabboHotel.Rooms;

namespace Plus.Communication.Packets.Incoming.Rooms.Engine;

internal class MoveWallItemEvent : IPacketEvent
{
    private readonly IRoomManager _roomManager;

    public MoveWallItemEvent(IRoomManager roomManager)
    {
        _roomManager = roomManager;
    }

    public void Parse(GameClient session, ClientPacket packet)
    {
        if (!_roomManager.TryGetRoom(session.GetHabbo().CurrentRoomId, out var room))
            return;
        if (!room.CheckRights(session))
            return;
        var itemId = packet.PopInt();
        var wallPositionData = packet.PopString();
        var item = room.GetRoomItemHandler().GetItem(itemId);
        if (item == null)
            return;
        try
        {
            var wallPos = room.GetRoomItemHandler().WallPositionCheck(":" + wallPositionData.Split(':')[1]);
            item.WallCoord = wallPos;
        }
        catch
        {
            return;
        }
        room.GetRoomItemHandler().UpdateItem(item);
        room.SendPacket(new ItemUpdateComposer(item, room.OwnerId));
    }
}