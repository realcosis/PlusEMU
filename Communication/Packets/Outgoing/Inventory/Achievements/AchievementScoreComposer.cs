using Plus.HabboHotel.GameClients;

namespace Plus.Communication.Packets.Outgoing.Inventory.Achievements;

internal class AchievementScoreComposer : IServerPacket
{
    private readonly int _score;
    public int MessageId => ServerPacketHeader.AchievementScoreMessageComposer;

    public AchievementScoreComposer(int score)
    {
        _score = score;
    }

    public void Compose(IOutgoingPacket packet) => packet.WriteInteger(_score);
}