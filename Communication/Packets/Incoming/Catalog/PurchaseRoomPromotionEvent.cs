using Plus.Communication.Packets.Outgoing.Catalog;
using Plus.Communication.Packets.Outgoing.Rooms.Engine;
using Plus.Database;
using Plus.HabboHotel.GameClients;
using Plus.HabboHotel.Rooms;
using Plus.HabboHotel.Rooms.Chat.Filter;
using Plus.HabboHotel.Users.Messenger;

namespace Plus.Communication.Packets.Incoming.Catalog;

public class PurchaseRoomPromotionEvent : IPacketEvent
{
    private readonly IWordFilterManager _wordFilterManager;
    private readonly IDatabase _database;

    public PurchaseRoomPromotionEvent(IWordFilterManager wordFilterManager, IDatabase database)
    {
        _wordFilterManager = wordFilterManager;
        _database = database;
    }

    public void Parse(GameClient session, ClientPacket packet)
    {
        packet.PopInt(); //pageId
        packet.PopInt(); //itemId
        var roomId = packet.PopInt();
        var name = _wordFilterManager.CheckMessage(packet.PopString());
        packet.PopBoolean(); //junk
        var desc = _wordFilterManager.CheckMessage(packet.PopString());
        var categoryId = packet.PopInt();
        if (!RoomFactory.TryGetData(roomId, out var data))
            return;
        if (data.OwnerId != session.GetHabbo().Id)
            return;
        if (data.Promotion == null)
            data.Promotion = new RoomPromotion(name, desc, categoryId);
        else
        {
            data.Promotion.Name = name;
            data.Promotion.Description = desc;
            data.Promotion.TimestampExpires += 7200;
        }
        using (var dbClient = _database.GetQueryReactor())
        {
            dbClient.SetQuery(
                "REPLACE INTO `room_promotions` (`room_id`,`title`,`description`,`timestamp_start`,`timestamp_expire`,`category_id`) VALUES (@room_id, @title, @description, @start, @expires, @CategoryId)");
            dbClient.AddParameter("room_id", roomId);
            dbClient.AddParameter("title", name);
            dbClient.AddParameter("description", desc);
            dbClient.AddParameter("start", data.Promotion.TimestampStarted);
            dbClient.AddParameter("expires", data.Promotion.TimestampExpires);
            dbClient.AddParameter("CategoryId", categoryId);
            dbClient.RunQuery();
        }
        if (!session.GetHabbo().GetBadgeComponent().HasBadge("RADZZ"))
            session.GetHabbo().GetBadgeComponent().GiveBadge("RADZZ", true, session);
        session.SendPacket(new PurchaseOkComposer());
        if (session.GetHabbo().InRoom && session.GetHabbo().CurrentRoomId == roomId)
            session.GetHabbo().CurrentRoom?.SendPacket(new RoomEventComposer(data, data.Promotion));
        session.GetHabbo().GetMessenger().BroadcastAchievement(session.GetHabbo().Id, MessengerEventTypes.EventStarted, name);
    }
}