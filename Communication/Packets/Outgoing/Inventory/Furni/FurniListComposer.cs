using Plus.HabboHotel.GameClients;
using Plus.HabboHotel.Items;
using Plus.HabboHotel.Users.Inventory.Furniture;

namespace Plus.Communication.Packets.Outgoing.Inventory.Furni;

public class FurniListComposer : IServerPacket
{
    private readonly ICollection<InventoryItem> _items;
    private readonly int _pages;
    private readonly int _page;

    public uint MessageId => ServerPacketHeader.FurniListComposer;

    public FurniListComposer(ICollection<InventoryItem> items, int pages, int page)
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

    private void WriteItem(IOutgoingPacket packet, InventoryItem item)
    {
        packet.WriteUInteger(item.Id);
        packet.WriteString(item.Definition.Type.ToCharCode());
        packet.WriteUInteger(item.Id);
        packet.WriteInteger(item.Definition.SpriteId);
        packet.WriteInteger((int)item.Definition.Category);
        ItemBehaviourUtility.Serialize(packet, item.ExtraData, item.UniqueNumber, item.UniqueSeries);
        packet.WriteBoolean(item.Definition.AllowEcotronRecycle);
        packet.WriteBoolean(item.Definition.AllowTrade);
        packet.WriteBoolean(item.ShouldStackInInventory());
        packet.WriteBoolean(item.Definition.AllowMarketplaceSell);
        packet.WriteInteger(-1); //SecondsToExpiration.
        packet.WriteBoolean(false); // HasRentPeriodStarted
        packet.WriteInteger(-1); //Flatid
        if (!item.IsWallItem)
        {
            packet.WriteString(string.Empty);
            packet.WriteInteger(0);
        }
    }
}