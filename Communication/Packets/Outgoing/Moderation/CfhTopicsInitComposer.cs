using Plus.HabboHotel.GameClients;
using Plus.HabboHotel.Moderation;

namespace Plus.Communication.Packets.Outgoing.Moderation;

internal class CfhTopicsInitComposer : IServerPacket
{
    private readonly Dictionary<string, List<ModerationPresetActions>> _userActionPresets;
    public int MessageId => ServerPacketHeader.CfhTopicsInitMessageComposer;

    public CfhTopicsInitComposer(Dictionary<string, List<ModerationPresetActions>> userActionPresets)
    {
        _userActionPresets = userActionPresets;
    }

    public void Compose(IOutgoingPacket packet)
    {
        packet.WriteInteger(_userActionPresets.Count);
        foreach (var cat in _userActionPresets.ToList())
        {
            packet.WriteString(cat.Key);
            packet.WriteInteger(cat.Value.Count);
            foreach (var preset in cat.Value.ToList())
            {
                packet.WriteString(preset.Caption);
                packet.WriteInteger(preset.Id);
                packet.WriteString(preset.Type);
            }
        }
    }
}