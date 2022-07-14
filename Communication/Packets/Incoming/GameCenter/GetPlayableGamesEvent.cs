using Plus.Communication.Packets.Outgoing.GameCenter;
using Plus.HabboHotel.Achievements;
using Plus.HabboHotel.GameClients;

namespace Plus.Communication.Packets.Incoming.GameCenter;

internal class GetPlayableGamesEvent : IPacketEvent
{
    private readonly IAchievementManager _achievementManager;

    public GetPlayableGamesEvent(IAchievementManager achievementManager)
    {
        _achievementManager = achievementManager;
    }

    public Task Parse(GameClient session, IIncomingPacket packet)
    {
        var gameId = packet.ReadInt();
        session.Send(new GameAccountStatusComposer(gameId));
        session.Send(new PlayableGamesComposer(gameId));
        session.Send(new GameAchievementListComposer(session, _achievementManager.GetGameAchievements(gameId), gameId));
        return Task.CompletedTask;
    }
}