using Plus.HabboHotel.Items.Data.Moodlight;

namespace Plus.Communication.Packets.Outgoing.Rooms.Furni.Moodlight
{
    class MoodlightConfigComposer : ServerPacket
    {
        public MoodlightConfigComposer(MoodlightData moodlightData)
            : base(ServerPacketHeader.MoodlightConfigMessageComposer)
        {
            WriteInteger(moodlightData.Presets.Count);
            WriteInteger(moodlightData.CurrentPreset);

            int i = 1;
            foreach (MoodlightPreset preset in moodlightData.Presets)
            {
                WriteInteger(i);
                WriteInteger(preset.BackgroundOnly ? 2 : 1);
               WriteString(preset.ColorCode);
                WriteInteger(preset.ColorIntensity);
                i++;
            }
        }
    }
}