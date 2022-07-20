using Plus.HabboHotel.GameClients;

namespace Plus.Communication.Packets.Outgoing.Users;

// TODO @80O: Implement
public class ScrSendUserInfoComposer : IServerPacket
{
    public uint MessageId => ServerPacketHeader.ScrSendUserInfoComposer;

    public void Compose(IOutgoingPacket packet)
    {
        packet.WriteString("habbo_club");
        packet.WriteInteger(0); //display days
        packet.WriteInteger(2);
        packet.WriteInteger(0); //display months
        packet.WriteInteger(1);
        packet.WriteBoolean(true); // hc
        packet.WriteBoolean(true); // vip
        packet.WriteInteger(0);
        packet.WriteInteger(0);
        packet.WriteInteger(495);
    }
}