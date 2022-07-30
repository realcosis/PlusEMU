using Plus.Database;
using Plus.HabboHotel.GameClients;

namespace Plus.HabboHotel.Rooms.Chat.Commands.Moderator;

internal class UnmuteCommand : IChatCommand
{
    private readonly IGameClientManager _gameClientManager;
    private readonly IDatabase _database;
    public string Key => "unmute";
    public string PermissionRequired => "command_unmute";

    public string Parameters => "%username%";

    public string Description => "Unmute a currently muted user.";

    public UnmuteCommand(IGameClientManager gameClientManager, IDatabase database)
    {
        _gameClientManager = gameClientManager;
        _database = database;
    }

    public void Execute(GameClient session, Room room, string[] parameters)
    {
        if (parameters.Length == 1)
        {
            session.SendWhisper("Please enter the username of the user you would like to unmute.");
            return;
        }
        var targetClient = _gameClientManager.GetClientByUsername(parameters[1]);
        if (targetClient == null || targetClient.GetHabbo() == null)
        {
            session.SendWhisper("An error occoured whilst finding that user, maybe they're not online.");
            return;
        }
        using (var dbClient = _database.GetQueryReactor())
        {
            dbClient.RunQuery("UPDATE `users` SET `time_muted` = '0' WHERE `id` = '" + targetClient.GetHabbo().Id + "' LIMIT 1");
        }
        targetClient.GetHabbo().TimeMuted = 0;
        targetClient.SendNotification("You have been un-muted by " + session.GetHabbo().Username + "!");
        session.SendWhisper("You have successfully un-muted " + targetClient.GetHabbo().Username + "!");
    }
}