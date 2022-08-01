using Plus.HabboHotel.GameClients;
using Plus.HabboHotel.Items;
using Plus.HabboHotel.Users.Inventory.Furniture;

namespace Plus.Communication.Packets.Outgoing.Inventory.Furni;

public class FurniListAddComposer : IServerPacket
{
    private readonly InventoryItem _item;
    public uint MessageId => ServerPacketHeader.FurniListAddComposer;

    public FurniListAddComposer(InventoryItem item)
    {
        _item = item;
    }

    public void Compose(IOutgoingPacket packet)
    {
        packet.WriteUInteger(_item.Id);
        packet.WriteString(_item.Definition.Type.ToString().ToUpper());
        packet.WriteUInteger(_item.Id);
        packet.WriteInteger(_item.Definition.SpriteId);
        ItemBehaviourUtility.Serialize(packet, _item.ExtraData, _item.UniqueNumber, _item.UniqueSeries);
        packet.WriteBoolean(_item.Definition.AllowEcotronRecycle);
        packet.WriteBoolean(_item.Definition.AllowTrade);
        packet.WriteBoolean(_item.UniqueNumber == 0 && _item.Definition.AllowInventoryStack);
        packet.WriteBoolean(_item.Definition.AllowMarketplaceSell);
        packet.WriteInteger(-1); //Seconds to expiration.
        packet.WriteBoolean(false); // HasRentPeriodStarted
        packet.WriteInteger(-1); //Item RoomId
        if (!_item.IsWallItem)
        {
            packet.WriteString(string.Empty);
            packet.WriteInteger(0);
        }
    }
}
