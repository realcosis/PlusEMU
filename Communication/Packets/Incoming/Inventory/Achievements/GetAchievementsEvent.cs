using Plus.Communication.Packets.Outgoing.Inventory.Achievements;
using Plus.HabboHotel.Achievements;
using Plus.HabboHotel.GameClients;

namespace Plus.Communication.Packets.Incoming.Inventory.Achievements;

internal class GetAchievementsEvent : IPacketEvent
{
    private readonly IAchievementManager _achievementManager;

    public GetAchievementsEvent(IAchievementManager achievementManager)
    {
        _achievementManager = achievementManager;
    }

    public Task Parse(GameClient session, IIncomingPacket packet)
    {
        session.Send(new AchievementsComposer(session, _achievementManager.Achievements.Values.ToList()));
        return Task.CompletedTask;
    }
}