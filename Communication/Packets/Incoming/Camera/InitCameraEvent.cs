using Plus.HabboHotel.GameClients;

namespace Plus.Communication.Packets.Incoming.Camera;

internal class InitCameraEvent : IPacketEvent
{
    public Task Parse(GameClient session, IIncomingPacket packet) => throw new NotImplementedException();
}