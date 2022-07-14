using Plus.HabboHotel.GameClients;

namespace Plus.Communication.Packets.Outgoing.Handshake;

internal class GenericErrorComposer : IServerPacket
{
    private readonly int _errorId;
    public int MessageId => ServerPacketHeader.GenericErrorMessageComposer;

    public GenericErrorComposer(int errorId)
    {
        // TODO @80O: Introduce enum with error values
        _errorId = errorId;
    }

    public void Compose(IOutgoingPacket packet)
    {
        packet.WriteInteger(_errorId);
    }
}