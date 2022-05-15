namespace Plus.Communication.Packets.Outgoing.Handshake;

public class UserRightsComposer : ServerPacket
{
    public UserRightsComposer(int rank, bool isAmbassador)
        : base(ServerPacketHeader.UserRightsMessageComposer)
    {
        WriteInteger(2); //Club level
        WriteInteger(rank);
        WriteBoolean(isAmbassador); //Is an ambassador
    }
}