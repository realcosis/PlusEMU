using Plus.HabboHotel.GameClients;
using Plus.Utilities;

namespace Plus.Communication.Packets.Incoming.Users;

internal class SetUserFocusPreferenceEvent : IPacketEvent
{
    public void Parse(GameClient session, ClientPacket packet)
    {
        var focusPreference = packet.PopBoolean();
        session.GetHabbo().FocusPreference = focusPreference;
        using var dbClient = PlusEnvironment.GetDatabaseManager().GetQueryReactor();
        dbClient.SetQuery("UPDATE `users` SET `focus_preference` = @focusPreference WHERE `id` = @habboId LIMIT 1");
        dbClient.AddParameter("habboId", session.GetHabbo().Id);
        dbClient.AddParameter("focusPreference", ConvertExtensions.ToStringEnumValue(focusPreference));
        dbClient.RunQuery();
    }
}