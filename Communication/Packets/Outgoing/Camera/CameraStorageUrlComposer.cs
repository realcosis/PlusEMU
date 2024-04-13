using Plus.HabboHotel.GameClients;

namespace Plus.Communication.Packets.Outgoing.Camera;

public class CameraStorageUrlComposer : IServerPacket
{
    readonly string _url;

    public uint MessageId => ServerPacketHeader.CameraStorageUrlComposer;

    public CameraStorageUrlComposer(string url)
        => _url = url;

    public void Compose(IOutgoingPacket packet)
        => packet.WriteString(_url);
}