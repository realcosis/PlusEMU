using Plus.HabboHotel.GameClients;

namespace Plus.HabboHotel.Rooms.Chat.Commands.Moderator.Fun;

internal class UnFreezeCommand : IChatCommand
{
    private readonly IGameClientManager _gameClientManager;
    public string Key => "unfreeze";
    public string PermissionRequired => "command_unfreeze";

    public string Parameters => "%username%";

    public string Description => "Allow another user to walk again.";

    public UnFreezeCommand(IGameClientManager gameClientManager)
    {
        _gameClientManager = gameClientManager;
    }

    public void Execute(GameClient session, Room room, string[] parameters)
    {
        if (parameters.Length == 1)
        {
            session.SendWhisper("Please enter the username of the user you wish to un-freeze.");
            return;
        }
        var targetClient = _gameClientManager.GetClientByUsername(parameters[1]);
        if (targetClient == null)
        {
            session.SendWhisper("An error occoured whilst finding that user, maybe they're not online.");
            return;
        }
        var targetUser = session.GetHabbo().CurrentRoom.GetRoomUserManager().GetRoomUserByHabbo(parameters[1]);
        if (targetUser != null)
            targetUser.Frozen = false;
        session.SendWhisper("Successfully unfroze " + targetClient.GetHabbo().Username + "!");
    }
}