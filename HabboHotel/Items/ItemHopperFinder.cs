namespace Plus.HabboHotel.Items;

/// TODO @80O: Make this an injectable service. Pass database via contstructor. Use Dapper
public static class ItemHopperFinder
{
    public static uint GetAHopper(uint curRoom)
    {
        using var dbClient = PlusEnvironment.GetDatabaseManager().GetQueryReactor();
        var roomId = 0;
        dbClient.SetQuery("SELECT room_id FROM items_hopper WHERE room_id <> @room ORDER BY room_id ASC LIMIT 1");
        dbClient.AddParameter("room", curRoom);
        roomId = dbClient.GetInteger();
        return (uint)roomId;
    }

    public static uint GetHopperId(uint nextRoom)
    {
        using var dbClient = PlusEnvironment.GetDatabaseManager().GetQueryReactor();
        dbClient.SetQuery("SELECT hopper_id FROM items_hopper WHERE room_id = @room LIMIT 1");
        dbClient.AddParameter("room", nextRoom);
        var row = dbClient.GetString();
        if (row == null)
            return 0;
        return Convert.ToUInt32(row);
    }
}