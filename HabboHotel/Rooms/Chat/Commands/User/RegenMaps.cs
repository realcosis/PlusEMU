namespace Plus.HabboHotel.Rooms.Chat.Commands.User
{
    class RegenMaps : IChatCommand
    {
        public string PermissionRequired
        {
            get { return "command_regen_maps"; }
        }

        public string Parameters
        {
            get { return ""; }
        }

        public string Description
        {
            get { return "Is the game map of your room broken? Fix it with this command!"; }
        }

        public void Execute(GameClients.GameClient session, Room room, string[] @params)
        {
            if (!room.CheckRights(session, true))
            {
                session.SendWhisper("Oops, only the owner of this room can run this command!");
                return;
            }

            room.GetGameMap().GenerateMaps();
            session.SendWhisper("Game map of this room successfully re-generated.");
        }
    }
}
