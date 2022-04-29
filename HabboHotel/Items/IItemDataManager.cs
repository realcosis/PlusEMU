namespace Plus.HabboHotel.Items;

public interface IItemDataManager
{
    void Init();
    bool GetItem(int id, out ItemData item);
    ItemData? GetItemData(int id);
    ItemData GetItemByName(string name);
    bool GetGift(int spriteId, out ItemData item);
}