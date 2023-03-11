using System.Data;
using Plus.HabboHotel.Items;

namespace Plus.HabboHotel.Catalog;

public class CatalogDeal
{
    public CatalogDeal(int id, string items, string displayName, int roomId, IItemDataManager itemDataManager)
    {
        Id = id;
        DisplayName = displayName;
        RoomId = roomId;
        ItemDataList = new();
        if (roomId != 0)
        {
            DataTable data = null;
            using (var dbClient = PlusEnvironment.DatabaseManager.GetQueryReactor())
            {
                dbClient.SetQuery(
                    "SELECT `items`.base_item, COALESCE(`items_groups`.`group_id`, 0) AS `group_id` FROM `items` LEFT OUTER JOIN `items_groups` ON `items`.`id` = `items_groups`.`id` WHERE `items`.`room_id` = @rid;");
                dbClient.AddParameter("rid", roomId);
                data = dbClient.GetTable();
            }
            var roomItems = new Dictionary<int, int>();
            if (data != null)
            {
                foreach (DataRow dRow in data.Rows)
                {
                    var itemId = Convert.ToInt32(dRow["base_item"]);
                    if (roomItems.ContainsKey(itemId))
                        roomItems[itemId]++;
                    else
                        roomItems.Add(itemId, 1);
                }
            }
            foreach (var roomItem in roomItems) items += $"{roomItem.Key}*{roomItem.Value};";
            if (roomItems.Count > 0) items = items.Remove(items.Length - 1);
        }
        var splitItems = items.Split(';');
        foreach (var split in splitItems)
        {
            var item = split.Split('*');
            if (!uint.TryParse(item[0], out var itemId) || !int.TryParse(item[1], out var amount))
                continue;
            if (!itemDataManager.Items.TryGetValue(itemId, out var data))
                continue;
            //itemDataList.Add(new(0, itemId, data, string.Empty, 0, 0, 0, 0, amount, 0, 0, false, "", "", 0));
        }
    }

    public int Id { get; set; }
    public List<CatalogItem> ItemDataList { get; }
    public string DisplayName { get; set; }
    public int RoomId { get; set; }
}