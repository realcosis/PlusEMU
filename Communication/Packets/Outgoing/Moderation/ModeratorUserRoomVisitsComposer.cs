using System.Collections.Generic;

using Plus.Utilities;
using Plus.HabboHotel.Rooms;
using Plus.HabboHotel.Users;

namespace Plus.Communication.Packets.Outgoing.Moderation
{
    class ModeratorUserRoomVisitsComposer : ServerPacket
    {
        public ModeratorUserRoomVisitsComposer(Habbo data, Dictionary<double, RoomData> visits)
            : base(ServerPacketHeader.ModeratorUserRoomVisitsMessageComposer)
        {
            WriteInteger(data.Id);
            WriteString(data.Username);
            WriteInteger(visits.Count);

            foreach (KeyValuePair<double, RoomData> visit in visits)
            {
                WriteInteger(visit.Value.Id);
                WriteString(visit.Value.Name);
                WriteInteger(UnixTimestamp.FromUnixTimestamp(visit.Key).Hour);
                WriteInteger(UnixTimestamp.FromUnixTimestamp(visit.Key).Minute);
            }
        }
    }
}
