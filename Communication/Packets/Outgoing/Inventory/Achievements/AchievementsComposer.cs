using System.Collections.Generic;

using Plus.HabboHotel.GameClients;
using Plus.HabboHotel.Achievements;

namespace Plus.Communication.Packets.Outgoing.Inventory.Achievements
{
    class AchievementsComposer : ServerPacket
    {
        public AchievementsComposer(GameClient session, List<Achievement> achievements)
            : base(ServerPacketHeader.AchievementsMessageComposer)
        {
            WriteInteger(achievements.Count);
            foreach (Achievement achievement in achievements)
            {
                UserAchievement userData = session.GetHabbo().GetAchievementData(achievement.GroupName);
                int targetLevel = (userData != null ? userData.Level + 1 : 1);
                int totalLevels = achievement.Levels.Count;

                targetLevel = (targetLevel > totalLevels ? totalLevels : targetLevel);

                AchievementLevel targetLevelData = achievement.Levels[targetLevel];
                WriteInteger(achievement.Id); // Unknown (ID?)
                WriteInteger(targetLevel); // Target level
               WriteString(achievement.GroupName + targetLevel); // Target name/desc/badge

                WriteInteger(1);
                WriteInteger(targetLevelData.Requirement); // Progress req/target          
                WriteInteger(targetLevelData.RewardPixels);

                WriteInteger(0); // Type of reward
                WriteInteger(userData != null ? userData.Progress : 0); // Current progress
                
                WriteBoolean(userData != null ? (userData.Level >= totalLevels) : false);// Set 100% completed(??)
               WriteString(achievement.Category); // Category
               WriteString(string.Empty);
                WriteInteger(totalLevels); // Total amount of levels 
                WriteInteger(0);
            }
           WriteString("");
        }
    }
}