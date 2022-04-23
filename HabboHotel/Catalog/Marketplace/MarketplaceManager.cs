using System;
using System.Collections.Generic;

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
    int OfferCountForSprite(int spriteId);
    int CalculateComissionPrice(float price);
}

public class MarketplaceManager : IMarketplaceManager
{
    public Dictionary<int, int> MarketAverages { get; } = new();
    public Dictionary<int, int> MarketCounts { get; } = new();
    public List<int> MarketItemKeys { get; } = new();
    public List<MarketOffer> MarketItems { get; } = new();

    public int AvgPriceForSprite(int spriteId)
    {
        var num = 0;
        var num2 = 0;
        if (MarketAverages.ContainsKey(spriteId) && MarketCounts.ContainsKey(spriteId))
        {
            if (MarketCounts[spriteId] > 0) return MarketAverages[spriteId] / MarketCounts[spriteId];
            return 0;
        }
        using (var dbClient = PlusEnvironment.GetDatabaseManager().GetQueryReactor())
        {
            dbClient.SetQuery("SELECT `avgprice` FROM `catalog_marketplace_data` WHERE `sprite` = '" + spriteId + "' LIMIT 1");
            num = dbClient.GetInteger();
            dbClient.SetQuery("SELECT `sold` FROM `catalog_marketplace_data` WHERE `sprite` = '" + spriteId + "' LIMIT 1");
            num2 = dbClient.GetInteger();
        }
        MarketAverages.Add(spriteId, num);
        MarketCounts.Add(spriteId, num2);
        if (num2 > 0)
            return Convert.ToInt32(Math.Ceiling((double)(num / num2)));
        return 0;
    }

    public string FormatTimestampString()
    {
        return FormatTimestamp().ToString().Split(new[] { ',' })[0];
    }

    public double FormatTimestamp() => PlusEnvironment.GetUnixTimestamp() - 172800.0;

    public int OfferCountForSprite(int spriteId)
    {
        var dictionary = new Dictionary<int, MarketOffer>();
        var dictionary2 = new Dictionary<int, int>();
        foreach (var item in MarketItems)
        {
            if (dictionary.ContainsKey(item.SpriteId))
            {
                if (dictionary[item.SpriteId].TotalPrice > item.TotalPrice)
                {
                    dictionary.Remove(item.SpriteId);
                    dictionary.Add(item.SpriteId, item);
                }
                var num = dictionary2[item.SpriteId];
                dictionary2.Remove(item.SpriteId);
                dictionary2.Add(item.SpriteId, num + 1);
            }
            else
            {
                dictionary.Add(item.SpriteId, item);
                dictionary2.Add(item.SpriteId, 1);
            }
        }
        if (dictionary2.ContainsKey(spriteId)) return dictionary2[spriteId];
        return 0;
    }

    public int CalculateComissionPrice(float price) => Convert.ToInt32(Math.Ceiling(price / 100 * 1));
}