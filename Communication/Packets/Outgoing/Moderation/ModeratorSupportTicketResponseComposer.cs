using Plus.HabboHotel.GameClients;

namespace Plus.Communication.Packets.Outgoing.Moderation;

internal class ModeratorSupportTicketResponseComposer : IServerPacket
{
    private readonly int _result;
    public int MessageId => ServerPacketHeader.ModeratorSupportTicketResponseMessageComposer;

    public ModeratorSupportTicketResponseComposer(int result)
    {
        _result = result;
    }

    public void Compose(IOutgoingPacket packet)
    {
        packet.WriteInteger(_result);
        packet.WriteString("");
    }
}