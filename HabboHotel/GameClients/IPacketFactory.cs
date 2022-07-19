using Microsoft.IO;
using Plus.Utilities.DependencyInjection;

namespace Plus.HabboHotel.GameClients
{
    [Transient]
    public interface IPacketFactory
    {
        IIncomingPacket CreateIncomingPacket(Memory<byte> buffer);
        IOutgoingPacket CreateOutgoingPacket(RecyclableMemoryStream stream);
    }
}