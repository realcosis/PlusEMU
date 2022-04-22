using Plus.HabboHotel.Rooms;

namespace Plus.Communication.Packets.Outgoing.Rooms.Engine;

internal class UserChangeComposer : ServerPacket
{
    public UserChangeComposer(RoomUser user, bool self)
        : base(ServerPacketHeader.UserChangeMessageComposer)
    {
        WriteInteger(self ? -1 : user.VirtualId);
        WriteString(user.GetClient().GetHabbo().Look);
        WriteString(user.GetClient().GetHabbo().Gender);
        WriteString(user.GetClient().GetHabbo().Motto);
        WriteInteger(user.GetClient().GetHabbo().GetStats().AchievementPoints);
    }
}