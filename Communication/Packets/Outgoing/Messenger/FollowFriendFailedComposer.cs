using Plus.HabboHotel.GameClients;

namespace Plus.Communication.Packets.Outgoing.Messenger;

internal class FollowFriendFailedComposer : IServerPacket
{
    private readonly int _errorCode;
    public int MessageId => ServerPacketHeader.FollowFriendFailedMessageComposer;

    public FollowFriendFailedComposer(int errorCode)
    {
        _errorCode = errorCode;
    }

    public void Compose(IOutgoingPacket packet) => packet.WriteInteger(_errorCode);
}