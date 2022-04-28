using System;
using System.Linq;
using Plus.Communication.Packets.Outgoing;
using Plus.HabboHotel.GameClients;
using Plus.HabboHotel.Rooms;
using Plus.HabboHotel.Users.Relationships;

namespace Plus.HabboHotel.Users.Messenger;

public class MessengerBuddy
{
    public GameClient? Client;
    public bool AppearOffline { get; set; }
    public bool HideInRoom { get; set; }
    public int LastOnline { get; set; }
    public string Look { get; set; }
    public string Motto { get; set; }
    public string Username { get; set; }

    public int Id { get; set; }

    public bool IsOnline =>
        Client != null && Client.GetHabbo() != null && Client.GetHabbo().GetMessenger() != null &&
        !Client.GetHabbo().GetMessenger().AppearOffline;

    public bool InRoom => CurrentRoom != null;

    public Room CurrentRoom { get; set; }

    public void UpdateUser(GameClient client)
    {
        Client = client;
        if (client != null && client.GetHabbo() != null)
            CurrentRoom = client.GetHabbo().CurrentRoom;
    }

    public void Serialize(ServerPacket message, GameClient session)
    {
        Relationship relationship = null;
        if (session.GetHabbo() != null && session.GetHabbo().Relationships != null)
            relationship = session.GetHabbo().Relationships.FirstOrDefault(x => x.Value.UserId == Convert.ToInt32(Id)).Value;
        var y = relationship?.Type ?? 0;
        message.WriteInteger(Id);
        message.WriteString(Username);
        message.WriteInteger(1);
        message.WriteBoolean(!AppearOffline || session.GetHabbo().GetPermissions().HasRight("mod_tool") ? IsOnline : false);
        message.WriteBoolean(!HideInRoom || session.GetHabbo().GetPermissions().HasRight("mod_tool") ? InRoom : false);
        message.WriteString(IsOnline ? Look : "");
        message.WriteInteger(0); // categoryid
        message.WriteString(Motto);
        message.WriteString(string.Empty); // Facebook username
        message.WriteString(string.Empty);
        message.WriteBoolean(true); // Allows offline messaging
        message.WriteBoolean(false); // ?
        message.WriteBoolean(false); // Uses phone
        message.WriteShort(y);
    }
}