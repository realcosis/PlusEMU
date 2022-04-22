using Plus.Communication.Packets.Outgoing.Rooms.FloorPlan;
using Plus.Communication.Packets.Outgoing.Rooms.Engine;
using Plus.HabboHotel.GameClients;

namespace Plus.Communication.Packets.Incoming.Rooms.FloorPlan
{
    class FloorPlanEditorRoomPropertiesEvent : IPacketEvent
    {
        public void Parse(GameClient session, ClientPacket packet)
        {
            if (!session.GetHabbo().InRoom)
                return;

            var room = session.GetHabbo().CurrentRoom;
            if (room == null)
                return;

            var model = room.GetGameMap().Model;
            if (model == null)
                return;

            var floorItems = room.GetRoomItemHandler().GetFloor;

            session.SendPacket(new FloorPlanFloorMapComposer(floorItems));
            session.SendPacket(new FloorPlanSendDoorComposer(model.DoorX, model.DoorY, model.DoorOrientation));
            session.SendPacket(new RoomVisualizationSettingsComposer(room.WallThickness, room.FloorThickness, PlusEnvironment.EnumToBool(room.Hidewall.ToString())));
        }
    }
}
