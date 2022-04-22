namespace Plus.HabboHotel.Rooms.Chat.Commands.Moderator.Fun
{
    class SuperFastwalkCommand : IChatCommand
    {
        public string PermissionRequired
        {
            get { return "command_super_fastwalk"; }
        }

        public string Parameters
        {
            get { return ""; }
        }

        public string Description
        {
            get { return "Gives you the ability to walk very very fast."; }
        }

        public void Execute(GameClients.GameClient session, Room room, string[] @params)
        {
            var user = room.GetRoomUserManager().GetRoomUserByHabbo(session.GetHabbo().Id);
            if (user == null)
                return;

            user.SuperFastWalking = !user.SuperFastWalking;

            if (user.FastWalking)
                user.FastWalking = false;

            session.SendWhisper("Walking mode updated.");
        }
    }
}
