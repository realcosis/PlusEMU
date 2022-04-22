namespace Plus.HabboHotel.Catalog.Marketplace
{
    public class MarketOffer
    {
        public int OfferId { get; private set; }
        public int ItemType { get; private set; }
        public int SpriteId { get; private set; }
        public int TotalPrice { get; private set; }
        public int LimitedNumber { get; private set; }
        public int LimitedStack { get; private set; }

        public MarketOffer(int offerId, int spriteId, int totalPrice, int itemType, int limitedNumber, int limitedStack)
        {
            OfferId = offerId;
            SpriteId = spriteId;
            ItemType = itemType;
            TotalPrice = totalPrice;
            LimitedNumber = limitedNumber;
            LimitedStack = limitedStack;
        }
    }
}
