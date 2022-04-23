using Plus.HabboHotel.GameClients;
using Plus.Utilities;

namespace Plus.Communication.Packets.Incoming.Users;

internal class SetChatPreferenceEvent : IPacketEvent
{
    public void Parse(GameClient session, ClientPacket packet)
    {
        var chatPreference = packet.PopBoolean();
        session.GetHabbo().ChatPreference = chatPreference;
        using var dbClient = PlusEnvironment.GetDatabaseManager().GetQueryReactor();
        dbClient.SetQuery("UPDATE `users` SET `chat_preference` = @chatPreference WHERE `id` = @habboId LIMIT 1");
        dbClient.AddParameter("habboId", session.GetHabbo().Id);
        dbClient.AddParameter("chatPreference", ConvertExtensions.ToStringEnumValue(chatPreference));
        dbClient.RunQuery();
    }
}