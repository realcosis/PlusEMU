namespace Plus.HabboHotel.Rooms.Chat.Commands.User.Fun
{
    class MoonwalkCommand : IChatCommand
    {
        public string PermissionRequired
        {
            get { return "command_moonwalk"; }
        }

        public string Parameters
        {
            get { return ""; }
        }

        public string Description
        {
            get { return "Wear the shoes of Michael Jackson."; }
        }

        public void Execute(GameClients.GameClient session, Room room, string[] @params)
        {
            RoomUser user = room.GetRoomUserManager().GetRoomUserByHabbo(session.GetHabbo().Id);
            if (user == null)
                return;

            user.MoonwalkEnabled = !user.MoonwalkEnabled;

            if (user.MoonwalkEnabled)
                session.SendWhisper("Moonwalk enabled!");
            else
                session.SendWhisper("Moonwalk disabled!");
        }
    }
}
