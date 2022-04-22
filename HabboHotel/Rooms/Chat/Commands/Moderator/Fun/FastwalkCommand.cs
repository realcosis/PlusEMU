namespace Plus.HabboHotel.Rooms.Chat.Commands.Moderator.Fun
{
    class FastwalkCommand : IChatCommand
    {
        public string PermissionRequired
        {
            get { return "command_fastwalk"; }
        }

        public string Parameters
        {
            get { return ""; }
        }

        public string Description
        {
            get { return "Gives you the ability to walk very fast."; }
        }

        public void Execute(GameClients.GameClient session, Room room, string[] @params)
        {
            var user = room.GetRoomUserManager().GetRoomUserByHabbo(session.GetHabbo().Id);
            if (user == null)
                return;

            user.FastWalking = !user.FastWalking;

            if (user.SuperFastWalking)
                user.SuperFastWalking = false;

            session.SendWhisper("Walking mode updated.");
        }
    }
}
