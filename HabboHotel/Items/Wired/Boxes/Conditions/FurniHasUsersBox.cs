using System.Collections.Concurrent;
using Plus.HabboHotel.GameClients;
using Plus.HabboHotel.Rooms;

namespace Plus.HabboHotel.Items.Wired.Boxes.Conditions;

internal class FurniHasUsersBox : IWiredItem
{
    public FurniHasUsersBox(Room instance, Item item)
    {
        Instance = instance;
        Item = item;
        SetItems = new ConcurrentDictionary<int, Item>();
    }

    public Room Instance { get; set; }

    public Item Item { get; set; }

    public WiredBoxType Type => WiredBoxType.ConditionFurniHasUsers;

    public ConcurrentDictionary<int, Item> SetItems { get; set; }

    public string StringData { get; set; }

    public bool BoolData { get; set; }
    public string ItemsData { get; set; }

    public void HandleSave(IIncomingPacket packet)
    {
        var unknown = packet.ReadInt();
        var unknown2 = packet.ReadString();
        if (SetItems.Count > 0)
            SetItems.Clear();
        var furniCount = packet.ReadInt();
        for (var i = 0; i < furniCount; i++)
        {
            var selectedItem = Instance.GetRoomItemHandler().GetItem(packet.ReadInt());
            if (selectedItem != null)
                SetItems.TryAdd(selectedItem.Id, selectedItem);
        }
    }

    public bool Execute(params object[] @params)
    {
        foreach (var item in SetItems.Values.ToList())
        {
            if (item == null || !Instance.GetRoomItemHandler().GetFloor.Contains(item))
                continue;
            var hasUsers = false;
            foreach (var tile in item.GetAffectedTiles.Values)
            {
                if (Instance.GetGameMap().SquareHasUsers(tile.X, tile.Y))
                    hasUsers = true;
            }
            if (Instance.GetGameMap().SquareHasUsers(item.GetX, item.GetY))
                hasUsers = true;
            if (!hasUsers)
                return false;
        }
        return true;
    }
}