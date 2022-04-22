using System.Linq;
using System.Collections.Generic;
using System.Collections.Concurrent;

using Plus.Communication.Packets.Incoming;
using Plus.HabboHotel.Rooms;

namespace Plus.HabboHotel.Items.Wired.Boxes.Triggers
{
    class RepeaterBox : IWiredItem, IWiredCycle
    {
        public Room Instance { get; set; }
        public Item Item { get; set; }
        public WiredBoxType Type { get { return WiredBoxType.TriggerRepeat; } }
        public ConcurrentDictionary<int, Item> SetItems { get; set; }
        public string StringData { get; set; }
        public bool BoolData { get; set; }
        public int Delay { get { return _delay; } set { _delay = value; TickCount = value; } }
        public int TickCount { get; set; }
        public string ItemsData { get; set; }

        private int _delay = 0;

        public RepeaterBox(Room instance, Item item)
        {
            this.Instance = instance;
            this.Item = item;
            SetItems = new ConcurrentDictionary<int, Item>();
        }

        public void HandleSave(ClientPacket packet)
        {
            var unknown = packet.PopInt();
            var delay = packet.PopInt();

            this.Delay = delay;
            TickCount = delay;
        }

        public bool Execute(params object[] @params)
        {
            return true;
        }

        public bool OnCycle()
        {
            var success = false;
            ICollection<RoomUser> avatars = Instance.GetRoomUserManager().GetRoomUsers().ToList();
            var effects = Instance.GetWired().GetEffects(this);
            var conditions = Instance.GetWired().GetConditions(this);

            foreach (var condition in conditions.ToList())
            {
                foreach (var avatar in avatars.ToList())
                {
                    if (avatar == null || avatar.GetClient() == null || avatar.GetClient().GetHabbo() == null)
                        continue;

                    if (!condition.Execute(avatar.GetClient().GetHabbo()))
                        continue;

                    success = true;
                }

                if (!success)
                    return false;

                success = false;
                Instance.GetWired().OnEvent(condition.Item);
            }

            success = false;

            //Check the ICollection to find the random addon effect.
            var hasRandomEffectAddon = effects.Count(x => x.Type == WiredBoxType.AddonRandomEffect) > 0;
            if (hasRandomEffectAddon)
            {
                //Okay, so we have a random addon effect, now lets get the IWiredItem and attempt to execute it.
                var randomBox = effects.FirstOrDefault(x => x.Type == WiredBoxType.AddonRandomEffect);
                if (!randomBox.Execute())
                    return false;

                //Success! Let's get our selected box and continue.
                var selectedBox = Instance.GetWired().GetRandomEffect(effects.ToList());
                if (!selectedBox.Execute())
                    return false;

                //Woo! Almost there captain, now lets broadcast the update to the room instance.
                if (Instance != null)
                {
                    Instance.GetWired().OnEvent(randomBox.Item);
                    Instance.GetWired().OnEvent(selectedBox.Item);
                }
            }
            else
            {
                foreach (var effect in effects.ToList())
                {
                    if (!effect.Execute())
                        continue;

                    success = true;

                    if (!success)
                        return false;

                    if (Instance != null)
                        Instance.GetWired().OnEvent(effect.Item);
                }
            }

            TickCount = Delay;

            return true;
        }
    }
}