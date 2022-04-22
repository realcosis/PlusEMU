using System.Linq;
using System.Collections.Concurrent;

using Plus.Communication.Packets.Incoming;
using Plus.HabboHotel.Users;
using Plus.HabboHotel.Rooms;

namespace Plus.HabboHotel.Items.Wired.Boxes.Conditions
{
    class TriggererNotOnFurniBox : IWiredItem
    {
        public Room Instance { get; set; }
        public Item Item { get; set; }
        public WiredBoxType Type => WiredBoxType.ConditionTriggererNotOnFurni;
        public ConcurrentDictionary<int, Item> SetItems { get; set; }
        public string StringData { get; set; }
        public bool BoolData { get; set; }
        public string ItemsData { get; set; }

        public TriggererNotOnFurniBox(Room instance, Item item)
        {
            this.Instance = instance;
            this.Item = item;
            SetItems = new ConcurrentDictionary<int, Item>();
        }

        public void HandleSave(ClientPacket packet)
        {
            var unknown = packet.PopInt();
            var unknown2 = packet.PopString();

            if (SetItems.Count > 0)
                SetItems.Clear();

            var furniCount = packet.PopInt();
            for (var i = 0; i < furniCount; i++)
            {
                var selectedItem = Instance.GetRoomItemHandler().GetItem(packet.PopInt());
                if (selectedItem != null)
                    SetItems.TryAdd(selectedItem.Id, selectedItem);
            }
        }

        public bool Execute(params object[] @params)
        {
            if (@params.Length == 0)
                return false;

            var player = (Habbo)@params[0];
            if (player == null)
                return false;

            if (player.CurrentRoom == null)
                return false;

            var user = player.CurrentRoom.GetRoomUserManager().GetRoomUserByHabbo(player.Username);
            if (user == null)
                return false;

            var itemsOnSquare = Instance.GetGameMap().GetAllRoomItemForSquare(user.X, user.Y);
            foreach (var item in itemsOnSquare.ToList())
            {
                if (SetItems.ContainsKey(item.Id))
                    return false;
                else continue;
            }

            return true;
        }
    }
}