using NetCoreServer;
using Plus.Communication.Abstractions;
using Plus.Communication.Revisions;

namespace Plus.Communication.Flash
{
    public class FlashClientFactory : IGameClientFactory<FlashGameClient>
    {
        private readonly FlashPacketFactory _packetFactory;
        private readonly IRevisionsCache _revisionsCache;

        public FlashClientFactory(FlashPacketFactory packetFactory, IRevisionsCache revisionsCache)
        {
            _packetFactory = packetFactory;
            _revisionsCache = revisionsCache;
        }

        public FlashGameClient Create(TcpServer server) => new(server as FlashServer, _packetFactory)
            { Revision = _revisionsCache.InternalRevision };
    }
}