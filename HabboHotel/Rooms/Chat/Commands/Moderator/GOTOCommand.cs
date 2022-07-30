using Plus.HabboHotel.GameClients;

namespace Plus.HabboHotel.Rooms.Chat.Commands.Moderator;

internal class GotoCommand : IChatCommand
{
    public string Key => "goto";
    public string PermissionRequired => "command_goto";

    public string Parameters => "%room_id%";

    public string Description => "";

    public void Execute(GameClient session, Room room, string[] parameters)
    {
        if (parameters.Length == 1)
        {
            session.SendWhisper("You must specify a room id!");
            return;
        }
        if (!int.TryParse(parameters[1], out var roomId))
            session.SendWhisper("You must enter a valid room ID");
        else
        {
            RoomData data = null;
            if (!RoomFactory.TryGetData(roomId, out data))
            {
                session.SendWhisper("This room does not exist!");
                return;
            }
            session.GetHabbo().PrepareRoom(roomId, "");
        }
    }
}