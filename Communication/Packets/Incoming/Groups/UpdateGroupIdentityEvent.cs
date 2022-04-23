using Plus.Communication.Packets.Outgoing.Groups;
using Plus.Database;
using Plus.HabboHotel.GameClients;
using Plus.HabboHotel.Groups;
using Plus.HabboHotel.Rooms.Chat.Filter;

namespace Plus.Communication.Packets.Incoming.Groups;

internal class UpdateGroupIdentityEvent : IPacketEvent
{
    private readonly IGroupManager _groupManager;
    private readonly IWordFilterManager _wordFilterManager;
    private readonly IDatabase _database;

    public UpdateGroupIdentityEvent(IGroupManager groupManager, IWordFilterManager wordFilterManager, IDatabase database)
    {
        _groupManager = groupManager;
        _wordFilterManager = wordFilterManager;
        _database = database;
    }

    public void Parse(GameClient session, ClientPacket packet)
    {
        var groupId = packet.PopInt();
        var name = _wordFilterManager.CheckMessage(packet.PopString());
        var description = _wordFilterManager.CheckMessage(packet.PopString());
        if (!_groupManager.TryGetGroup(groupId, out var group))
            return;
        if (group.CreatorId != session.GetHabbo().Id)
            return;
        using (var dbClient = _database.GetQueryReactor())
        {
            dbClient.SetQuery("UPDATE `groups` SET `name`= @name, `desc` = @desc WHERE `id` = @groupId LIMIT 1");
            dbClient.AddParameter("name", name);
            dbClient.AddParameter("desc", description);
            dbClient.AddParameter("groupId", groupId);
            dbClient.RunQuery();
        }
        group.Name = name;
        group.Description = description;
        session.SendPacket(new GroupInfoComposer(group, session));
    }
}