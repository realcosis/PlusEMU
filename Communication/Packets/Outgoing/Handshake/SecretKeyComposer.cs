using Plus.HabboHotel.GameClients;

namespace Plus.Communication.Packets.Outgoing.Handshake;

public class SecretKeyComposer : IServerPacket
{
    private readonly string _publicKey;
    public int MessageId => ServerPacketHeader.SecretKeyMessageComposer;

    public SecretKeyComposer(string publicKey)
    {
        _publicKey = publicKey;
    }

    public void Compose(IOutgoingPacket packet) => packet.WriteString(_publicKey);
}