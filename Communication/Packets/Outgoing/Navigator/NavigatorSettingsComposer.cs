using Plus.HabboHotel.GameClients;

namespace Plus.Communication.Packets.Outgoing.Navigator;

public class NavigatorSettingsComposer : IServerPacket
{
    private readonly int _homeroom;
    public uint MessageId => ServerPacketHeader.NavigatorSettingsComposer;

    public NavigatorSettingsComposer(int homeroom)
    {
        _homeroom = homeroom;
    }

    public void Compose(IOutgoingPacket packet)
    {
        packet.WriteInteger(_homeroom);
        packet.WriteInteger(_homeroom);
    }
}