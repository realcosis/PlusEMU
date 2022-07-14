using Plus.HabboHotel.GameClients;

namespace Plus.Communication.Packets.Outgoing.Help;

// TODO @80O: Implement
public class SanctionStatusComposer : IServerPacket
{
    public int MessageId => ServerPacketHeader.SanctionStatusMessageComposer;

    public void Compose(IOutgoingPacket packet)
    {
        packet.WriteBoolean(false);
        packet.WriteBoolean(false);
        packet.WriteString("The General was here");
        packet.WriteInteger(1); //Hours
        packet.WriteInteger(10);
        packet.WriteString("ccccc");
        packet.WriteString("bbb");
        packet.WriteInteger(0);
        packet.WriteString("abb");
        packet.WriteInteger(0);
        packet.WriteInteger(0);
        packet.WriteBoolean(true); //if true and second boolean is false it does something. - if false, we got banned, so true is mute
    }
}