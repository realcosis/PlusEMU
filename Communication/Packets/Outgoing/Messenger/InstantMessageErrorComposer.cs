using Plus.HabboHotel.Users.Messenger;

namespace Plus.Communication.Packets.Outgoing.Messenger;

internal class InstantMessageErrorComposer : ServerPacket
{
    public InstantMessageErrorComposer(MessengerMessageErrors error, int target)
        : base(ServerPacketHeader.InstantMessageErrorMessageComposer)
    {
        WriteInteger(MessengerMessageErrorsUtility.GetMessageErrorPacketNum(error));
        WriteInteger(target);
        WriteString("");
    }
}