using Plus.HabboHotel.Items;

namespace Plus.HabboHotel.Catalog;

public class CatalogItem
{
    public CatalogItem(int id, uint itemId, ItemDefinition definition, string catalogName, int pageId, int costCredits, int costPixels,
        int costDiamonds, int amount, uint limitedEditionSells, uint limitedEditionStack, bool hasOffer, string extraData, string badge, int offerId)
    {
        Id = id;
        Name = catalogName;
        ItemId = itemId;
        Definition = definition;
        PageId = pageId;
        CostCredits = costCredits;
        CostPixels = costPixels;
        CostDiamonds = costDiamonds;
        Amount = amount;
        LimitedEditionSells = limitedEditionSells;
        LimitedEditionStack = limitedEditionStack;
        IsLimited = limitedEditionStack > 0;
        HaveOffer = hasOffer;
        ExtraData = extraData;
        Badge = badge;
        OfferId = offerId;
    }

    public int Id { get; }
    public uint ItemId { get; }
    public ItemDefinition Definition { get; }
    public int Amount { get; }
    public int CostCredits { get; }
    public string ExtraData { get; }
    public bool HaveOffer { get; }
    public bool IsLimited { get; }
    public string Name { get; }
    public int PageId { get; }
    public int CostPixels { get; }
    public uint LimitedEditionStack { get; }
    public uint LimitedEditionSells { get; set; }
    public int CostDiamonds { get; }
    public string Badge { get; }
    public int OfferId { get; }
}