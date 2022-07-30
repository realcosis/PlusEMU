using Plus.Database;
using Plus.HabboHotel.GameClients;

namespace Plus.HabboHotel.Rooms.Chat.Commands.User;

internal class DisableMimicCommand : IChatCommand
{
    private readonly IDatabase _database;
    public string Key => "disablemimic";
    public string PermissionRequired => "command_disable_mimic";

    public string Parameters => "";

    public string Description => "Allows you to disable the ability to be mimiced or to enable the ability to be mimiced.";

    public DisableMimicCommand(IDatabase database)
    {
        _database = database;
    }

    public void Execute(GameClient session, Room room, string[] parameters)
    {
        session.GetHabbo().AllowMimic = !session.GetHabbo().AllowMimic;
        session.SendWhisper("You're " + (session.GetHabbo().AllowMimic ? "now" : "no longer") + " able to be mimiced.");
        using var dbClient = _database.GetQueryReactor();
        dbClient.SetQuery("UPDATE `users` SET `allow_mimic` = @AllowMimic WHERE `id` = '" + session.GetHabbo().Id + "'");
        dbClient.AddParameter("AllowMimic", PlusEnvironment.BoolToEnum(session.GetHabbo().AllowMimic));
        dbClient.RunQuery();
    }
}