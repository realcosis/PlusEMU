using Plus.HabboHotel.GameClients;

namespace Plus.Communication.Packets.Incoming.GameCenter;

internal class InitializeGameCenterEvent : IPacketEvent
{
    public Task Parse(GameClient session, IIncomingPacket packet) => Task.CompletedTask;
}