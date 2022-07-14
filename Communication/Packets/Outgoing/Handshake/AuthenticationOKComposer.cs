using Plus.HabboHotel.GameClients;

namespace Plus.Communication.Packets.Outgoing.Handshake;

public class AuthenticationOkComposer : IServerPacket
{
    public int MessageId => ServerPacketHeader.AuthenticationOkMessageComposer;

    public void Compose(IOutgoingPacket packet)
    {
        // Empty Body
    }
}