using Plus.HabboHotel.GameClients;
using Plus.HabboHotel.Rooms;

namespace Plus.Communication.Packets.Outgoing.Rooms.Settings;

public class FlatControllerRemovedComposer : IServerPacket
{
    private readonly Room _instance;
    private readonly int _userId;
    public uint MessageId => ServerPacketHeader.FlatControllerRemovedComposer;

    public FlatControllerRemovedComposer(Room instance, int userId)
    {
        _instance = instance;
        _userId = userId;
    }

    public void Compose(IOutgoingPacket packet)
    {
        packet.WriteInteger(_instance.Id);
        packet.WriteInteger(_userId);
    }
}