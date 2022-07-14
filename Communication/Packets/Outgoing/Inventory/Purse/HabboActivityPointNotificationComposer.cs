using Plus.HabboHotel.GameClients;

namespace Plus.Communication.Packets.Outgoing.Inventory.Purse;

internal class HabboActivityPointNotificationComposer : IServerPacket
{
    private readonly int _balance;
    private readonly int _notify;
    private readonly int _type;
    public int MessageId => ServerPacketHeader.HabboActivityPointNotificationMessageComposer;

    public HabboActivityPointNotificationComposer(int balance, int notify, int type = 0)
    {
        _balance = balance;
        _notify = notify;
        _type = type;
    }

    public void Compose(IOutgoingPacket packet)
    {
        packet.WriteInteger(_balance);
        packet.WriteInteger(_notify);
        packet.WriteInteger(_type);
    }
}