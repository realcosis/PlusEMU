using Plus.HabboHotel.GameClients;
using Plus.HabboHotel.Rooms;

namespace Plus.Communication.Packets.Outgoing.Catalog;

public class PromotableRoomsComposer : IServerPacket
{
    private readonly ICollection<RoomData> _rooms;
    public uint MessageId => ServerPacketHeader.PromotableRoomsComposer;

    public PromotableRoomsComposer(ICollection<RoomData> rooms)
    {
        _rooms = rooms;
    }

    public void Compose(IOutgoingPacket packet)
    {
        packet.WriteBoolean(true);
        packet.WriteInteger(_rooms.Count);
        foreach (var data in _rooms)
        {
            packet.WriteUInteger(data.Id);
            packet.WriteString(data.Name);
            packet.WriteBoolean(false);
        }
    }
}