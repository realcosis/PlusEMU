using Plus.HabboHotel.GameClients;

namespace Plus.Communication.Packets.Outgoing.Rooms.Furni;

public class GnomeBoxComposer : IServerPacket
{
    private readonly int _itemId;
    public uint MessageId => ServerPacketHeader.GnomeBoxComposer;

    public GnomeBoxComposer(int itemId)
    {
        _itemId = itemId;
    }

    public void Compose(IOutgoingPacket packet) => packet.WriteInteger(_itemId);
}