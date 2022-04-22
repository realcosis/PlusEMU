namespace Plus.HabboHotel.Rooms.Chat.Commands.User
{
    class MuteBotsCommand : IChatCommand
    {
        public string PermissionRequired
        {
            get { return "command_mute_bots"; }
        }

        public string Parameters
        {
            get { return ""; }
        }

        public string Description
        {
            get { return "Ignore bot chat or enable it again."; }
        }

        public void Execute(GameClients.GameClient session, Room room, string[] @params)
        {
            session.GetHabbo().AllowBotSpeech = !session.GetHabbo().AllowBotSpeech;
            using (var dbClient = PlusEnvironment.GetDatabaseManager().GetQueryReactor())
            {
                dbClient.RunQuery("UPDATE `users` SET `bots_muted` = '" + ((session.GetHabbo().AllowBotSpeech) ? 1 : 0) + "' WHERE `id` = '" + session.GetHabbo().Id + "' LIMIT 1");
            }

            if (session.GetHabbo().AllowBotSpeech)
                session.SendWhisper("Change successful, you can no longer see speech from bots.");
            else
                session.SendWhisper("Change successful, you can now see speech from bots.");                       
        }
    }
}
