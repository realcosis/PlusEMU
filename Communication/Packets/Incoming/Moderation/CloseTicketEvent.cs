using Plus.Communication.Packets.Outgoing.Moderation;
using Plus.Database;
using Plus.HabboHotel.GameClients;
using Plus.HabboHotel.Moderation;

namespace Plus.Communication.Packets.Incoming.Moderation;

internal class CloseTicketEvent : IPacketEvent
{
    private readonly IModerationManager _moderationManager;
    private readonly IGameClientManager _clientManager;
    private readonly IDatabase _database;

    public CloseTicketEvent(IModerationManager moderationManager, IGameClientManager clientManager, IDatabase database)
    {
        _moderationManager = moderationManager;
        _clientManager = clientManager;
        _database = database;
    }

    public void Parse(GameClient session, ClientPacket packet)
    {
        if (!session.GetHabbo().GetPermissions().HasRight("mod_tool"))
            return;
        var result = packet.PopInt(); // 1 = useless, 2 = abusive, 3 = resolved
        packet.PopInt(); //junk
        var ticketId = packet.PopInt();
        if (!_moderationManager.TryGetTicket(ticketId, out var ticket))
            return;
        if (ticket.Moderator.Id != session.GetHabbo().Id)
            return;
        var client = _clientManager.GetClientByUserId(ticket.Sender.Id);
        if (client != null) client.SendPacket(new ModeratorSupportTicketResponseComposer(result));
        if (result == 2)
        {
            using var dbClient = _database.GetQueryReactor();
            dbClient.RunQuery("UPDATE `user_info` SET `cfhs_abusive` = `cfhs_abusive` + 1 WHERE `user_id` = '" + ticket.Sender.Id + "' LIMIT 1");
        }
        ticket.Answered = true;
        _clientManager.SendPacket(new ModeratorSupportTicketComposer(session.GetHabbo().Id, ticket), "mod_tool");
    }
}