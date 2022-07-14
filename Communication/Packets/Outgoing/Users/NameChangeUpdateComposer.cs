using Plus.HabboHotel.GameClients;

namespace Plus.Communication.Packets.Outgoing.Users;

internal class NameChangeUpdateComposer : IServerPacket
{
    private readonly string _name;
    private readonly int _error;
    private readonly ICollection<string> _tags;

    public int MessageId => ServerPacketHeader.NameChangeUpdateMessageComposer;

    public NameChangeUpdateComposer(string name, int error, ICollection<string> tags)
    {
        _name = name;
        _error = error;
        _tags = tags;
    }

    public NameChangeUpdateComposer(string name, int error)
    {
        _name = name;
        _error = error;
        _tags = Array.Empty<string>();
    }

    public void Compose(IOutgoingPacket packet)
    {
        packet.WriteInteger(_error);
        packet.WriteString(_name);
        packet.WriteInteger(_tags.Count);
        foreach (var tag in _tags) packet.WriteString(_name + tag);
    }
}