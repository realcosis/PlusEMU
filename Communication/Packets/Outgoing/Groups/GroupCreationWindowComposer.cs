using Plus.HabboHotel.GameClients;
using Plus.HabboHotel.Rooms;

namespace Plus.Communication.Packets.Outgoing.Groups;

public class GroupCreationWindowComposer : IServerPacket
{
    private readonly ICollection<RoomData> _rooms;
    public uint MessageId => ServerPacketHeader.GroupCreationWindowComposer;

    public GroupCreationWindowComposer(ICollection<RoomData> rooms)
    {
        _rooms = rooms;
    }

    public void Compose(IOutgoingPacket packet)
    {
        packet.WriteInteger(Convert.ToInt32(PlusEnvironment.SettingsManager.TryGetValue("catalog.group.purchase.cost"))); //Price // TODO @80O: Pass via constructor
        packet.WriteInteger(_rooms.Count); //Room count that the user has.
        foreach (var room in _rooms)
        {
            packet.WriteUInteger(room.Id); //Room Id
            packet.WriteString(room.Name); //Room Name
            packet.WriteBoolean(false); //What?
        }
        packet.WriteInteger(5);
        packet.WriteInteger(5);
        packet.WriteInteger(11);
        packet.WriteInteger(4);
        packet.WriteInteger(6);
        packet.WriteInteger(11);
        packet.WriteInteger(4);
        packet.WriteInteger(0);
        packet.WriteInteger(0);
        packet.WriteInteger(0);
        packet.WriteInteger(0);
        packet.WriteInteger(0);
        packet.WriteInteger(0);
        packet.WriteInteger(0);
        packet.WriteInteger(0);
        packet.WriteInteger(0);
    }
}