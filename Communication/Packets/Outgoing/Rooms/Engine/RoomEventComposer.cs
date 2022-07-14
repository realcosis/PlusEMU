using Plus.HabboHotel.GameClients;
using Plus.HabboHotel.Rooms;

namespace Plus.Communication.Packets.Outgoing.Rooms.Engine;

internal class RoomEventComposer : IServerPacket
{
    private readonly RoomData _data;
    private readonly RoomPromotion? _promotion;

    public int MessageId => ServerPacketHeader.RoomEventMessageComposer;

    public RoomEventComposer(RoomData data, RoomPromotion? promotion)
    {
        _data = data;
        _promotion = promotion;
    }

    public void Compose(IOutgoingPacket packet)
    {
        packet.WriteInteger(_promotion == null ? -1 : Convert.ToInt32(_data.Id));
        packet.WriteInteger(_promotion == null ? -1 : _data.OwnerId);
        packet.WriteString(_promotion == null ? "" : _data.OwnerName);
        packet.WriteInteger(_promotion == null ? 0 : 1);
        packet.WriteInteger(0);
        packet.WriteString(_promotion == null ? "" : _promotion.Name);
        packet.WriteString(_promotion == null ? "" : _promotion.Description);
        packet.WriteInteger(0);
        packet.WriteInteger(0);
        packet.WriteInteger(0); //Unknown, came in build RELEASE63-201411181343-400753188

    }
}