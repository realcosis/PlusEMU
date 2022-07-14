using Plus.HabboHotel.GameClients;

namespace Plus.Communication.Packets.Outgoing.Moderation;

internal class BroadcastMessageAlertComposer : IServerPacket
{
    private readonly string _message;
    private readonly string _url;
    public int MessageId => ServerPacketHeader.BroadcastMessageAlertMessageComposer;

    public BroadcastMessageAlertComposer(string message, string url = "")
    {
        _message = message;
        _url = url;
    }

    public void Compose(IOutgoingPacket packet)
    {
        packet.WriteString(_message);
        packet.WriteString(_url);
    }
}