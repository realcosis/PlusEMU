using System;
using System.Collections.Concurrent;

using Plus.HabboHotel.Rooms;
using Plus.HabboHotel.Users;
using Plus.Communication.Packets.Incoming;
using Plus.HabboHotel.Rooms.Games.Teams;

namespace Plus.HabboHotel.Items.Wired.Boxes.Conditions
{
    class ActorIsInTeamBox : IWiredItem
    {
        public Room Instance { get; set; }
        public Item Item { get; set; }
        public WiredBoxType Type { get { return WiredBoxType.ConditionActorIsInTeamBox; } }
        public ConcurrentDictionary<int, Item> SetItems { get; set; }
        public string StringData { get; set; }
        public bool BoolData { get; set; }
        public string ItemsData { get; set; }

        public ActorIsInTeamBox(Room instance, Item item)
        {
            this.Instance = instance;
            this.Item = item;

            SetItems = new ConcurrentDictionary<int, Item>();
        }

        public void HandleSave(ClientPacket packet)
        {
            var unknown = packet.PopInt();
            var unknown2 = packet.PopInt();

            StringData = unknown2.ToString();
        }

        public bool Execute(params object[] @params)
        {
            if (@params.Length == 0 || Instance == null || String.IsNullOrEmpty(StringData))
                return false;

            var player = (Habbo)@params[0];
            if (player == null)
                return false;

            var user = Instance.GetRoomUserManager().GetRoomUserByHabbo(player.Id);
            if (user == null)
                return false;

            if (int.Parse(StringData) == 1 && user.Team == Team.Red)
                return true;
            else if (int.Parse(StringData) == 2 && user.Team == Team.Green)
                return true;
            else if (int.Parse(StringData) == 3 && user.Team == Team.Blue)
                return true;
            else if (int.Parse(StringData) == 4 && user.Team == Team.Yellow)
                return true;
            return false;
        }
    }
}