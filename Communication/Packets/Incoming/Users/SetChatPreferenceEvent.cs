using Plus.HabboHotel.GameClients;
using Plus.Database.Interfaces;

namespace Plus.Communication.Packets.Incoming.Users
{
    class SetChatPreferenceEvent : IPacketEvent
    {
        public void Parse(GameClient session, ClientPacket packet)
        {
            var preference = packet.PopBoolean();

            session.GetHabbo().ChatPreference = preference;
            using var dbClient = PlusEnvironment.GetDatabaseManager().GetQueryReactor();
            dbClient.SetQuery("UPDATE `users` SET `chat_preference` = @chatPreference WHERE `id` = '" + session.GetHabbo().Id + "' LIMIT 1");
            dbClient.AddParameter("chatPreference", PlusEnvironment.BoolToEnum(preference));
            dbClient.RunQuery();
        }
    }
}
