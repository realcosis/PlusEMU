using Plus.HabboHotel.GameClients;

namespace Plus.Communication.Packets.Outgoing.Messenger;

internal class MessengerInitComposer : IServerPacket
{
    public int MessageId => ServerPacketHeader.MessengerInitMessageComposer;

    public void Compose(IOutgoingPacket packet)
    {
        packet.WriteInteger(Convert.ToInt32(PlusEnvironment.GetSettingsManager().TryGetValue("messenger.buddy_limit"))); //Friends max.
        packet.WriteInteger(300);
        packet.WriteInteger(800);
        packet.WriteInteger(0); // category count
    }
}