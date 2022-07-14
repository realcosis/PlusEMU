using Plus.HabboHotel.GameClients;

namespace Plus.Communication.Packets;

public interface IServerPacket
{
    int MessageId { get; }
    void Compose(IOutgoingPacket packet);
}