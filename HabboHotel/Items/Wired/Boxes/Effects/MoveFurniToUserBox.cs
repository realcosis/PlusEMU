using System.Linq;
using System.Collections.Concurrent;

using Plus.Communication.Packets.Incoming;
using Plus.HabboHotel.Rooms;
using Plus.Communication.Packets.Outgoing.Rooms.Engine;


namespace Plus.HabboHotel.Items.Wired.Boxes.Effects
{
    internal class MoveFurniToUserBox : IWiredItem, IWiredCycle
    {
        public Room Instance { get; set; }
        public Item Item { get; set; }

        public WiredBoxType Type => WiredBoxType.EffectMoveFurniToNearestUser;

        public ConcurrentDictionary<int, Item> SetItems { get; set; }
        public string StringData { get; set; }
        public bool BoolData { get; set; }

        public int Delay
        {
            get => _delay;
            set
            {
                _delay = value;
                TickCount = value + 1;
            }
        }

        public int TickCount { get; set; }
        public string ItemsData { get; set; }
        private bool _requested;
        private int _delay = 0;
        private long _next = 0;

        public MoveFurniToUserBox(Room instance, Item item)
        {
            this.Instance = instance;
            this.Item = item;
            SetItems = new ConcurrentDictionary<int, Item>();
            TickCount = Delay;
            _requested = false;
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

                if (selectedItem != null && !Instance.GetWired().OtherBoxHasItem(this, selectedItem.Id))
                    SetItems.TryAdd(selectedItem.Id, selectedItem);
            }

            var delay = packet.PopInt();
            this.Delay = delay;
        }

        public bool Execute(params object[] @params)
        {
            if (SetItems.Count == 0)
                return false;


            if (_next == 0 || _next < PlusEnvironment.Now())
                _next = PlusEnvironment.Now() + Delay;

            if (!_requested)
            {
                TickCount = Delay;
                _requested = true;
            }
            return true;
        }

        public bool OnCycle()
        {
            if (Instance == null || !_requested || _next == 0)
                return false;

            var now = PlusEnvironment.Now();
            if (_next < now)
            {
                foreach (var item in SetItems.Values.ToList())
                {
                    if (item == null)
                        continue;

                    if (!Instance.GetRoomItemHandler().GetFloor.Contains(item))
                        continue;

                    Item toRemove = null;

                    if (Instance.GetWired().OtherBoxHasItem(this, item.Id))
                        SetItems.TryRemove(item.Id, out toRemove);

                    var point = Instance.GetGameMap().GetChaseMovement(item);

                    Instance.GetWired().OnUserFurniCollision(Instance, item);

                    if (!Instance.GetGameMap().ItemCanMove(item, point))
                        continue;

                    if (Instance.GetGameMap().CanRollItemHere(point.X, point.Y) && !Instance.GetGameMap().SquareHasUsers(point.X, point.Y))
                    {    
                        var newZ = item.GetZ;
                        var canBePlaced = true;

                        var coordinatedItems = Instance.GetGameMap().GetCoordinatedItems(point);
                        foreach (var coordinateItem in coordinatedItems.ToList())
                        {
                            if (coordinateItem == null || coordinateItem.Id == item.Id)
                                continue;

                            if (!coordinateItem.GetBaseItem().Walkable)
                            {
                                _next = 0;
                                canBePlaced = false;
                                break;
                            }

                            if (coordinateItem.TotalHeight > newZ)
                                newZ = coordinateItem.TotalHeight;

                            if (canBePlaced == true && !coordinateItem.GetBaseItem().Stackable)
                                canBePlaced = false;
                        }

                        if (canBePlaced && point != item.Coordinate)
                        {
                            Instance.SendPacket(new SlideObjectBundleComposer(item.GetX, item.GetY, item.GetZ, point.X,
                                point.Y, newZ, 0, 0, item.Id));
                            Instance.GetRoomItemHandler().SetFloorItem(item, point.X, point.Y, newZ);
                        }
                    }
                }

                _next = 0;
                return true;
            }
            return false;
        }
    }
}