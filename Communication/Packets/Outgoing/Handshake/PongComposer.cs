using Plus.HabboHotel.GameClients;

namespace Plus.Communication.Packets.Outgoing.Handshake;

public class PongComposer : IServerPacket
{
    public int MessageId => ServerPacketHeader.PongMessageComposer;

    public void Compose(IOutgoingPacket packet)
    {
        // Empty Body
    }
}