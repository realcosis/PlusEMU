using System;
using Plus.HabboHotel.GameClients;
using Plus.HabboHotel.Rooms;

namespace Plus.Communication.Packets.Incoming.Groups
{
    class DeleteGroupEvent : IPacketEvent
    {
        public void Parse(GameClient session, ClientPacket packet)
        {
            if (!PlusEnvironment.GetGame().GetGroupManager().TryGetGroup(packet.PopInt(), out var group))
            {
                session.SendNotification("Oops, we couldn't find that group!");
                return;
            }

            if (group.CreatorId != session.GetHabbo().Id && !session.GetHabbo().GetPermissions().HasRight("group_delete_override"))//Maybe a FUSE check for staff override?
            {
                session.SendNotification("Oops, only the group owner can delete a group!");
                return;
            }

            if (group.MemberCount >= Convert.ToInt32(PlusEnvironment.GetSettingsManager().TryGetValue("group.delete.member.limit")) && !session.GetHabbo().GetPermissions().HasRight("group_delete_limit_override"))
            {
                session.SendNotification("Oops, your group exceeds the maximum amount of members (" + Convert.ToInt32(PlusEnvironment.GetSettingsManager().TryGetValue("group.delete.member.limit")) + ") a group can exceed before being eligible for deletion. Seek assistance from a staff member.");
                return;
            }

            if (!PlusEnvironment.GetGame().GetRoomManager().TryGetRoom(group.RoomId, out var room))
                return;
            
            if (!RoomFactory.TryGetData(group.RoomId, out var _))
                return;

            room.Group = null;

            //Remove it from the cache.
            PlusEnvironment.GetGame().GetGroupManager().DeleteGroup(group.Id);

            //Now the :S stuff.
            using (var dbClient = PlusEnvironment.GetDatabaseManager().GetQueryReactor())
            {
                dbClient.RunQuery("DELETE FROM `groups` WHERE `id` = '" + group.Id + "'");
                dbClient.RunQuery("DELETE FROM `group_memberships` WHERE `group_id` = '" + group.Id + "'");
                dbClient.RunQuery("DELETE FROM `group_requests` WHERE `group_id` = '" + group.Id + "'");
                dbClient.RunQuery("UPDATE `rooms` SET `group_id` = '0' WHERE `group_id` = '" + group.Id + "' LIMIT 1");
                dbClient.RunQuery("UPDATE `user_stats` SET `groupid` = '0' WHERE `groupid` = '" + group.Id + "' LIMIT 1");
                dbClient.RunQuery("DELETE FROM `items_groups` WHERE `group_id` = '" + group.Id + "'");
            }

            //Unload it last.
            PlusEnvironment.GetGame().GetRoomManager().UnloadRoom(room.Id);

            //Say hey!
            session.SendNotification("You have successfully deleted your group.");
        }
    }
}