using Plus.HabboHotel.GameClients;

namespace Plus.Communication.Packets.Outgoing.Navigator.New;

// TODO @80O: Implement
internal class NavigatorCollapsedCategoriesComposer : IServerPacket
{
    public int MessageId => ServerPacketHeader.NavigatorCollapsedCategoriesMessageComposer;

    public void Compose(IOutgoingPacket packet) => packet.WriteInteger(0);
}