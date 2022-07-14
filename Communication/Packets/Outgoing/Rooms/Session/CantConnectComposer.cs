using Plus.HabboHotel.GameClients;

namespace Plus.Communication.Packets.Outgoing.Rooms.Session;

internal class CantConnectComposer : IServerPacket
{
    private readonly int _error;
    public int MessageId => ServerPacketHeader.CantConnectMessageComposer;


    // TODO @80O: Extract list of all error values and move to enum.
    public CantConnectComposer(int error)
    {
        _error = error;
    }

    public void Compose(IOutgoingPacket packet) => packet.WriteInteger(_error);
}