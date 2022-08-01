using Plus.Communication.Packets.Outgoing.Inventory.Furni;
using Plus.Communication.Packets.Outgoing.Rooms.Notifications;
using Plus.Core.Settings;
using Plus.HabboHotel.Achievements;
using Plus.HabboHotel.GameClients;
using Plus.HabboHotel.Items;
using Plus.HabboHotel.Rooms;

namespace Plus.Communication.Packets.Incoming.Rooms.Engine;

internal class PlaceObjectEvent : IPacketEvent
{
    private readonly IRoomManager _roomManager;
    private readonly ISettingsManager _settingsManager;
    private readonly IAchievementManager _achievementManager;

    public PlaceObjectEvent(IRoomManager roomManager, ISettingsManager settingsManager, IAchievementManager achievementManager)
    {
        _roomManager = roomManager;
        _settingsManager = settingsManager;
        _achievementManager = achievementManager;
    }

    /// TODO @80O: Unfuck this mess
    public Task Parse(GameClient session, IIncomingPacket packet)
    {
        if (!session.GetHabbo().InRoom)
            return Task.CompletedTask;
        if (!_roomManager.TryGetRoom(session.GetHabbo().CurrentRoomId, out var room))
            return Task.CompletedTask;
        var rawData = packet.ReadString();
        var data = rawData.Split(' ');
        if (!uint.TryParse(data[0], out var itemId))
            return Task.CompletedTask;
        var hasRights = room.CheckRights(session, false, true);
        if (!hasRights)
        {
            session.Send(new RoomNotificationComposer("furni_placement_error", "message", "${room.error.cant_set_not_owner}"));
            return Task.CompletedTask;
        }
        if (room.GetRoomItemHandler().GetWallAndFloor.Count() > Convert.ToInt32(_settingsManager.TryGetValue("room.item.placement_limit")))
        {
            session.SendNotification("You cannot have more than " + Convert.ToInt32(_settingsManager.TryGetValue("room.item.placement_limit")) + " items in a room!");
            return Task.CompletedTask;
        }
        var inventoryItem = session.GetHabbo().Inventory.Furniture.GetItem(itemId);
        var item = inventoryItem.ToRoomObject();
        if (item == null)
            return Task.CompletedTask;

        if (item.Definition.InteractionType == InteractionType.Exchange && room.OwnerId != session.GetHabbo().Id && !session.GetHabbo().GetPermissions().HasRight("room_item_place_exchange_anywhere"))
        {
            session.SendNotification("You cannot place exchange items in other people's rooms!");
            return Task.CompletedTask;
        }

        //TODO: Make neat.
        switch (item.Definition.InteractionType)
        {
            case InteractionType.Moodlight:
            {
                var moodData = room.MoodlightData;
                if (moodData != null && room.GetRoomItemHandler().GetItem(moodData.ItemId) != null)
                {
                    session.SendNotification("You can only have one background moodlight per room!");
                    return Task.CompletedTask;
                }
                break;
            }
            case InteractionType.Toner:
            {
                var tonerData = room.TonerData;
                if (tonerData != null && room.GetRoomItemHandler().GetItem(tonerData.ItemId) != null)
                {
                    session.SendNotification("You can only have one background toner per room!");
                    return Task.CompletedTask;
                }
                break;
            }
            case InteractionType.Hopper:
            {
                if (room.GetRoomItemHandler().HopperCount > 0)
                {
                    session.SendNotification("You can only have one hopper per room!");
                    return Task.CompletedTask;
                }
                break;
            }
            case InteractionType.Tent:
            case InteractionType.TentSmall:
            {
                room.AddTent(item.Id);
                break;
            }
        }
        if (!item.IsWallItem)
        {
            if (data.Length < 4)
                return Task.CompletedTask;
            if (!int.TryParse(data[1], out var x)) return Task.CompletedTask;
            if (!int.TryParse(data[2], out var y)) return Task.CompletedTask;
            if (!int.TryParse(data[3], out var rotation)) return Task.CompletedTask;
            if (room.GetRoomItemHandler().SetFloorItem(session, item, x, y, rotation, true, false, true))
            {
                session.GetHabbo().Inventory.Furniture.RemoveItem(itemId);
                session.Send(new FurniListRemoveComposer(itemId));
                if (session.GetHabbo().Id == room.OwnerId)
                    _achievementManager.ProgressAchievement(session, "ACH_RoomDecoFurniCount", 1);
                if (item.IsWired)
                {
                    try
                    {
                        room.GetWired().LoadWiredBox(item);
                    }
                    catch
                    {
                        Console.WriteLine(item.Definition.InteractionType);
                    }
                }
            }
            else
                session.Send(new RoomNotificationComposer("furni_placement_error", "message", "${room.error.cant_set_item}"));
        }
        else if (item.IsWallItem)
        {
            var correctedData = new string[data.Length - 1];
            for (var i = 1; i < data.Length; i++) correctedData[i - 1] = data[i];
            if (TrySetWallItem(correctedData, out var wallPos))
            {
                try
                {
                    if (room.GetRoomItemHandler().SetWallItem(session, item))
                    {
                        session.GetHabbo().Inventory.Furniture.RemoveItem(itemId);
                        session.Send(new FurniListRemoveComposer(itemId));
                        if (session.GetHabbo().Id == room.OwnerId)
                            _achievementManager.ProgressAchievement(session, "ACH_RoomDecoFurniCount", 1);
                    }
                }
                catch
                {
                    session.Send(new RoomNotificationComposer("furni_placement_error", "message", "${room.error.cant_set_item}"));
                }
            }
            else
                session.Send(new RoomNotificationComposer("furni_placement_error", "message", "${room.error.cant_set_item}"));
        }
        return Task.CompletedTask;
    }

    private static bool TrySetWallItem(IReadOnlyList<string> data, out string position)
    {
        if (data.Count != 3 || !data[0].StartsWith(":w=") || !data[1].StartsWith("l=") || data[2] != "r" && data[2] != "l")
        {
            position = null;
            return false;
        }
        var wBit = data[0].Substring(3, data[0].Length - 3);
        var lBit = data[1].Substring(2, data[1].Length - 2);
        if (!wBit.Contains(",") || !lBit.Contains(","))
        {
            position = null;
            return false;
        }
        int.TryParse(wBit.Split(',')[0], out var w1);
        int.TryParse(wBit.Split(',')[1], out var w2);
        int.TryParse(lBit.Split(',')[0], out var l1);
        int.TryParse(lBit.Split(',')[1], out var l2);
        //
        //if (!Habbo.HasFuse("super_admin") && (w1 < 0 || w2 < 0 || l1 < 0 || l2 < 0 || w1 > 200 || w2 > 200 || l1 > 200 || l2 > 200))
        //{
        //    position = null;
        //    return false;
        //}
        var wallPos = ":w=" + w1 + "," + w2 + " l=" + l1 + "," + l2 + " " + data[2];
        position = WallPositionCheck(wallPos);
        return position != null;
    }

    private static string WallPositionCheck(string wallPosition)
    {
        //:w=3,2 l=9,63 l
        try
        {
            if (wallPosition.Contains(Convert.ToChar(13))) return null;
            if (wallPosition.Contains(Convert.ToChar(9))) return null;
            var posD = wallPosition.Split(' ');
            if (posD[2] != "l" && posD[2] != "r")
                return null;
            var widD = posD[0].Substring(3).Split(',');
            var widthX = int.Parse(widD[0]);
            var widthY = int.Parse(widD[1]);
            if (widthX < -1000 || widthY < -1 || widthX > 700 || widthY > 700)
                return null;
            var lenD = posD[1].Substring(2).Split(',');
            var lengthX = int.Parse(lenD[0]);
            var lengthY = int.Parse(lenD[1]);
            if (lengthX < -1 || lengthY < -1000 || lengthX > 700 || lengthY > 700)
                return null;
            return ":w=" + widthX + "," + widthY + " " + "l=" + lengthX + "," + lengthY + " " + posD[2];
        }
        catch
        {
            return null;
        }
    }
}
