using Plus.HabboHotel.GameClients;

namespace Plus.Communication.Packets.Incoming.Recycler;

internal class RecyclerRecycleEvent : IPacketEvent
{
    public Task Parse(GameClient session, IIncomingPacket packet) => throw new NotImplementedException();
}
