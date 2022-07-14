using Plus.HabboHotel.GameClients;

namespace Plus.Communication.Packets.Outgoing.Messenger;

internal class FindFriendsProcessResultComposer : IServerPacket
{
    private readonly bool _found;
    public int MessageId => ServerPacketHeader.FindFriendsProcessResultMessageComposer;

    public FindFriendsProcessResultComposer(bool found)
    {
        _found = found;
    }

    public void Compose(IOutgoingPacket packet)
    {
        packet.WriteBoolean(_found);
    }
}