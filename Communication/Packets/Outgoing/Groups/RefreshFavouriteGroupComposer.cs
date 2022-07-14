using Plus.HabboHotel.GameClients;

namespace Plus.Communication.Packets.Outgoing.Groups;

internal class RefreshFavouriteGroupComposer : IServerPacket
{
    private readonly int _id;
    public int MessageId => ServerPacketHeader.RefreshFavouriteGroupMessageComposer;

    public RefreshFavouriteGroupComposer(int id)
    {
        _id = id;
    }

    public void Compose(IOutgoingPacket packet) => packet.WriteInteger(_id);
}