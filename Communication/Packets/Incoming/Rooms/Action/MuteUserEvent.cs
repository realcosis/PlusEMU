using Plus.HabboHotel.Achievements;
using Plus.HabboHotel.GameClients;

namespace Plus.Communication.Packets.Incoming.Rooms.Action;

internal class MuteUserEvent : IPacketEvent
{
    private readonly IAchievementManager _achievementManager;

    public MuteUserEvent(IAchievementManager achievementManager)
    {
        _achievementManager = achievementManager;
    }

    public void Parse(GameClient session, ClientPacket packet)
    {
        if (!session.GetHabbo().InRoom)
            return;
        var userId = packet.PopInt();
        packet.PopInt(); //roomId
        var time = packet.PopInt();
        var room = session.GetHabbo().CurrentRoom;
        if (room == null)
            return;
        if (room.WhoCanMute == 0 && !room.CheckRights(session, true) && room.Group == null || room.WhoCanMute == 1 && !room.CheckRights(session) && room.Group == null ||
            room.Group != null && !room.CheckRights(session, false, true))
            return;
        var target = room.GetRoomUserManager().GetRoomUserByHabbo(PlusEnvironment.GetUsernameById(userId));
        if (target == null)
            return;
        if (target.GetClient().GetHabbo().GetPermissions().HasRight("mod_tool"))
            return;
        if (room.MutedUsers.ContainsKey(userId))
        {
            if (room.MutedUsers[userId] < PlusEnvironment.GetUnixTimestamp())
                room.MutedUsers.Remove(userId);
            else
                return;
        }
        room.MutedUsers.Add(userId, PlusEnvironment.GetUnixTimestamp() + time * 60);
        target.GetClient().SendWhisper("The room owner has muted you for " + time + " minutes!");
        _achievementManager.ProgressAchievement(session, "ACH_SelfModMuteSeen", 1);
    }
}