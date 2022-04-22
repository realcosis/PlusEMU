using System.Linq;
using System.Collections.Generic;

namespace Plus.HabboHotel.Rooms.Chat.Commands.Moderator.Fun
{
    class MassEnableCommand : IChatCommand
    {
        public string PermissionRequired
        {
            get { return "command_massenable"; }
        }

        public string Parameters
        {
            get { return "%EffectId%"; }
        }

        public string Description
        {
            get { return "Give every user in the room a specific enable ID."; }
        }

        public void Execute(GameClients.GameClient session, Room room, string[] @params)
        {
            if (@params.Length == 1)
            {
                session.SendWhisper("Please enter an effect ID.");
                return;
            }

            var enableId = 0;
            if (int.TryParse(@params[1], out enableId))
            {
                if (enableId == 102 || enableId == 178)
                {
                    session.Disconnect();
                    return;
                }

                if (!session.GetHabbo().GetPermissions().HasCommand("command_override_massenable") && room.OwnerId != session.GetHabbo().Id)
                {
                    session.SendWhisper("You can only use this command in your own room.");
                    return;
                }

                var users = room.GetRoomUserManager().GetRoomUsers();
                if (users.Count > 0)
                {
                    foreach (var u in users.ToList())
                    {
                        if (u == null || u.RidingHorse)
                            continue;

                        u.ApplyEffect(enableId);
                    }
                }
            }
            else
            {
                session.SendWhisper("Please enter an effect ID.");
                return;
            }
        }
    }
}
