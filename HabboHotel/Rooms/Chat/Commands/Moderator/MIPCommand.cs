using System;
using Plus.HabboHotel.GameClients;


using Plus.HabboHotel.Moderation;

namespace Plus.HabboHotel.Rooms.Chat.Commands.Moderator
{
    class MipCommand : IChatCommand
    {
        public string PermissionRequired
        {
            get { return "command_mip"; }
        }

        public string Parameters
        {
            get { return "%username%"; }
        }

        public string Description
        {
            get { return "Machine ban, IP ban and account ban another user."; }
        }

        public void Execute(GameClient session, Room room, string[] @params)
        {

            if (@params.Length == 1)
            {
                session.SendWhisper("Please enter the username of the user you'd like to IP ban & account ban.");
                return;
            }

            var habbo = PlusEnvironment.GetHabboByUsername(@params[1]);
            if (habbo == null)
            {
                session.SendWhisper("An error occoured whilst finding that user in the database.");
                return;
            }

            if (habbo.GetPermissions().HasRight("mod_tool") && !session.GetHabbo().GetPermissions().HasRight("mod_ban_any"))
            {
                session.SendWhisper("Oops, you cannot ban that user.");
                return;
            }

            var ipAddress = String.Empty;
            var expire = PlusEnvironment.GetUnixTimestamp() + 78892200;
            var username = habbo.Username;

            using (var dbClient = PlusEnvironment.GetDatabaseManager().GetQueryReactor())
            {
                dbClient.RunQuery("UPDATE `user_info` SET `bans` = `bans` + '1' WHERE `user_id` = '" + habbo.Id + "' LIMIT 1");

                dbClient.SetQuery("SELECT `ip_last` FROM `users` WHERE `id` = '" + habbo.Id + "' LIMIT 1");
                ipAddress = dbClient.GetString();
            }

            string reason = null;
            if (@params.Length >= 3)
                reason = CommandManager.MergeParams(@params, 2);
            else
                reason = "No reason specified.";

            if (!string.IsNullOrEmpty(ipAddress))
                PlusEnvironment.GetGame().GetModerationManager().BanUser(session.GetHabbo().Username, ModerationBanType.Ip, ipAddress, reason, expire);
            PlusEnvironment.GetGame().GetModerationManager().BanUser(session.GetHabbo().Username, ModerationBanType.Username, habbo.Username, reason, expire);

            if (!string.IsNullOrEmpty(habbo.MachineId))
                PlusEnvironment.GetGame().GetModerationManager().BanUser(session.GetHabbo().Username, ModerationBanType.Machine, habbo.MachineId, reason, expire);

            var targetClient = PlusEnvironment.GetGame().GetClientManager().GetClientByUsername(username);
            if (targetClient != null)
                targetClient.Disconnect();
            session.SendWhisper("Success, you have machine, IP and account banned the user '" + username + "' for '" + reason + "'!");
        }
    }
}