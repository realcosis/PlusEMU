using System;
using System.Linq;
using System.Collections.Generic;
using Plus.Communication.Packets.Outgoing.Rooms.Avatar;

namespace Plus.HabboHotel.Rooms.Chat.Commands.Moderator.Fun
{
    class MassDanceCommand : IChatCommand
    {
        public string PermissionRequired
        {
            get { return "command_massdance"; }
        }

        public string Parameters
        {
            get { return "%DanceId%"; }
        }

        public string Description
        {
            get { return "Force everyone in the room to dance to a dance of your choice."; }
        }

        public void Execute(GameClients.GameClient session, Room room, string[] @params)
        {
            if (@params.Length == 1)
            {
                session.SendWhisper("Please enter a dance ID. (1-4)");
                return;
            }

            var danceId = Convert.ToInt32(@params[1]);
            if (danceId < 0 || danceId > 4)
            {
                session.SendWhisper("Please enter a dance ID. (1-4)");
                return;
            }

            var users = room.GetRoomUserManager().GetRoomUsers();
            if (users.Count > 0)
            {
                foreach (var u in users.ToList())
                {
                    if (u == null)
                        continue;

                    if (u.CarryItemId > 0)
                        u.CarryItemId = 0;

                    u.DanceId = danceId;
                    room.SendPacket(new DanceComposer(u, danceId));
                }
            }
        }
    }
}
