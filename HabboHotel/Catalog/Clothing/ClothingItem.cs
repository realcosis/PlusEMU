namespace Plus.HabboHotel.Catalog.Clothing;

public class ClothingItem
{
    public ClothingItem(int id, string name, string partIds)
    {
        Id = id;
        ClothingName = name;
        PartIds = new();
        if (partIds.Contains(","))
            foreach (var partId in partIds.Split(','))
                PartIds.Add(int.Parse(partId));
        else if (!string.IsNullOrEmpty(partIds) && int.Parse(partIds) > 0) PartIds.Add(int.Parse(partIds));
    }

    public int Id { get; }
    public string ClothingName { get; }
    public List<int> PartIds { get; }
}