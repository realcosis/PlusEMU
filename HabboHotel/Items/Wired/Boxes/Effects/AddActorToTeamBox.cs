using System;
using System.Collections.Concurrent;

using Plus.HabboHotel.Rooms;
using Plus.HabboHotel.Users;
using Plus.HabboHotel.Rooms.Games.Teams;
using Plus.Communication.Packets.Incoming;

namespace Plus.HabboHotel.Items.Wired.Boxes.Effects
{
    class AddActorToTeamBox : IWiredItem
    {
        public Room Instance { get; set; }
        public Item Item { get; set; }
        public WiredBoxType Type { get { return WiredBoxType.EffectAddActorToTeam; } }
        public ConcurrentDictionary<int, Item> SetItems { get; set; }
        public string StringData { get; set; }
        public bool BoolData { get; set; }
        public string ItemsData { get; set; }

        public AddActorToTeamBox(Room instance, Item item)
        {
            this.Instance = instance;
            this.Item = item;

            SetItems = new ConcurrentDictionary<int, Item>();
        }

        public void HandleSave(ClientPacket packet)
        {
            int unknown = packet.PopInt();
            int team = packet.PopInt();

            StringData = team.ToString();
        }

        public bool Execute(params object[] @params)
        {
            if (@params.Length == 0 || Instance == null || String.IsNullOrEmpty(StringData))
                return false;

            Habbo player = (Habbo)@params[0];
            if (player == null)
                return false;

            RoomUser user = Instance.GetRoomUserManager().GetRoomUserByHabbo(player.Id);
            if (user == null)
                return false;

            Team toJoin = (int.Parse(StringData) == 1 ? Rooms.Games.Teams.Team.Red : int.Parse(StringData) == 2 ? Rooms.Games.Teams.Team.Green : int.Parse(StringData) == 3 ? Rooms.Games.Teams.Team.Blue : int.Parse(StringData) == 4 ? Rooms.Games.Teams.Team.Yellow : Rooms.Games.Teams.Team.None);

            TeamManager team = Instance.GetTeamManagerForFreeze();
            if (team != null)
            {
                if (team.CanEnterOnTeam(toJoin))
                {
                    if (user.Team != Rooms.Games.Teams.Team.None)
                        team.OnUserLeave(user);

                    user.Team = toJoin;
                    team.AddUser(user);

                    if (user.GetClient().GetHabbo().Effects().CurrentEffect != Convert.ToInt32(toJoin + 39))
                        user.GetClient().GetHabbo().Effects().ApplyEffect(Convert.ToInt32(toJoin + 39));
                }
            }
            return true;
        }
    }
}