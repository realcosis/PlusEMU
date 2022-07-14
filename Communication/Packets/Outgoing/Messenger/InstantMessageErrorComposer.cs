using Plus.HabboHotel.GameClients;
using Plus.HabboHotel.Users.Messenger;

namespace Plus.Communication.Packets.Outgoing.Messenger;

internal class InstantMessageErrorComposer : IServerPacket
{
    private readonly MessengerMessageErrors _error;
    private readonly int _target;
    public int MessageId => ServerPacketHeader.InstantMessageErrorMessageComposer;

    public InstantMessageErrorComposer(MessengerMessageErrors error, int target)
    {
        _error = error;
        _target = target;
    }

    public void Compose(IOutgoingPacket packet)
    {
        packet.WriteInteger(MessengerMessageErrorsUtility.GetMessageErrorPacketNum(_error));
        packet.WriteInteger(_target);
        packet.WriteString("");
    }
}