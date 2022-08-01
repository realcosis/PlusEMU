using Plus.Communication.Packets.Outgoing.Rooms.Notifications;
using Plus.HabboHotel.GameClients;

namespace Plus.HabboHotel.Rooms.Chat.Commands.User;

internal class InfoCommand : IChatCommand
{
    private readonly IGameClientManager _gameClientManager;
    private readonly IRoomManager _roomManager;
    public string Key => "about";
    public string PermissionRequired => "command_info";

    public string Parameters => "";

    public string Description => "Displays generic information that everybody loves to see.";

    public InfoCommand(IGameClientManager gameClientManager, IRoomManager roomManager)
    {
        _gameClientManager = gameClientManager;
        _roomManager = roomManager;
    }
    public void Execute(GameClient session, Room room, string[] parameters)
    {
        var uptime = DateTime.Now - PlusEnvironment.ServerStarted;
        var onlineUsers = _gameClientManager.Count;
        var roomCount = _roomManager.Count;
        session.Send(new RoomNotificationComposer("Powered by Plus++ Emulator",
            "<b>Created by the Habbo Hotel Community</b>\n\n" +
            "<b>Current run time information</b>:\n" +
            "Online Users: " + onlineUsers + "\n" +
            "Rooms Loaded: " + roomCount + "\n" +
            "Uptime: " + uptime.Days + " day(s), " + uptime.Hours + " hours and " + uptime.Minutes + " minutes.\n\n", "plus", "View on github >", "https://github.com/80O/PlusEmu"));
    }
}