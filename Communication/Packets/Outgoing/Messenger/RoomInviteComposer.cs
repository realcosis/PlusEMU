using Plus.HabboHotel.GameClients;

namespace Plus.Communication.Packets.Outgoing.Messenger;

internal class RoomInviteComposer : IServerPacket
{
    private readonly int _senderId;
    private readonly string _text;

    public int MessageId => ServerPacketHeader.RoomInviteMessageComposer;

    public RoomInviteComposer(int senderId, string text)
    {
        _senderId = senderId;
        _text = text;
    }

    public void Compose(IOutgoingPacket packet)
    {
        packet.WriteInteger(_senderId);
        packet.WriteString(_text);
    }
}