using Plus.HabboHotel.GameClients;
using Plus.HabboHotel.Rooms;

namespace Plus.Communication.Packets.Outgoing.Catalog;

public class GetCatalogRoomPromotionComposer : IServerPacket
{
    private readonly List<RoomData> _usersRooms;

    public GetCatalogRoomPromotionComposer(List<RoomData> usersRooms)
    {
        _usersRooms = usersRooms;
    }

    public uint MessageId => ServerPacketHeader.PromotableRoomsComposer;

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