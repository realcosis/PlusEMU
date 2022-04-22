using Plus.Communication.Packets.Outgoing.Moderation;

namespace Plus.HabboHotel.Rooms.Chat.Commands.Moderator
{
    class HotelAlertCommand : IChatCommand
    {
        public string PermissionRequired
        {
            get { return "command_hotel_alert"; }
        }

        public string Parameters
        {
            get { return "%message%"; }
        }

        public string Description
        {
            get { return "Send a message to the entire hotel."; }
        }

        public void Execute(GameClients.GameClient session, Room room, string[] @params)
        {
            if (@params.Length == 1)
            {
                session.SendWhisper("Please enter a message to send.");
                return;
            }

            string message = CommandManager.MergeParams(@params, 1);
            PlusEnvironment.GetGame().GetClientManager().SendPacket(new BroadcastMessageAlertComposer(message + "\r\n" + "- " + session.GetHabbo().Username));
                            
            return;
        }
    }
}
