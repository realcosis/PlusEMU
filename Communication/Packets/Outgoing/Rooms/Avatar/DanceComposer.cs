using Plus.HabboHotel.GameClients;
using Plus.HabboHotel.Rooms;

namespace Plus.Communication.Packets.Outgoing.Rooms.Avatar;

public class DanceComposer : IServerPacket
{
    private readonly RoomUser _avatar;
    private readonly int _dance;

    public uint MessageId => ServerPacketHeader.DanceComposer;

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