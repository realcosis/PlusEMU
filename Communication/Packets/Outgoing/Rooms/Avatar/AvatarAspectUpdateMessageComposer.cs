namespace Plus.Communication.Packets.Outgoing.Rooms.Engine;

internal class AvatarAspectUpdateMessageComposer : ServerPacket
{
    public AvatarAspectUpdateMessageComposer(string figure, string gender)
        : base(ServerPacketHeader.AvatarAspectUpdateMessageComposer)
    {
        WriteString(figure);
        WriteString(gender);
    }
}