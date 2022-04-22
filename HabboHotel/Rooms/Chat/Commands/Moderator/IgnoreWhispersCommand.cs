namespace Plus.HabboHotel.Rooms.Chat.Commands.Moderator
{
    class IgnoreWhispersCommand : IChatCommand
    {
        public string PermissionRequired
        {
            get { return "command_ignore_whispers"; }
        }

        public string Parameters
        {
            get { return ""; }
        }

        public string Description
        {
            get { return "Allows you to ignore all of the whispers in the room, except from your own."; }
        }

        public void Execute(GameClients.GameClient session, Room room, string[] @params)
        {
            session.GetHabbo().IgnorePublicWhispers = !session.GetHabbo().IgnorePublicWhispers;
            session.SendWhisper("You're " + (session.GetHabbo().IgnorePublicWhispers ? "now" : "no longer") + " ignoring public whispers!");
        }
    }
}
