using Plus.Communication.Packets.Outgoing.Rooms.Settings;
using Plus.HabboHotel.Achievements;
using Plus.HabboHotel.GameClients;

namespace Plus.Communication.Packets.Incoming.Rooms.Settings;

internal class GetRoomFilterListEvent : IPacketEvent
{
    private readonly IAchievementManager _achievementManager;

    public GetRoomFilterListEvent(IAchievementManager achievementManager)
    {
        _achievementManager = achievementManager;
    }

    public void Parse(GameClient session, ClientPacket packet)
    {
        if (!session.GetHabbo().InRoom)
            return;
        var instance = session.GetHabbo().CurrentRoom;
        if (instance == null)
            return;
        if (!instance.CheckRights(session))
            return;
        session.SendPacket(new GetRoomFilterListComposer(instance));
        _achievementManager.ProgressAchievement(session, "ACH_SelfModRoomFilterSeen", 1);
    }
}