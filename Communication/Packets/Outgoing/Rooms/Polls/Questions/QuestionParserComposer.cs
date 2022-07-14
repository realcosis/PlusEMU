using Plus.HabboHotel.GameClients;

namespace Plus.Communication.Packets.Outgoing.Rooms.Polls.Questions;

// TODO @80O: Implement Polls
internal class QuestionParserComposer : IServerPacket
{
    public int MessageId => ServerPacketHeader.QuestionParserMessageComposer;

    public void Compose(IOutgoingPacket packet)
    {
        packet.WriteString("MATCHING_POLL");
        packet.WriteInteger(2686); //??
        packet.WriteInteger(10016); //???
        packet.WriteInteger(60); //Duration
        packet.WriteInteger(10016);
        packet.WriteInteger(9);
        packet.WriteInteger(6);
        packet.WriteString("MAFIA WARS: WEAPONS VOTE");
        packet.WriteInteger(0);
        packet.WriteInteger(6);
        packet.WriteInteger(0);
        packet.WriteInteger(0);
    }
}