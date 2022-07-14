using Plus.HabboHotel.GameClients;

namespace Plus.Communication.Packets.Outgoing.Messenger;

internal class NewConsoleMessageComposer : IServerPacket
{
    private readonly int _sender;
    private readonly string _message;
    private readonly int _secondsAgo;

    public int MessageId => ServerPacketHeader.NewConsoleMessageMessageComposer;

    public NewConsoleMessageComposer(int sender, string message, int secondsAgo = 0)
    {
        _sender = sender;
        _message = message;
        _secondsAgo = secondsAgo;
    }

    public void Compose(IOutgoingPacket packet)
    {
        packet.WriteInteger(_sender);
        packet.WriteString(_message);
        packet.WriteInteger(_secondsAgo);
    }
}