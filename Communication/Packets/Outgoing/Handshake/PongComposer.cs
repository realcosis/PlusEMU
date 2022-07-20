using Plus.HabboHotel.GameClients;

namespace Plus.Communication.Packets.Outgoing.Handshake;

public class PongComposer : IServerPacket
{
    public uint MessageId => ServerPacketHeader.PongComposer;

    public void Compose(IOutgoingPacket packet)
    {
        // Empty Body
    }
}