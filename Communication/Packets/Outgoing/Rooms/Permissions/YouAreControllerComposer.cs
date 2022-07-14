using Plus.HabboHotel.GameClients;

namespace Plus.Communication.Packets.Outgoing.Rooms.Permissions;

internal class YouAreControllerComposer : IServerPacket
{
    private readonly int _setting;
    public int MessageId => ServerPacketHeader.YouAreControllerMessageComposer;

    public YouAreControllerComposer(int setting)
    {
        _setting = setting;
    }

    public void Compose(IOutgoingPacket packet) => packet.WriteInteger(_setting);
}