using Plus.HabboHotel.GameClients;

namespace Plus.Communication.Packets.Incoming.Camera;

internal class RenderRoomThumbnailEvent : IPacketEvent
{
    public Task Parse(GameClient session, IIncomingPacket packet) => throw new NotImplementedException();
}