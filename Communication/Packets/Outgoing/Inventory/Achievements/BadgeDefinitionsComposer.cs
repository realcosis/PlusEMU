using System.Collections.Generic;
using Plus.HabboHotel.Achievements;

namespace Plus.Communication.Packets.Outgoing.Inventory.Achievements
{
    class BadgeDefinitionsComposer: ServerPacket
    {
        public BadgeDefinitionsComposer(Dictionary<string, Achievement> achievements)
            : base(ServerPacketHeader.BadgeDefinitionsMessageComposer)
        {
            WriteInteger(achievements.Count);

            foreach (Achievement achievement in achievements.Values)
            {
               WriteString(achievement.GroupName.Replace("ACH_", ""));
                WriteInteger(achievement.Levels.Count);
                foreach (AchievementLevel level in achievement.Levels.Values)
                {
                    WriteInteger(level.Level);
                    WriteInteger(level.Requirement);
                }
            }
        }
    }
}
