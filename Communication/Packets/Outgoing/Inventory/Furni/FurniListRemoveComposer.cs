using Plus.HabboHotel.GameClients;

namespace Plus.Communication.Packets.Outgoing.Inventory.Furni;

public class FurniListRemoveComposer : IServerPacket
{
    private readonly int _id;
    public uint MessageId => ServerPacketHeader.FurniListRemoveComposer;

    public FurniListRemoveComposer(int id)
    {
        _id = id;
    }

    public void Compose(IOutgoingPacket packet)
    {
        packet.WriteInteger(_id);
    }
}