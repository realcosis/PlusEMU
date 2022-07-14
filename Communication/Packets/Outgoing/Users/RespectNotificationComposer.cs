using Plus.HabboHotel.GameClients;

namespace Plus.Communication.Packets.Outgoing.Users;

internal class RespectNotificationComposer : IServerPacket
{
    private readonly int _userId;
    private readonly int _respect;
    public int MessageId => ServerPacketHeader.RespectNotificationMessageComposer;

    public RespectNotificationComposer(int userId, int respect)
    {
        _userId = userId;
        _respect = respect;
    }

    public void Compose(IOutgoingPacket packet)
    {
        packet.WriteInteger(_userId);
        packet.WriteInteger(_respect);
    }
}