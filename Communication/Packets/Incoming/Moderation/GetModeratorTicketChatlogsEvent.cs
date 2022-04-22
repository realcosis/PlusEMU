using Plus.HabboHotel.Rooms;
using Plus.Communication.Packets.Outgoing.Moderation;
using Plus.HabboHotel.GameClients;

namespace Plus.Communication.Packets.Incoming.Moderation
{
    class GetModeratorTicketChatlogsEvent : IPacketEvent
    {
        public void Parse(GameClient session, ClientPacket packet)
        {
            if (session == null || session.GetHabbo() == null || !session.GetHabbo().GetPermissions().HasRight("mod_tickets"))
                return;

            var ticketId = packet.PopInt();

            if (!PlusEnvironment.GetGame().GetModerationManager().TryGetTicket(ticketId, out var ticket) || ticket.Room == null)
                return;

            if (!RoomFactory.TryGetData(ticket.Room.Id, out var data))
                return;

            session.SendPacket(new ModeratorTicketChatlogComposer(ticket, data, ticket.Timestamp));
        }
    }
}
