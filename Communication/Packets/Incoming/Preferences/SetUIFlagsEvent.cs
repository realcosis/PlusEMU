using Plus.Communication.Packets.Outgoing.Sound;
using Plus.HabboHotel.GameClients;
using Plus.HabboHotel.Users.Messenger.FriendBar;

namespace Plus.Communication.Packets.Incoming.Preferences;

internal class SetUIFlagsEvent : IPacketEvent
{
    public Task Parse(GameClient session, IIncomingPacket packet)
    {
        session.GetHabbo().FriendbarState = FriendBarStateUtility.GetEnum(packet.ReadInt());
        session.Send(new SoundSettingsComposer(session.GetHabbo().ClientVolume, session.GetHabbo().ChatPreference, session.GetHabbo().AllowMessengerInvites, session.GetHabbo().FocusPreference,
            FriendBarStateUtility.GetInt(session.GetHabbo().FriendbarState)));
        return Task.CompletedTask;
    }
}