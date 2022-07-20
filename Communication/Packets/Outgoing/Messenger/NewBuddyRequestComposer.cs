using Plus.HabboHotel.GameClients;

namespace Plus.Communication.Packets.Outgoing.Messenger;

public class NewBuddyRequestComposer : IServerPacket
{
    private readonly int _id;
    private readonly string _name;
    private readonly string _figure;
    public uint MessageId => ServerPacketHeader.NewBuddyRequestComposer;

    public NewBuddyRequestComposer(int id, string name, string figure)
    {
        _id = id;
        _name = name;
        _figure = figure;
    }

    public void Compose(IOutgoingPacket packet)
    {
        packet.WriteInteger(_id);
        packet.WriteString(_name);
        packet.WriteString(_figure);
    }
}