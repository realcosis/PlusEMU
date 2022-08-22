using Plus.Database;
using Plus.HabboHotel.GameClients;
using Dapper;

namespace Plus.Communication.Packets.Incoming.Users;

internal class SetMessengerInviteStatusEvent : IPacketEvent
{
    private readonly IDatabase _database;

    public SetMessengerInviteStatusEvent(IDatabase database)
    {
        _database = database;
    }

    public Task Parse(GameClient session, IIncomingPacket packet)
    {
        var allowMessengerInvites = packet.ReadBool();
        session.GetHabbo().AllowMessengerInvites = allowMessengerInvites;
        using (var connection = _database.Connection())
        {
            connection.Execute("UPDATE users SET ignore_invites = @ignoreInvites WHERE id = @userId LIMIT 1",
            new { ignoreInvites = allowMessengerInvites, userId = session.GetHabbo().Id });
            return Task.CompletedTask;
        }
    }
}