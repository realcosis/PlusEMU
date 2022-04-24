using Plus.Communication.Packets.Outgoing.Users;
using Plus.Database;
using Plus.HabboHotel.GameClients;
using Plus.HabboHotel.Groups;

namespace Plus.Communication.Packets.Incoming.Users;

internal class OpenPlayerProfileEvent : IPacketEvent
{
    private readonly IGroupManager _groupManager;
    private readonly IDatabase _database;

    public OpenPlayerProfileEvent(IGroupManager groupManager, IDatabase database)
    {
        _groupManager = groupManager;
        _database = database;
    }

    public void Parse(GameClient session, ClientPacket packet)
    {
        var userId = packet.PopInt();
        packet.PopBoolean(); //IsMe?
        var targetData = PlusEnvironment.GetHabboById(userId);
        if (targetData == null)
        {
            session.SendNotification("An error occured whilst finding that user's profile.");
            return;
        }
        var groups = _groupManager.GetGroupsForUser(targetData.Id);
        int friendCount;
        using (var dbClient = _database.GetQueryReactor())
        {
            dbClient.SetQuery("SELECT COUNT(0) FROM `messenger_friendships` WHERE (`user_one_id` = @userid OR `user_two_id` = @userid)");
            dbClient.AddParameter("userid", userId);
            friendCount = dbClient.GetInteger();
        }
        session.SendPacket(new ProfileInformationComposer(targetData, session, groups, friendCount));
    }
}