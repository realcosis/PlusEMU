using Plus.HabboHotel.GameClients;
using Plus.HabboHotel.Rooms;

namespace Plus.Communication.Packets.Outgoing.Rooms.Settings;

public class GetRoomFilterListComposer : IServerPacket
{
    private readonly Room _instance;

    public uint MessageId => ServerPacketHeader.GetRoomFilterListComposer;

    public GetRoomFilterListComposer(Room instance)
    {
        _instance = instance;
    }

    public void Compose(IOutgoingPacket packet)
    {
        packet.WriteInteger(_instance.WordFilterList.Count);
        foreach (var word in _instance.WordFilterList) packet.WriteString(word);
    }
}