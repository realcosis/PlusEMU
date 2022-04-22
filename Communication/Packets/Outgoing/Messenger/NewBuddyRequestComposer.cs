using Plus.HabboHotel.Cache.Type;

namespace Plus.Communication.Packets.Outgoing.Messenger;

internal class NewBuddyRequestComposer : ServerPacket
{
    public NewBuddyRequestComposer(UserCache habbo)
        : base(ServerPacketHeader.NewBuddyRequestMessageComposer)
    {
        WriteInteger(habbo.Id);
        WriteString(habbo.Username);
        WriteString(habbo.Look);
    }
}