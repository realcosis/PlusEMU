using Plus.HabboHotel.GameClients;

namespace Plus.Communication.Packets.Outgoing.Rooms.Furni;

public class UpdateMagicTileComposer : IServerPacket
{
    private readonly int _itemId;
    private readonly int _height;

    public uint MessageId => ServerPacketHeader.UpdateMagicTileComposer;

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