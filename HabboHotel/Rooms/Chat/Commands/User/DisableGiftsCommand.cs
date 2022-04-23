using Plus.HabboHotel.GameClients;
using Plus.Utilities;

namespace Plus.HabboHotel.Rooms.Chat.Commands.User;

internal class DisableGiftsCommand : IChatCommand
{
    public string PermissionRequired => "command_disable_gifts";

    public string Parameters => "";

    public string Description => "Allows you to disable the ability to receive gifts or to enable the ability to receive gifts.";

    public void Execute(GameClient session, Room room, string[] @params)
    {
        session.GetHabbo().AllowGifts = !session.GetHabbo().AllowGifts;
        session.SendWhisper("You're " + (session.GetHabbo().AllowGifts ? "now" : "no longer") + " accepting gifts.");
        using var dbClient = PlusEnvironment.GetDatabaseManager().GetQueryReactor();
        dbClient.SetQuery("UPDATE `users` SET `allow_gifts` = @allowGifts WHERE `id` = @habboId'");
        dbClient.AddParameter("habboId", session.GetHabbo().Id);
        dbClient.AddParameter("allowGifts", ConvertExtensions.ToStringEnumValue(session.GetHabbo().AllowGifts));
        dbClient.RunQuery();
    }
}