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

    public void Execute(GameClient session, Room room, string[] @params)
    {
        if (@params.Length != 3)
        {
            session.SendWhisper("Please enter a username and the code of the badge you'd like to give!");
            return;
        }
        var targetClient = _gameClientManager.GetClientByUsername(@params[1]);
        if (targetClient != null)
        {
            if (!targetClient.GetHabbo().Inventory.Badges.HasBadge(@params[2]))
            {
                _badgeManager.GiveBadge(targetClient.GetHabbo(), @params[2]).Wait();
                if (targetClient.GetHabbo().Id != session.GetHabbo().Id)
                    targetClient.SendNotification("You have just been given a badge!");
                else
                    session.SendWhisper("You have successfully given yourself the badge " + @params[2] + "!");
            }
            else
                session.SendWhisper("Oops, that user already has this badge (" + @params[2] + ") !");
            return;
        }
        session.SendWhisper("Oops, we couldn't find that target user!");
    }
}