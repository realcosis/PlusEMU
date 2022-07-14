using Plus.HabboHotel.GameClients;

namespace Plus.Communication.Packets.Outgoing.Rooms.Furni;

internal class GnomeBoxComposer : IServerPacket
{
    private readonly int _itemId;
    public int MessageId => ServerPacketHeader.GnomeBoxMessageComposer;

    public GnomeBoxComposer(int itemId)
    {
        _itemId = itemId;
    }

    public void Compose(IOutgoingPacket packet) => packet.WriteInteger(_itemId);
}