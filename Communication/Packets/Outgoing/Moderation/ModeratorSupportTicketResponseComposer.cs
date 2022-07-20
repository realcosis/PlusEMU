using Plus.HabboHotel.GameClients;

namespace Plus.Communication.Packets.Outgoing.Moderation;

public class ModeratorSupportTicketResponseComposer : IServerPacket
{
    private readonly int _result;
    public uint MessageId => ServerPacketHeader.ModeratorSupportTicketResponseComposer;

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