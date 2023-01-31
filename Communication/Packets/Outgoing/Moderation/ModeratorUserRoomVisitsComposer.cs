using Plus.HabboHotel.GameClients;
using Plus.HabboHotel.Rooms;
using Plus.HabboHotel.Users;
using Plus.Utilities;

namespace Plus.Communication.Packets.Outgoing.Moderation;

public class ModeratorUserRoomVisitsComposer : IServerPacket
{
    private readonly Habbo _data;
    private readonly Dictionary<double, RoomData> _visits;
    public uint MessageId => ServerPacketHeader.ModeratorUserRoomVisitsComposer;

    public ModeratorUserRoomVisitsComposer(Habbo data, Dictionary<double, RoomData> visits)
    {
        _data = data;
        _visits = visits;
    }

    public void Compose(IOutgoingPacket packet)
    {
        packet.WriteInteger(_data.Id);
        packet.WriteString(_data.Username);
        packet.WriteInteger(_visits.Count);
        foreach (var (key, roomData) in _visits)
        {
            packet.WriteUInteger(roomData.Id);
            packet.WriteString(roomData.Name);
            packet.WriteInteger(UnixTimestamp.FromUnixTimestamp(key).Hour);
            packet.WriteInteger(UnixTimestamp.FromUnixTimestamp(key).Minute);
        }
    }
}