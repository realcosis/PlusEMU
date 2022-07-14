using Plus.HabboHotel.GameClients;

namespace Plus.Communication.Packets.Outgoing.Moderation;

internal class OpenHelpToolComposer : IServerPacket
{
    public int MessageId => ServerPacketHeader.OpenHelpToolMessageComposer;

    public void Compose(IOutgoingPacket packet) => packet.WriteInteger(0);
}