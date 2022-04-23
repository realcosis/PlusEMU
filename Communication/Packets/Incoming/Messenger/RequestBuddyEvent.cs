using Plus.HabboHotel.GameClients;
using Plus.HabboHotel.Quests;

namespace Plus.Communication.Packets.Incoming.Messenger;

internal class RequestBuddyEvent : IPacketEvent
{
    private readonly IQuestManager _questManager;

    public RequestBuddyEvent(IQuestManager questManager)
    {
        _questManager = questManager;
    }

    public void Parse(GameClient session, ClientPacket packet)
    {
        if (session == null || session.GetHabbo() == null || session.GetHabbo().GetMessenger() == null)
            return;
        if (session.GetHabbo().GetMessenger().RequestBuddy(packet.PopString()))
            _questManager.ProgressUserQuest(session, QuestType.SocialFriend);
    }
}