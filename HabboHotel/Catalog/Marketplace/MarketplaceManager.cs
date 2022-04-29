using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;
using Plus.Communication.Packets.Outgoing.Inventory.Furni;
using Plus.Database;
using Plus.HabboHotel.Items;
using Plus.HabboHotel.Users;
using Plus.Utilities;

namespace Plus.HabboHotel.Catalog.Marketplace;

public class MarketplaceManager : IMarketplaceManager
{
    private readonly IDatabase _database;
    private readonly IItemDataManager _itemDataManager;
    public Dictionary<int, int> MarketAverages { get; } = new();
    public Dictionary<int, int> MarketCounts { get; } = new();
    public List<int> MarketItemKeys { get; } = new();
    public List<MarketOffer> MarketItems { get; } = new();

    public MarketplaceManager(IDatabase database, IItemDataManager itemDataManager)
    {
        _database = database;
        _itemDataManager = itemDataManager;
    }
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

    public double FormatTimestamp() => UnixTimestamp.GetNow() - 172800.0;

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

    public Task<bool> TryCancelOffer(Habbo habbo, int offerId)
    {
        DataRow row;
        using (var dbClient = _database.GetQueryReactor())
        {
            dbClient.SetQuery(
                "SELECT `furni_id`, `item_id`, `user_id`, `extra_data`, `offer_id`, `state`, `timestamp`, `limited_number`, `limited_stack` FROM `catalog_marketplace_offers` WHERE `offer_id` = @OfferId LIMIT 1");
            dbClient.AddParameter("OfferId", offerId);
            row = dbClient.GetRow();
        }

        if (row == null)
        {
            return Task.FromResult(false);
        }
        if (Convert.ToInt32(row["user_id"]) != habbo.Id)
        {
            return Task.FromResult(false);
        }
        if (!_itemDataManager.GetItem(Convert.ToInt32(row["item_id"]), out var item))
        {
            return Task.FromResult(false);
        }

        //PlusEnvironment.GetGame().GetCatalog().DeliverItems(Session, Item, 1, Convert.ToString(Row["extra_data"]), Convert.ToInt32(Row["limited_number"]), Convert.ToInt32(Row["limited_stack"]), Convert.ToInt32(Row["furni_id"]));
        var giveItem = ItemFactory.CreateSingleItem(item, habbo, Convert.ToString(row["extra_data"]), Convert.ToString(row["extra_data"]), Convert.ToInt32(row["furni_id"]),
            Convert.ToInt32(row["limited_number"]), Convert.ToInt32(row["limited_stack"]));
        habbo.GetClient().SendPacket(new FurniListNotificationComposer(giveItem.Id, 1));
        habbo.GetClient().SendPacket(new FurniListUpdateComposer());
        using (var dbClient = _database.GetQueryReactor())
        {
            dbClient.SetQuery("DELETE FROM `catalog_marketplace_offers` WHERE `offer_id` = @OfferId AND `user_id` = @UserId LIMIT 1");
            dbClient.AddParameter("OfferId", offerId);
            dbClient.AddParameter("UserId", habbo.Id);
            dbClient.RunQuery();
        }
        return Task.FromResult(true);
    }
}