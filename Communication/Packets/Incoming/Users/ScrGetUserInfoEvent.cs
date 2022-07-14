using Plus.Communication.Packets.Outgoing.Users;
using Plus.HabboHotel.GameClients;

namespace Plus.Communication.Packets.Incoming.Users;

internal class ScrGetUserInfoEvent : IPacketEvent
{
    public Task Parse(GameClient session, IIncomingPacket packet)
    {
        session.Send(new ScrSendUserInfoComposer());
        return Task.CompletedTask;
    }
}