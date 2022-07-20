using Plus.HabboHotel.Achievements;
using Plus.HabboHotel.GameClients;

namespace Plus.Communication.Packets.Outgoing.Inventory.Achievements;

public class AchievementUnlockedComposer : IServerPacket
{
    private readonly Achievement _achievement;
    private readonly int _level;
    private readonly int _pointReward;
    private readonly int _pixelReward;
    public uint MessageId => ServerPacketHeader.AchievementUnlockedComposer;

    public AchievementUnlockedComposer(Achievement achievement, int level, int pointReward, int pixelReward)
    {
        _achievement = achievement;
        _level = level;
        _pointReward = pointReward;
        _pixelReward = pixelReward;
    }

    public void Compose(IOutgoingPacket packet)
    {
        packet.WriteInteger(_achievement.Id); // Achievement ID
        packet.WriteInteger(_level); // Achieved level
        packet.WriteInteger(144); // Unknown. Random useless number.
        packet.WriteString(_achievement.GroupName + _level); // Achieved name
        packet.WriteInteger(_pointReward); // Point reward
        packet.WriteInteger(_pixelReward); // Pixel reward
        packet.WriteInteger(0); // Unknown.
        packet.WriteInteger(10); // Unknown.
        packet.WriteInteger(21); // Unknown. (Extra reward?)
        packet.WriteString(_level > 1 ? _achievement.GroupName + (_level - 1) : string.Empty);
        packet.WriteString(_achievement.Category);
        packet.WriteBoolean(true);

    }
}