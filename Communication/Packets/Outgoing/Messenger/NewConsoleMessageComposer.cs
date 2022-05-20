namespace Plus.Communication.Packets.Outgoing.Messenger;

internal class NewConsoleMessageComposer : ServerPacket
{
    public NewConsoleMessageComposer(int sender, string message, int secondsAgo = 0)
        : base(ServerPacketHeader.NewConsoleMessageMessageComposer)
    {
        WriteInteger(sender);
        WriteString(message);
        WriteInteger(secondsAgo);
    }
}