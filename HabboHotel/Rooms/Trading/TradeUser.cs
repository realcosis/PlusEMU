using System.Collections.Generic;

using Plus.HabboHotel.Items;

namespace Plus.HabboHotel.Rooms.Trading
{
    public sealed class TradeUser
    {
        private RoomUser _user;
        private bool _accepted;
        private Dictionary<int, Item> _offeredItems;

        public TradeUser(RoomUser user)
        {
            _user = user;
            _accepted = false;
            _offeredItems = new Dictionary<int, Item>();
        }

        public RoomUser RoomUser => _user;

        public bool HasAccepted
        {
            get => _accepted;
            set => _accepted = value;
        }

        public Dictionary<int, Item> OfferedItems
        {
            get => _offeredItems;
            set => _offeredItems = value;
        }
    }
}
