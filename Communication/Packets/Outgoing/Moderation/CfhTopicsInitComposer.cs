using System.Linq;
using System.Collections.Generic;
using Plus.HabboHotel.Moderation;

namespace Plus.Communication.Packets.Outgoing.Moderation
{
    class CfhTopicsInitComposer : ServerPacket
    {
        public CfhTopicsInitComposer(Dictionary<string, List<ModerationPresetActions>> userActionPresets)
            : base(ServerPacketHeader.CfhTopicsInitMessageComposer)
        {

            WriteInteger(userActionPresets.Count);
            foreach (KeyValuePair<string, List<ModerationPresetActions>> cat in userActionPresets.ToList())
            {
                WriteString(cat.Key);
                WriteInteger(cat.Value.Count);
                foreach (ModerationPresetActions preset in cat.Value.ToList())
                {
                    WriteString(preset.Caption);
                    WriteInteger(preset.Id);
                    WriteString(preset.Type);
                }
            }
        }
    }
}
