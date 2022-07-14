using Plus.HabboHotel.GameClients;
using Plus.HabboHotel.Rooms;

namespace Plus.Communication.Packets.Outgoing.Rooms.Avatar;

internal class DanceComposer : IServerPacket
{
    private readonly RoomUser _avatar;
    private readonly int _dance;

    public int MessageId => ServerPacketHeader.DanceMessageComposer;

    public DanceComposer(RoomUser avatar, int dance)
    {
        _avatar = avatar;
        _dance = dance;
    }

    public void Compose(IOutgoingPacket packet)
    {
        packet.WriteInteger(_avatar.VirtualId);
        packet.WriteInteger(_dance);
    }
}