using Plus.Database;
using Plus.HabboHotel.GameClients;
using Plus.HabboHotel.Items;
using Plus.HabboHotel.Rooms;

namespace Plus.Communication.Packets.Incoming.Rooms.Furni.Stickys;

internal class DeleteStickyNoteEvent : IPacketEvent
{
    private readonly IRoomManager _roomManager;
    private readonly IDatabase _database;

    public DeleteStickyNoteEvent(IRoomManager roomManager, IDatabase database)
    {
        _roomManager = roomManager;
        _database = database;
    }

    public Task Parse(GameClient session, IIncomingPacket packet)
    {
        if (!session.GetHabbo().InRoom)
            return Task.CompletedTask;
        if (!_roomManager.TryGetRoom(session.GetHabbo().CurrentRoomId, out var room))
            return Task.CompletedTask;
        if (!room.CheckRights(session))
            return Task.CompletedTask;
        var item = room.GetRoomItemHandler().GetItem(packet.ReadInt());
        if (item == null)
            return Task.CompletedTask;
        if (item.Definition.GetBaseItem(item).InteractionType == InteractionType.Postit || item.Definition.GetBaseItem(item).InteractionType == InteractionType.CameraPicture)
        {
            room.GetRoomItemHandler().RemoveFurniture(session, item.Id);
            using var dbClient = _database.GetQueryReactor();
            dbClient.RunQuery("DELETE FROM `items` WHERE `id` = '" + item.Id + "' LIMIT 1");
        }
        return Task.CompletedTask;
    }
}