using System.Linq;
using Plus.Communication.Packets.Outgoing.Inventory.Furni;

namespace Plus.HabboHotel.Rooms.Chat.Commands.User
{
    class PickAllCommand : IChatCommand
    {
        public string PermissionRequired => "command_pickall";

        public string Parameters => "";

        public string Description => "Picks up all of the furniture from your room.";

        public void Execute(GameClients.GameClient session, Room room, string[] @params)
        {
            if (!room.CheckRights(session, true))
                return;

            room.GetRoomItemHandler().RemoveItems(session);
            room.GetGameMap().GenerateMaps();

            using (var dbClient = PlusEnvironment.GetDatabaseManager().GetQueryReactor())
            {
                dbClient.SetQuery("UPDATE `items` SET `room_id` = '0' WHERE `room_id` = @RoomId AND `user_id` = @UserId");
                dbClient.AddParameter("RoomId", room.Id);
                dbClient.AddParameter("UserId", session.GetHabbo().Id);
                dbClient.RunQuery();
            }

            var items = room.GetRoomItemHandler().GetWallAndFloor.ToList();
            if (items.Count > 0)
                session.SendWhisper("There are still more items in this room, manually remove them or use :ejectall to eject them!");

            session.SendPacket(new FurniListUpdateComposer());
        }
    }
}