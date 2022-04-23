using Plus.Communication.Packets.Outgoing.Moderation;
using Plus.HabboHotel.GameClients;

namespace Plus.HabboHotel.Rooms.Chat.Commands.Moderator;

internal class StaffAlertCommand : IChatCommand
{
    private readonly IGameClientManager _gameClientManager;
    public string Key => "sa";
    public string PermissionRequired => "command_staff_alert";

    public string Parameters => "%message%";

    public string Description => "Sends a message typed by you to the current online staff members.";

    public StaffAlertCommand(IGameClientManager gameClientManager)
    {
        _gameClientManager = gameClientManager;
    }

    public void Execute(GameClient session, Room room, string[] @params)
    {
        if (@params.Length == 1)
        {
            session.SendWhisper("Please enter a message to send.");
            return;
        }
        var message = CommandManager.MergeParams(@params, 1);
        _gameClientManager.StaffAlert(new BroadcastMessageAlertComposer("Staff Alert:\r\r" + message + "\r\n" + "- " + session.GetHabbo().Username));
    }
}