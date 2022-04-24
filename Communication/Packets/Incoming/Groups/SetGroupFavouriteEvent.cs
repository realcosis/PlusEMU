using Plus.Communication.Packets.Outgoing.Groups;
using Plus.Communication.Packets.Outgoing.Users;
using Plus.Database;
using Plus.HabboHotel.GameClients;
using Plus.HabboHotel.Groups;

namespace Plus.Communication.Packets.Incoming.Groups;

internal class SetGroupFavouriteEvent : IPacketEvent
{
    private readonly IGroupManager _groupManager;
    private readonly IDatabase _database;

    public SetGroupFavouriteEvent(IGroupManager groupManager,IDatabase database)
    {
        _groupManager = groupManager;
        _database = database;
    }

    public void Parse(GameClient session, ClientPacket packet)
    {
        var groupId = packet.PopInt();
        if (groupId == 0)
            return;
        if (!_groupManager.TryGetGroup(groupId, out var group))
            return;
        session.GetHabbo().GetStats().FavouriteGroupId = group.Id;
        using (var dbClient = _database.GetQueryReactor())
        {
            dbClient.SetQuery("UPDATE `user_stats` SET `groupid` = @groupId WHERE `id` = @userId LIMIT 1");
            dbClient.AddParameter("groupId", session.GetHabbo().GetStats().FavouriteGroupId);
            dbClient.AddParameter("userId", session.GetHabbo().Id);
            dbClient.RunQuery();
        }
        if (session.GetHabbo().InRoom && session.GetHabbo().CurrentRoom != null)
        {
            session.GetHabbo().CurrentRoom.SendPacket(new RefreshFavouriteGroupComposer(session.GetHabbo().Id));
            session.GetHabbo().CurrentRoom.SendPacket(new HabboGroupBadgesComposer(group));
            var user = session.GetHabbo().CurrentRoom.GetRoomUserManager()
                .GetRoomUserByHabbo(session.GetHabbo().Id);
            if (user != null)
                session.GetHabbo().CurrentRoom.SendPacket(new UpdateFavouriteGroupComposer(group, user.VirtualId));
        }
        else
            session.SendPacket(new RefreshFavouriteGroupComposer(session.GetHabbo().Id));
    }
}