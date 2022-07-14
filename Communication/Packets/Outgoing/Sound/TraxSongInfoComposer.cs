using Plus.HabboHotel.GameClients;

namespace Plus.Communication.Packets.Outgoing.Sound;

// TODO @80O: Implement
internal class TraxSongInfoComposer : IServerPacket
{
    public int MessageId => ServerPacketHeader.TraxSongInfoMessageComposer;

    public void Compose(IOutgoingPacket packet) => packet.WriteInteger(0); //Count;
}