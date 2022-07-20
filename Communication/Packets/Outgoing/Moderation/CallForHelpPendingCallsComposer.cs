using Plus.HabboHotel.GameClients;
using Plus.HabboHotel.Moderation;
using Plus.Utilities;

namespace Plus.Communication.Packets.Outgoing.Moderation;

public class CallForHelpPendingCallsComposer : IServerPacket
{
    private readonly ModerationTicket _ticket;
    public uint MessageId => ServerPacketHeader.CallForHelpPendingCallsComposer;

    public CallForHelpPendingCallsComposer(ModerationTicket ticket)
    {
        _ticket = ticket;
    }

    public void Compose(IOutgoingPacket packet)
    {
        packet.WriteInteger(1); // Count for whatever reason?
        {
            packet.WriteString(_ticket.Id.ToString());
            packet.WriteString(UnixTimestamp.FromUnixTimestamp(_ticket.Timestamp).ToShortTimeString()); // "11-02-2017 04:07:05";
            packet.WriteString(_ticket.Issue);
        }
    }
}