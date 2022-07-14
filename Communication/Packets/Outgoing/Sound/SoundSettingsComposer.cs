using Plus.HabboHotel.GameClients;

namespace Plus.Communication.Packets.Outgoing.Sound;

internal class SoundSettingsComposer : IServerPacket
{
    private readonly IEnumerable<int> _volumes;
    private readonly bool _chatPreference;
    private readonly bool _invitesStatus;
    private readonly bool _focusPreference;
    private readonly int _friendBarState;

    public int MessageId => ServerPacketHeader.SoundSettingsMessageComposer;

    public SoundSettingsComposer(IEnumerable<int> volumes, bool chatPreference, bool invitesStatus, bool focusPreference, int friendBarState)
    {
        _volumes = volumes;
        _chatPreference = chatPreference;
        _invitesStatus = invitesStatus;
        _focusPreference = focusPreference;
        _friendBarState = friendBarState;
    }

    public void Compose(IOutgoingPacket packet)
    {
        foreach (var volume in _volumes)
            packet.WriteInteger(volume);
        packet.WriteBoolean(_chatPreference);
        packet.WriteBoolean(_invitesStatus);
        packet.WriteBoolean(_focusPreference);
        packet.WriteInteger(_friendBarState);
        packet.WriteInteger(0);
        packet.WriteInteger(0);

    }
}