using Plus.Communication.Packets.Outgoing.Inventory.Furni;
using Plus.Database;
using Plus.HabboHotel.GameClients;
using Plus.HabboHotel.Items;
using Plus.HabboHotel.Quests;

namespace Plus.Communication.Packets.Incoming.Rooms.Engine;

internal class PickupObjectEvent : IPacketEvent
{
    private readonly IGameClientManager _clientManager;
    private readonly IQuestManager _questManager;
    private readonly IDatabase _database;

    public PickupObjectEvent(IGameClientManager clientManager, IQuestManager questManager, IDatabase database)
    {
        _clientManager = clientManager;
        _questManager = questManager;
        _database = database;
    }

    public Task Parse(GameClient session, IIncomingPacket packet)
    {
        if (!session.GetHabbo().InRoom)
            return Task.CompletedTask;
        var room = session.GetHabbo().CurrentRoom;
        if (room == null)
            return Task.CompletedTask;
        packet.ReadInt(); //unknown
        var itemId = packet.ReadInt();
        var item = room.GetRoomItemHandler().GetItem(itemId);
        if (item == null)
            return Task.CompletedTask;
        if (item.Definition.GetBaseItem(item).InteractionType == InteractionType.Postit)
            return Task.CompletedTask;
        var itemRights = false;
        if (item.UserId == session.GetHabbo().Id || room.CheckRights(session, false))
            itemRights = true;
        else if (room.Group != null && room.CheckRights(session, false, true)) //Room has a group, this user has group rights.
            itemRights = true;
        else if (session.GetHabbo().GetPermissions().HasRight("room_item_take"))
            itemRights = true;
        if (itemRights)
        {
            if (item.Definition.GetBaseItem(item).InteractionType == InteractionType.Tent || item.Definition.GetBaseItem(item).InteractionType == InteractionType.TentSmall)
                room.RemoveTent(item.Id);
            if (item.Definition.GetBaseItem(item).InteractionType == InteractionType.Moodlight)
            {
                using var dbClient = _database.GetQueryReactor();
                dbClient.RunQuery("DELETE FROM `room_items_moodlight` WHERE `item_id` = '" + item.Id + "' LIMIT 1");
            }
            else if (item.Definition.GetBaseItem(item).InteractionType == InteractionType.Toner)
            {
                using var dbClient = _database.GetQueryReactor();
                dbClient.RunQuery("DELETE FROM `room_items_toner` WHERE `id` = '" + item.Id + "' LIMIT 1");
            }
            if (item.UserId == session.GetHabbo().Id)
            {
                room.GetRoomItemHandler().RemoveFurniture(session, item.Id);
                session.GetHabbo().Inventory.AddNewItem(item.Id, item.BaseItem, item.LegacyDataString, item.GroupId, true, true, item.UniqueNumber, item.UniqueSeries);
                session.Send(new FurniListUpdateComposer());
            }
            else if (session.GetHabbo().GetPermissions().HasRight("room_item_take")) //Staff are taking this item
            {
                room.GetRoomItemHandler().RemoveFurniture(session, item.Id);
                session.GetHabbo().Inventory.AddNewItem(item.Id, item.BaseItem, item.LegacyDataString, item.GroupId, true, true, item.UniqueNumber, item.UniqueSeries);
                session.Send(new FurniListUpdateComposer());
            }
            else //Item is being ejected.
            {
                var targetClient = _clientManager.GetClientByUserId(item.UserId);
                if (targetClient != null && targetClient.GetHabbo() != null) //Again, do we have an active client?
                {
                    room.GetRoomItemHandler().RemoveFurniture(targetClient, item.Id);
                    targetClient.GetHabbo().Inventory.AddNewItem(item.Id, item.BaseItem, item.LegacyDataString, item.GroupId, true, true, item.UniqueNumber, item.UniqueSeries);
                    targetClient.Send(new FurniListUpdateComposer());
                }
                else //No, query time.
                {
                    room.GetRoomItemHandler().RemoveFurniture(null, item.Id);
                    using var dbClient = _database.GetQueryReactor();
                    dbClient.RunQuery("UPDATE `items` SET `room_id` = '0' WHERE `id` = '" + item.Id + "' LIMIT 1");
                }
            }
            _questManager.ProgressUserQuest(session, QuestType.FurniPick);
        }
        return Task.CompletedTask;
    }
}
