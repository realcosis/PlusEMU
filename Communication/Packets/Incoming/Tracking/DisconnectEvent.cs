using Plus.HabboHotel.GameClients;

namespace Plus.Communication.Packets.Incoming.Tracking;

internal class DisconnectEvent : IPacketEvent
{
    public Task Parse(GameClient session, IIncomingPacket packet)
    {
        session.Disconnect();
        return Task.CompletedTask;
    }
}