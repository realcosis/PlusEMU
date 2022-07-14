using Plus.HabboHotel.GameClients;

namespace Plus.Communication.Packets.Outgoing.Navigator;

internal class NavigatorSettingsComposer : IServerPacket
{
    private readonly int _homeroom;
    public int MessageId => ServerPacketHeader.NavigatorSettingsMessageComposer;

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