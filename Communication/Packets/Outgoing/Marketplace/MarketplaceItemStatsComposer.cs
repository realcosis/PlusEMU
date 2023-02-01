using Plus.HabboHotel.GameClients;

namespace Plus.Communication.Packets.Outgoing.Marketplace;

public class MarketplaceItemStatsComposer : IServerPacket
{
    private readonly int _itemId;
    private readonly uint _spriteId;
    private readonly int _averagePrice;
    public uint MessageId => ServerPacketHeader.MarketplaceItemStatsComposer;

    public MarketplaceItemStatsComposer(int itemId, uint spriteId, int averagePrice)
    {
        _itemId = itemId;
        _spriteId = spriteId;
        _averagePrice = averagePrice;
    }

    public void Compose(IOutgoingPacket packet)
    {
        packet.WriteInteger(_averagePrice); //Avg price in last 7 days.
        packet.WriteInteger(PlusEnvironment.Game.GetCatalog().GetMarketplace().OfferCountForSprite(_spriteId));
        packet.WriteInteger(0); //No idea.
        packet.WriteInteger(0); //No idea.
        packet.WriteInteger(_itemId);
        packet.WriteUInteger(_spriteId);
    }
}