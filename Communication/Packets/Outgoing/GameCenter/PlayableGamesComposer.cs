using Plus.HabboHotel.GameClients;

namespace Plus.Communication.Packets.Outgoing.GameCenter;

internal class PlayableGamesComposer : IServerPacket
{
    private readonly int _gameId;

    public PlayableGamesComposer(int gameId)
    {
        _gameId = gameId;
    }

    public int MessageId => ServerPacketHeader.PlayableGamesMessageComposer;

    public void Compose(IOutgoingPacket packet)
    {
        packet.WriteInteger(_gameId);
        packet.WriteInteger(0);
    }
}