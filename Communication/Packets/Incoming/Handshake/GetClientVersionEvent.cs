using Plus.Communication.Attributes;
using Plus.Communication.Revisions;
using Plus.HabboHotel.GameClients;

namespace Plus.Communication.Packets.Incoming.Handshake;

[NoAuthenticationRequired]
public class GetClientVersionEvent : IPacketEvent
{
    private readonly IRevisionsCache _revisionsCache;

    public GetClientVersionEvent(IRevisionsCache revisionsCache)
    {
        _revisionsCache = revisionsCache;
    }

    public Task Parse(GameClient session, IIncomingPacket packet)
    {
        var build = packet.ReadString();
        if (!_revisionsCache.Revisions.TryGetValue(build, out var revision))
        {
            session.Disconnect();
            // TODO @80O: Log error user login with unknown revision.
            return Task.CompletedTask;
        }

        session.Revision = revision;
        return Task.CompletedTask;
    }
}