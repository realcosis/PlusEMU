using Plus.HabboHotel.Quests;
using Plus.HabboHotel.GameClients;

namespace Plus.Communication.Packets.Outgoing.Quests
{
    class QuestCompletedComposer : ServerPacket
    {
        public QuestCompletedComposer(GameClient session, Quest quest)
            : base(ServerPacketHeader.QuestCompletedMessageComposer)
        {
            var amountInCat = PlusEnvironment.GetGame().GetQuestManager().GetAmountOfQuestsInCategory(quest.Category);
            var number = quest == null ? amountInCat : quest.Number;
            var userProgress = quest == null ? 0 : session.GetHabbo().GetQuestProgress(quest.Id);

           WriteString(quest.Category);
            WriteInteger(number); // Quest progress in this cat
            WriteInteger((quest.Name.Contains("xmas2012")) ? 1 : amountInCat); // Total quests in this cat
            WriteInteger(quest == null ? 3 : quest.RewardType); // Reward type (1 = Snowflakes, 2 = Love hearts, 3 = Pixels, 4 = Seashells, everything else is pixels
            WriteInteger(quest == null ? 0 : quest.Id); // Quest id
            WriteBoolean(quest == null ? false : session.GetHabbo().GetStats().QuestId == quest.Id); // Quest started
           WriteString(quest == null ? string.Empty : quest.ActionName);
           WriteString(quest == null ? string.Empty : quest.DataBit);
            WriteInteger(quest == null ? 0 : quest.Reward);
           WriteString(quest == null ? string.Empty : quest.Name);
            WriteInteger(userProgress); // Current progress
            WriteInteger(quest == null ? 0 : quest.GoalData); // Target progress
            WriteInteger(quest == null ? 0 : quest.TimeUnlock); // "Next quest available countdown" in seconds
           WriteString("");
           WriteString("");
            WriteBoolean(true); // ?
            WriteBoolean(true); // Activate next quest..
        }
    }
}
