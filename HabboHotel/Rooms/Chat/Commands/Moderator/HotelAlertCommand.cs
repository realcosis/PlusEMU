using Plus.Communication.Packets.Outgoing.Moderation;
using Plus.HabboHotel.GameClients;

namespace Plus.HabboHotel.Rooms.Chat.Commands.Moderator;

internal class HotelAlertCommand : IChatCommand
{
    private readonly IGameClientManager _gameClientManager;
    public string Key => "ha";
    public string PermissionRequired => "command_hotel_alert";

    public string Parameters => "%message%";

    public string Description => "Send a message to the entire hotel.";

    public HotelAlertCommand(IGameClientManager gameClientManager)
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
        _gameClientManager.SendPacket(new BroadcastMessageAlertComposer(message + "\r\n" + "- " + session.GetHabbo().Username));
    }
}