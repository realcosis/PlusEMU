using Plus.HabboHotel.Users;

namespace Plus.HabboHotel.Catalog.Marketplace;

public interface IMarketplaceManager
{
    Dictionary<int, int> MarketAverages { get; }
    Dictionary<int, int> MarketCounts { get; }
    List<int> MarketItemKeys { get; }
    List<MarketOffer> MarketItems { get; }
    int AvgPriceForSprite(int spriteId);
    string FormatTimestampString();
    double FormatTimestamp();
    int OfferCountForSprite(uint spriteId);
    int CalculateComissionPrice(float price);

    Task<bool> TryCancelOffer(Habbo habbo, uint offerId);
    Task<MarketOffer?> GetOffer(uint offerId);
    Task DeleteOffer(uint offerId);
}