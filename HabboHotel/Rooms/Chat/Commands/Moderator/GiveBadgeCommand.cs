using Plus.HabboHotel.Badges;
using Plus.HabboHotel.GameClients;

namespace Plus.HabboHotel.Rooms.Chat.Commands.Moderator;

internal class GiveBadgeCommand : IChatCommand
{
    private readonly IGameClientManager _gameClientManager;
    private readonly IBadgeManager _badgeManager;
    public string Key => "givebadge";
    public string PermissionRequired => "command_give_badge";

    public string Parameters => "%username% %badge%";

    public string Description => "Give a badge to another user.";

    public GiveBadgeCommand(IGameClientManager gameClientManager, IBadgeManager badgeManager)
    {
        _gameClientManager = gameClientManager;
        _badgeManager = badgeManager;
    }

    public void Execute(GameClient session, Room room, string[] parameters)
    {
        if (parameters.Length != 3)
        {
            session.SendWhisper("Please enter a username and the code of the badge you'd like to give!");
            return;
        }
        var targetClient = _gameClientManager.GetClientByUsername(parameters[1]);
        if (targetClient != null)
        {
            if (!targetClient.GetHabbo().Inventory.Badges.HasBadge(parameters[2]))
            {
                _badgeManager.GiveBadge(targetClient.GetHabbo(), parameters[2]).Wait();
                if (targetClient.GetHabbo().Id != session.GetHabbo().Id)
                    targetClient.SendNotification("You have just been given a badge!");
                else
                    session.SendWhisper("You have successfully given yourself the badge " + parameters[2] + "!");
            }
            else
                session.SendWhisper("Oops, that user already has this badge (" + parameters[2] + ") !");
            return;
        }
        session.SendWhisper("Oops, we couldn't find that target user!");
    }
}