using Plus.HabboHotel.Items.Wired;
using Plus.HabboHotel.Users.Inventory.Furniture;

namespace Plus.HabboHotel.Items;

public class ItemDefinition
{
    public ItemDefinition(int id, int sprite, string name, string publicName, string type, int width, int length, double height, bool stackable, bool walkable, bool isSeat,
        bool allowRecycle, bool allowTrade, bool allowMarketplaceSell, bool allowGift, bool allowInventoryStack, InteractionType interactionType, int behaviourData, int modes,
        string vendingIds, string adjustableHeights, int effectId, bool isRare, bool extraRot)
    {
        Id = id;
        SpriteId = sprite;
        ItemName = name;
        PublicName = publicName;
        Type = string.Equals(type, "s", StringComparison.OrdinalIgnoreCase) ? ItemType.Floor : ItemType.Wall;
        Width = width;
        Length = length;
        Height = height;
        Stackable = stackable;
        Walkable = walkable;
        IsSeat = isSeat;
        AllowEcotronRecycle = allowRecycle;
        AllowTrade = allowTrade;
        AllowMarketplaceSell = allowMarketplaceSell;
        AllowGift = allowGift;
        AllowInventoryStack = allowInventoryStack;
        InteractionType = interactionType;
        BehaviourData = behaviourData;
        Modes = modes;
        VendingIds = (!string.IsNullOrEmpty(vendingIds) && vendingIds != "0") ? vendingIds.Split(",").Select(int.Parse).ToList() : new(0);
        AdjustableHeights = (!string.IsNullOrEmpty(adjustableHeights) && adjustableHeights != "0") ? adjustableHeights.Split(",").Select(double.Parse).ToList() : new(0);
        EffectId = effectId;
        var wiredId = 0;
        if (InteractionType == InteractionType.WiredCondition || InteractionType == InteractionType.WiredTrigger || InteractionType == InteractionType.WiredEffect)
            wiredId = BehaviourData;
        WiredType = WiredBoxTypeUtility.FromWiredId(wiredId);
        IsRare = isRare;
        ExtraRot = extraRot;
    }

    public int Id { get; set; }
    public int SpriteId { get; set; }
    public string ItemName { get; set; }
    public string PublicName { get; set; }
    public ItemType Type { get; set; }
    public int Width { get; set; }
    public int Length { get; set; }
    public double Height { get; set; }
    public bool Stackable { get; set; }
    public bool Walkable { get; set; }
    public bool IsSeat { get; set; }
    public bool AllowEcotronRecycle { get; set; }
    public bool AllowTrade { get; set; }
    public bool AllowMarketplaceSell { get; set; }
    public bool AllowGift { get; set; }
    public bool AllowInventoryStack { get; set; }

    /// TODO @80O: Convert to string so plugins can add new interactions.
    public InteractionType InteractionType { get; set; }
    public int BehaviourData { get; set; }
    public int Modes { get; set; }
    public List<int> VendingIds { get; set; }
    public List<double> AdjustableHeights { get; set; }
    public int EffectId { get; set; }

    /// TODO @80O: Should be removed, use unique interaction name instead.
    public WiredBoxType WiredType { get; set; }

    /// TODO @80O: This is dumb, remove it.
    public bool IsRare { get; set; }

    /// TODO @80O: I think this can be removed. Seems useless and unclear what its supposed to do.
    public bool ExtraRot { get; set; }
}