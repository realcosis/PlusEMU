using Plus.HabboHotel.Rooms;

namespace Plus.Communication.Packets.Outgoing.Rooms.Settings;

internal class GetRoomFilterListComposer : ServerPacket
{
    public GetRoomFilterListComposer(Room instance)
        : base(ServerPacketHeader.GetRoomFilterListMessageComposer)
    {
        WriteInteger(instance.WordFilterList.Count);
        foreach (var word in instance.WordFilterList) WriteString(word);
    }
}