using System.Linq;
using Plus.HabboHotel.Rooms;
using Plus.HabboHotel.Cache.Type;

namespace Plus.Communication.Packets.Outgoing.Rooms.Settings
{
    class RoomRightsListComposer : ServerPacket
    {
        public RoomRightsListComposer(Room instance)
            : base(ServerPacketHeader.RoomRightsListMessageComposer)
        {
            WriteInteger(instance.Id);

            WriteInteger(instance.UsersWithRights.Count);
            foreach (int id in instance.UsersWithRights.ToList())
            {
                UserCache data = PlusEnvironment.GetGame().GetCacheManager().GenerateUser(id);
                if (data == null)
                {
                    WriteInteger(0);
                    WriteString("Unknown Error");
                }
                else
                {
                    WriteInteger(data.Id);
                    WriteString(data.Username);
                }
            }
        }
    }
}
