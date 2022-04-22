using System.Collections.Generic;

namespace Plus.HabboHotel.Rooms.Chat.Commands.Moderator
{
    class RoomMuteCommand : IChatCommand
    {
        public string PermissionRequired
        {
            get { return "command_roommute"; }
        }

        public string Parameters
        {
            get { return "%message%"; }
        }

        public string Description
        {
            get { return "Mute the room with a reason."; }
        }

        public void Execute(GameClients.GameClient session, Room room, string[] @params)
        {
            if (@params.Length == 1)
            {
                session.SendWhisper("Please provide a reason for muting the room to show to the users.");
                return;
            }

            if (!room.RoomMuted)
                room.RoomMuted = true;

            var msg = CommandManager.MergeParams(@params, 1);

            var roomUsers = room.GetRoomUserManager().GetRoomUsers();
            if (roomUsers.Count > 0)
            {
                foreach (var user in roomUsers)
                {
                    if (user == null || user.GetClient() == null || user.GetClient().GetHabbo() == null || user.GetClient().GetHabbo().Username == session.GetHabbo().Username)
                        continue;

                    user.GetClient().SendWhisper("This room has been muted because: " + msg);
                }
            }
        }
    }
}
