using Plus.HabboHotel.GameClients;

namespace Plus.Communication.Packets.Outgoing.Rooms.Notifications;

public class RoomNotificationComposer : IServerPacket
{
    private string _type;
    private Dictionary<string, string> _values;

    public uint MessageId => ServerPacketHeader.RoomNotificationComposer;

    public RoomNotificationComposer(string type, string key, string value)
    {
        _type = type;
        _values = new() { { key, value } };
    }

    public RoomNotificationComposer(string type)
    {
        _type = type;
        _values = new();
    }

    public RoomNotificationComposer(string title, string message, string type, string hotelName = "", string hotelUrl = "")
    {
        _type = type;
        _values = new()
        {
            { "title", title },
            { "message", message },
            { "linkUrl", hotelUrl },
            { "linkTitle", hotelName }
        };
    }

    public void Compose(IOutgoingPacket packet)
    {

        packet.WriteString(_type);
        var values = _values.Where(kvp => !string.IsNullOrWhiteSpace(kvp.Value)).ToList();
        packet.WriteInteger(values.Count);
        foreach (var (key, value) in values)
        {
            packet.WriteString(key);
            packet.WriteString(value);
        }
    }
}