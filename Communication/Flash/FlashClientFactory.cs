using NetCoreServer;
using Plus.Communication.Abstractions;

namespace Plus.Communication.Flash
{
    public class FlashClientFactory : IGameClientFactory<FlashGameClient>
    {
        private readonly FlashPacketFactory _packetFactory;

        public FlashClientFactory(FlashPacketFactory packetFactory)
        {
            _packetFactory = packetFactory;
        }
        public FlashGameClient Create(TcpServer server) => new(server as FlashServer, _packetFactory);
    }
}