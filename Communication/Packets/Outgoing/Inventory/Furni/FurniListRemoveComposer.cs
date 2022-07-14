using Plus.HabboHotel.GameClients;

namespace Plus.Communication.Packets.Outgoing.Inventory.Furni;

internal class FurniListRemoveComposer : IServerPacket
{
    private readonly int _id;
    public int MessageId => ServerPacketHeader.FurniListRemoveMessageComposer;

    public FurniListRemoveComposer(int id)
    {
        _id = id;
    }

    public void Compose(IOutgoingPacket packet)
    {
        packet.WriteInteger(_id);
    }
}