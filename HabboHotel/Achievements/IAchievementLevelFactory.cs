namespace Plus.HabboHotel.Achievements;

public interface IAchievementLevelFactory
{
    Dictionary<string, Achievement> GetAchievementLevels();
}