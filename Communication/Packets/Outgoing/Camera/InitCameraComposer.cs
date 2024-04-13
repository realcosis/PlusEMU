using Plus.HabboHotel.GameClients;

namespace Plus.Communication.Packets.Outgoing.Camera;

public class InitCameraComposer : IServerPacket
{
    readonly int _purchaseValue;
    readonly int _purchasePointsValue;
    readonly int _publishPoints;

    public uint MessageId => ServerPacketHeader.InitCameraComposer;

    public InitCameraComposer(int purchaseValue, int purchasePointsValue, int publishPoints)
    {
        _publishPoints = publishPoints;
        _purchaseValue = purchaseValue;
        _purchasePointsValue = purchasePointsValue;
    }

    public void Compose(IOutgoingPacket packet)
    {
        packet.WriteInteger(_purchaseValue);
        packet.WriteInteger(_purchasePointsValue);
        packet.WriteInteger(_publishPoints);
    }
}