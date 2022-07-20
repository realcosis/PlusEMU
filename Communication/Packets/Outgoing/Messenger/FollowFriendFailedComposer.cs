using Plus.HabboHotel.GameClients;

namespace Plus.Communication.Packets.Outgoing.Messenger;

public class FollowFriendFailedComposer : IServerPacket
{
    private readonly int _errorCode;
    public uint MessageId => ServerPacketHeader.FollowFriendFailedComposer;

    public FollowFriendFailedComposer(int errorCode)
    {
        _errorCode = errorCode;
    }

    public void Compose(IOutgoingPacket packet) => packet.WriteInteger(_errorCode);
}