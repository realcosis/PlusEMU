using Plus.HabboHotel.GameClients;
using Plus.Communication.Packets.Outgoing.LandingView;

namespace Plus.Communication.Packets.Incoming.Quests
{
    class GetDailyQuestEvent : IPacketEvent
    {
        public void Parse(GameClient session, ClientPacket packet)
        {
            var usersOnline = PlusEnvironment.GetGame().GetClientManager().Count;

            session.SendPacket(new ConcurrentUsersGoalProgressComposer(usersOnline));
        }
    }
}
