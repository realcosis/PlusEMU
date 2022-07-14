using Plus.HabboHotel.Catalog.Utilities;
using Plus.HabboHotel.GameClients;
using Plus.HabboHotel.Items;

namespace Plus.Communication.Packets.Outgoing.Inventory.Furni;

internal class FurniListComposer : IServerPacket
{
    private readonly ICollection<Item> _items;
    private readonly int _pages;
    private readonly int _page;

    public int MessageId => ServerPacketHeader.FurniListMessageComposer;

    public FurniListComposer(ICollection<Item> items, int pages, int page)
    {
        _items = items;
        _pages = pages;
        _page = page;
    }

    public void Compose(IOutgoingPacket packet)
    {
        packet.WriteInteger(_pages); //Pages
        packet.WriteInteger(_page); //Page?
        packet.WriteInteger(_items.Count);
        foreach (var item in _items)
            WriteItem(packet, item);
    }

    private void WriteItem(IOutgoingPacket packet, Item item)
    {
        packet.WriteInteger(item.Id);
        packet.WriteString(item.GetBaseItem().Type.ToString().ToUpper());
        packet.WriteInteger(item.Id);
        packet.WriteInteger(item.GetBaseItem().SpriteId);
        if (item.LimitedNo > 0)
        {
            packet.WriteInteger(1);
            packet.WriteInteger(256);
            packet.WriteString(item.ExtraData);
            packet.WriteInteger(item.LimitedNo);
            packet.WriteInteger(item.LimitedTot);
        }
        else
            ItemBehaviourUtility.GenerateExtradata(item, packet);
        packet.WriteBoolean(item.GetBaseItem().AllowEcotronRecycle);
        packet.WriteBoolean(item.GetBaseItem().AllowTrade);
        packet.WriteBoolean(item.LimitedNo == 0 ? item.GetBaseItem().AllowInventoryStack : false);
        packet.WriteBoolean(ItemUtility.IsRare(item));
        packet.WriteInteger(-1); //Seconds to expiration.
        packet.WriteBoolean(true);
        packet.WriteInteger(-1); //Item RoomId
        if (!item.IsWallItem)
        {
            packet.WriteString(string.Empty);
            packet.WriteInteger(0);
        }
    }
}