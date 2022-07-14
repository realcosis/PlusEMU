using Plus.Communication.Packets.Outgoing.Avatar;
using Plus.HabboHotel.GameClients;

namespace Plus.Communication.Packets.Incoming.Avatar;

internal class GetWardrobeEvent : IPacketEvent
{
    public Task Parse(GameClient session, IIncomingPacket packet)
    {
        session.Send(new WardrobeComposer(session.GetHabbo().Id));
        return Task.CompletedTask;
    }
}