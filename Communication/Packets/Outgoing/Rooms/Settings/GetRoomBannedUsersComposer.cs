using Plus.HabboHotel.GameClients;
using Plus.HabboHotel.Rooms;

namespace Plus.Communication.Packets.Outgoing.Rooms.Settings;

public class GetRoomBannedUsersComposer : IServerPacket
{
    private readonly Room _instance;
    public uint MessageId => ServerPacketHeader.GetRoomBannedUsersComposer;

    public GetRoomBannedUsersComposer(Room instance)
    {
        _instance = instance;
    }

    public void Compose(IOutgoingPacket packet)
    {
        packet.WriteInteger(_instance.Id);
        packet.WriteInteger(_instance.GetBans().BannedUsers().Count); //Count
        foreach (var id in _instance.GetBans().BannedUsers().ToList())
        {
            var data = PlusEnvironment.GetGame().GetCacheManager().GenerateUser(id);
            if (data == null)
            {
                packet.WriteInteger(0);
                packet.WriteString("Unknown Error");
            }
            else
            {
                packet.WriteInteger(data.Id);
                packet.WriteString(data.Username);
            }
        }
    }
}