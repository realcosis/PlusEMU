using Plus.HabboHotel.GameClients;

namespace Plus.Communication.Packets.Outgoing.Rooms.Engine;

// TODO @80O: Implement
internal class FurnitureAliasesComposer : IServerPacket
{
    public int MessageId => ServerPacketHeader.FurnitureAliasesMessageComposer;

    public void Compose(IOutgoingPacket packet) => packet.WriteInteger(0);
}