using Plus.HabboHotel.GameClients;

namespace Plus.Communication.Packets.Outgoing.GameCenter;

internal class JoinQueueComposer : IServerPacket
{
    private readonly int _gameId;
    public int MessageId => ServerPacketHeader.JoinQueueMessageComposer;

    public JoinQueueComposer(int gameId)
    {
        _gameId = gameId;
    }

    public void Compose(IOutgoingPacket packet) => packet.WriteInteger(_gameId);
}