using System.Linq;

namespace Plus.HabboHotel.Rooms.Chat.Commands.Moderator.Fun
{
    class AllAroundMeCommand : IChatCommand
    {
        public string PermissionRequired
        {
            get { return "command_allaroundme"; }
        }

        public string Parameters
        {
            get { return ""; }
        }

        public string Description
        {
            get { return "Need some attention? Pull all of the users to you."; }
        }

        public void Execute(GameClients.GameClient session, Room room, string[] @params)
        {
            var user = room.GetRoomUserManager().GetRoomUserByHabbo(session.GetHabbo().Id);
            if (user == null)
                return;

            var users = room.GetRoomUserManager().GetRoomUsers();
            foreach (var u in users.ToList())
            {
                if (u == null || session.GetHabbo().Id == u.UserId)
                    continue;

                u.MoveTo(user.X, user.Y, true);
            }
        }
    }
}
