using Plus.HabboHotel.GameClients;
using Plus.Utilities.DependencyInjection;

namespace Plus.Communication.Packets;

[Transient]
public interface IPacketEvent
{
    Task Parse(GameClient session, IIncomingPacket packet);
}