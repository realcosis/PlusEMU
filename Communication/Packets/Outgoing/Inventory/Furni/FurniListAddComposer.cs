using Plus.HabboHotel.Catalog.Utilities;
using Plus.HabboHotel.GameClients;
using Plus.HabboHotel.Items;

namespace Plus.Communication.Packets.Outgoing.Inventory.Furni;

public class FurniListAddComposer : IServerPacket
{
    private readonly Item _item;
    public uint MessageId => ServerPacketHeader.FurniListAddComposer;

    public FurniListAddComposer(Item item)
    {
        _item = item;
    }

    public void Compose(IOutgoingPacket packet)
    {
        packet.WriteInteger(_item.Id);
        packet.WriteString(_item.Definition.GetBaseItem(_item).Type.ToString().ToUpper());
        packet.WriteInteger(_item.Id);
        packet.WriteInteger(_item.Definition.GetBaseItem(_item).SpriteId);
        if (_item.UniqueNumber > 0)
        {
            packet.WriteInteger(1);
            packet.WriteInteger(256);
            packet.WriteString(_item.LegacyDataString);
            packet.WriteInteger(_item.UniqueNumber);
            packet.WriteInteger(_item.UniqueSeries);
        }
        else
            ItemBehaviourUtility.GenerateExtradata(_item, packet);
        packet.WriteBoolean(_item.Definition.GetBaseItem(_item).AllowEcotronRecycle);
        packet.WriteBoolean(_item.Definition.GetBaseItem(_item).AllowTrade);
        packet.WriteBoolean(_item.UniqueNumber == 0 && _item.Definition.GetBaseItem(_item).AllowInventoryStack);
        packet.WriteBoolean(ItemUtility.IsRare(_item));
        packet.WriteInteger(-1); //Seconds to expiration.
        packet.WriteBoolean(true);
        packet.WriteInteger(-1); //Item RoomId
        if (!_item.IsWallItem)
        {
            packet.WriteString(string.Empty);
            packet.WriteInteger(0);
        }
    }
}
