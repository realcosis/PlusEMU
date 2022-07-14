using Plus.HabboHotel.GameClients;
using Plus.HabboHotel.Rooms;

namespace Plus.Communication.Packets.Outgoing.Catalog;

internal class GetCatalogRoomPromotionComposer : IServerPacket
{
    private readonly List<RoomData> _usersRooms;

    public GetCatalogRoomPromotionComposer(List<RoomData> usersRooms)
    {
        _usersRooms = usersRooms;
    }

    public int MessageId => ServerPacketHeader.PromotableRoomsMessageComposer;

    public void Compose(IOutgoingPacket packet)
    {

        packet.WriteBoolean(true); //wat
        packet.WriteInteger(_usersRooms.Count); //Count of rooms?
        foreach (var room in _usersRooms)
        {
            packet.WriteInteger(room.Id);
            packet.WriteString(room.Name);
            packet.WriteBoolean(true);
        }
    }
}