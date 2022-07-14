using Plus.Communication.Packets.Outgoing.Handshake;
using Plus.HabboHotel.GameClients;

namespace Plus.Communication.Packets.Incoming.Handshake;

public class InfoRetrieveEvent : IPacketEvent
{
    public Task Parse(GameClient session, IIncomingPacket packet)
    {
        session.Send(new UserObjectComposer(session.GetHabbo()));
        session.Send(new UserPerksComposer());
        return Task.CompletedTask;
    }
}