namespace Plus.HabboHotel.Items;

public interface IItemDataManager
{
    void Init();
    bool GetItem(int id, out ItemDefinition item);
    ItemDefinition? GetItemData(int id);
    ItemDefinition GetItemByName(string name);
    bool GetGift(int spriteId, out ItemDefinition item);
}