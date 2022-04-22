using System;
using System.Linq;
using System.Collections.Generic;

using Plus.HabboHotel.Items;
using Plus.HabboHotel.Items.Wired;


namespace Plus.Communication.Packets.Outgoing.Rooms.Furni.Wired
{
    class WiredEffectConfigComposer : ServerPacket
    {
        public WiredEffectConfigComposer(IWiredItem box, List<int> blockedItems)
            : base(ServerPacketHeader.WiredEffectConfigMessageComposer)
        {
            WriteBoolean(false);
            WriteInteger(15);
          
            WriteInteger(box.SetItems.Count);
            foreach (Item item in box.SetItems.Values.ToList())
            {
                WriteInteger(item.Id);
            }

            WriteInteger(box.Item.GetBaseItem().SpriteId);
            WriteInteger(box.Item.Id);
           
            if(box.Type == WiredBoxType.EffectBotGivesHanditemBox)
            {
                if (String.IsNullOrEmpty(box.StringData))
                    box.StringData = "Bot name;0";

               WriteString(box.StringData != null ? (box.StringData.Split(';')[0]) : "");
            }
            else if (box.Type == WiredBoxType.EffectBotFollowsUserBox)
            {
                if (String.IsNullOrEmpty(box.StringData))
                    box.StringData = "0;Bot name";

               WriteString(box.StringData != null ? (box.StringData.Split(';')[1]) : "");
            }
            else
            {
               WriteString(box.StringData);
            }

            if (box.Type != WiredBoxType.EffectMatchPosition && box.Type != WiredBoxType.EffectMoveAndRotate && box.Type != WiredBoxType.EffectMuteTriggerer && box.Type != WiredBoxType.EffectBotFollowsUserBox)
                WriteInteger(0); // Loop
            else if (box.Type == WiredBoxType.EffectMatchPosition)
            {
                if (String.IsNullOrEmpty(box.StringData))
                    box.StringData = "0;0;0";

                WriteInteger(3);
                WriteInteger(box.StringData != null ? int.Parse(box.StringData.Split(';')[0]) : 0);
                WriteInteger(box.StringData != null ? int.Parse(box.StringData.Split(';')[1]) : 0);
                WriteInteger(box.StringData != null ? int.Parse(box.StringData.Split(';')[2]) : 0);
            }
            else if (box.Type == WiredBoxType.EffectMoveAndRotate)
            {
                if (String.IsNullOrEmpty(box.StringData))
                    box.StringData = "0;0";

                WriteInteger(2);
                WriteInteger(box.StringData != null ? int.Parse(box.StringData.Split(';')[0]) : 0);
                WriteInteger(box.StringData != null ? int.Parse(box.StringData.Split(';')[1]) : 0);
            }
            else if (box.Type == WiredBoxType.EffectMuteTriggerer)
            {
                if (String.IsNullOrEmpty(box.StringData))
                    box.StringData = "0;Message";

                WriteInteger(1);//Count, for the time.
                WriteInteger(box.StringData != null ? int.Parse(box.StringData.Split(';')[0]) : 0);
            }
            else if (box.Type == WiredBoxType.EffectBotFollowsUserBox)
            {
                WriteInteger(1);//Count, for the time.
                WriteInteger(box.StringData != null ? int.Parse(box.StringData.Split(';')[0]) : 0);
            }
            else if(box.Type == WiredBoxType.EffectBotGivesHanditemBox)
            {
                WriteInteger(box.StringData != null ? int.Parse(box.StringData.Split(';')[1]) : 0);
            }

            if (box is IWiredCycle && box.Type != WiredBoxType.EffectKickUser && box.Type != WiredBoxType.EffectMatchPosition && box.Type != WiredBoxType.EffectMoveAndRotate && box.Type != WiredBoxType.EffectSetRollerSpeed)
            {
                IWiredCycle cycle = (IWiredCycle)box;
                WriteInteger(WiredBoxTypeUtility.GetWiredId(box.Type));
                WriteInteger(0);
                WriteInteger(cycle.Delay);
            }
            else if (box.Type == WiredBoxType.EffectMatchPosition || box.Type == WiredBoxType.EffectMoveAndRotate)
            {
                IWiredCycle cycle = (IWiredCycle)box;
                WriteInteger(0);
                WriteInteger(WiredBoxTypeUtility.GetWiredId(box.Type));
                WriteInteger(cycle.Delay);
            }
            else
            {
                WriteInteger(0);
                WriteInteger(WiredBoxTypeUtility.GetWiredId(box.Type));
                WriteInteger(0);
            }

            WriteInteger(blockedItems.Count()); // Incompatable items loop
            if (blockedItems.Count() > 0)
            {
                foreach (int itemId in blockedItems.ToList())
                    WriteInteger(itemId);
            }
        }
    }
}