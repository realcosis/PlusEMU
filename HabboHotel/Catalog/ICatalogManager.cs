using Plus.HabboHotel.Catalog.Clothing;
using Plus.HabboHotel.Catalog.Marketplace;
using Plus.HabboHotel.Catalog.Pets;
using Plus.HabboHotel.Catalog.Vouchers;
using Plus.HabboHotel.Items;

namespace Plus.HabboHotel.Catalog;

public interface ICatalogManager
{
    Dictionary<int, int> ItemOffers { get; }
    void Init(IItemDataManager itemDataManager);
    bool TryGetBot(uint itemId, out CatalogBot bot);
    bool TryGetPage(int pageId, out CatalogPage page);
    bool TryGetDeal(int dealId, out CatalogDeal deal);
    ICollection<CatalogPage> GetPages();
    ICollection<CatalogPromotion> GetPromotions();
    [Obsolete("Use dependency injection instead.")]
    IMarketplaceManager GetMarketplace();
    [Obsolete("Use dependency injection instead.")]
    IPetRaceManager GetPetRaceManager();
    [Obsolete("Use dependency injection instead.")]
    IVoucherManager GetVoucherManager();
    [Obsolete("Use dependency injection instead.")]
    IClothingManager GetClothingManager();
}