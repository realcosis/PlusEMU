using Plus.HabboHotel.GameClients;

namespace Plus.Communication.Packets.Outgoing.Marketplace;

internal class MarketplaceItemStatsComposer : IServerPacket
{
    private readonly int _itemId;
    private readonly int _spriteId;
    private readonly int _averagePrice;
    public int MessageId => ServerPacketHeader.MarketplaceItemStatsMessageComposer;

    public MarketplaceItemStatsComposer(int itemId, int spriteId, int averagePrice)
    {
        _itemId = itemId;
        _spriteId = spriteId;
        _averagePrice = averagePrice;
    }

    public void Compose(IOutgoingPacket packet)
    {
        packet.WriteInteger(_averagePrice); //Avg price in last 7 days.
        packet.WriteInteger(PlusEnvironment.GetGame().GetCatalog().GetMarketplace().OfferCountForSprite(_spriteId));
        packet.WriteInteger(0); //No idea.
        packet.WriteInteger(0); //No idea.
        packet.WriteInteger(_itemId);
        packet.WriteInteger(_spriteId);
    }
}