using Plus.HabboHotel.GameClients;

namespace Plus.Communication.Packets.Outgoing.Navigator;

public class UpdateFavouriteRoomComposer : IServerPacket
{
    private readonly int _roomId;
    private readonly bool _added;

    public int MessageId => ServerPacketHeader.UpdateFavouriteRoomMessageComposer;

    public UpdateFavouriteRoomComposer(int roomId, bool added)
    {
        _roomId = roomId;
        _added = added;
    }

    public void Compose(IOutgoingPacket packet)
    {
        packet.WriteInteger(_roomId);
        packet.WriteBoolean(_added);
    }
}