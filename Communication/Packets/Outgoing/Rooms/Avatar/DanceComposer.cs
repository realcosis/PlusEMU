using Plus.HabboHotel.Rooms;

namespace Plus.Communication.Packets.Outgoing.Rooms.Avatar;

internal class DanceComposer : ServerPacket
{
    public DanceComposer(RoomUser avatar, int dance)
        : base(ServerPacketHeader.DanceMessageComposer)
    {
        WriteInteger(avatar.VirtualId);
        WriteInteger(dance);
    }
}