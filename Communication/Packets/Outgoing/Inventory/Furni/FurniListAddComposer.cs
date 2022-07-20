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
        packet.WriteString(_item.GetBaseItem().Type.ToString().ToUpper());
        packet.WriteInteger(_item.Id);
        packet.WriteInteger(_item.GetBaseItem().SpriteId);
        if (_item.LimitedNo > 0)
        {
            packet.WriteInteger(1);
            packet.WriteInteger(256);
            packet.WriteString(_item.ExtraData);
            packet.WriteInteger(_item.LimitedNo);
            packet.WriteInteger(_item.LimitedTot);
        }
        else
            ItemBehaviourUtility.GenerateExtradata(_item, packet);
        packet.WriteBoolean(_item.GetBaseItem().AllowEcotronRecycle);
        packet.WriteBoolean(_item.GetBaseItem().AllowTrade);
        packet.WriteBoolean(_item.LimitedNo == 0 && _item.GetBaseItem().AllowInventoryStack);
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