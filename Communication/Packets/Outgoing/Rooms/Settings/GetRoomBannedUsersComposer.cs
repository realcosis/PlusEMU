using System.Linq;
using Plus.HabboHotel.Rooms;
using Plus.HabboHotel.Cache.Type;

namespace Plus.Communication.Packets.Outgoing.Rooms.Settings
{
    class GetRoomBannedUsersComposer : ServerPacket
    {
        public GetRoomBannedUsersComposer(Room instance)
            : base(ServerPacketHeader.GetRoomBannedUsersMessageComposer)
        {
            WriteInteger(instance.Id);

            WriteInteger(instance.GetBans().BannedUsers().Count);//Count
            foreach (int id in instance.GetBans().BannedUsers().ToList())
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