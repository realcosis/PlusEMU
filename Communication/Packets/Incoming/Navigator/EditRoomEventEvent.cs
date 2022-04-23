using System;
using Plus.Communication.Packets.Outgoing.Rooms.Engine;
using Plus.Database;
using Plus.HabboHotel.GameClients;
using Plus.HabboHotel.Rooms;
using Plus.HabboHotel.Rooms.Chat.Filter;

namespace Plus.Communication.Packets.Incoming.Navigator;

internal class EditRoomEventEvent : IPacketEvent
{
    private readonly IWordFilterManager _wordFilterManager;
    private readonly IRoomManager _roomManager;
    private readonly IDatabase _database;

    public EditRoomEventEvent(IWordFilterManager wordFilterManager, IRoomManager roomManager, IDatabase database)
    {
        _wordFilterManager = wordFilterManager;
        _roomManager = roomManager;
        _database = database;
    }

    public void Parse(GameClient session, ClientPacket packet)
    {
        var roomId = packet.PopInt();
        var name = _wordFilterManager.CheckMessage(packet.PopString());
        var desc = _wordFilterManager.CheckMessage(packet.PopString());
        if (!RoomFactory.TryGetData(roomId, out var data))
            return;
        if (data.OwnerId != session.GetHabbo().Id)
            return;
        if (data.Promotion == null)
        {
            session.SendNotification("Oops, it looks like there isn't a room promotion in this room?");
            return;
        }
        using (var dbClient = _database.GetQueryReactor())
        {
            dbClient.SetQuery("UPDATE `room_promotions` SET `title` = @title, `description` = @desc WHERE `room_id` = " + roomId + " LIMIT 1");
            dbClient.AddParameter("title", name);
            dbClient.AddParameter("desc", desc);
            dbClient.RunQuery();
        }
        Room room;
        if (!_roomManager.TryGetRoom(Convert.ToInt32(roomId), out room))
            return;
        data.Promotion.Name = name;
        data.Promotion.Description = desc;
        room.SendPacket(new RoomEventComposer(data, data.Promotion));
    }
}