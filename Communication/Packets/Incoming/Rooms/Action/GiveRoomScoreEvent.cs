using Plus.Communication.Packets.Outgoing.Navigator;
using Plus.Database;
using Plus.HabboHotel.GameClients;
using Plus.HabboHotel.Rooms;

namespace Plus.Communication.Packets.Incoming.Rooms.Action;

internal class GiveRoomScoreEvent : IPacketEvent
{
    private readonly IRoomManager _roomManager;
    private readonly IDatabase _database;

    public GiveRoomScoreEvent(IRoomManager roomManager, IDatabase database)
    {
        _roomManager = roomManager;
        _database = database;
    }
    public void Parse(GameClient session, ClientPacket packet)
    {
        if (!session.GetHabbo().InRoom)
            return;
        if (!_roomManager.TryGetRoom(session.GetHabbo().CurrentRoomId, out var room))
            return;
        if (session.GetHabbo().RatedRooms.Contains(room.RoomId) || room.CheckRights(session, true))
            return;
        var rating = packet.PopInt();
        switch (rating)
        {
            case -1:
                room.Score--;
                break;
            case 1:
                room.Score++;
                break;
            default:
                return;
        }
        using (var dbClient = _database.GetQueryReactor())
        {
            dbClient.RunQuery("UPDATE rooms SET score = '" + room.Score + "' WHERE id = '" + room.RoomId + "' LIMIT 1");
        }
        session.GetHabbo().RatedRooms.Add(room.RoomId);
        session.SendPacket(new RoomRatingComposer(room.Score, !(session.GetHabbo().RatedRooms.Contains(room.RoomId) || room.CheckRights(session, true))));
    }
}