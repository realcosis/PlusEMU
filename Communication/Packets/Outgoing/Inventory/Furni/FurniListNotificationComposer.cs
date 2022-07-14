using Plus.HabboHotel.GameClients;

namespace Plus.Communication.Packets.Outgoing.Inventory.Furni;

internal class FurniListNotificationComposer : IServerPacket
{
    private readonly int _id;
    private readonly int _type;
    public int MessageId => ServerPacketHeader.FurniListNotificationMessageComposer;

    public FurniListNotificationComposer(int id, int type)
    {
        _id = id;
        _type = type;
    }

    public void Compose(IOutgoingPacket packet)
    {
        packet.WriteInteger(1);
        packet.WriteInteger(_type);
        packet.WriteInteger(1);
        packet.WriteInteger(_id);
    }
}