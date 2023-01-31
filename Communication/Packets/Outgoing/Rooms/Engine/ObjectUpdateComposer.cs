using Plus.HabboHotel.GameClients;
using Plus.HabboHotel.Items;

namespace Plus.Communication.Packets.Outgoing.Rooms.Engine;

public class ObjectUpdateComposer : IServerPacket
{
    private readonly Item _item;
    public uint MessageId => ServerPacketHeader.ObjectUpdateComposer;

    public ObjectUpdateComposer(Item item) => _item = item;

    public void Compose(IOutgoingPacket packet) => packet.Serialize(_item);
}
