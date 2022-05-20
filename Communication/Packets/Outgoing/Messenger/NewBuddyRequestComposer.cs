namespace Plus.Communication.Packets.Outgoing.Messenger;

internal class NewBuddyRequestComposer : ServerPacket
{
    public NewBuddyRequestComposer(int id, string name, string figure)
        : base(ServerPacketHeader.NewBuddyRequestMessageComposer)
    {
        WriteInteger(id);
        WriteString(name);
        WriteString(figure);
    }
}