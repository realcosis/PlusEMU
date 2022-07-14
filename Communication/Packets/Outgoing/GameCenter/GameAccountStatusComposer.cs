using Plus.HabboHotel.GameClients;

namespace Plus.Communication.Packets.Outgoing.GameCenter;

// TODO @80O: Implement
internal class GameAccountStatusComposer : IServerPacket
{
    private readonly int _gameId;
    public int MessageId => ServerPacketHeader.GameAccountStatusMessageComposer;

    public GameAccountStatusComposer(int gameId)
    {
        _gameId = gameId;
    }

    public void Compose(IOutgoingPacket packet)
    {
        packet.WriteInteger(_gameId);
        packet.WriteInteger(-1); // Games Left
        packet.WriteInteger(0); //Was 16?
    }
}