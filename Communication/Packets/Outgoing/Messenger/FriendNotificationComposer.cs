using Plus.HabboHotel.Users.Messenger;

namespace Plus.Communication.Packets.Outgoing.Messenger
{
    class FriendNotificationComposer : ServerPacket
    {
        public FriendNotificationComposer(int userId, MessengerEventTypes type, string data)
            : base(ServerPacketHeader.FriendNotificationMessageComposer)
        {
            WriteString(userId.ToString());
            WriteInteger(MessengerEventTypesUtility.GetEventTypePacketNum(type));
            WriteString(data);
        }
    }
}
