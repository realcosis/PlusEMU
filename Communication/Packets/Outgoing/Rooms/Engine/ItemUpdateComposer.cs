using Plus.HabboHotel.GameClients;
using Plus.HabboHotel.Items;

namespace Plus.Communication.Packets.Outgoing.Rooms.Engine;

internal class ItemUpdateComposer : IServerPacket
{
    private readonly Item _item;
    private readonly int _userId;
    public int MessageId => ServerPacketHeader.ItemUpdateMessageComposer;

    public ItemUpdateComposer(Item item, int userId)
    {
        _item = item;
        _userId = userId;
    }

    public void Compose(IOutgoingPacket packet)
    {
        WriteWallItem(packet, _item, _userId);
    }

    private void WriteWallItem(IOutgoingPacket packet, Item item, int userId)
    {
        packet.WriteString(item.Id.ToString());
        packet.WriteInteger(item.GetBaseItem().SpriteId);
        packet.WriteString(item.WallCoord);
        switch (item.GetBaseItem().InteractionType)
        {
            case InteractionType.Postit:
                packet.WriteString(item.ExtraData.Split(' ')[0]);
                break;
            default:
                packet.WriteString(item.ExtraData);
                break;
        }
        packet.WriteInteger(-1);
        packet.WriteInteger(item.GetBaseItem().Modes > 1 ? 1 : 0);
        packet.WriteInteger(userId);
    }
}