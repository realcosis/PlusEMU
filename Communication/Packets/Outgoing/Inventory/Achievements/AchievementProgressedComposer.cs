using Plus.HabboHotel.Achievements;
using Plus.HabboHotel.GameClients;

namespace Plus.Communication.Packets.Outgoing.Inventory.Achievements;

internal class AchievementProgressedComposer : IServerPacket
{
    private readonly Achievement _achievement;
    private readonly int _targetLevel;
    private readonly AchievementLevel _targetLevelData;
    private readonly int _totalLevels;
    private readonly UserAchievement _userData;
    public int MessageId => ServerPacketHeader.AchievementProgressedMessageComposer;

    public AchievementProgressedComposer(Achievement achievement, int targetLevel, AchievementLevel targetLevelData, int totalLevels, UserAchievement userData)
    {
        _achievement = achievement;
        _targetLevel = targetLevel;
        _targetLevelData = targetLevelData;
        _totalLevels = totalLevels;
        _userData = userData;
    }

    public void Compose(IOutgoingPacket packet)
    {

        packet.WriteInteger(_achievement.Id); // Unknown (ID?)
        packet.WriteInteger(_targetLevel); // Target level
        packet.WriteString(_achievement.GroupName + _targetLevel); // Target name/desc/badge
        packet.WriteInteger(1); // Progress req/target
        packet.WriteInteger(_targetLevelData.Requirement); // Reward in Pixels
        packet.WriteInteger(_targetLevelData.RewardPixels); // Reward Ach Score
        packet.WriteInteger(0); // ?
        packet.WriteInteger(_userData?.Progress ?? 0); // Current progress
        packet.WriteBoolean(_userData != null && _userData.Level >= _totalLevels); // Set 100% completed(??)
        packet.WriteString(_achievement.Category); // Category
        packet.WriteString(string.Empty);
        packet.WriteInteger(_totalLevels); // Total amount of levels
        packet.WriteInteger(0);

    }
}