using Plus.HabboHotel.Rooms;

namespace Plus.Communication.Packets.Outgoing.Rooms.Settings;

internal class FlatControllerRemovedComposer : ServerPacket
{
    public FlatControllerRemovedComposer(Room instance, int userId)
        : base(ServerPacketHeader.FlatControllerRemovedMessageComposer)
    {
        WriteInteger(instance.Id);
        WriteInteger(userId);
    }
}