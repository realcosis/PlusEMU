using Plus.Communication.Packets.Outgoing.Rooms.Engine;
using Plus.HabboHotel.GameClients;

namespace Plus.Communication.Packets.Incoming.Rooms.Engine
{
    class MoveWallItemEvent : IPacketEvent
    {
        public void Parse(GameClient session, ClientPacket packet)
        {
            if (!PlusEnvironment.GetGame().GetRoomManager().TryGetRoom(session.GetHabbo().CurrentRoomId, out var room))
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
            catch { return; }

            room.GetRoomItemHandler().UpdateItem(item);
            room.SendPacket(new ItemUpdateComposer(item, room.OwnerId));
        }
    }
}
