using System;
using System.Collections.Generic;
using System.Data;

namespace Plus.HabboHotel.Catalog.Clothing;

public class ClothingManager : IClothingManager
{
    private readonly Dictionary<int, ClothingItem> _clothing;

    public ClothingManager()
    {
        _clothing = new Dictionary<int, ClothingItem>();
    }

    public ICollection<ClothingItem> GetClothingAllParts => _clothing.Values;

    public void Init()
    {
        if (_clothing.Count > 0)
            _clothing.Clear();
        DataTable data = null;
        using (var dbClient = PlusEnvironment.GetDatabaseManager().GetQueryReactor())
        {
            dbClient.SetQuery("SELECT `id`,`clothing_name`,`clothing_parts` FROM `catalog_clothing`");
            data = dbClient.GetTable();
        }
        if (data != null)
        {
            foreach (DataRow row in data.Rows)
                _clothing.Add(Convert.ToInt32(row["id"]), new ClothingItem(Convert.ToInt32(row["id"]), Convert.ToString(row["clothing_name"]), Convert.ToString(row["clothing_parts"])));
        }
    }

    public bool TryGetClothing(int itemId, out ClothingItem clothing) => _clothing.TryGetValue(itemId, out clothing);
}