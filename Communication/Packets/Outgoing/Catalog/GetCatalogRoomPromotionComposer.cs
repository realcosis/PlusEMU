using System.Collections.Generic;

using Plus.HabboHotel.Rooms;

namespace Plus.Communication.Packets.Outgoing.Catalog
{
    class GetCatalogRoomPromotionComposer : ServerPacket
    {
        public GetCatalogRoomPromotionComposer(List<RoomData> usersRooms)
            : base(ServerPacketHeader.PromotableRoomsMessageComposer)
        {
            WriteBoolean(true);//wat
            WriteInteger(usersRooms.Count);//Count of rooms?
            foreach (RoomData room in usersRooms)
            {
                WriteInteger(room.Id);
               WriteString(room.Name);
                WriteBoolean(true);
            }
        }
    }
}
