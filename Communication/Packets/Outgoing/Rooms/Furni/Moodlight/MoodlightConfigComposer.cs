using Plus.HabboHotel.GameClients;
using Plus.HabboHotel.Items.Data.Moodlight;

namespace Plus.Communication.Packets.Outgoing.Rooms.Furni.Moodlight;

public class MoodlightConfigComposer : IServerPacket
{
    private readonly MoodlightData _moodlightData;

    public uint MessageId => ServerPacketHeader.MoodlightConfigComposer;

    public MoodlightConfigComposer(MoodlightData moodlightData)
    {
        _moodlightData = moodlightData;
    }

    public void Compose(IOutgoingPacket packet)
    {
        packet.WriteInteger(_moodlightData.Presets.Count);
        packet.WriteInteger(_moodlightData.CurrentPreset);
        var i = 1;
        foreach (var preset in _moodlightData.Presets)
        {
            packet.WriteInteger(i);
            packet.WriteInteger(preset.BackgroundOnly ? 2 : 1);
            packet.WriteString(preset.ColorCode);
            packet.WriteInteger(preset.ColorIntensity);
            i++;
        }
    }
}