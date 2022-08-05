namespace Plus.HabboHotel.Catalog.Marketplace;

public class MarketOffer
{
    public MarketOffer(uint offerId, uint spriteId, int totalPrice, int itemType, int limitedNumber, int limitedStack)
    {
        OfferId = offerId;
        SpriteId = spriteId;
        ItemType = itemType;
        TotalPrice = totalPrice;
        LimitedNumber = limitedNumber;
        LimitedStack = limitedStack;
    }

    public MarketOffer()
    {
    }

    public uint OfferId { get; set;  }
    public int ItemType { get; set; }
    public uint SpriteId { get; set; }
    public int TotalPrice { get; set; }
    public int LimitedNumber { get; set; }
    public int LimitedStack { get; set; }
    public uint ItemId { get; set; }
    public int UserId { get; set; }
    public string ExtraData { get; set; } = string.Empty;
    public int FurniId { get; set; }
}