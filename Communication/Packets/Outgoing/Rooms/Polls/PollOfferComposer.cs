using Plus.HabboHotel.GameClients;

namespace Plus.Communication.Packets.Outgoing.Rooms.Polls;

internal class PollOfferComposer : IServerPacket
{
    public int MessageId => ServerPacketHeader.PollOfferComposer;

    public void Compose(IOutgoingPacket packet)
    {
        packet.WriteInteger(111141); //Room Id
        packet.WriteString("CLIENT_NPS");
        packet.WriteString("Customer Satisfaction Poll");
        packet.WriteString("Give us your opinion!");
    }
}