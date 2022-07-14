using Plus.HabboHotel.GameClients;

namespace Plus.Communication.Packets.Outgoing.Rooms.Furni;

internal class UpdateMagicTileComposer : IServerPacket
{
    private readonly int _itemId;
    private readonly int _height;

    public int MessageId => ServerPacketHeader.UpdateMagicTileMessageComposer;

    public UpdateMagicTileComposer(int itemId, int height)
    {
        _itemId = itemId;
        _height = height;
    }

    public void Compose(IOutgoingPacket packet)
    {
        packet.WriteInteger(Convert.ToInt32(_itemId));
        packet.WriteInteger(_height);
    }
}