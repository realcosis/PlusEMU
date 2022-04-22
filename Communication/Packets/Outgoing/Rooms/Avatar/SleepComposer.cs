using Plus.HabboHotel.Rooms;

namespace Plus.Communication.Packets.Outgoing.Rooms.Avatar;

public class SleepComposer : ServerPacket
{
    public SleepComposer(RoomUser user, bool isSleeping)
        : base(ServerPacketHeader.SleepMessageComposer)
    {
        WriteInteger(user.VirtualId);
        WriteBoolean(isSleeping);
    }
}