using Microsoft.Extensions.Options;
using Plus.Communication.Abstractions;
using Plus.Communication.Packets;

namespace Plus.Communication.Flash
{
    public class FlashServer : TcpGameServer<FlashServerConfiguration>, IFlashServer
    {
        public FlashServer(IOptions<FlashServerConfiguration> options, FlashClientFactory flashClientFactory, IPacketManager packetManager) : base(options, flashClientFactory, packetManager)
        {
        }
    }
}
