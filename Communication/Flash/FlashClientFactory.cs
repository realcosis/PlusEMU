using NetCoreServer;
using Plus.Communication.Abstractions;
using Plus.Communication.Revisions;
using Plus.HabboHotel.GameClients;

namespace Plus.Communication.Flash;

public class FlashClientFactory : IGameClientFactory<TcpSessionProxy, TcpServer>
{
    private readonly FlashPacketFactory _packetFactory;
    private readonly IRevisionsCache _revisionsCache;

    public FlashClientFactory(FlashPacketFactory packetFactory, IRevisionsCache revisionsCache)
    {
        _packetFactory = packetFactory;
        _revisionsCache = revisionsCache;
    }

    public TcpSessionProxy Create(TcpServer server) => new((FlashServer)server, new FlashGameClient((FlashServer)server, _packetFactory) { Revision = _revisionsCache.InternalRevision });
}