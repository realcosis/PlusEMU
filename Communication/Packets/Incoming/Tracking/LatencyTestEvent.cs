using Plus.HabboHotel.GameClients;

namespace Plus.Communication.Packets.Incoming.Tracking;

internal class LatencyTestEvent : IPacketEvent
{
    public Task Parse(GameClient session, IIncomingPacket packet)
    {
        //Session.SendMessage(new LatencyTestComposer(Packet.PopInt()));
        return Task.CompletedTask;
    }
}