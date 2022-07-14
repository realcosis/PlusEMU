using Plus.HabboHotel.GameClients;

namespace Plus.Communication.Packets.Outgoing.Rooms.Furni.Wired;

internal class HideWiredConfigComposer : IServerPacket
{
    public int MessageId => ServerPacketHeader.HideWiredConfigMessageComposer;

    public void Compose(IOutgoingPacket packet)
    {
        // Empty Body
    }
}