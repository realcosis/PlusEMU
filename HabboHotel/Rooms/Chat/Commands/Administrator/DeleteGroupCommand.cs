namespace Plus.HabboHotel.Rooms.Chat.Commands.Administrator
{
    class DeleteGroupCommand : IChatCommand
    {
        public string PermissionRequired
        {
            get { return "command_delete_group"; }
        }

        public string Parameters
        {
            get { return ""; }
        }

        public string Description
        {
            get { return "Delete a group from the database and cache."; }
        }

        public void Execute(GameClients.GameClient session, Room room, string[] @params)
        {
            room = session.GetHabbo().CurrentRoom;
            if (room == null)
                return;

            if (room.Group == null)
            {
                session.SendWhisper("Oops, there is no group here?");
                return;
            }

            using (var dbClient = PlusEnvironment.GetDatabaseManager().GetQueryReactor())
            {
                dbClient.RunQuery("DELETE FROM `groups` WHERE `id` = '" + room.Group.Id + "'");
                dbClient.RunQuery("DELETE FROM `group_memberships` WHERE `group_id` = '" + room.Group.Id + "'");
                dbClient.RunQuery("DELETE FROM `group_requests` WHERE `group_id` = '" + room.Group.Id + "'");
                dbClient.RunQuery("UPDATE `rooms` SET `group_id` = '0' WHERE `group_id` = '" + room.Group.Id + "' LIMIT 1");
                dbClient.RunQuery("UPDATE `user_stats` SET `groupid` = '0' WHERE `groupid` = '" + room.Group.Id + "' LIMIT 1");
                dbClient.RunQuery("DELETE FROM `items_groups` WHERE `group_id` = '" + room.Group.Id + "'");
            }

            PlusEnvironment.GetGame().GetGroupManager().DeleteGroup(room.Group.Id);

            room.Group = null;

            PlusEnvironment.GetGame().GetRoomManager().UnloadRoom(room.Id);

            session.SendNotification("Success, group deleted.");
            return;
        }
    }
}
