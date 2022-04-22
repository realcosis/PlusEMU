using System.Collections.Generic;

namespace Plus.HabboHotel.Rooms.Chat.Commands.Moderator
{
    class RoomUnmuteCommand : IChatCommand
    {
        public string PermissionRequired
        {
            get { return "command_unroommute"; }
        }

        public string Parameters
        {
            get { return ""; }
        }

        public string Description
        {
            get { return "Unmute the room."; }
        }

        public void Execute(GameClients.GameClient session, Room room, string[] @params)
        {
            if (!room.RoomMuted)
            {
                session.SendWhisper("This room isn't muted.");
                return;
            }

            room.RoomMuted = false;

            List<RoomUser> roomUsers = room.GetRoomUserManager().GetRoomUsers();
            if (roomUsers.Count > 0)
            {
                foreach (RoomUser user in roomUsers)
                {
                    if (user == null || user.GetClient() == null || user.GetClient().GetHabbo() == null || user.GetClient().GetHabbo().Username == session.GetHabbo().Username)
                        continue;

                    user.GetClient().SendWhisper("This room has been un-muted .");
                }
            }
        }
    }
}