using Plus.HabboHotel.GameClients;

namespace Plus.HabboHotel.Rooms.Chat.Commands.Moderator;

internal class AlertCommand : IChatCommand
{
    private readonly IGameClientManager _gameClientManager;
    public string Key => "alert";
    public string PermissionRequired => "command_alert_user";

    public string Parameters => "%username% %Messages%";

    public string Description => "Alert a user with a message of your choice.";

    public AlertCommand(IGameClientManager gameClientManager)
    {
        _gameClientManager = gameClientManager;
    }

    public void Execute(GameClient session, Room room, string[] @params)
    {
        if (@params.Length == 1)
        {
            session.SendWhisper("Please enter the username of the user you wish to alert.");
            return;
        }
        var targetClient = _gameClientManager.GetClientByUsername(@params[1]);
        if (targetClient == null)
        {
            session.SendWhisper("An error occoured whilst finding that user, maybe they're not online.");
            return;
        }
        if (targetClient.GetHabbo() == null)
        {
            session.SendWhisper("An error occoured whilst finding that user, maybe they're not online.");
            return;
        }
        if (targetClient.GetHabbo().Username == session.GetHabbo().Username)
        {
            session.SendWhisper("Get a life.");
            return;
        }
        var message = CommandManager.MergeParams(@params, 2);
        targetClient.SendNotification(session.GetHabbo().Username + " alerted you with the following message:\n\n" + message);
        session.SendWhisper("Alert successfully sent to " + targetClient.GetHabbo().Username);
    }
}