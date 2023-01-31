using Plus.HabboHotel.GameClients;

namespace Plus.Communication.Packets.Incoming.Camera;

internal class PublishPhotoEvent : IPacketEvent
{
    public Task Parse(GameClient session, IIncomingPacket packet) => throw new NotImplementedException();
}