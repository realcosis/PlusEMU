using Plus.Database;
using Plus.HabboHotel.GameClients;

namespace Plus.HabboHotel.Rooms.Chat.Commands.Moderator;

internal class DisableForcedFxCommand : IChatCommand
{
    private readonly IDatabase _database;
    public string Key => "forced_effects";
    public string PermissionRequired => "command_forced_effects";

    public string Parameters => "";

    public string Description => "Gives you the ability to ignore or allow forced effects.";

    public DisableForcedFxCommand(IDatabase database)
    {
        _database = database;
    }

    public void Execute(GameClient session, Room room, string[] @params)
    {
        session.GetHabbo().DisableForcedEffects = !session.GetHabbo().DisableForcedEffects;
        using (var dbClient = _database.GetQueryReactor())
        {
            dbClient.SetQuery("UPDATE `users` SET `disable_forced_effects` = @DisableForcedEffects WHERE `id` = '" + session.GetHabbo().Id + "' LIMIT 1");
            dbClient.AddParameter("DisableForcedEffects", (session.GetHabbo().DisableForcedEffects ? 1 : 0).ToString());
            dbClient.RunQuery();
        }
        session.SendWhisper("Forced FX mode is now " + (session.GetHabbo().DisableForcedEffects ? "disabled!" : "enabled!"));
    }
}