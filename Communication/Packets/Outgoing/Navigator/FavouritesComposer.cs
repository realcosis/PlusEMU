using System.Collections;
using Plus.HabboHotel.GameClients;

namespace Plus.Communication.Packets.Outgoing.Navigator;

internal class FavouritesComposer : IServerPacket
{
    private readonly ArrayList _favouriteIds;
    public int MessageId => ServerPacketHeader.FavouritesMessageComposer;

    public FavouritesComposer(ArrayList favouriteIds)
    {
        _favouriteIds = favouriteIds;
    }

    public void Compose(IOutgoingPacket packet)
    {
        packet.WriteInteger(50);
        packet.WriteInteger(_favouriteIds.Count);
        foreach (int id in _favouriteIds.ToArray()) packet.WriteInteger(id);
    }
}