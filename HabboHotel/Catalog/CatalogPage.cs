namespace Plus.HabboHotel.Catalog;

public class CatalogPage
{
    public int Id { get; set; }

    public int ParentId { get; set; }

    public bool Enabled { get; set; }

    public string? Caption { get; set; }

    public string? PageLink { get; set; }

    public int IconImage { get; set; }

    public int MinRank { get; set; }

    public int MinVip { get; set; }

    public bool Visible { get; set; }

    public string? PageLayout { get; set; }

    public string? PageStrings1 { get; set; }

    public string? PageStrings2 { get; set; }

    public List<string> PageStringsList1 { get; set; } = new();

    public List<string> PageStringsList2 { get; set; } = new();

    public Dictionary<int, CatalogItem> Items { get; set; } = new();

    public Dictionary<int, CatalogItem> ItemOffers { get; set; } = new();

    public CatalogItem? GetItem(int pId)
    {
        if (Items.ContainsKey(pId))
            return Items[pId];
        return null;
    }
}