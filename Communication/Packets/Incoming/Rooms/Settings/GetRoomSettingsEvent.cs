using Plus.HabboHotel.Rooms;
using Plus.Communication.Packets.Outgoing.Rooms.Settings;
using Plus.HabboHotel.GameClients;

namespace Plus.Communication.Packets.Incoming.Rooms.Settings
{
    class GetRoomSettingsEvent : IPacketEvent
    {
        public void Parse(GameClient session, ClientPacket packet)
        {
            var roomId = packet.PopInt();

            if (!PlusEnvironment.GetGame().GetRoomManager().TryLoadRoom(roomId, out var room))
                return;

            if (!room.CheckRights(session, true))
                return;

            session.SendPacket(new RoomSettingsDataComposer(room));
        }
    }
}
