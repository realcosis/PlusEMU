using Plus.HabboHotel.GameClients;
using Plus.HabboHotel.Items;

namespace Plus.Communication.Packets.Outgoing.Rooms.Engine;

public class ItemUpdateComposer : IServerPacket
{
    private readonly Item _item;
    private readonly int _userId;
    public uint MessageId => ServerPacketHeader.ItemUpdateComposer;

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
        packet.WriteInteger(item.Definition.GetBaseItem(item).SpriteId);
        packet.WriteString(item.WallCoordinates);
        switch (item.Definition.GetBaseItem(item).InteractionType)
        {
            case InteractionType.Postit:
                packet.WriteString(item.LegacyDataString.Split(' ')[0]);
                break;
            default:
                packet.WriteString(item.LegacyDataString);
                break;
        }
        packet.WriteInteger(-1);
        packet.WriteInteger(item.Definition.GetBaseItem(item).Modes > 1 ? 1 : 0);
        packet.WriteInteger(userId);
    }
}
