using Microsoft.IO;
using Plus.HabboHotel.GameClients;

namespace Plus.Communication.Flash
{
    public class FlashPacketFactory : IPacketFactory
    {
        public IIncomingPacket CreateIncomingPacket(Memory<byte> buffer) => new FlashIncomingPacket { Buffer = buffer };

        public IOutgoingPacket CreateOutgoingPacket(RecyclableMemoryStream stream) => new FlashOutgoingPacket(stream);
    }
}