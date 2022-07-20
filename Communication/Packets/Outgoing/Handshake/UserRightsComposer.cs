using Plus.HabboHotel.GameClients;

namespace Plus.Communication.Packets.Outgoing.Handshake;

public class UserRightsComposer : IServerPacket
{
    private readonly int _rank;
    private readonly bool _isAmbassador;
    public uint MessageId => ServerPacketHeader.UserRightsComposer;

    public UserRightsComposer(int rank, bool isAmbassador)
    {
        _rank = rank;
        _isAmbassador = isAmbassador;
    }

    public void Compose(IOutgoingPacket packet)
    {
        packet.WriteInteger(2); //Club level
        packet.WriteInteger(_rank);
        packet.WriteBoolean(_isAmbassador); //Is an ambassador
    }
}