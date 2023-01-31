using Plus.HabboHotel.GameClients;

namespace Plus.Communication.Packets.Incoming.Game.Directory;

internal class Game2GetAccountGameStatusEven : IPacketEvent
{
    public Task Parse(GameClient session, IIncomingPacket packet) => throw new NotImplementedException();
}