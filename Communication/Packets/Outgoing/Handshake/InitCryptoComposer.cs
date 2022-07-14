using Plus.HabboHotel.GameClients;

namespace Plus.Communication.Packets.Outgoing.Handshake;

public class InitCryptoComposer : IServerPacket
{
    private readonly string _prime;
    private readonly string _generator;
    public int MessageId => ServerPacketHeader.InitCryptoMessageComposer;

    public InitCryptoComposer(string prime, string generator)
    {
        _prime = prime;
        _generator = generator;
    }

    public void Compose(IOutgoingPacket packet)
    {
        packet.WriteString(_prime);
        packet.WriteString(_generator);
    }
}