namespace Plus.HabboHotel.Items;

public interface IItemDataManager
{
    void Init();
    ItemDefinition GetItemByName(string name);
}