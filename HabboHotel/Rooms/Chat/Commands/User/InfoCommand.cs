using System;
using Plus.HabboHotel.GameClients;
using Plus.Communication.Packets.Outgoing.Rooms.Notifications;

namespace Plus.HabboHotel.Rooms.Chat.Commands.User
{
    class InfoCommand : IChatCommand
    {
        public string PermissionRequired
        {
            get { return "command_info"; }
        }

        public string Parameters
        {
            get { return ""; }
        }

        public string Description
        {
            get { return "Displays generic information that everybody loves to see."; }
        }

        public void Execute(GameClient session, Room room, string[] @params)
        {
            var uptime = DateTime.Now - PlusEnvironment.ServerStarted;
            var onlineUsers = PlusEnvironment.GetGame().GetClientManager().Count;
            var roomCount = PlusEnvironment.GetGame().GetRoomManager().Count;

            session.SendPacket(new RoomNotificationComposer("Powered by PlusEmulator",
                 "<b>Credits</b>:\n" +
                 "DevBest Community\n\n" +
                 "<b>Current run time information</b>:\n" +
                 "Online Users: " + onlineUsers + "\n" +
                 "Rooms Loaded: " + roomCount + "\n" +
                 "Uptime: " + uptime.Days + " day(s), " + uptime.Hours + " hours and " + uptime.Minutes + " minutes.\n\n" +
                 "<b>SWF Revision</b>:\n" + PlusEnvironment.SwfRevision, "plus", ""));
        }
    }
}